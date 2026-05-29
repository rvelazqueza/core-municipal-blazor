namespace BlazorApp.Models;

public class DeclaracionBienDto
{
    public string Periodo { get; set; } = string.Empty;
    public string Estado { get; set; } = "Pendiente";
    public DateTime? FechaPresentacion { get; set; }
    public string MedioRecepcion { get; set; } = string.Empty;
    public string Observacion { get; set; } = string.Empty;
}
