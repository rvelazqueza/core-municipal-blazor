namespace BlazorApp.Models;

public class PatLiquorLicenseDto
{
    public string NumeroLicenciaLicores { get; set; } = string.Empty;
    public string NumeroLicenciaComercial { get; set; } = string.Empty;
    public string NumeroExpediente { get; set; } = string.Empty;
    public string NombreComercial { get; set; } = string.Empty;
    public string Patentado { get; set; } = string.Empty;
    public string Categoria { get; set; } = string.Empty;
    public string Subcategoria { get; set; } = string.Empty;
    public string Distrito { get; set; } = string.Empty;
    public DateTime FechaAprobacion { get; set; } = DateTime.Today;
    public DateTime FechaVencimiento { get; set; } = DateTime.Today;
    public string EstadoRenovacion { get; set; } = string.Empty;
    public string EstadoVigencia { get; set; } = string.Empty;
}
