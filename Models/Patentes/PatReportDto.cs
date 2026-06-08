namespace BlazorApp.Models;

public class PatReportDto
{
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public List<string> FiltrosPrincipales { get; set; } = new();
    public DateTime? UltimaGeneracion { get; set; }
}
