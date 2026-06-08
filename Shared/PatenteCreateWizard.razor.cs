using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorApp.Models;
using BlazorApp.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BlazorApp.Shared;

public partial class PatenteCreateWizard : ComponentBase
{
    private const string ProcessKey = "patentes-nueva-solicitud";

    private static readonly ConcurrentDictionary<string, PatenteSolicitudWizardModel> DraftStore = new();

    private readonly IWizardStateService wizardStateService = new WizardStateMockService();
    private readonly List<WizardStepDto> wizardSteps =
    [
        new() { Index = 0, Title = "Tipo de solicitud y canal", Subtitle = "Defina el trámite y su origen de ingreso.", Icon = Icons.Material.Filled.NoteAdd },
        new() { Index = 1, Title = "Solicitante / contribuyente", Subtitle = "Identifique a la persona física o jurídica que gestiona la solicitud.", Icon = Icons.Material.Filled.Badge },
        new() { Index = 2, Title = "Local, finca o actividad sin local físico", Subtitle = "Ubique el negocio o indique la modalidad sin local físico.", Icon = Icons.Material.Filled.LocationOn },
        new() { Index = 3, Title = "Actividad económica y datos del negocio", Subtitle = "Registre el giro comercial y la identificación operativa del negocio.", Icon = Icons.Material.Filled.BusinessCenter },
        new() { Index = 4, Title = "Uso de Suelo", Subtitle = "Consigne el estado base de la validación urbanística.", Icon = Icons.Material.Filled.Map },
        new() { Index = 5, Title = "Requisitos y declaración jurada", Subtitle = "Marque el avance documental mínimo y la declaración jurada.", Icon = Icons.Material.Filled.FactCheck },
        new() { Index = 6, Title = "Hacienda / CCSS / FODESAF / INS", Subtitle = "Controle el avance de las validaciones externas simuladas.", Icon = Icons.Material.Filled.Description },
        new() { Index = 7, Title = "Morosidad, inspección y revisión", Subtitle = "Complete el estado general de revisión institucional.", Icon = Icons.Material.Filled.Search },
        new() { Index = 8, Title = "Tasación, cobro mock y resolución", Subtitle = "Deje preparada la salida financiera y resolutiva demo.", Icon = Icons.Material.Filled.Article },
        new() { Index = 9, Title = "Resumen y finalización", Subtitle = "Revise el proceso antes de registrar la solicitud demo.", Icon = Icons.Material.Filled.CheckCircle }
    ];

    private readonly List<string> tiposSolicitud = ["Licencia comercial nueva", "Licencia temporal", "Licencia de licores", "Renovación", "Modificación", "Exoneración", "Declaración", "Reposición de certificado"];
    private readonly List<string> tiposLicencia = ["Ordinaria", "Permanente", "Temporal", "Ambulante", "Estacionaria", "Días festivos", "Actividad económica sin local comercial", "Espectáculos públicos", "Extracción de materiales", "Licores"];
    private readonly List<string> canalesIngreso = ["Plataforma de Servicios", "Presencial", "MIMUNIENCASA", "VUI", "Gestión de oficio"];
    private readonly List<string> estadosIniciales = ["Borrador", "Recibida", "En revisión"];
    private readonly List<string> tiposPersona = ["Fisica", "Juridica"];
    private readonly List<string> mediosNotificacion = ["Correo electrónico", "SMS", "Presencial"];
    private readonly List<string> estadosContribuyente = ["Activo", "Pendiente", "Suspendido"];
    private readonly List<string> calidadesRuc = ["Validado", "Revisar", "Pendiente actualización"];
    private readonly List<string> tiposUbicacion = ["Local físico", "Actividad sin local físico"];
    private readonly List<string> estadosUsoSuelo = ["Conforme", "No conforme", "Pendiente", "Vencido"];
    private readonly List<string> resultadosUsoSuelo = ["Aprobado", "Requiere revisión", "No cumple"];
    private readonly List<string> estadosCumplimiento = ["Cumple", "No cumple", "Pendiente", "No aplica"];
    private readonly List<string> estadosDeclaracion = ["Sí", "No"];
    private readonly List<string> estadosIntegracion = ["Conforme", "Pendiente", "Observado"];
    private readonly List<string> estadosRevision = ["Conforme", "Pendiente", "Observado"];
    private readonly List<string> estadosInspeccion = ["Solicitada", "Programada", "Completada"];
    private readonly List<string> estadosResolucion = ["Preparada", "Pendiente", "Aprobada", "Prevenida", "Rechazada", "Pendiente firma", "Pendiente validación"];

    private List<ContribuyenteDto> contribuyentes = new();
    private List<string> distritos = new();
    private List<PatBusinessLocationDto> localesMock = new();
    private List<ActividadEconomicaDto> actividadesEconomicasMock = new();
    private readonly List<string> validationMessages = new();
    private string selectedContribuyenteId = string.Empty;
    private string selectedLocalId = string.Empty;
    private string selectedActividadCodigo = string.Empty;
    private int currentStepIndex;
    private string currentStatus = "Borrador";
    private WizardStateDto wizardState = new();
    private PatenteSolicitudWizardModel model = new();
    private PatentesModuleSnapshotDto? moduleSnapshot;

    [Inject] private IContribuyentesService ContribuyentesService { get; set; } = default!;
    [Inject] private IMaestroDatosService MaestroDatosService { get; set; } = default!;
    [Inject] private IPatentesService PatentesService { get; set; } = default!;
    [Inject] private IAuditoriaService AuditoriaService { get; set; } = default!;
    [Inject] private ISnackbar Snackbar { get; set; } = default!;

    [Parameter] public EventCallback OnCancelled { get; set; }
    [Parameter] public EventCallback<SolicitudPatenteDto> OnCompleted { get; set; }

    private WizardStepDto CurrentStep => wizardSteps[currentStepIndex];

