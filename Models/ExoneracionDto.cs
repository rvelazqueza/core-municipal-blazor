namespace BlazorApp.Models;

public class ExoneracionDto
{
    public string TipoExoneracion { get; set; } = string.Empty;
    public decimal Porcentaje { get; set; }
    public DateTime FechaInicio { get; set; } = DateTime.Today;
    public DateTime FechaFin { get; set; } = DateTime.Today;
    public string Estado { get; set; } = "Activa";
    public string Institucion { get; set; } = string.Empty;
}
