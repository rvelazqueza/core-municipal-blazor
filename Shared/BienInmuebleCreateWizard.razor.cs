using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorApp.Models;
using BlazorApp.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BlazorApp.Shared;

public partial class BienInmuebleCreateWizard : ComponentBase
{
    private const int TotalSteps = 6;

    private readonly string[] stepTitles =
    [
        "Identificación de finca",
        "Propietarios y derechos",
        "Características del inmueble",
        "Valoración",
        "Integraciones",
        "Revisión y creación"
    ];

    private readonly string[] stepSubtitles =
    [
        "Captura el identificador registral y la ubicación.",
        "Vincula el propietario principal y define el derecho.",
        "Completa el uso, la zona y la tipología del inmueble.",
        "Revisa el valor fiscal y recalcula cuando sea necesario.",
        "Visualiza las integraciones del expediente.",
        "Verifica el resumen completo antes de crear la finca."
    ];

    private readonly string[] stepIcons =
    [
        Icons.Material.Filled.Badge,
        Icons.Material.Filled.Person,
        Icons.Material.Filled.HomeWork,
        Icons.Material.Filled.Calculate,
        Icons.Material.Filled.Sync,
        Icons.Material.Filled.Checklist
    ];

    private readonly string[] integrationWarnings =
    [
        "Registro Público pendiente de simulación.",
        "GIS pendiente de simulación.",
        "Hacienda pendiente de simulación.",
        "Cobro pendiente de simulación."
    ];

    private int currentStepIndex;
    private string currentStatus = "Borrador";
    private DateTime? lastSavedAt;
    private WizardStateDto wizardState = new();
    private List<StepValidationResultDto> validationResults = new();
    private List<string> pendingValidations = new();
    private List<string> summaryItems = new();
    private List<string> identificationSummary = new();
    private List<string> ownershipSummary = new();
    private List<string> characteristicsSummary = new();
    private List<string> valuationSummary = new();
    private List<string> distritos = new();
    private List<string> estadosBien = new();
    private List<string> condiciones = new();
    private List<string> tiposFinca = new();
    private List<string> estadosRegistrales = new();
    private List<string> usos = new();
    private List<string> zonasHomogeneas = new();
    private List<string> tipologias = new();
    private List<ContribuyenteDto> propietariosRuc = new();

    [Parameter] public BienInmuebleDto Model { get; set; } = new();
    [Parameter] public EventCallback<BienInmuebleDto> ModelChanged { get; set; }
    [Parameter] public EventCallback<BienInmuebleDto> OnCreated { get; set; }
    [Parameter] public EventCallback OnCanceled { get; set; }

    [Inject] public IMaestroDatosService MaestroDatosService { get; set; } = default!;
    [Inject] public IContribuyentesService ContribuyentesService { get; set; } = default!;
    [Inject] public IBienesInmueblesService BienesInmueblesService { get; set; } = default!;
    [Inject] public IValoracionService ValoracionService { get; set; } = default!;
    [Inject] public IFiscalizacionService FiscalizacionService { get; set; } = default!;
    [Inject] public ISnackbar Snackbar { get; set; } = default!;

    protected override async Task OnInitializedAsync()
    {
        await LoadReferenceDataAsync();
        EnsureDefaultValues();
        RefreshWizardState();
    }

    protected override void OnParametersSet()
    {
        EnsureDefaultValues();
        RefreshWizardState();
    }

    private string CurrentStepTitle => stepTitles[currentStepIndex];
    private string CurrentStepSubtitle => stepSubtitles[currentStepIndex];
    private bool CanGoBack => currentStepIndex > 0;
    private bool CanGoNext => currentStepIndex < TotalSteps - 1 && BuildStepValidationResult(currentStepIndex).IsValid;
    private bool CanFinish => currentStepIndex == TotalSteps - 1 && BuildValidationResults().Take(TotalSteps - 1).All(x => x.IsValid);
    private double ProgressPercent => (currentStepIndex + 1) * 100d / TotalSteps;
    private string CalculatedValorFiscalTotal => (Model.ValorTerreno + Model.ValorConstruccion).ToString("N2");

