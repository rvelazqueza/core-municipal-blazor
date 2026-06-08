namespace BlazorApp.Models;

public class PatApplicantDto
{
    public int Id { get; set; }
    public string Identificacion { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string TipoPersona { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string DireccionFiscal { get; set; } = string.Empty;
    public string MedioNotificacion { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string CalidadDatosRuc { get; set; } = string.Empty;
}
