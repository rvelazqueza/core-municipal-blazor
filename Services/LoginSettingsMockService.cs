using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorApp.Models;
using MudBlazor;

namespace BlazorApp.Services;

public class LoginSettingsMockService : ILoginSettingsService
{
    private static LoginSettingsDto settings = new()
    {
        ImagenBienvenidaUrl = "https://placehold.co/1200x1600/0b2440/e6f4ff?text=Core+Municipal+Inteligente",
        HeroImageUrl = "https://placehold.co/1200x1600/0b2440/e6f4ff?text=Core+Municipal+Inteligente",
        Titulo = "Core Municipal Inteligente",
        HeroTitle = "Core Municipal Inteligente",
        Subtitulo = "Gestión tributaria municipal segura, trazable y digital.",
        HeroSubtitle = "Gestión tributaria municipal segura, trazable y digital.",
        MensajeInformativo = "RUC, Bienes Inmuebles y Patentes integrados en una plataforma moderna para la operación municipal.",
        HeroModulesSummary = "RUC, Bienes Inmuebles y Patentes integrados en una plataforma moderna para la operación municipal.",
        MensajeConfianza = "Acceso institucional preparado para continuidad operativa y control seguro.",
        Beneficios = new List<string>
        {
            "Seguridad institucional",
            "Trazabilidad operativa",
            "Gobierno digital"
        },
        HeroBenefits = new List<LoginHeroBenefitModel>
        {
            new() { Icon = Icons.Material.Filled.Security, Title = "Seguridad institucional", Description = "Control de acceso alineado con operación pública segura." },
            new() { Icon = Icons.Material.Filled.Timeline, Title = "Trazabilidad operativa", Description = "Seguimiento visible de accesos, cambios y eventos críticos." },
            new() { Icon = Icons.Material.Filled.AccountBalance, Title = "Gobierno digital", Description = "Experiencia moderna para la gestión tributaria municipal." }
        },
        DemoUser = "admin@municipal.go.cr",
        DemoPassword = "Admin123!Demo",
        DemoTwoFactorCode = "123456",
        DemoHelperText = "Use estas credenciales únicamente para pruebas del prototipo.",
        HabilitarFirmaDigital = true,
        HabilitarTwoFactor = true,
        MaxIntentosFallidos = 5
    };

    public Task<LoginSettingsDto> GetAsync()
    {
        return Task.FromResult(Clone(settings));
    }

    public Task SaveAsync(LoginSettingsDto model)
    {
        settings = Clone(model);
        return Task.CompletedTask;
    }

    public static int GetMaxFailedAttempts()
    {
        return settings.MaxIntentosFallidos < 1 ? 1 : settings.MaxIntentosFallidos;
    }

    private static LoginSettingsDto Clone(LoginSettingsDto source)
    {
        var normalizedHeroImage = source.HeroImageUrl?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(normalizedHeroImage))
        {
            normalizedHeroImage = source.ImagenBienvenidaUrl?.Trim() ?? string.Empty;
        }

        return new LoginSettingsDto
        {
            ImagenBienvenidaUrl = normalizedHeroImage,
            HeroImageUrl = normalizedHeroImage,
            Titulo = source.Titulo?.Trim() ?? string.Empty,
            HeroTitle = string.IsNullOrWhiteSpace(source.HeroTitle) ? source.Titulo?.Trim() ?? string.Empty : source.HeroTitle.Trim(),
            Subtitulo = source.Subtitulo?.Trim() ?? string.Empty,
            HeroSubtitle = string.IsNullOrWhiteSpace(source.HeroSubtitle) ? source.Subtitulo?.Trim() ?? string.Empty : source.HeroSubtitle.Trim(),
            MensajeInformativo = source.MensajeInformativo?.Trim() ?? string.Empty,
            HeroModulesSummary = string.IsNullOrWhiteSpace(source.HeroModulesSummary) ? source.MensajeInformativo?.Trim() ?? string.Empty : source.HeroModulesSummary.Trim(),
            MensajeConfianza = source.MensajeConfianza?.Trim() ?? string.Empty,
            Beneficios = source.Beneficios?
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => x.Trim())
                .Take(3)
                .ToList() ?? new List<string>(),
            HeroBenefits = source.HeroBenefits?
                .Where(x => !string.IsNullOrWhiteSpace(x.Title) || !string.IsNullOrWhiteSpace(x.Description))
                .Take(3)
                .Select(x => new LoginHeroBenefitModel
                {
                    Icon = x.Icon,
                    Title = x.Title?.Trim() ?? string.Empty,
                    Description = x.Description?.Trim() ?? string.Empty
                }).ToList() ?? new List<LoginHeroBenefitModel>(),
            DemoUser = source.DemoUser?.Trim() ?? string.Empty,
            DemoPassword = source.DemoPassword ?? string.Empty,
            DemoTwoFactorCode = source.DemoTwoFactorCode?.Trim() ?? string.Empty,
            DemoHelperText = source.DemoHelperText?.Trim() ?? string.Empty,
            HabilitarFirmaDigital = source.HabilitarFirmaDigital,
            HabilitarTwoFactor = source.HabilitarTwoFactor,
            MaxIntentosFallidos = source.MaxIntentosFallidos < 1 ? 1 : source.MaxIntentosFallidos
        };
    }
}
