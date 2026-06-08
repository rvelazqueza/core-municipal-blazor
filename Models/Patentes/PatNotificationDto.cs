namespace BlazorApp.Models;

public class PatNotificationDto
{
    public string NumeroLicencia { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public string Medio { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public DateTime Fecha { get; set; } = DateTime.Today;
    public string Resultado { get; set; } = string.Empty;
    public string Destinatario { get; set; } = string.Empty;
}
