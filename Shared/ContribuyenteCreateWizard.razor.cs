using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BlazorApp.Models;
using BlazorApp.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BlazorApp.Shared;

public partial class ContribuyenteCreateWizard
{
    [Inject] private IMaestroDatosService MaestroDatosService { get; set; } = default!;
    [Inject] private IContribuyentesService ContribuyentesService { get; set; } = default!;
    [Inject] private IAuditoriaService AuditoriaService { get; set; } = default!;
    [Inject] private ISnackbar Snackbar { get; set; } = default!;

    [Parameter] public ContribuyenteDto Model { get; set; } = new();
    [Parameter] public EventCallback<ContribuyenteDto> ModelChanged { get; set; }
    [Parameter] public EventCallback<ContribuyenteDto> Created { get; set; }
    [Parameter] public EventCallback Cancelled { get; set; }

    private readonly IWizardStateService wizardStateService = new WizardStateMockService();
    private readonly List<WizardStepDto> wizardSteps =
    [
        new() { Index = 0, Title = "Identificación", Subtitle = "Tipo de persona y documento de identificación.", Icon = Icons.Material.Filled.Badge },
        new() { Index = 1, Title = "Datos generales", Subtitle = "Información principal del contribuyente.", Icon = Icons.Material.Filled.Article },
        new() { Index = 2, Title = "Dirección y contacto", Subtitle = "Ubicación fiscal y medios de contacto.", Icon = Icons.Material.Filled.LocationOn },
        new() { Index = 3, Title = "Representante y actividad", Subtitle = "Representación legal y actividad económica.", Icon = Icons.Material.Filled.BusinessCenter },
        new() { Index = 4, Title = "Vínculos iniciales", Subtitle = "Tributos, bienes y observaciones de arranque.", Icon = Icons.Material.Filled.Link },
        new() { Index = 5, Title = "Revisión y confirmación", Subtitle = "Control final antes de crear el expediente.", Icon = Icons.Material.Filled.CheckCircle }
    ];

    private List<string> tiposPersona = new();
    private List<string> tiposIdentificacion = new();
    private List<string> estadosContribuyente = new();
    private List<string> distritos = new();
    private List<string> actividades = new();
    private List<string> provincias = new();
    private List<string> cantones = new();
    private List<string> estadosCiviles = new();
    private readonly List<string> estadosRelacion = ["Activo", "Pendiente", "Seguimiento"];
    private int currentStepIndex;
    private string currentStatus = "Borrador";
    private readonly string processKey = $"ruc-create-{Guid.NewGuid():N}";
    private WizardStateDto wizardState = new();
    private TributoVinculadoDto draftTributo = new() { Estado = "Activo" };
    private BienInmuebleVinculadoDto draftBien = new() { Estado = "Activo" };

    private WizardStepDto CurrentStep => wizardSteps[currentStepIndex];

    protected override async Task OnInitializedAsync()
    {
        tiposPersona = await MaestroDatosService.GetTiposPersonaAsync();
        tiposIdentificacion = await MaestroDatosService.GetTiposIdentificacionAsync();
        estadosContribuyente = await MaestroDatosService.GetEstadosContribuyenteAsync();
        distritos = await MaestroDatosService.GetDistritosAsync();
        actividades = await MaestroDatosService.GetActividadesEconomicasAsync();
        provincias = await MaestroDatosService.GetProvinciasAsync();
        cantones = await MaestroDatosService.GetCantonesAsync();
        estadosCiviles = await MaestroDatosService.GetEstadosCivilesAsync();

        Model.DireccionFiscal ??= new DireccionFiscalDto();
        Model.RepresentanteLegal ??= new RepresentanteLegalDto();
        Model.TributosVinculados ??= new List<TributoVinculadoDto>();
        Model.BienesInmueblesVinculados ??= new List<BienInmuebleVinculadoDto>();

        await RefreshWizardStateAsync();
    }

    private async Task GoNextAsync()
    {
        var result = BuildValidationResults().First(x => x.StepIndex == currentStepIndex);
        if (!result.IsValid)
        {
            currentStatus = "En progreso";
            await RefreshWizardStateAsync();
            Snackbar.Add("Complete los campos obligatorios del paso actual para continuar.", Severity.Warning);
            return;
        }

        if (currentStepIndex < wizardSteps.Count - 1)
        {
            currentStepIndex++;
            currentStatus = "En progreso";
            await RefreshWizardStateAsync();
        }
    }

