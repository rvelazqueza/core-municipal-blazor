namespace BlazorApp.Models;

public class LoginRequestDto
{
    public string UsuarioOCorreo { get; set; } = string.Empty;
    public string Contrasena { get; set; } = string.Empty;
    public bool Recordarme { get; set; }
    public bool SolicitarTwoFactor { get; set; }
}
