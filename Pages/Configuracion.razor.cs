using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorApp.Models;
using BlazorApp.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BlazorApp.Pages;

public partial class Configuracion
{
    [Inject] private ILoginSettingsService LoginSettingsService { get; set; } = default!;
    [Inject] private ISnackbar Snackbar { get; set; } = default!;

    private readonly List<BreadcrumbItem> breadcrumbs = new()
    {
        new("Inicio", href: "/"),
        new("Configuración", href: null, disabled: true)
    };

    private LoginSettingsDto loginSettings = new();
    private string beneficiosTexto = string.Empty;
    private string maxIntentosTexto = "5";
    private bool isLoading = true;
    private bool isSaving;

    protected override async Task OnInitializedAsync()
    {
        loginSettings = await LoginSettingsService.GetAsync();
        beneficiosTexto = string.Join(Environment.NewLine, loginSettings.Beneficios);
        maxIntentosTexto = loginSettings.MaxIntentosFallidos.ToString();
        isLoading = false;
    }

    private async Task SaveAsync()
    {
        isSaving = true;
        loginSettings.Beneficios = beneficiosTexto
            .Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
            .Select(x => x.Trim())
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Take(3)
            .ToList();

        if (!int.TryParse(maxIntentosTexto, out var maxIntentos) || maxIntentos < 1)
        {
            Snackbar.Add("La cantidad máxima de intentos fallidos debe ser un número mayor que cero.", Severity.Warning, _ => { }, "login-settings-validation");
            isSaving = false;
            return;
        }

        loginSettings.MaxIntentosFallidos = maxIntentos;
        await LoginSettingsService.SaveAsync(loginSettings);
        await Task.Delay(300);
        isSaving = false;

        Snackbar.Add("La parametrización visual de la pantalla de login fue actualizada correctamente.", Severity.Success, _ => { }, "login-settings-save");
    }
}