    private async Task GoBackAsync()
    {
        if (currentStepIndex <= 0)
            return;

        currentStepIndex--;
        currentStatus = "En progreso";
        await RefreshWizardStateAsync();
    }

    private async Task SaveDraftAsync()
    {
        currentStatus = "Borrador";
        await RefreshWizardStateAsync(saveDraft: true);
        await ModelChanged.InvokeAsync(Model);
    }

    private async Task CancelAsync()
    {
        currentStatus = "Cancelado";
        await RefreshWizardStateAsync();
        await wizardStateService.ClearStateAsync(processKey);
        await Cancelled.InvokeAsync();
    }

    private async Task CreateContribuyenteAsync()
    {
        var validationResults = BuildValidationResults();
        if (validationResults.Any(x => !x.IsValid))
        {
            currentStatus = "En progreso";
            await RefreshWizardStateAsync();
            Snackbar.Add("Existen validaciones pendientes antes de crear el contribuyente.", Severity.Warning);
            return;
        }

        Model.DatosIncompletos = false;
        Model.PendienteCalidad = false;

        await ContribuyentesService.SaveAsync(Model);
        await AuditoriaService.RegistrarCambioAsync(Model.Id == 0 ? 1 : Model.Id, new AuditoriaCambioDto
        {
            Usuario = "analista.ruc",
            FechaHora = DateTime.Now,
            CampoModificado = "Registro general",
            ValorAnterior = "N/A",
            ValorNuevo = Model.NombreCompleto,
            Origen = "Wizard RUC"
        });

        currentStepIndex = wizardSteps.Count - 1;
        currentStatus = "Completado";
        await RefreshWizardStateAsync();
        await wizardStateService.ClearStateAsync(processKey);
        await ModelChanged.InvokeAsync(Model);
        await Created.InvokeAsync(Model);
        Snackbar.Add("Contribuyente creado correctamente.", Severity.Success);
    }

    private async Task AddTributoAsync()
    {
        if (string.IsNullOrWhiteSpace(draftTributo.Codigo) || string.IsNullOrWhiteSpace(draftTributo.Nombre))
        {
            Snackbar.Add("Complete el código y nombre del tributo antes de agregarlo.", Severity.Warning);
            return;
        }

        Model.TributosVinculados.Add(new TributoVinculadoDto
        {
            Codigo = draftTributo.Codigo.Trim(),
            Nombre = draftTributo.Nombre.Trim(),
            Estado = string.IsNullOrWhiteSpace(draftTributo.Estado) ? "Activo" : draftTributo.Estado
        });

        draftTributo = new TributoVinculadoDto { Estado = "Activo" };
        await RefreshWizardStateAsync();
    }

    private async Task RemoveTributoAsync(TributoVinculadoDto tributo)
    {
        Model.TributosVinculados.Remove(tributo);
        await RefreshWizardStateAsync();
    }

    private async Task AddBienAsync()
    {
        if (string.IsNullOrWhiteSpace(draftBien.FincaNumero) || string.IsNullOrWhiteSpace(draftBien.Distrito) || string.IsNullOrWhiteSpace(draftBien.Uso))
        {
            Snackbar.Add("Complete finca, distrito y uso antes de agregar el bien.", Severity.Warning);
            return;
        }

        Model.BienesInmueblesVinculados.Add(new BienInmuebleVinculadoDto
        {
            FincaNumero = draftBien.FincaNumero.Trim(),
            Distrito = draftBien.Distrito.Trim(),
            Uso = draftBien.Uso.Trim(),
            Estado = string.IsNullOrWhiteSpace(draftBien.Estado) ? "Activo" : draftBien.Estado
        });

        draftBien = new BienInmuebleVinculadoDto { Estado = "Activo" };
        await RefreshWizardStateAsync();
    }

    private async Task RemoveBienAsync(BienInmuebleVinculadoDto bien)
    {
        Model.BienesInmueblesVinculados.Remove(bien);
        await RefreshWizardStateAsync();
    }