    protected override async Task OnInitializedAsync()
    {
        contribuyentes = await ContribuyentesService.GetAllAsync();
        distritos = await MaestroDatosService.GetDistritosAsync();
        moduleSnapshot = await PatentesService.GetModuleSnapshotAsync();
        localesMock = moduleSnapshot.Locales.OrderBy(x => x.NombreLocal).ToList();
        actividadesEconomicasMock = moduleSnapshot.ActividadesEconomicas.OrderBy(x => x.Descripcion).ToList();
        await RestoreDraftAsync();
        await EnsureMockIdentifiersAsync();
        EnsureDefaults();
        SyncCatalogSelectionsFromModel();
        await EnsureLocalSelectionAsync(refresh: false);
        await EnsureActividadSelectionAsync(refresh: false);
        if (model.ContribuyenteId is null && !model.PendienteRegistroContribuyente && contribuyentes.Any())
        {
            selectedContribuyenteId = contribuyentes[0].Id.ToString();
            await OnContribuyenteChangedAsync();
        }
        await RefreshWizardStateAsync(persist: false);
    }

    private async Task RestoreDraftAsync()
    {
        if (DraftStore.TryGetValue(ProcessKey, out var storedModel))
        {
            model = CloneModel(storedModel);
            selectedContribuyenteId = model.ContribuyenteId?.ToString() ?? string.Empty;
        }
        else
        {
            model = new PatenteSolicitudWizardModel();
        }

        var savedState = await wizardStateService.GetStateAsync(ProcessKey);
        if (savedState is null)
            return;

        currentStepIndex = Math.Clamp(savedState.CurrentStepIndex, 0, wizardSteps.Count - 1);
        currentStatus = string.IsNullOrWhiteSpace(savedState.Status) ? "Borrador" : savedState.Status;
        wizardState.LastSavedAt = savedState.LastSavedAt;
    }

    private async Task EnsureMockIdentifiersAsync()
    {
        if (!string.IsNullOrWhiteSpace(model.NumeroSolicitud) && !string.IsNullOrWhiteSpace(model.NumeroExpediente))
            return;

        moduleSnapshot ??= await PatentesService.GetModuleSnapshotAsync();
        var sequence = moduleSnapshot.Solicitudes.Count + moduleSnapshot.LicenciasComerciales.Count + 1;

        if (string.IsNullOrWhiteSpace(model.NumeroSolicitud))
            model.NumeroSolicitud = $"SOL-PT-{DateTime.Today:yyyy}-{sequence:0000}";

        if (string.IsNullOrWhiteSpace(model.NumeroExpediente))
            model.NumeroExpediente = $"EXP-PT-{DateTime.Today:yyyy}-{sequence:0000}";
    }

    private void SyncCatalogSelectionsFromModel()
    {
        var selectedLocal = localesMock.FirstOrDefault(x =>
            (!string.IsNullOrWhiteSpace(model.IdPredial) && x.IdPredial.Equals(model.IdPredial, StringComparison.OrdinalIgnoreCase)) ||
            (!string.IsNullOrWhiteSpace(model.FincaOIdPredial) && x.IdPredial.Equals(model.FincaOIdPredial, StringComparison.OrdinalIgnoreCase)) ||
            (!string.IsNullOrWhiteSpace(model.NombreComercialLocal) && x.NombreLocal.Equals(model.NombreComercialLocal, StringComparison.OrdinalIgnoreCase)));
        selectedLocalId = selectedLocal?.Id.ToString() ?? string.Empty;

        var selectedActividad = actividadesEconomicasMock.FirstOrDefault(x => x.CodigoCaecr.Equals(model.CodigoCaecr, StringComparison.OrdinalIgnoreCase));
        selectedActividadCodigo = selectedActividad?.CodigoCaecr ?? string.Empty;
    }

    private async Task EnsureLocalSelectionAsync(bool refresh = true)
    {
        if (localesMock.Count == 0)
            return;

        if (string.IsNullOrWhiteSpace(selectedLocalId))
        {
            var fallback = model.RequiereLocalFisico
                ? localesMock.FirstOrDefault(x => !x.SinLocalFisico)
                : localesMock.FirstOrDefault(x => x.SinLocalFisico);

            selectedLocalId = (fallback ?? localesMock[0]).Id.ToString();
        }

        await ApplyLocalSelectionAsync(refresh);
    }

    private async Task EnsureActividadSelectionAsync(bool refresh = true)
    {
        if (actividadesEconomicasMock.Count == 0)
            return;

        if (string.IsNullOrWhiteSpace(selectedActividadCodigo))
            selectedActividadCodigo = actividadesEconomicasMock[0].CodigoCaecr;

        await ApplyActividadSelectionAsync(refresh);
    }

