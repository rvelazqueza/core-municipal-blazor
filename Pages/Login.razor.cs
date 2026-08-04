using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorApp.Models;
using BlazorApp.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;

namespace BlazorApp.Pages;

public partial class Login
{
    [Inject] private IAuthService AuthService { get; set; } = default!;
    [Inject] private ILoginSettingsService LoginSettingsService { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private ISnackbar Snackbar { get; set; } = default!;

    private LoginSettingsDto settings = new();
    private LoginRequestDto loginRequest = new();
    private TwoFactorRequestDto twoFactorRequest = new();
    private List<LoginHeroBenefitModel> heroBenefits = new();
    private bool isProcessing;
    private bool isLocked;
    private int failedAttempts;
    private string feedbackMessage = string.Empty;
    private Severity feedbackSeverity = Severity.Info;
    private string currentInitials = "CM";
    private string heroTitle = string.Empty;
    private string heroSubtitle = string.Empty;
    private string heroModulesSummary = string.Empty;
    private string twoFactorHelperText = string.Empty;
    private string heroBackgroundStyle = "min-height:100vh;";
    private string heroImagePanelStyle = string.Empty;
    private bool hasHeroImage;

    protected override async Task OnInitializedAsync()
    {
        settings = await LoginSettingsService.GetAsync();
        ConfigureDisplayContent();

        failedAttempts = await AuthService.GetFailedAttemptsAsync();
        isLocked = failedAttempts >= settings.MaxIntentosFallidos;

        var currentUser = await AuthService.GetCurrentUserAsync();
        if (currentUser?.IsAuthenticated == true)
        {
            Navigation.NavigateTo("/", replace: true);
            return;
        }

        if (!string.IsNullOrWhiteSpace(currentUser?.Iniciales))
        {
            currentInitials = currentUser.Iniciales;
        }
    }

    private void ConfigureDisplayContent()
    {
        heroTitle = GetPreferredText(settings.HeroTitle, settings.Titulo, "Core Municipal Inteligente");
        heroSubtitle = GetPreferredText(settings.HeroSubtitle, settings.Subtitulo, "Gestión tributaria municipal segura, trazable y digital.");
        heroModulesSummary = GetPreferredText(settings.HeroModulesSummary, settings.MensajeInformativo, "RUC, Bienes Inmuebles y Patentes integrados en una plataforma moderna para la operación municipal.");

        twoFactorHelperText = settings.HabilitarTwoFactor ? "Código de verificación de dos factores requerido para este acceso." : string.Empty;
        heroBenefits = BuildHeroBenefits();
        hasHeroImage = !string.IsNullOrWhiteSpace(GetPreferredText(settings.HeroImageUrl, settings.ImagenBienvenidaUrl));
        heroBackgroundStyle = BuildHeroBackgroundStyle();
        heroImagePanelStyle = BuildHeroImagePanelStyle();
    }

    private async Task HandleLoginAsync()
    {
        if (isLocked)
        {
            SetFeedback("El acceso se encuentra bloqueado por seguridad.", Severity.Error);
            return;
        }

        isProcessing = true;
        twoFactorRequest.UsuarioOCorreo = loginRequest.UsuarioOCorreo;
        var response = await AuthService.LoginAsync(loginRequest);
        await Task.Delay(450);
        isProcessing = false;

        failedAttempts = await AuthService.GetFailedAttemptsAsync();
        isLocked = failedAttempts >= settings.MaxIntentosFallidos || response.IsLocked;

        if (response.RequiresTwoFactor)
        {
            SetFeedback(response.Message, Severity.Warning);
            Snackbar.Add(response.Message, Severity.Warning, _ => { }, "login-2fa");
            return;
        }

        if (response.IsSuccess)
        {
            Snackbar.Add(response.Message, Severity.Success, _ => { }, "login-success");
            Navigation.NavigateTo("/", replace: true);
            return;
        }

        SetFeedback(response.Message, response.IsLocked ? Severity.Error : Severity.Warning);
    }

    private async Task HandleTwoFactorAsync()
    {
        isProcessing = true;
        twoFactorRequest.UsuarioOCorreo = loginRequest.UsuarioOCorreo;
        var response = await AuthService.VerifyTwoFactorAsync(twoFactorRequest);
        await Task.Delay(350);
        isProcessing = false;

        failedAttempts = await AuthService.GetFailedAttemptsAsync();
        isLocked = failedAttempts >= settings.MaxIntentosFallidos || response.IsLocked;

        if (response.IsSuccess)
        {
            Snackbar.Add(response.Message, Severity.Success, _ => { }, "login-2fa-success");
            Navigation.NavigateTo("/", replace: true);
            return;
        }

        SetFeedback(response.Message, response.IsLocked ? Severity.Error : Severity.Warning);
    }

    private async Task HandleDigitalSignatureAsync()
    {
        isProcessing = true;
        var response = await AuthService.LoginWithDigitalSignatureAsync();
        await Task.Delay(350);
        isProcessing = false;

        if (response.IsSuccess)
        {
            Snackbar.Add(response.Message, Severity.Success, _ => { }, "login-firma-success");
            Navigation.NavigateTo("/", replace: true);
            return;
        }

        SetFeedback(response.Message, response.IsLocked ? Severity.Error : Severity.Warning);
    }

    private void SetFeedback(string message, Severity severity)
    {
        feedbackMessage = message;
        feedbackSeverity = severity;
    }

    private string GetPreferredText(params string[] values)
    {
        foreach (var value in values)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                return value.Trim();
            }
        }