    private async Task RefreshWizardStateAsync(bool saveDraft = false)
    {
        var validationResults = BuildValidationResults();
        var pendingValidations = validationResults
            .Where(x => (x.StepIndex <= currentStepIndex || currentStepIndex == wizardSteps.Count - 1 || currentStatus == "Completado") && (!x.IsValid || x.HasWarnings))
            .SelectMany(x => x.Messages)
            .Distinct()
            .ToList();
        var summaryItems = BuildSummaryItems();
        var steps = wizardSteps.Select(step =>
        {
            var result = validationResults.First(x => x.StepIndex == step.Index);
            var isVisited = step.Index <= currentStepIndex || currentStatus == "Completado";
            return new WizardStepDto
            {
                Index = step.Index,
                Title = step.Title,
                Subtitle = step.Subtitle,
                Icon = step.Icon,
                IsCompleted = currentStatus == "Completado" || (result.IsValid && step.Index < currentStepIndex),
                HasErrors = isVisited && !result.IsValid,
                HasWarnings = isVisited && result.HasWarnings,
                ValidationMessages = isVisited ? result.Messages.ToList() : new List<string>()
            };
        }).ToList();

        wizardState = new WizardStateDto
        {
            ProcessKey = processKey,
            ProcessTitle = "Nuevo contribuyente",
            Status = currentStatus,
            CurrentStepIndex = currentStepIndex,
            TotalSteps = steps.Count,
            ProgressPercent = steps.Count == 0 ? 0 : Math.Round((double)(currentStepIndex + 1) / steps.Count * 100d, 0),
            LastSavedAt = wizardState.LastSavedAt,
            Steps = steps,
            ValidationResults = validationResults,
            SummaryItems = summaryItems,
            PendingValidations = pendingValidations
        };

        wizardState = saveDraft
            ? await wizardStateService.SaveDraftAsync(wizardState)
            : await wizardStateService.UpdateStateAsync(wizardState);
    }

    private List<StepValidationResultDto> BuildValidationResults()
    {
        var results = Enumerable.Range(0, 5).Select(BuildStepValidationResult).ToList();
        var pendingSteps = results.Where(x => !x.IsValid).Select(x => x.StepTitle).ToList();

        var reviewMessages = new List<string>();
        if (pendingSteps.Any())
            reviewMessages.Add($"Complete los pasos pendientes antes de crear el expediente: {string.Join(", ", pendingSteps)}.");

        results.Add(new StepValidationResultDto
        {
            StepIndex = 5,
            StepTitle = wizardSteps[5].Title,
            IsValid = !pendingSteps.Any(),
            HasWarnings = false,
            Messages = reviewMessages
        });

        return results;
    }

