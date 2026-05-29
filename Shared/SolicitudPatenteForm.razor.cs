using BlazorApp.Models;
using BlazorApp.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Threading.Tasks;

namespace BlazorApp.Shared;

public partial class SolicitudPatenteForm : ComponentBase
{
    [Inject] private IContribuyentesService ContribuyentesService { get; set; } = default!;
    [Inject] private IBienesInmueblesService BienesInmueblesService { get; set; } = default!;
    [Inject] private IActividadEconomicaService ActividadEconomicaService { get; set; } = default!;
    [Inject] private IUsoSueloMockService UsoSueloService { get; set; } = default!;
    [Inject] private IMaestroDatosService MaestroDatosService { get; set; } = default!;
    [Inject] private IPatentesService PatentesService { get; set; } = default!;
    [Inject] private IPatentesFormularioService PatentesFormularioService { get; set; } = default!;
    [Inject] private IPatentesValidacionService PatentesValidacionService { get; set; } = default!;
    [Inject] private ISnackbar Snackbar { get; set; } = default!;

    [Parameter] public SolicitudPatenteDto Model { get; set; } = new();
    [Parameter] public EventCallback<SolicitudPatenteDto> ModelChanged { get; set; }
    [Parameter] public EventCallback<int> OnSaved { get; set; }

    private MudForm? form;
    private List<string> validationErrors = new();
    private List<ContribuyenteDto> contribuyentes = new();
    private List<BienInmuebleDto> bienes = new();
    private List<ActividadEconomicaDto> actividades = new();
    private List<string> distritos = new();
    private List<string> tiposPatente = new();
    private List<string> estadosPatente = new();
    private List<string> adjuntosCatalogo = new();
    private HashSet<string> selectedAdjuntos = new();
    private SolicitudPatenteDto solicitudBase = new();
    private int selectedContribuyenteId;
    private string selectedFinca = string.Empty;
    private string fechaSolicitudInput = DateTime.Today.ToString("yyyy-MM-dd");

    protected override async Task OnInitializedAsync()
    {
        contribuyentes = await ContribuyentesService.GetAllAsync();
        bienes = await BienesInmueblesService.GetAllAsync();
        actividades = await ActividadEconomicaService.GetAllAsync();
        distritos = await MaestroDatosService.GetDistritosAsync();
        tiposPatente = await MaestroDatosService.GetTiposPatenteAsync();
        estadosPatente = await MaestroDatosService.GetEstadosPatenteAsync();
        adjuntosCatalogo = await PatentesFormularioService.GetAdjuntosCatalogoAsync();
        solicitudBase = await PatentesFormularioService.CrearNuevaSolicitudAsync();
        EnsureDefaults();
    }

    protected override void OnParametersSet()
    {
        EnsureDefaults();
        selectedContribuyenteId = Model.ContribuyenteId;
        selectedFinca = Model.FincaOIdPredial;
        selectedAdjuntos = Model.Adjuntos.ToHashSet();
        fechaSolicitudInput = Model.FechaSolicitud == default
            ? DateTime.Today.ToString("yyyy-MM-dd")
            : Model.FechaSolicitud.ToString("yyyy-MM-dd");
    }

    private void EnsureDefaults()
    {
        Model.TipoPatente = string.IsNullOrWhiteSpace(Model.TipoPatente) ? solicitudBase.TipoPatente : Model.TipoPatente;
        Model.Estado = string.IsNullOrWhiteSpace(Model.Estado) ? solicitudBase.Estado : Model.Estado;
        Model.Responsable = string.IsNullOrWhiteSpace(Model.Responsable) ? solicitudBase.Responsable : Model.Responsable;
        Model.EstadoUsoSuelo = string.IsNullOrWhiteSpace(Model.EstadoUsoSuelo) ? solicitudBase.EstadoUsoSuelo : Model.EstadoUsoSuelo;
        Model.FechaSolicitud = Model.FechaSolicitud == default ? solicitudBase.FechaSolicitud : Model.FechaSolicitud;
        Model.UsoSuelo ??= new UsoSueloVinculadoDto();
        Model.Adjuntos ??= new List<string>();
        if (!Model.Requisitos.Any())
            Model.Requisitos = solicitudBase.Requisitos.Select(x => new RequisitoPatenteDto { Nombre = x.Nombre, Obligatorio = x.Obligatorio, Cumplido = x.Cumplido, Observacion = x.Observacion }).ToList();
        UpdateRequirements();
    }