    private void EnsureDefaults()
    {
        model.ProcessKey = ProcessKey;
        model.NumeroSolicitud = string.IsNullOrWhiteSpace(model.NumeroSolicitud) ? $"SOL-PT-{DateTime.Today:yyyy}-0001" : model.NumeroSolicitud;
        model.NumeroExpediente = string.IsNullOrWhiteSpace(model.NumeroExpediente) ? $"EXP-PT-{DateTime.Today:yyyy}-0001" : model.NumeroExpediente;
        model.TipoSolicitud = string.IsNullOrWhiteSpace(model.TipoSolicitud) ? "Licencia comercial nueva" : model.TipoSolicitud;
        model.TipoLicencia = string.IsNullOrWhiteSpace(model.TipoLicencia) ? "Ordinaria" : model.TipoLicencia;
        model.CanalIngreso = string.IsNullOrWhiteSpace(model.CanalIngreso) ? "Plataforma de Servicios" : model.CanalIngreso;
        model.EstadoInicial = string.IsNullOrWhiteSpace(model.EstadoInicial) ? "Borrador" : model.EstadoInicial;
        model.ModalidadDeclaracionJurada = string.IsNullOrWhiteSpace(model.ModalidadDeclaracionJurada) ? "Sí" : model.ModalidadDeclaracionJurada;
        model.TieneDeclaracionJurada = string.Equals(model.ModalidadDeclaracionJurada, "Sí", StringComparison.OrdinalIgnoreCase);
        model.TipoPersona = string.IsNullOrWhiteSpace(model.TipoPersona) ? "Fisica" : model.TipoPersona;
        model.Correo = string.IsNullOrWhiteSpace(model.Correo) ? "demo.patentes@municipalidad.go.cr" : model.Correo;
        model.Telefono = string.IsNullOrWhiteSpace(model.Telefono) ? "2222-2222" : model.Telefono;
        model.DireccionFiscal = string.IsNullOrWhiteSpace(model.DireccionFiscal) ? "Dirección fiscal demo" : model.DireccionFiscal;
        model.MedioNotificacion = string.IsNullOrWhiteSpace(model.MedioNotificacion) ? "Correo electrónico" : model.MedioNotificacion;
        model.EstadoContribuyente = string.IsNullOrWhiteSpace(model.EstadoContribuyente) ? "Activo" : model.EstadoContribuyente;
        model.CalidadDatosRuc = string.IsNullOrWhiteSpace(model.CalidadDatosRuc) ? "Validado" : model.CalidadDatosRuc;
        model.PendienteRegistroContribuyente = model.ContribuyenteId is null && model.PendienteRegistroContribuyente;
        model.RequiereLocalFisico = !model.TipoLicencia.Equals("Actividad económica sin local comercial", StringComparison.OrdinalIgnoreCase);
        model.TipoUbicacion = string.IsNullOrWhiteSpace(model.TipoUbicacion)
            ? (model.RequiereLocalFisico ? "Local físico" : "Actividad sin local físico")
            : model.TipoUbicacion;
        model.DireccionLocal = string.IsNullOrWhiteSpace(model.DireccionLocal) ? "Avenida Central, local demo" : model.DireccionLocal;
        model.FincaOIdPredial = string.IsNullOrWhiteSpace(model.FincaOIdPredial) ? "F-0001" : model.FincaOIdPredial;
        model.IdPredial = string.IsNullOrWhiteSpace(model.IdPredial) ? "ID-PR-0001" : model.IdPredial;
        model.NumeroFinca = string.IsNullOrWhiteSpace(model.NumeroFinca) ? "1-23456" : model.NumeroFinca;
        model.Dueno = string.IsNullOrWhiteSpace(model.Dueno) ? "Municipalidad Demo" : model.Dueno;
        model.CondicionOcupacion = string.IsNullOrWhiteSpace(model.CondicionOcupacion) ? "Propiedad" : model.CondicionOcupacion;
        model.AreaLocal = string.IsNullOrWhiteSpace(model.AreaLocal) ? "120 m²" : model.AreaLocal;
        model.NombreComercialLocal = string.IsNullOrWhiteSpace(model.NombreComercialLocal) ? "Local demo" : model.NombreComercialLocal;
        model.EstadoGis = string.IsNullOrWhiteSpace(model.EstadoGis) ? "Pendiente" : model.EstadoGis;
        model.Distrito = string.IsNullOrWhiteSpace(model.Distrito) && distritos.Any() ? distritos[0] : model.Distrito;
        model.ActividadEconomica = string.IsNullOrWhiteSpace(model.ActividadEconomica) ? "Restaurante" : model.ActividadEconomica;
        model.CodigoCaecr = string.IsNullOrWhiteSpace(model.CodigoCaecr) ? "5610" : model.CodigoCaecr;
        model.CodigoCiiuVisual = string.IsNullOrWhiteSpace(model.CodigoCiiuVisual) ? "CIIU 5610" : model.CodigoCiiuVisual;
        model.NombreComercial = string.IsNullOrWhiteSpace(model.NombreComercial) ? "Comercio Demo" : model.NombreComercial;
        model.ActividadPrincipal = string.IsNullOrWhiteSpace(model.ActividadPrincipal) ? "Venta de alimentos" : model.ActividadPrincipal;
        model.ActividadesSecundarias = string.IsNullOrWhiteSpace(model.ActividadesSecundarias) ? "Servicio al cliente, Delivery" : model.ActividadesSecundarias;
        model.RiesgoActividad = string.IsNullOrWhiteSpace(model.RiesgoActividad) ? "Bajo" : model.RiesgoActividad;
        model.HorarioAtencion = string.IsNullOrWhiteSpace(model.HorarioAtencion) ? "Lunes a viernes 8:00 a.m. - 5:00 p.m." : model.HorarioAtencion;
        model.Empleados = string.IsNullOrWhiteSpace(model.Empleados) ? "3" : model.Empleados;
        model.RequiereLicores = model.RequiereLicores;
        model.RequierePermisoSanitario = true;
        model.RequiereInspeccion = true;
        model.ActividadTemporal = false;
        model.CertificadoUsoSuelo = string.IsNullOrWhiteSpace(model.CertificadoUsoSuelo) ? "CUS-DEMO-001" : model.CertificadoUsoSuelo;
        model.NumeroCertificadoUsoSuelo = string.IsNullOrWhiteSpace(model.NumeroCertificadoUsoSuelo) ? "CUS-DEMO-001" : model.NumeroCertificadoUsoSuelo;
        model.Zonificacion = string.IsNullOrWhiteSpace(model.Zonificacion) ? "Mixta comercial" : model.Zonificacion;
        model.ActividadesAutorizadas = string.IsNullOrWhiteSpace(model.ActividadesAutorizadas) ? "Comercio, alimentos, atención al público" : model.ActividadesAutorizadas;
        model.CompatibilidadUsoSuelo = string.IsNullOrWhiteSpace(model.CompatibilidadUsoSuelo) ? "Compatible" : model.CompatibilidadUsoSuelo;
        model.FechaValidacionUsoSuelo ??= DateTime.Today;
        model.FechaVencimientoUsoSuelo ??= DateTime.Today.AddYears(1);
        model.ResultadoUsoSuelo = string.IsNullOrWhiteSpace(model.ResultadoUsoSuelo) ? "Aprobado" : model.ResultadoUsoSuelo;
        model.EstadoUsoSuelo = string.IsNullOrWhiteSpace(model.EstadoUsoSuelo) ? "Conforme" : model.EstadoUsoSuelo;
        model.EstadoRequisitos = string.IsNullOrWhiteSpace(model.EstadoRequisitos) ? "Cumple" : model.EstadoRequisitos;
        model.EstadoIdentificacion = string.IsNullOrWhiteSpace(model.EstadoIdentificacion) ? "Cumple" : model.EstadoIdentificacion;
        model.EstadoPersoneriaJuridica = string.IsNullOrWhiteSpace(model.EstadoPersoneriaJuridica) ? "Cumple" : model.EstadoPersoneriaJuridica;
        model.EstadoUsoSueloChecklist = string.IsNullOrWhiteSpace(model.EstadoUsoSueloChecklist) ? "Cumple" : model.EstadoUsoSueloChecklist;
        model.EstadoPermisoSanitario = string.IsNullOrWhiteSpace(model.EstadoPermisoSanitario) ? "Cumple" : model.EstadoPermisoSanitario;
        model.EstadoArrendamientoPropiedad = string.IsNullOrWhiteSpace(model.EstadoArrendamientoPropiedad) ? "Cumple" : model.EstadoArrendamientoPropiedad;
        model.EstadoCcssChecklist = string.IsNullOrWhiteSpace(model.EstadoCcssChecklist) ? "Cumple" : model.EstadoCcssChecklist;
        model.EstadoFodesafChecklist = string.IsNullOrWhiteSpace(model.EstadoFodesafChecklist) ? "Cumple" : model.EstadoFodesafChecklist;
        model.EstadoInsChecklist = string.IsNullOrWhiteSpace(model.EstadoInsChecklist) ? "Cumple" : model.EstadoInsChecklist;
        model.EstadoDeclaracionJuradaChecklist = string.IsNullOrWhiteSpace(model.EstadoDeclaracionJuradaChecklist) ? "Cumple" : model.EstadoDeclaracionJuradaChecklist;
        model.EstadoComprobantePago = string.IsNullOrWhiteSpace(model.EstadoComprobantePago) ? "Cumple" : model.EstadoComprobantePago;
        model.EstadoCroquis = string.IsNullOrWhiteSpace(model.EstadoCroquis) ? "Cumple" : model.EstadoCroquis;
        model.DeclaracionJuradaDiasHabiles = model.DeclaracionJuradaDiasHabiles <= 0 ? 60 : model.DeclaracionJuradaDiasHabiles;
        model.EstadoDeclaracionJurada = string.IsNullOrWhiteSpace(model.EstadoDeclaracionJurada) ? "Sí" : model.EstadoDeclaracionJurada;
        model.EstadoHacienda = string.IsNullOrWhiteSpace(model.EstadoHacienda) ? "Conforme" : model.EstadoHacienda;
        model.EstadoCcss = string.IsNullOrWhiteSpace(model.EstadoCcss) ? "Conforme" : model.EstadoCcss;
        model.EstadoFodesaf = string.IsNullOrWhiteSpace(model.EstadoFodesaf) ? "Conforme" : model.EstadoFodesaf;
        model.EstadoIns = string.IsNullOrWhiteSpace(model.EstadoIns) ? "Conforme" : model.EstadoIns;
        model.FechaInicioActividad = string.IsNullOrWhiteSpace(model.FechaInicioActividad) ? DateTime.Today.AddMonths(-2).ToString("dd/MM/yyyy") : model.FechaInicioActividad;
        model.RegimenTributario = string.IsNullOrWhiteSpace(model.RegimenTributario) ? "Simplificado" : model.RegimenTributario;
        model.PeriodoFiscal = string.IsNullOrWhiteSpace(model.PeriodoFiscal) ? DateTime.Today.Year.ToString() : model.PeriodoFiscal;
        model.ActividadesEnOtrosCantones = string.IsNullOrWhiteSpace(model.ActividadesEnOtrosCantones) ? "No" : model.ActividadesEnOtrosCantones;
        model.EstadoIntegracionGeneral = string.IsNullOrWhiteSpace(model.EstadoIntegracionGeneral) ? "Completo" : model.EstadoIntegracionGeneral;
        model.SolicitanteAlDia = true;
        model.DuenoPropiedadAlDia = true;
        model.ArreglosPago = string.IsNullOrWhiteSpace(model.ArreglosPago) ? "No aplica" : model.ArreglosPago;
        model.EstadoMorosidad = string.IsNullOrWhiteSpace(model.EstadoMorosidad) ? "Conforme" : model.EstadoMorosidad;
        model.ResultadoMorosidad = string.IsNullOrWhiteSpace(model.ResultadoMorosidad) ? "Conforme" : model.ResultadoMorosidad;
        model.SolicitarInspeccion = model.SolicitarInspeccion;
        model.EstadoInspeccion = string.IsNullOrWhiteSpace(model.EstadoInspeccion) ? "Solicitada" : model.EstadoInspeccion;
        model.InspectorAsignado = string.IsNullOrWhiteSpace(model.InspectorAsignado) ? "Inspector Demo" : model.InspectorAsignado;
        model.EstadoRevision = string.IsNullOrWhiteSpace(model.EstadoRevision) ? "Conforme" : model.EstadoRevision;
        model.ResultadoRevision = string.IsNullOrWhiteSpace(model.ResultadoRevision) ? "Conforme" : model.ResultadoRevision;
        model.Multa = model.Multa < 0 ? 0m : model.Multa;
        model.Intereses = model.Intereses < 0 ? 0m : model.Intereses;
        model.EstadoFinalExpediente = string.IsNullOrWhiteSpace(model.EstadoFinalExpediente) ? "Borrador" : model.EstadoFinalExpediente;
        EnsureRequirementDefaults();
        EnsureAssessmentDefaults();
    }

