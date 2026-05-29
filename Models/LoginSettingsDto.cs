using System.Collections.Generic;

namespace BlazorApp.Models;

public class LoginSettingsDto
{
    public string ImagenBienvenidaUrl { get; set; } = "https://placehold.co/1200x1600/0b2440/e6f4ff?text=Core+Municipal+Inteligente";
    public string HeroImageUrl { get; set; } = "https://placehold.co/1200x1600/0b2440/e6f4ff?text=Core+Municipal+Inteligente";
    public string Titulo { get; set; } = "Core Municipal Inteligente";
    public string HeroTitle { get; set; } = "Core Municipal Inteligente";
    public string Subtitulo { get; set; } = "Gestión tributaria municipal segura, trazable y digital.";
    public string HeroSubtitle { get; set; } = "Gestión tributaria municipal segura, trazable y digital.";
    public string MensajeInformativo { get; set; } = "RUC, Bienes Inmuebles y Patentes integrados en una plataforma moderna para la operación municipal.";
    public string HeroModulesSummary { get; set; } = "RUC, Bienes Inmuebles y Patentes integrados en una plataforma moderna para la operación municipal.";
    public string MensajeConfianza { get; set; } = "Acceso institucional preparado para continuidad operativa y control seguro.";
    public List<string> Beneficios { get; set; } = new()
    {
        "Seguridad institucional",
        "Trazabilidad operativa",
        "Gobierno digital"
    };
    public List<LoginHeroBenefitModel> HeroBenefits { get; set; } = new();
    public string DemoUser { get; set; } = "admin@municipal.go.cr";
    public string DemoPassword { get; set; } = "Admin123!Demo";
    public string DemoTwoFactorCode { get; set; } = "123456";
    public string DemoHelperText { get; set; } = "Use estas credenciales únicamente para pruebas del prototipo.";
    public bool HabilitarFirmaDigital { get; set; } = true;
    public bool HabilitarTwoFactor { get; set; } = true;
    public int MaxIntentosFallidos { get; set; } = 5;
}
