namespace BlazorApp.Models;

public class PatPaymentDto
{
    public string NumeroLicencia { get; set; } = string.Empty;
    public string Identificacion { get; set; } = string.Empty;
    public string NombreContribuyente { get; set; } = string.Empty;
    public string EntidadRecaudadora { get; set; } = string.Empty;
    public string NumeroFactura { get; set; } = string.Empty;
    public DateTime FechaPago { get; set; } = DateTime.Today;
    public string Trimestre { get; set; } = string.Empty;
    public string PeriodoFiscal { get; set; } = string.Empty;
    public decimal MontoImpuestoCancelado { get; set; }
    public decimal MontoTimbre { get; set; }
    public decimal MontoPublicidadExterior { get; set; }
    public decimal MontoIntereses { get; set; }
    public decimal MontoMultas { get; set; }
    public string Estado { get; set; } = string.Empty;
}