        return string.Empty;
    }

    private List<LoginHeroBenefitModel> BuildHeroBenefits()
    {
        if (settings.HeroBenefits is { Count: > 0 })
        {
            return settings.HeroBenefits
                .Where(x => !string.IsNullOrWhiteSpace(x.Title) || !string.IsNullOrWhiteSpace(x.Description))
                .Select(x => new LoginHeroBenefitModel
                {
                    Icon = string.IsNullOrWhiteSpace(x.Icon) ? Icons.Material.Filled.VerifiedUser : x.Icon,
                    Title = x.Title,
                    Description = x.Description
                })
                .ToList();
        }

        return new List<LoginHeroBenefitModel>
        {
            new() { Icon = Icons.Material.Filled.Security, Title = "Seguridad institucional", Description = "Autenticación segura, verificación 2FA y firma digital para accesos de alto nivel." },
            new() { Icon = Icons.Material.Filled.Timeline, Title = "Trazabilidad total", Description = "Auditoría de accesos, cambios y operaciones críticas con visibilidad para control interno." },
            new() { Icon = Icons.Material.Filled.AccountBalance, Title = "Gobierno digital", Description = "Servicios municipales modernos, catastro digital y autogestión con experiencia unificada." }
        };
    }

    private string BuildHeroBackgroundStyle()
    {
        var configuredImage = GetPreferredText(settings.HeroImageUrl, settings.ImagenBienvenidaUrl);
        if (string.IsNullOrWhiteSpace(configuredImage))
        {
            return "min-height:100vh;";
        }

        return $"min-height:100vh; background-image: linear-gradient(135deg, rgba(4, 15, 32, 0.88), rgba(8, 32, 59, 0.80)), url('{configuredImage}'); background-size: cover; background-position: center;";
    }

    private string BuildHeroImagePanelStyle()
    {
        var configuredImage = GetPreferredText(settings.HeroImageUrl, settings.ImagenBienvenidaUrl);
        if (string.IsNullOrWhiteSpace(configuredImage))
        {
            return string.Empty;
        }

        return $"background-image: linear-gradient(180deg, rgba(4, 15, 32, 0.18), rgba(4, 15, 32, 0.58)), url('{configuredImage}'); background-size: cover; background-position: center;";
    }
}
