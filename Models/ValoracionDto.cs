namespace BlazorApp.Models;

public class ValoracionDto
{
    public string Periodo { get; set; } = string.Empty;
    public string ZonaHomogenea { get; set; } = string.Empty;
    public string TipologiaConstructiva { get; set; } = string.Empty;
    public decimal ValorTerreno { get; set; }
    public decimal ValorConstruccion { get; set; }
    public decimal ValorFiscalTotal { get; set; }
    public DateTime FechaValoracion { get; set; } = DateTime.Today;
    public bool Actualizado { get; set; }
}
