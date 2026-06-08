namespace BlazorApp.Models;

public class PatIntegrationStatusDto
{
    public string NombreIntegracion { get; set; } = string.Empty;
    public string ModuloRelacionado { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string UltimoEvento { get; set; } = string.Empty;
    public string AccionVisual { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
}
