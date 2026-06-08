namespace BlazorApp.Models;

public class PatAdministrativeProcessDto
{
    public string NumeroOrganoDirector { get; set; } = string.Empty;
    public string NumeroLicencia { get; set; } = string.Empty;
    public DateTime FechaEmision { get; set; } = DateTime.Today;
    public DateTime FechaAudiencia { get; set; } = DateTime.Today;
    public string Funcionario { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string Resolucion { get; set; } = string.Empty;
    public bool AlertaVencimiento { get; set; }
}