    private void SyncLegacyWizardState()
    {
        if (!string.IsNullOrWhiteSpace(model.EstadoMorosidad))
            model.ResultadoMorosidad = model.EstadoMorosidad;

        if (!string.IsNullOrWhiteSpace(model.EstadoRevision))
            model.ResultadoRevision = model.EstadoRevision;

        if (!string.IsNullOrWhiteSpace(model.EstadoInspeccion))
            model.SolicitarInspeccion = !model.EstadoInspeccion.Equals("Solicitada", StringComparison.OrdinalIgnoreCase);

        SyncUsoSueloState();
        SyncRequirementsState();
        SyncAssessmentState();
    }

    private async Task OnContribuyenteChangedAsync()
    {
        if (!int.TryParse(selectedContribuyenteId, out var contribuyenteId))
        {
            model.ContribuyenteId = null;
            await RefreshWizardStateAsync(persist: false);
            return;
        }

        var contribuyente = contribuyentes.FirstOrDefault(x => x.Id == contribuyenteId);
        if (contribuyente is null)
        {
            await RefreshWizardStateAsync(persist: false);
            return;
        }

        model.ContribuyenteId = contribuyente.Id;
        model.PendienteRegistroContribuyente = false;
        model.Identificacion = contribuyente.NumeroIdentificacion;
        model.ContribuyenteNombre = contribuyente.NombreCompleto;
        model.TipoPersona = contribuyente.TipoPersona;
        model.Correo = contribuyente.Correo;
        model.Telefono = contribuyente.Telefono;
        model.DireccionFiscal = contribuyente.DireccionFiscal?.DireccionExacta ?? string.Empty;
        model.MedioNotificacion = string.IsNullOrWhiteSpace(model.MedioNotificacion) ? "Correo electrónico" : model.MedioNotificacion;
        model.EstadoContribuyente = contribuyente.EstadoContribuyente;
        model.CalidadDatosRuc = contribuyente.DatosIncompletos || contribuyente.PendienteCalidad ? "Revisar" : "Validado";
        model.Distrito = string.IsNullOrWhiteSpace(model.Distrito) ? (contribuyente.DireccionFiscal?.Distrito ?? string.Empty) : model.Distrito;
        model.ActividadEconomica = string.IsNullOrWhiteSpace(model.ActividadEconomica) ? contribuyente.ActividadEconomica : model.ActividadEconomica;
        model.Dueno = string.IsNullOrWhiteSpace(model.Dueno) ? contribuyente.NombreCompleto : model.Dueno;
        await RefreshWizardStateAsync(persist: false);
    }

