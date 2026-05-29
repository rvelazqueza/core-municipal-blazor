namespace BlazorApp.Models;

public class UsoSueloVinculadoDto
{
    public string NumeroCertificado { get; set; } = string.Empty;
    public string Estado { get; set; } = "Pendiente";
    public bool EsConforme { get; set; }
    public DateTime FechaValidacion { get; set; } = DateTime.Today;
    public string Observaciones { get; set; } = string.Empty;
    public string Fuente { get; set; } = string.Empty;
}
