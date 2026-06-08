namespace BlazorApp.Models;

public class PatentesModuleSnapshotDto
{
    public List<LicenciaComercialDto> LicenciasComerciales { get; set; } = new();
    public List<SolicitudPatenteDto> Solicitudes { get; set; } = new();
    public List<PatApplicantDto> Contribuyentes { get; set; } = new();
    public List<PatBusinessLocationDto> Locales { get; set; } = new();
    public List<ActividadEconomicaDto> ActividadesEconomicas { get; set; } = new();
    public List<UsoSueloVinculadoDto> ValidacionesUsoSuelo { get; set; } = new();
    public List<RequisitoPatenteDto> Requisitos { get; set; } = new();
    public List<PatComplianceRecordDto> HaciendaSeguridadSocial { get; set; } = new();
    public List<PatMorosityValidationDto> Morosidad { get; set; } = new();
    public List<PatAssessmentDto> Tasaciones { get; set; } = new();
    public List<PatResolutionDto> Resoluciones { get; set; } = new();
    public List<PatReceivableDto> CuentasPorCobrar { get; set; } = new();
    public List<PatPaymentDto> Pagos { get; set; } = new();
    public List<PatTaxAccountEntryDto> CuentaTributaria { get; set; } = new();
    public List<PatDeclarationDto> Declaraciones { get; set; } = new();
    public List<PatExemptionDto> Exoneraciones { get; set; } = new();
    public List<PatEmissionDto> Emisiones { get; set; } = new();
    public List<MovimientoPatenteDto> Movimientos { get; set; } = new();
    public List<PatTemporaryLicenseDto> LicenciasTemporales { get; set; } = new();
    public List<PatLiquorLicenseDto> LicenciasLicores { get; set; } = new();
    public List<PatShareCapitalDeclarationDto> CapitalAccionario { get; set; } = new();
    public List<PatInspectionDto> Inspecciones { get; set; } = new();
    public List<PatInfractionDto> Infracciones { get; set; } = new();
    public List<PatAdministrativeProcessDto> ProcesosAdministrativos { get; set; } = new();
    public List<PatComplaintDto> Denuncias { get; set; } = new();
    public List<PatNotificationDto> Notificaciones { get; set; } = new();
    public List<PatHistoryEntryDto> Historial { get; set; } = new();
    public List<PatAuditEventDto> Auditoria { get; set; } = new();
    public List<PatQualityCaseDto> ControlCalidad { get; set; } = new();
    public List<PatReportDto> Reportes { get; set; } = new();
    public List<PatCatalogItemDto> Catalogos { get; set; } = new();
    public List<PatIntegrationStatusDto> Integraciones { get; set; } = new();
}
