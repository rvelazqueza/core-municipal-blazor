namespace BlazorApp.Models;

public class PatExemptionDto
{
    public string NumeroLicencia { get; set; } = string.Empty;
    public string TipoExoneracion { get; set; } = string.Empty;
    public string Origen { get; set; } = string.Empty;
    public string OrganizacionBeneficiaria { get; set; } = string.Empty;
    public bool EsTotal { get; set; }
    public decimal Porcentaje { get; set; }
    public decimal MontoExonerado { get; set; }
    public DateTime FechaInicio { get; set; } = DateTime.Today;
    public DateTime FechaFin { get; set; } = DateTime.Today;
    public string Estado { get; set; } = string.Empty;
}
