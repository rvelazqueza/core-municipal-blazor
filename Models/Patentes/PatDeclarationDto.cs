namespace BlazorApp.Models;

public class PatDeclarationDto
{
    public string NumeroDeclaracion { get; set; } = string.Empty;
    public string NumeroLicencia { get; set; } = string.Empty;
    public DateTime FechaPresentacion { get; set; } = DateTime.Today;
    public string Canal { get; set; } = string.Empty;
    public string PeriodoFiscal { get; set; } = string.Empty;
    public string TipoDeclaracion { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public decimal IngresosBrutos { get; set; }
    public decimal Gastos { get; set; }
    public decimal Deducciones { get; set; }
    public decimal MontoImpuesto { get; set; }
    public decimal TimbreBiodiversidad { get; set; }
    public decimal Multa { get; set; }
}