    private async Task OnTipoLicenciaChangedAsync()
    {
        model.RequiereLocalFisico = !model.TipoLicencia.Equals("Actividad económica sin local comercial", StringComparison.OrdinalIgnoreCase);
        model.TipoUbicacion = model.RequiereLocalFisico ? "Local físico" : "Actividad sin local físico";
        await EnsureLocalSelectionAsync(refresh: false);
        await RefreshWizardStateAsync(persist: false);
    }

    private async Task OnDeclaracionJuradaChangedAsync()
    {
        model.ModalidadDeclaracionJurada = model.TieneDeclaracionJurada ? "Sí" : "No";
        await RefreshWizardStateAsync(persist: false);
    }

    private async Task GenerateMockExpedienteAsync()
    {
        model.NumeroSolicitud = string.Empty;
        model.NumeroExpediente = string.Empty;
        await EnsureMockIdentifiersAsync();
        await RefreshWizardStateAsync(persist: false);
    }

    private async Task OnPendienteRegistroChangedAsync()
    {
        if (model.PendienteRegistroContribuyente)
        {
            selectedContribuyenteId = string.Empty;
            model.ContribuyenteId = null;
            model.EstadoContribuyente = string.IsNullOrWhiteSpace(model.EstadoContribuyente) ? "Pendiente" : model.EstadoContribuyente;
            model.CalidadDatosRuc = string.IsNullOrWhiteSpace(model.CalidadDatosRuc) ? "Pendiente actualización" : model.CalidadDatosRuc;
        }

        await RefreshWizardStateAsync(persist: false);
    }

    private async Task OnLocalChangedAsync()
    {
        await ApplyLocalSelectionAsync(refresh: true);
    }

    private async Task OnActividadChangedAsync()
    {
        await ApplyActividadSelectionAsync(refresh: true);
    }

    private async Task ApplyLocalSelectionAsync(bool refresh)
    {
        var local = localesMock.FirstOrDefault(x => x.Id.ToString() == selectedLocalId);
        if (local is null)
            return;

        model.RequiereLocalFisico = !local.SinLocalFisico;
        model.TipoUbicacion = local.SinLocalFisico ? "Actividad sin local físico" : "Local físico";
        model.IdPredial = local.SinLocalFisico ? string.Empty : local.IdPredial;
        model.FincaOIdPredial = local.SinLocalFisico ? "Sin local físico" : local.IdPredial;
        model.NumeroFinca = local.SinLocalFisico ? "No aplica" : local.NumeroFinca;
        model.DireccionLocal = local.Direccion;
        model.Distrito = local.Distrito;
        model.Dueno = local.Propietario;
        model.CondicionOcupacion = local.CondicionOcupacion;
        model.AreaLocal = local.SinLocalFisico ? "No aplica" : $"{local.AreaLocal:0.##} m²";
        model.NombreComercialLocal = local.NombreLocal;
        model.CuentaServiciosMunicipales = local.CuentaServiciosMunicipales;
        model.EstadoGis = local.EstadoGis;

        if (local.SinLocalFisico)
        {
            Snackbar.Add("Actividad sin local físico seleccionada en modo mock.", Severity.Info);
        }

        if (refresh)
            await RefreshWizardStateAsync(persist: false);
    }