    private async Task LoadReferenceDataAsync()
    {
        distritos = await MaestroDatosService.GetDistritosAsync();
        estadosBien = await MaestroDatosService.GetEstadosBienInmuebleAsync();
        condiciones = await MaestroDatosService.GetCondicionesBienInmuebleAsync();
        tiposFinca = await MaestroDatosService.GetTiposFincaAsync();
        estadosRegistrales = await MaestroDatosService.GetEstadosRegistralesAsync();
        usos = await MaestroDatosService.GetUsosInmuebleAsync();
        zonasHomogeneas = await MaestroDatosService.GetZonasHomogeneasAsync();
        tipologias = await MaestroDatosService.GetTipologiasConstructivasAsync();
        propietariosRuc = await ContribuyentesService.GetAllAsync();
    }

    private void EnsureDefaultValues()
    {
        if (string.IsNullOrWhiteSpace(Model.TipoFinca))
        {
            Model.TipoFinca = "Individual";
        }

        if (string.IsNullOrWhiteSpace(Model.EstadoRegistral))
        {
            Model.EstadoRegistral = "Inscrita";
        }

        if (string.IsNullOrWhiteSpace(Model.Estado))
        {
            Model.Estado = "Activo";
        }

        if (string.IsNullOrWhiteSpace(Model.Condicion))
        {
            Model.Condicion = "Al dia";
        }

        if (Model.PorcentajeDerecho < 0)
        {
            Model.PorcentajeDerecho = 100m;
        }
    }

    private async Task GoBackAsync()
    {
        if (!CanGoBack)
        {
            return;
        }

        currentStepIndex--;
        currentStatus = currentStepIndex == 0 ? "Borrador" : "En progreso";
        RefreshWizardState();
        await Task.CompletedTask;
    }

    private async Task GoNextAsync()
    {
        if (!CanGoNext)
        {
            Snackbar.Add("Complete los campos obligatorios de este paso antes de continuar.", Severity.Warning);
            return;
        }

        if (currentStepIndex < TotalSteps - 1)
        {
            currentStepIndex++;
            currentStatus = "En progreso";
            RefreshWizardState();
        }

        await Task.CompletedTask;
    }

    private Task SaveDraftAsync()
    {
        currentStatus = "Borrador";
        lastSavedAt = DateTime.Now;
        RefreshWizardState();
        return Task.CompletedTask;
    }

    private async Task CancelAsync()
    {
        currentStatus = "Cancelado";
        RefreshWizardState();
        await OnCanceled.InvokeAsync();
    }

    private async Task CreateFincaAsync()
    {
        RefreshWizardState();
        if (!CanFinish)
        {
            Snackbar.Add("Existen validaciones pendientes antes de crear la finca.", Severity.Warning);
            return;
        }

        SyncCalculatedFields();
        SyncOwnershipRecord();
        Model.HistorialCambios.Insert(0, new AuditoriaCambioDto
        {
            Usuario = "analista.bienes",
            FechaHora = DateTime.Now,
            CampoModificado = "Expediente de finca",
            ValorAnterior = "N/A",
            ValorNuevo = string.IsNullOrWhiteSpace(Model.NumeroFinca) ? Model.IdPredial : Model.NumeroFinca,
            Origen = "Alta de finca"
        });

        await BienesInmueblesService.SaveAsync(Model);
        lastSavedAt = DateTime.Now;
        currentStatus = "Completado";
        RefreshWizardState();
        await ModelChanged.InvokeAsync(Model);
        await OnCreated.InvokeAsync(Model);
        Snackbar.Add("Finca creada correctamente.", Severity.Success);
    }

    private async Task RecalcularValoracionAsync()
    {
        SyncCalculatedFields();
        await ValoracionService.RecalcularAsync(Model);
        var nueva = await ValoracionService.GenerarNuevaValoracionAsync(Model);
        Model.Valoraciones.Insert(0, nueva);
        lastSavedAt = DateTime.Now;
        RefreshWizardState();
        await ModelChanged.InvokeAsync(Model);
        Snackbar.Add("Valoración recalculada.", Severity.Info);
    }

    private async Task SimularRegistroPublicoAsync()
    {
        await BienesInmueblesService.SimularRegistroPublicoAsync(Model);
        AddAuditChange("registro.publico", "Estado registral", "Sincronización previa", Model.EstadoRegistral, "Registro Público");
        await ModelChanged.InvokeAsync(Model);
        RefreshWizardState();
        Snackbar.Add("Registro Público sincronizado.", Severity.Info);
    }

