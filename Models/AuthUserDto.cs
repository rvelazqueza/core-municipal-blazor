namespace BlazorApp.Models;

public class AuthUserDto
{
    public string NombreCompleto { get; set; } = string.Empty;
    public string Rol { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string Iniciales { get; set; } = string.Empty;
    public bool IsAuthenticated { get; set; }
}
