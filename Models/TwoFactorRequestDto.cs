namespace BlazorApp.Models;

public class TwoFactorRequestDto
{
    public string UsuarioOCorreo { get; set; } = string.Empty;
    public string Codigo { get; set; } = string.Empty;
}