    private async Task OnContribuyenteChanged(int value)
    {
        selectedContribuyenteId = value;
        var item = contribuyentes.FirstOrDefault(x => x.Id == value);
        if (item is null)
            return;

        Model.ContribuyenteId = item.Id;
        Model.ContribuyenteRuc = item.NumeroIdentificacion;
        Model.Identificacion = item.NumeroIdentificacion;
        Model.ContribuyenteNombre = item.NombreCompleto;
        await ModelChanged.InvokeAsync(Model);
    }

    private async Task OnFincaChanged(string value)
    {
        selectedFinca = value ?? string.Empty;
        Model.FincaOIdPredial = selectedFinca;
        var item = bienes.FirstOrDefault(x => x.NumeroFinca == selectedFinca);
        if (item is not null)
        {
            Model.DireccionLocal = string.IsNullOrWhiteSpace(Model.DireccionLocal) ? item.DireccionExacta : Model.DireccionLocal;
            Model.Distrito = item.Distrito;
        }

        await ModelChanged.InvokeAsync(Model);
    }

    private async Task OnActividadChanged(string value)
    {
        Model.CodigoCaecr = value ?? string.Empty;
        var item = actividades.FirstOrDefault(x => x.CodigoCaecr == Model.CodigoCaecr);
        Model.ActividadEconomica = item?.Descripcion ?? string.Empty;
        await ModelChanged.InvokeAsync(Model);
    }

    private async Task OnAdjuntosChanged(IEnumerable<string> values)
    {
        selectedAdjuntos = values?.ToHashSet() ?? new HashSet<string>();
        Model.Adjuntos = selectedAdjuntos.ToList();
        UpdateRequirements();
        await ModelChanged.InvokeAsync(Model);
    }

    private async Task OnFechaSolicitudChanged(string value)
    {
        fechaSolicitudInput = value ?? string.Empty;
        if (DateTime.TryParse(fechaSolicitudInput, out var parsed))
            Model.FechaSolicitud = parsed;
        await ModelChanged.InvokeAsync(Model);
    }

    private async Task ValidateUsoSueloAsync()
    {
        Model.UsoSuelo = await UsoSueloService.ValidarAsync(Model.FincaOIdPredial, Model.NumeroCertificadoUsoSuelo, Model.CodigoCaecr);
        Model.NumeroCertificadoUsoSuelo = Model.UsoSuelo.NumeroCertificado;
        Model.EstadoUsoSuelo = Model.UsoSuelo.Estado;
        Model.UsoSueloConforme = Model.UsoSuelo.EsConforme;
        UpdateRequirements();
        Snackbar.Add(Model.UsoSuelo.Observaciones, Model.UsoSuelo.EsConforme ? Severity.Success : Severity.Warning);
        await ModelChanged.InvokeAsync(Model);
    }

    private async Task SaveAsync()
    {
        UpdateRequirements();
        validationErrors = PatentesValidacionService.ValidarSolicitud(Model, estadosPatente, false).Errores;
        if (validationErrors.Any())
            return;

        var saved = await PatentesService.SaveSolicitudAsync(Model);
        Model.LicenciaId = saved.Id;
        Model.NumeroSolicitud = saved.Solicitud.NumeroSolicitud;
        await ModelChanged.InvokeAsync(Model);
        Snackbar.Add("Solicitud de patente guardada correctamente.", Severity.Success);
        await OnSaved.InvokeAsync(saved.Id);
    }

    private async Task ApproveAsync()
    {
        UpdateRequirements();
        validationErrors = PatentesValidacionService.ValidarSolicitud(Model, estadosPatente, true).Errores;
        if (validationErrors.Any())
            return;

        var saved = await PatentesService.SaveSolicitudAsync(Model);
        var approved = await PatentesService.AprobarSolicitudAsync(saved.Id, Model.Responsable);
        Model.LicenciaId = approved.Id;
        Model.NumeroSolicitud = approved.Solicitud.NumeroSolicitud;
        Model.Estado = approved.Estado;
        await ModelChanged.InvokeAsync(Model);
        Snackbar.Add("Patente aprobada y remitida a cobro.", Severity.Success);
        await OnSaved.InvokeAsync(approved.Id);
    }

    private void UpdateRequirements()
        => PatentesValidacionService.ActualizarRequisitos(Model, selectedAdjuntos);

    private Severity UsoSueloSeverity => Model.EstadoUsoSuelo switch
    {
        "Conforme" => Severity.Success,
        "No conforme" => Severity.Error,
        _ => Severity.Info
    };

    private string UsoSueloMessage => string.IsNullOrWhiteSpace(Model.UsoSuelo.Observaciones)
        ? "Valide el certificado y la finca vinculada antes de resolver la solicitud."
        : Model.UsoSuelo.Observaciones;
}
