using System.Security.Claims;
using BlazorApp.Services;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazorApp.Services;

public class MockAuthenticationStateProvider(IAuthService authService) : AuthenticationStateProvider
{
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var user = await authService.GetCurrentUserAsync();
        if (user?.IsAuthenticated != true)
        {
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
        }

        var identity = new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.Name, user.NombreCompleto),
            new Claim(ClaimTypes.Email, user.Correo),
            new Claim(ClaimTypes.Role, user.Rol)
        }, "MockAuthentication");

        return new AuthenticationState(new ClaimsPrincipal(identity));
    }

    public void NotifyAuthenticationChanged()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}
