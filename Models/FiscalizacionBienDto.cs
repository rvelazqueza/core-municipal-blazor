namespace BlazorApp.Models;

public class FiscalizacionBienDto
{
    public string NumeroCaso { get; set; } = string.Empty;
    public string Inspector { get; set; } = string.Empty;
    public string Estado { get; set; } = "Programada";
    public string Resultado { get; set; } = string.Empty;
    public DateTime FechaProgramada { get; set; } = DateTime.Today;
    public DateTime? FechaCierre { get; set; }
    public string Hallazgos { get; set; } = string.Empty;
}