    private async Task ApplyActividadSelectionAsync(bool refresh)
    {
        var actividad = actividadesEconomicasMock.FirstOrDefault(x => x.CodigoCaecr == selectedActividadCodigo);
        if (actividad is null)
            return;

        model.ActividadEconomica = actividad.Descripcion;
        model.CodigoCaecr = actividad.CodigoCaecr;
        model.CodigoCiiuVisual = $"CAECR {actividad.CodigoCaecr}";
        model.CategoriaActividad = actividad.Categoria;
        model.ActividadPrincipal = string.IsNullOrWhiteSpace(model.ActividadPrincipal) ? actividad.Descripcion : model.ActividadPrincipal;
        model.RiesgoActividad = ResolveRiskByCategory(actividad.Categoria);
        model.NombreComercial = string.IsNullOrWhiteSpace(model.NombreComercial) ? model.NombreComercialLocal : model.NombreComercial;
        model.ActividadesSecundarias = string.IsNullOrWhiteSpace(model.ActividadesSecundarias) ? activitySecondarySummary(actividad) : model.ActividadesSecundarias;
        model.RequiereLicores = actividad.Categoria.Equals("Licores", StringComparison.OrdinalIgnoreCase) || model.RequiereLicores;
        model.RequierePermisoSanitario = model.RequierePermisoSanitario || actividad.Descripcion.Contains("Restaurantes", StringComparison.OrdinalIgnoreCase);
        model.RequiereInspeccion = model.RequiereInspeccion || actividad.RequiereUsoSuelo || actividad.Categoria.Equals("Temporal", StringComparison.OrdinalIgnoreCase);
        model.ActividadTemporal = actividad.Categoria.Equals("Temporal", StringComparison.OrdinalIgnoreCase) || model.ActividadTemporal;

        if (model.RequierePermisoSanitario)
            model.EstadoPermisoSanitario = "Pendiente";

        if (model.RequiereInspeccion)
            model.EstadoInspeccion = "Solicitada";

        if (model.RequiereLicores)
            Snackbar.Add("Revisión especial de licencias de licores en modo demo.", Severity.Warning);

        if (refresh)
            await RefreshWizardStateAsync(persist: false);
    }

    private static string ResolveRiskByCategory(string categoria)
        => categoria.Equals("Licores", StringComparison.OrdinalIgnoreCase) || categoria.Equals("Extractiva", StringComparison.OrdinalIgnoreCase)
            ? "Alto"
            : categoria.Equals("Industrial", StringComparison.OrdinalIgnoreCase) || categoria.Equals("Temporal", StringComparison.OrdinalIgnoreCase)
                ? "Medio"
                : "Bajo";

    private static string activitySecondarySummary(ActividadEconomicaDto actividad)
        => actividad.Categoria.Equals("Licores", StringComparison.OrdinalIgnoreCase)
            ? "Venta complementaria y control de horario"
            : actividad.Categoria.Equals("Temporal", StringComparison.OrdinalIgnoreCase)
                ? "Operación temporal y permisos asociados"
                : actividad.Categoria.Equals("Ambulante", StringComparison.OrdinalIgnoreCase)
                    ? "Cobertura móvil y atención por ruta"
                    : "Operación comercial demo";

    private async Task ViewRucAsync()
    {
        if (string.IsNullOrWhiteSpace(model.ContribuyenteNombre) || string.IsNullOrWhiteSpace(model.Identificacion))
        {
            Snackbar.Add("Primero seleccione o complete los datos del contribuyente para ver el RUC mock.", Severity.Info);
            return;
        }

        Snackbar.Add($"RUC mock: {model.ContribuyenteNombre} · {model.Identificacion}", Severity.Info);
        await RefreshWizardStateAsync(persist: false);
    }

    private async Task ValidateContribuyenteAsync()
    {
        if (string.IsNullOrWhiteSpace(model.Identificacion) || string.IsNullOrWhiteSpace(model.ContribuyenteNombre))
        {
            Snackbar.Add("Complete identificación y nombre o razón social antes de validar el contribuyente.", Severity.Warning);
            return;
        }

        model.CalidadDatosRuc = "Validado";
        model.EstadoContribuyente = string.IsNullOrWhiteSpace(model.EstadoContribuyente) ? "Activo" : model.EstadoContribuyente;
        Snackbar.Add("Contribuyente validado en modo mock.", Severity.Success);
        await RefreshWizardStateAsync(persist: false);
    }

    private async Task CreateOrConsultContribuyenteAsync()
    {
        Snackbar.Add("Acción mock: apertura simulada para crear o consultar contribuyente sin salir del wizard.", Severity.Info);
        await RefreshWizardStateAsync(persist: false);
    }

    private async Task ViewFincaAsync()
    {
        if (string.IsNullOrWhiteSpace(model.NombreComercialLocal) && string.IsNullOrWhiteSpace(model.FincaOIdPredial))
        {
            Snackbar.Add("Seleccione una finca o local mock para ver su detalle.", Severity.Info);
            return;
        }

        Snackbar.Add($"Finca mock: {model.NombreComercialLocal} · {model.FincaOIdPredial} · {model.Distrito}", Severity.Info);
        await RefreshWizardStateAsync(persist: false);
    }

    private async Task ValidateLocalAsync()
    {
        if (string.IsNullOrWhiteSpace(model.DireccionLocal) && string.IsNullOrWhiteSpace(model.IdPredial))
        {
            Snackbar.Add("Complete la selección del local mock antes de validarlo.", Severity.Warning);
            return;
        }

        Snackbar.Add("Local validado en modo mock.", Severity.Success);
        await RefreshWizardStateAsync(persist: false);
    }

    private async Task OnEstadoUsoSueloChangedAsync()
    {
        SyncUsoSueloState();
        await RefreshWizardStateAsync(persist: false);
    }

