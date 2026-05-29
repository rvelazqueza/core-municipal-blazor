using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using BlazorApp.Models;

namespace BlazorApp.Services;

public class AuthMockService : IAuthService
{
    private static AuthUserDto? currentUser = new()
    {
        NombreCompleto = string.Empty,
        Rol = string.Empty,
        Correo = string.Empty,
        Iniciales = "U",
        IsAuthenticated = false
    };

    private static string currentPassword = "Admin123!Demo";
    private static readonly List<string> passwordHistory = new() { "Municipio2023!*", "Municipio2024!*", "Admin123!Demo" };
    private static int failedAttempts;
    private static bool pendingTwoFactor;
    private static string pendingUser = string.Empty;

    public Task<AuthUserDto?> GetCurrentUserAsync()
    {
        return Task.FromResult(currentUser is null ? null : CloneUser(currentUser));
    }

    public Task<int> GetFailedAttemptsAsync()
    {
        return Task.FromResult(failedAttempts);
    }

    public Task<AuthResultDto> LoginAsync(LoginRequestDto request)
    {
        if (IsLocked())
        {
            return Task.FromResult(new AuthResultDto
            {
                IsLocked = true,
                Message = "El acceso se encuentra temporalmente bloqueado por seguridad. Contacte al administrador institucional."
            });
        }

        var userName = request.UsuarioOCorreo?.Trim() ?? string.Empty;
        var password = request.Contrasena?.Trim() ?? string.Empty;
        var validUser = string.Equals(userName, "admin", StringComparison.OrdinalIgnoreCase)
                        || string.Equals(userName, "admin@municipal.go.cr", StringComparison.OrdinalIgnoreCase)
                        || string.Equals(userName, "analista", StringComparison.OrdinalIgnoreCase)
                        || string.Equals(userName, "analista.municipal@core.local", StringComparison.OrdinalIgnoreCase);

        if (!validUser || password != currentPassword)
        {
            failedAttempts++;
            return Task.FromResult(new AuthResultDto
            {
                IsLocked = IsLocked(),
                Message = IsLocked()
                    ? "Se alcanzó el máximo de intentos permitidos. El acceso visual ha sido bloqueado."
                    : "No fue posible validar sus credenciales. Verifique los datos e intente nuevamente."
            });
        }

        if (request.SolicitarTwoFactor)
        {
            pendingTwoFactor = true;
            pendingUser = userName;
            return Task.FromResult(new AuthResultDto
            {
                RequiresTwoFactor = true,
                Message = "Se envió un código temporal al canal seguro configurado para esta cuenta."
            });
        }

        failedAttempts = 0;
        pendingTwoFactor = false;
        pendingUser = string.Empty;
        currentUser = BuildDefaultUser();

        return Task.FromResult(new AuthResultDto
        {
            IsSuccess = true,
            Message = "Acceso validado correctamente. Bienvenido al Core Municipal Inteligente.",
            User = CloneUser(currentUser)
        });
    }

    public Task<AuthResultDto> LoginWithDigitalSignatureAsync()
    {
        if (IsLocked())
        {
            return Task.FromResult(new AuthResultDto
            {
                IsLocked = true,
                Message = "El acceso se encuentra bloqueado temporalmente por seguridad."
            });
        }

        failedAttempts = 0;
        pendingTwoFactor = false;
        pendingUser = string.Empty;
        currentUser = new AuthUserDto
        {
            NombreCompleto = "Firma Digital Institucional",
            Rol = "Operador Certificado",
            Correo = "firma.digital@core.local",
            Iniciales = "FD",
            IsAuthenticated = true
        };

        return Task.FromResult(new AuthResultDto
        {
            IsSuccess = true,
            Message = "Acceso alternativo validado mediante firma digital mock.",
            User = CloneUser(currentUser)
        });
    }

    public Task<AuthResultDto> VerifyTwoFactorAsync(TwoFactorRequestDto request)
    {
        if (IsLocked())
        {
            return Task.FromResult(new AuthResultDto
            {
                IsLocked = true,
                Message = "El acceso se encuentra bloqueado temporalmente por seguridad."
            });
        }

        if (!pendingTwoFactor || string.IsNullOrWhiteSpace(pendingUser))
        {
            return Task.FromResult(new AuthResultDto
            {
                Message = "No existe una verificación 2FA pendiente para la sesión actual."
            });
        }

        if (!string.Equals(request.Codigo?.Trim(), "123456", StringComparison.Ordinal))
        {
            failedAttempts++;
            return Task.FromResult(new AuthResultDto
            {
                IsLocked = IsLocked(),
                Message = IsLocked()
                    ? "Se alcanzó el máximo de intentos permitidos. El acceso visual ha sido bloqueado."
                    : "El código temporal no es válido o expiró. Solicite un nuevo intento seguro."
            });
        }

        failedAttempts = 0;
        pendingTwoFactor = false;
        currentUser = BuildDefaultUser();
        pendingUser = string.Empty;

        return Task.FromResult(new AuthResultDto
        {
            IsSuccess = true,
            Message = "Verificación 2FA completada satisfactoriamente.",
            User = CloneUser(currentUser)
        });
    }

