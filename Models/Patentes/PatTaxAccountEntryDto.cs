namespace BlazorApp.Models;

public class PatTaxAccountEntryDto
{
    public string NumeroLicencia { get; set; } = string.Empty;
    public string Identificacion { get; set; } = string.Empty;
    public string Trimestre { get; set; } = string.Empty;
    public string PeriodoFiscal { get; set; } = string.Empty;
    public string CodigoTributario { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string CodigoSubtributo { get; set; } = string.Empty;
    public string TipoMovimiento { get; set; } = string.Empty;
    public decimal Monto { get; set; }
    public string Estado { get; set; } = string.Empty;
    public DateTime FechaCreacion { get; set; } = DateTime.Today;
}