    private async Task SimulateUsoSueloValidationAsync()
    {
        if (string.IsNullOrWhiteSpace(model.ActividadEconomica))
        {
            Snackbar.Add("Complete antes la actividad económica para simular el Uso de Suelo.", Severity.Warning);
            return;
        }

        model.NumeroCertificadoUsoSuelo = string.IsNullOrWhiteSpace(model.NumeroCertificadoUsoSuelo)
            ? $"CUS-DEMO-{DateTime.Today:yyyy}-{DateTime.Now:HHmm}"
            : model.NumeroCertificadoUsoSuelo;
        model.CertificadoUsoSuelo = model.NumeroCertificadoUsoSuelo;
        model.FechaValidacionUsoSuelo = DateTime.Today;
        model.FechaVencimientoUsoSuelo ??= DateTime.Today.AddYears(1);
        model.ActividadesAutorizadas = string.IsNullOrWhiteSpace(model.ActividadesAutorizadas)
            ? $"{model.ActividadEconomica}, operación comercial demo"
            : model.ActividadesAutorizadas;
        model.Observaciones = BuildUsoSueloObservation();
        SyncUsoSueloState();

        Snackbar.Add($"Validación mock de Uso de Suelo generada con estado {model.EstadoUsoSuelo}.", model.EstadoUsoSuelo.Equals("Conforme", StringComparison.OrdinalIgnoreCase) ? Severity.Success : Severity.Warning);
        await RefreshWizardStateAsync(persist: false);
    }

    private async Task ViewUsoSueloCertificateAsync()
    {
        var certificado = string.IsNullOrWhiteSpace(model.NumeroCertificadoUsoSuelo) ? "sin número" : model.NumeroCertificadoUsoSuelo;
        Snackbar.Add($"Certificado mock de Uso de Suelo: {certificado} · {model.EstadoUsoSuelo}", Severity.Info);
        await RefreshWizardStateAsync(persist: false);
    }

    private async Task RequestUsoSueloAsync()
    {
        model.EstadoUsoSuelo = "Pendiente";
        model.ResultadoUsoSuelo = "Requiere revisión";
        model.CertificadoUsoSuelo = string.Empty;
        model.NumeroCertificadoUsoSuelo = string.Empty;
        model.FechaValidacionUsoSuelo = DateTime.Today;
        model.FechaVencimientoUsoSuelo = null;
        model.Observaciones = "Solicitud mock de Uso de Suelo creada en modo demo.";
        SyncUsoSueloState();

        Snackbar.Add("Solicitud mock de Uso de Suelo registrada sin salir del wizard.", Severity.Info);
        await RefreshWizardStateAsync(persist: false);
    }

    private void SyncUsoSueloState()
    {
        if (!string.IsNullOrWhiteSpace(model.NumeroCertificadoUsoSuelo))
            model.CertificadoUsoSuelo = model.NumeroCertificadoUsoSuelo;

        var canAdjustWorkflow = currentStatus.Equals("En progreso", StringComparison.OrdinalIgnoreCase)
            || currentStatus.Equals("Pendiente validación", StringComparison.OrdinalIgnoreCase);

        if (model.EstadoUsoSuelo.Equals("Conforme", StringComparison.OrdinalIgnoreCase))
        {
            model.ResultadoUsoSuelo = model.ResultadoUsoSuelo.Equals("No cumple", StringComparison.OrdinalIgnoreCase) ? "Aprobado" : model.ResultadoUsoSuelo;
            model.ResultadoUsoSuelo = string.IsNullOrWhiteSpace(model.ResultadoUsoSuelo) || model.ResultadoUsoSuelo.Equals("Requiere revisión", StringComparison.OrdinalIgnoreCase)
                ? "Aprobado"
                : model.ResultadoUsoSuelo;
            model.CompatibilidadUsoSuelo = string.IsNullOrWhiteSpace(model.CompatibilidadUsoSuelo) ? "Compatible" : model.CompatibilidadUsoSuelo;
            model.EstadoUsoSueloChecklist = "Cumple";
            if (model.EstadoResolucion.Equals("Pendiente validación", StringComparison.OrdinalIgnoreCase))
                model.EstadoResolucion = "Preparada";
            if (canAdjustWorkflow && (model.EstadoFinalExpediente.Equals("Pendiente validación", StringComparison.OrdinalIgnoreCase) || model.EstadoFinalExpediente.Equals("Requiere revisión", StringComparison.OrdinalIgnoreCase)))
                model.EstadoFinalExpediente = "Borrador";
        }
        else if (model.EstadoUsoSuelo.Equals("Pendiente", StringComparison.OrdinalIgnoreCase))
        {
            model.ResultadoUsoSuelo = "Requiere revisión";
            model.EstadoUsoSueloChecklist = "Pendiente";
            model.EstadoResolucion = "Pendiente validación";
            if (canAdjustWorkflow)
                model.EstadoFinalExpediente = "Pendiente validación";
            model.EstadoRevision = "Pendiente";
        }
        else if (model.EstadoUsoSuelo.Equals("No conforme", StringComparison.OrdinalIgnoreCase))
        {
            model.ResultadoUsoSuelo = "No cumple";
            model.CompatibilidadUsoSuelo = string.IsNullOrWhiteSpace(model.CompatibilidadUsoSuelo) ? "Incompatible" : model.CompatibilidadUsoSuelo;
            model.EstadoUsoSueloChecklist = "Pendiente";
            model.EstadoResolucion = "Pendiente validación";
            if (canAdjustWorkflow)
                model.EstadoFinalExpediente = "Requiere revisión";
            model.EstadoRevision = "Observado";
        }
        else if (model.EstadoUsoSuelo.Equals("Vencido", StringComparison.OrdinalIgnoreCase))
        {
            model.ResultadoUsoSuelo = "Requiere revisión";
            model.EstadoUsoSueloChecklist = "Pendiente";
            model.EstadoResolucion = "Pendiente validación";
            if (canAdjustWorkflow)
                model.EstadoFinalExpediente = "Requiere revisión";
            model.EstadoRevision = "Pendiente";
        }
    }

