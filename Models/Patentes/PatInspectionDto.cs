namespace BlazorApp.Models;

public class PatInspectionDto
{
    public string NumeroInspeccion { get; set; } = string.Empty;
    public string NumeroExpediente { get; set; } = string.Empty;
    public string NumeroLicencia { get; set; } = string.Empty;
    public string Origen { get; set; } = string.Empty;
    public DateTime FechaSolicitud { get; set; } = DateTime.Today;
    public DateTime? FechaInspeccion { get; set; }
    public string InspectorAsignado { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public bool InspeccionEfectiva { get; set; }
    public string Resultado { get; set; } = string.Empty;
}
