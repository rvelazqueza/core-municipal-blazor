namespace BlazorApp.Models;

public class PatResolutionDto
{
    public int LicenciaId { get; set; }
    public string NumeroLicencia { get; set; } = string.Empty;
    public string NumeroResolucion { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string Resultado { get; set; } = string.Empty;
    public DateTime FechaResolucion { get; set; } = DateTime.Today;
    public string Firmante { get; set; } = string.Empty;
    public bool NotificacionSimulada { get; set; }
    public string Observaciones { get; set; } = string.Empty;
}
