namespace BlazorApp.Models;

public class AuthResultDto
{
    public bool IsSuccess { get; set; }
    public bool RequiresTwoFactor { get; set; }
    public bool IsLocked { get; set; }
    public string Message { get; set; } = string.Empty;
    public AuthUserDto? User { get; set; }
}
