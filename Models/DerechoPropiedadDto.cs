namespace BlazorApp.Models;

public class DerechoPropiedadDto
{
    public int ContribuyenteId { get; set; }
    public string Titular { get; set; } = string.Empty;
    public string Identificacion { get; set; } = string.Empty;
    public string TipoDerecho { get; set; } = "Pleno dominio";
    public decimal Porcentaje { get; set; }
    public string Estado { get; set; } = "Activo";
    public DateTime FechaInicio { get; set; } = DateTime.Today;
}
