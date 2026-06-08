namespace BlazorApp.Models;

public class PatShareCapitalDeclarationDto
{
    public string NumeroDeclaracion { get; set; } = string.Empty;
    public string NumeroLicencia { get; set; } = string.Empty;
    public string TipoDeclaracion { get; set; } = string.Empty;
    public DateTime FechaPresentacion { get; set; } = DateTime.Today;
    public string Estado { get; set; } = string.Empty;
    public DateTime VigenciaHasta { get; set; } = DateTime.Today;
    public bool AlertaDebidoProceso { get; set; }
    public int TotalAcciones { get; set; }
}
