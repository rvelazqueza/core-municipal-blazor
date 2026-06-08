namespace BlazorApp.Models;

public class PatAssessmentDto
{
    public int LicenciaId { get; set; }
    public string NumeroLicencia { get; set; } = string.Empty;
    public string TipoTasacion { get; set; } = string.Empty;
    public decimal MontoAnual { get; set; }
    public decimal MontoTrimestral { get; set; }
    public decimal TimbreBiodiversidadAnual { get; set; }
    public decimal PublicidadExteriorAnual { get; set; }
    public decimal Multa { get; set; }
    public decimal Intereses { get; set; }
    public DateTime FechaInicioCobro { get; set; } = DateTime.Today;
    public string Estado { get; set; } = string.Empty;
}