    private async Task SimularGisAsync()
    {
        await BienesInmueblesService.SimularGisAsync(Model);
        AddAuditChange("gis.municipal", "Ubicación GIS", "Pendiente", Model.UbicacionGis, "GIS");
        await ModelChanged.InvokeAsync(Model);
        RefreshWizardState();
        Snackbar.Add("GIS sincronizado.", Severity.Info);
    }

    private async Task SimularHaciendaAsync()
    {
        await BienesInmueblesService.SimularRemisionHaciendaAsync(Model);
        AddAuditChange("hacienda.integracion", "Remisión periódica", "Pendiente", "Transmitida", "Ministerio de Hacienda");
        await ModelChanged.InvokeAsync(Model);
        RefreshWizardState();
        Snackbar.Add("Hacienda sincronizada.", Severity.Info);
    }

    private async Task SimularCobroAsync()
    {
        await BienesInmueblesService.SimularCobroAsync(Model);
        AddAuditChange("cobro.integracion", "Cuenta tributaria", "Sin cuenta", Model.CuentaTributaria, "Cobro");
        await ModelChanged.InvokeAsync(Model);
        RefreshWizardState();
        Snackbar.Add("Cobro sincronizado.", Severity.Info);
    }

    private async Task OnOwnerChangedAsync()
    {
        var owner = propietariosRuc.FirstOrDefault(x => x.Id == Model.PropietarioPrincipalId);
        if (owner is not null)
        {
            Model.PropietarioPrincipal = owner.NombreCompleto;
            Model.PropietarioPrincipalIdentificacion = owner.NumeroIdentificacion;
        }

        currentStatus = currentStepIndex == 0 ? "Borrador" : "En progreso";
        SyncOwnershipRecord();
        await ModelChanged.InvokeAsync(Model);
        RefreshWizardState();
    }

    private void SyncCalculatedFields()
    {
        Model.ValorFiscalTotal = Model.ValorTerreno + Model.ValorConstruccion;
    }

    private void SyncOwnershipRecord()
    {
        if (Model.PropietarioPrincipalId <= 0 || string.IsNullOrWhiteSpace(Model.PropietarioPrincipal))
        {
            return;
        }

        if (!Model.DerechosPropiedad.Any())
        {
            Model.DerechosPropiedad.Add(new DerechoPropiedadDto());
        }

        var right = Model.DerechosPropiedad.First();
        right.ContribuyenteId = Model.PropietarioPrincipalId;
        right.Titular = Model.PropietarioPrincipal;
        right.Identificacion = Model.PropietarioPrincipalIdentificacion;
        right.TipoDerecho = string.IsNullOrWhiteSpace(right.TipoDerecho) ? "Pleno dominio" : right.TipoDerecho;
        right.Porcentaje = Model.PorcentajeDerecho;
        right.Estado = "Activo";
        if (right.FechaInicio == default)
        {
            right.FechaInicio = DateTime.Today;
        }
    }

    private void AddAuditChange(string usuario, string campo, string valorAnterior, string valorNuevo, string origen)
    {
        Model.HistorialCambios.Insert(0, new AuditoriaCambioDto
        {
            Usuario = usuario,
            FechaHora = DateTime.Now,
            CampoModificado = campo,
            ValorAnterior = valorAnterior,
            ValorNuevo = valorNuevo,
            Origen = origen
        });
    }

    private void RefreshWizardState()
    {
        validationResults = BuildValidationResults();
        pendingValidations = BuildPendingValidations();
        identificationSummary = BuildIdentificationSummary();
        ownershipSummary = BuildOwnershipSummary();
        characteristicsSummary = BuildCharacteristicsSummary();
        valuationSummary = BuildValuationSummary();
        summaryItems = BuildSummaryItems();

        wizardState.ProcessKey = "bienes-inmuebles-alta";
        wizardState.ProcessTitle = "Nueva finca";
        wizardState.Status = currentStatus;
        wizardState.CurrentStepIndex = currentStepIndex;
        wizardState.TotalSteps = TotalSteps;
        wizardState.ProgressPercent = ProgressPercent;
        wizardState.LastSavedAt = lastSavedAt;
        wizardState.Steps = BuildSteps();
        wizardState.ValidationResults = validationResults;
        wizardState.SummaryItems = summaryItems;
        wizardState.PendingValidations = pendingValidations;
    }
}