    private StepValidationResultDto BuildStepValidationResult(int stepIndex)
    {
        var messages = new List<string>();
        var hasWarnings = false;

        switch (stepIndex)
        {
            case 0:
                if (string.IsNullOrWhiteSpace(Model.TipoPersona))
                    messages.Add("Seleccione el tipo de persona.");
                if (string.IsNullOrWhiteSpace(Model.TipoIdentificacion))
                    messages.Add("Seleccione el tipo de identificación.");
                if (string.IsNullOrWhiteSpace(Model.NumeroIdentificacion))
                    messages.Add("Ingrese el número de identificación.");
                break;
            case 1:
                if (Model.TipoPersona == "Juridica")
                {
                    if (string.IsNullOrWhiteSpace(Model.RazonSocial))
                        messages.Add("La razón social es obligatoria para persona jurídica.");
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(Model.Nombre))
                        messages.Add("El nombre es obligatorio.");
                    if (string.IsNullOrWhiteSpace(Model.Apellidos))
                        messages.Add("Los apellidos son obligatorios.");
                    if (string.IsNullOrWhiteSpace(Model.EstadoCivil))
                        messages.Add("Seleccione el estado civil.");
                }
                if (string.IsNullOrWhiteSpace(Model.Nacionalidad))
                    messages.Add("La nacionalidad es obligatoria.");
                if (string.IsNullOrWhiteSpace(Model.EstadoContribuyente))
                    messages.Add("Seleccione el estado del contribuyente.");
                break;
            case 2:
                if (string.IsNullOrWhiteSpace(Model.DireccionFiscal.Provincia))
                    messages.Add("Seleccione la provincia.");
                if (string.IsNullOrWhiteSpace(Model.DireccionFiscal.Canton))
                    messages.Add("Seleccione el cantón.");
                if (string.IsNullOrWhiteSpace(Model.DireccionFiscal.Distrito))
                    messages.Add("Seleccione el distrito.");
                if (string.IsNullOrWhiteSpace(Model.DireccionFiscal.DireccionExacta))
                    messages.Add("La dirección exacta es obligatoria.");
                if (string.IsNullOrWhiteSpace(Model.Telefono))
                    messages.Add("El teléfono es obligatorio.");
                else if (!Regex.IsMatch(Model.Telefono, @"^[0-9]{4}-?[0-9]{4}$"))
                    messages.Add("El teléfono debe tener 8 dígitos.");
                if (string.IsNullOrWhiteSpace(Model.Correo))
                    messages.Add("El correo es obligatorio.");
                else if (!new EmailAddressAttribute().IsValid(Model.Correo))
                    messages.Add("El correo no tiene un formato válido.");
                break;
            case 3:
                if (Model.TipoPersona == "Juridica")
                {
                    if (string.IsNullOrWhiteSpace(Model.RepresentanteLegal.NombreCompleto))
                        messages.Add("El representante legal es obligatorio para persona jurídica.");
                    if (string.IsNullOrWhiteSpace(Model.RepresentanteLegal.Identificacion))
                        messages.Add("La identificación del representante es obligatoria.");
                    if (!string.IsNullOrWhiteSpace(Model.RepresentanteLegal.Correo) && !new EmailAddressAttribute().IsValid(Model.RepresentanteLegal.Correo))
                        messages.Add("El correo del representante no tiene un formato válido.");
                }
                if (string.IsNullOrWhiteSpace(Model.ActividadEconomica))
                    messages.Add("Seleccione la actividad económica.");
                break;
            case 4:
                if (!Model.TributosVinculados.Any() && !Model.BienesInmueblesVinculados.Any())
                {
                    hasWarnings = true;
                    messages.Add("No se han registrado vínculos iniciales; puede continuar si aún no aplica.");
                }
                break;
        }

        return new StepValidationResultDto
        {
            StepIndex = stepIndex,
            StepTitle = wizardSteps[stepIndex].Title,
            IsValid = !messages.Any() || hasWarnings,
            HasWarnings = hasWarnings,
            Messages = messages
        };
    }

    private List<string> BuildSummaryItems()
    {
        var items = new List<string>
        {
            $"Tipo de persona: {Model.TipoPersona}",
            $"Identificación: {Model.TipoIdentificacion} {Model.NumeroIdentificacion}".Trim(),
            Model.TipoPersona == "Juridica"
                ? $"Contribuyente jurídico: {Model.RazonSocial}"
                : $"Contribuyente físico: {Model.NombreCompleto}",
            $"Nacionalidad: {Model.Nacionalidad}",
            $"Estado del contribuyente: {Model.EstadoContribuyente}",
            $"Dirección fiscal: {string.Join(", ", new[] { Model.DireccionFiscal.DireccionExacta, Model.DireccionFiscal.Distrito, Model.DireccionFiscal.Canton, Model.DireccionFiscal.Provincia }.Where(x => !string.IsNullOrWhiteSpace(x)))}",
            $"Actividad económica: {Model.ActividadEconomica}",
            $"Tributos vinculados: {Model.TributosVinculados.Count}",
            $"Bienes vinculados: {Model.BienesInmueblesVinculados.Count}"
        };

        var contactParts = new[] { Model.Telefono, Model.Correo }.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
        if (contactParts.Any())
            items.Add($"Contacto: {string.Join(" · ", contactParts)}");
        if (Model.TipoPersona == "Juridica" && !string.IsNullOrWhiteSpace(Model.RepresentanteLegal.NombreCompleto))
            items.Add($"Representante legal: {Model.RepresentanteLegal.NombreCompleto}");
        if (!string.IsNullOrWhiteSpace(Model.Observaciones))
            items.Add($"Observaciones: {Model.Observaciones}");

        return items.Where(x => !string.IsNullOrWhiteSpace(x)).ToList();
    }
}
