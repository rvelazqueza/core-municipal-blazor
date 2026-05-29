namespace BlazorApp.Models;

public class ChangePasswordRequestDto
{
    public string ContrasenaActual { get; set; } = string.Empty;
    public string NuevaContrasena { get; set; } = string.Empty;
    public string ConfirmarNuevaContrasena { get; set; } = string.Empty;
}
