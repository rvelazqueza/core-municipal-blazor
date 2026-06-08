namespace BlazorApp.Models;

public class PatTemporaryLicenseDto
{
    public string NumeroSolicitud { get; set; } = string.Empty;
    public string TipoEvento { get; set; } = string.Empty;
    public DateTime FechaInicio { get; set; } = DateTime.Today;
    public DateTime FechaFin { get; set; } = DateTime.Today;
    public string Lugar { get; set; } = string.Empty;
    public string Actividad { get; set; } = string.Empty;
    public string Responsable { get; set; } = string.Empty;
    public bool IncluyeBebidasAlcoholicas { get; set; }
    public bool ViaPublica { get; set; }
    public string EstadoResolucion { get; set; } = string.Empty;
    public decimal CobroUnico { get; set; }
}
