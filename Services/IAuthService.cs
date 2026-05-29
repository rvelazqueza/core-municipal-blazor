using System.Threading.Tasks;
using BlazorApp.Models;

namespace BlazorApp.Services;

public interface IAuthService
{
    Task<AuthUserDto?> GetCurrentUserAsync();
    Task<AuthResultDto> LoginAsync(LoginRequestDto request);
    Task<AuthResultDto> LoginWithDigitalSignatureAsync();
    Task<AuthResultDto> VerifyTwoFactorAsync(TwoFactorRequestDto request);
    Task<AuthResultDto> ChangePasswordAsync(ChangePasswordRequestDto request);
    Task LogoutAsync();
    Task<int> GetFailedAttemptsAsync();
}