    public Task<AuthResultDto> ChangePasswordAsync(ChangePasswordRequestDto request)
    {
        if (currentUser is null || !currentUser.IsAuthenticated)
        {
            return Task.FromResult(new AuthResultDto
            {
                Message = "Debe contar con una sesión visual activa para actualizar la contraseña."
            });
        }

        if (request.ContrasenaActual != currentPassword)
        {
            return Task.FromResult(new AuthResultDto
            {
                Message = "La contraseña actual no coincide con los registros simulados del entorno."
            });
        }

        var validationMessage = ValidatePasswordPolicy(request.NuevaContrasena, request.ConfirmarNuevaContrasena, currentUser);
        if (!string.IsNullOrWhiteSpace(validationMessage))
        {
            return Task.FromResult(new AuthResultDto
            {
                Message = validationMessage
            });
        }

        currentPassword = request.NuevaContrasena;
        passwordHistory.Add(currentPassword);
        if (passwordHistory.Count > 5)
        {
            passwordHistory.RemoveAt(0);
        }

        return Task.FromResult(new AuthResultDto
        {
            IsSuccess = true,
            Message = "La contraseña fue actualizada correctamente en el entorno visual mock."
        });
    }

    public Task LogoutAsync()
    {
        currentUser = new AuthUserDto
        {
            NombreCompleto = string.Empty,
            Rol = string.Empty,
            Correo = string.Empty,
            Iniciales = "U",
            IsAuthenticated = false
        };

        pendingTwoFactor = false;
        pendingUser = string.Empty;
        return Task.CompletedTask;
    }

    private static bool IsLocked()
    {
        var maxAttempts = LoginSettingsMockService.GetMaxFailedAttempts();
        return failedAttempts >= maxAttempts;
    }

    private static string ValidatePasswordPolicy(string password, string confirmation, AuthUserDto user)
    {
        if (string.IsNullOrWhiteSpace(password) || password.Length < 12)
            return "La nueva contraseña debe tener al menos 12 caracteres.";

        if (!Regex.IsMatch(password, "[A-Z]"))
            return "La nueva contraseña debe incluir al menos una letra mayúscula.";

        if (!Regex.IsMatch(password, "[a-z]"))
            return "La nueva contraseña debe incluir al menos una letra minúscula.";

        if (!Regex.IsMatch(password, "[0-9]"))
            return "La nueva contraseña debe incluir al menos un número.";

        if (!Regex.IsMatch(password, "[^a-zA-Z0-9]"))
            return "La nueva contraseña debe incluir al menos un carácter especial.";

        if (!string.Equals(password, confirmation, StringComparison.Ordinal))
            return "La confirmación de contraseña no coincide.";

        if (passwordHistory.Any(x => string.Equals(x, password, StringComparison.Ordinal)))
            return "La nueva contraseña no puede repetir contraseñas anteriores.";

        var normalizedName = (user.NombreCompleto ?? string.Empty).Replace(" ", string.Empty, StringComparison.OrdinalIgnoreCase);
        var normalizedEmail = (user.Correo ?? string.Empty).Split('@').FirstOrDefault() ?? string.Empty;

        if (!string.IsNullOrWhiteSpace(normalizedName) && password.Contains(normalizedName, StringComparison.OrdinalIgnoreCase))
            return "La nueva contraseña no debe usar datos personales del usuario.";

        if (!string.IsNullOrWhiteSpace(normalizedEmail) && password.Contains(normalizedEmail, StringComparison.OrdinalIgnoreCase))
            return "La nueva contraseña no debe usar datos personales del usuario.";

        return string.Empty;
    }

    private static AuthUserDto BuildDefaultUser()
    {
        return new AuthUserDto
        {
            NombreCompleto = "Administrador Municipal",
            Rol = "Administrador Tributario",
            Correo = "admin@municipal.go.cr",
            Iniciales = "AM",
            IsAuthenticated = true
        };
    }

    private static AuthUserDto CloneUser(AuthUserDto source)
    {
        return new AuthUserDto
        {
            NombreCompleto = source.NombreCompleto,
            Rol = source.Rol,
            Correo = source.Correo,
            Iniciales = source.Iniciales,
            IsAuthenticated = source.IsAuthenticated
        };
    }
}
