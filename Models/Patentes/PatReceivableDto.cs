namespace BlazorApp.Models;

public class PatReceivableDto
{
    public string NumeroContribuyente { get; set; } = string.Empty;
    public string NumeroLicencia { get; set; } = string.Empty;
    public string Trimestre { get; set; } = string.Empty;
    public string PeriodoFiscal { get; set; } = string.Empty;
    public string Subtributo { get; set; } = string.Empty;
    public decimal MontoImpuesto { get; set; }
    public decimal MontoTimbre { get; set; }
    public decimal MontoPublicidadExterior { get; set; }
    public DateTime FechaInicio { get; set; } = DateTime.Today;
    public DateTime FechaVencimiento { get; set; } = DateTime.Today;
    public string Estado { get; set; } = string.Empty;
}
