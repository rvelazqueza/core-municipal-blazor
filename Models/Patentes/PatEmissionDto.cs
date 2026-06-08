namespace BlazorApp.Models;

public class PatEmissionDto
{
    public string TipoEmision { get; set; } = string.Empty;
    public string PeriodoFiscal { get; set; } = string.Empty;
    public string PeriodoEmision { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public int CantidadLicencias { get; set; }
    public decimal MontoTotal { get; set; }
    public decimal TimbreBiodiversidad { get; set; }
    public decimal Multas { get; set; }
    public decimal PublicidadExterior { get; set; }
    public int Inconsistencias { get; set; }
    public string ControlCalidad { get; set; } = string.Empty;
}