    private string BuildUsoSueloObservation()
    {
        if (model.EstadoUsoSuelo.Equals("Conforme", StringComparison.OrdinalIgnoreCase))
            return "Validación mock conforme para otorgamiento demo sujeto a revisión final.";

        if (model.EstadoUsoSuelo.Equals("No conforme", StringComparison.OrdinalIgnoreCase))
            return "El Uso de Suelo mock no es compatible con la actividad solicitada.";

        if (model.EstadoUsoSuelo.Equals("Vencido", StringComparison.OrdinalIgnoreCase))
            return "El certificado mock está vencido y requiere actualización.";

        return "La validación mock de Uso de Suelo queda pendiente de análisis técnico.";
    }

    private async Task OnRequierePermisoSanitarioChangedAsync()
    {
        model.EstadoPermisoSanitario = model.RequierePermisoSanitario ? "Pendiente" : "Cumple";
        model.EstadoRequisitos = model.RequierePermisoSanitario ? "Pendiente" : "Cumple";
        await RefreshWizardStateAsync(persist: false);
    }

    private async Task OnRequiereInspeccionChangedAsync()
    {
        model.EstadoInspeccion = model.RequiereInspeccion ? "Solicitada" : "Conforme";
        model.SolicitarInspeccion = false;
        await RefreshWizardStateAsync(persist: false);
    }

    private async Task GoNextAsync()
    {
        var result = ValidateCurrentStep();
        if (!result.IsValid)
        {
            currentStatus = "Pendiente validación";
            await UpdateSummaryAsync();
            Snackbar.Add("Complete las validaciones básicas del paso actual para continuar.", Severity.Warning);
            return;
        }

        if (currentStepIndex < wizardSteps.Count - 1)
        {
            currentStepIndex++;
            currentStatus = currentStepIndex == 0 ? "Borrador" : "En progreso";
            await UpdateSummaryAsync();
        }
    }

    private async Task GoBackAsync()
    {
        if (currentStepIndex <= 0)
            return;

        currentStepIndex--;
        currentStatus = currentStepIndex == 0 ? "Borrador" : "En progreso";
        await UpdateSummaryAsync();
    }

    private async Task SaveDraftAsync()
    {
        DraftStore[ProcessKey] = CloneModel(model);
        currentStatus = "Borrador";
        model.EstadoFinalExpediente = "Borrador";
        await UpdateSummaryAsync(saveDraft: true);
        Snackbar.Add("Borrador de Patente guardado correctamente.", Severity.Success);
    }

    private async Task CancelAsync()
    {
        currentStatus = "Cancelado";
        model.EstadoFinalExpediente = "Cancelado";
        await UpdateSummaryAsync();
        DraftStore.TryRemove(ProcessKey, out _);
        await wizardStateService.ClearStateAsync(ProcessKey);
        await OnCancelled.InvokeAsync();
        Snackbar.Add("Solicitud de patente cancelada.", Severity.Info);
    }

    private async Task FinalizeAsync()
    {
        if (currentStepIndex != wizardSteps.Count - 1)
        {
            currentStatus = "Pendiente validación";
            await UpdateSummaryAsync();
            Snackbar.Add("Avance hasta el último paso para finalizar la solicitud.", Severity.Warning);
            return;
        }

        if (!CanFinalizeWizard())
        {
            currentStatus = "Pendiente validación";
            await UpdateSummaryAsync();
            Snackbar.Add("Existen validaciones pendientes antes de finalizar la solicitud.", Severity.Warning);
            return;
        }

        ApplyFinalizationOutcome();
        currentStatus = "Completado";
        var solicitud = BuildSolicitudDemo();
        var licencia = await PatentesService.SaveSolicitudAsync(solicitud);
        solicitud.LicenciaId = licencia.Id;

        await AuditoriaService.RegistrarCambioAsync(model.ContribuyenteId ?? 0, new AuditoriaCambioDto
        {
            Usuario = "analista.patentes",
            FechaHora = DateTime.Now,
            CampoModificado = "Solicitud de patente demo",
            ValorAnterior = "Borrador",
            ValorNuevo = $"{solicitud.NumeroSolicitud} · {model.EstadoFinalExpediente}",
            Origen = "Wizard Patentes"
        });

        await UpdateSummaryAsync();
        DraftStore.TryRemove(ProcessKey, out _);
        await wizardStateService.ClearStateAsync(ProcessKey);
        await OnCompleted.InvokeAsync(solicitud);
        Snackbar.Add("Solicitud de patente registrada en modo demo.", Severity.Success);
        await NavigateAfterFinalizationAsync(solicitud);
    }

    private async Task RefreshWizardStateAsync(bool saveDraft = false, bool persist = true)
    {
        SyncLegacyWizardState();
        var validationResults = BuildValidationResults();
        RegisterValidationMessages(validationResults);
        var pendingValidations = validationResults
            .Where(x => (x.StepIndex <= currentStepIndex || currentStatus == "Pendiente validación" || currentStatus == "Completado") && (!x.IsValid || x.HasWarnings))
            .SelectMany(x => x.Messages)
            .Distinct()
            .ToList();

        var steps = wizardSteps.Select(step =>
        {
            var result = validationResults.First(x => x.StepIndex == step.Index);
            var isVisited = step.Index <= currentStepIndex || currentStatus == "Pendiente validación" || currentStatus == "Completado";
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
            ProcessKey = ProcessKey,
            ProcessTitle = "Nueva solicitud de patente",
            Status = currentStatus,
            CurrentStepIndex = currentStepIndex,
            TotalSteps = steps.Count,
            ProgressPercent = steps.Count == 0 ? 0 : Math.Round((double)(currentStepIndex + 1) / steps.Count * 100d, 0),
            LastSavedAt = wizardState.LastSavedAt,
            Steps = steps,
            ValidationResults = validationResults,
            SummaryItems = BuildSummaryItems(),
            PendingValidations = pendingValidations
        };

        if (!persist)
            return;

        wizardState = saveDraft
            ? await wizardStateService.SaveDraftAsync(wizardState)
            : await wizardStateService.UpdateStateAsync(wizardState);
    }
}
