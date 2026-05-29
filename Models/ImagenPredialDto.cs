namespace BlazorApp.Models;

public class ImagenPredialDto
{
    public string Titulo { get; set; } = string.Empty;
    public string Tipo { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string Fuente { get; set; } = string.Empty;
    public DateTime FechaRegistro { get; set; } = DateTime.Today;
}
