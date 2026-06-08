namespace BlazorApp.Models;

public class PatComplianceRecordDto
{
    public int LicenciaId { get; set; }
    public string NumeroLicencia { get; set; } = string.Empty;
    public DateTime FechaInicioActividad { get; set; } = DateTime.Today;
    public string TipoRegimen { get; set; } = string.Empty;
    public string TipoPeriodoFiscal { get; set; } = string.Empty;
    public bool ActividadesEnOtrosCantones { get; set; }
    public string EstadoHacienda { get; set; } = string.Empty;
    public string EstadoCcss { get; set; } = string.Empty;
    public string EstadoFodesaf { get; set; } = string.Empty;
    public string EstadoIns { get; set; } = string.Empty;
    public string EstadoRevision { get; set; } = string.Empty;
}
