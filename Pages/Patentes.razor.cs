using BlazorApp.Models;
using BlazorApp.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Threading.Tasks;

namespace BlazorApp.Pages;

public partial class Patentes : ComponentBase
{
    [Inject] private IPatentesService PatentesService { get; set; } = default!;
    [Inject] private IPatentesFormularioService PatentesFormularioService { get; set; } = default!;
    [Inject] private ISnackbar Snackbar { get; set; } = default!;

    private readonly List<BreadcrumbItem> breadcrumbs = new()
    {
        new("Inicio", href: "/"),
        new("Patentes", href: null, disabled: true)
    };

    private readonly FiltroLicenciasPatenteDto filtro = new();
    private List<LicenciaComercialDto> licencias = new();
    private List<LicenciaComercialDto> filteredLicencias = new();
    private LicenciaComercialDto selectedLicencia = new();
    private SolicitudPatenteDto selectedSolicitud = new();
    private int activePanelIndex;

    protected override async Task OnInitializedAsync()
    {
        await LoadAllAsync();
        selectedLicencia = licencias.FirstOrDefault() ?? new LicenciaComercialDto();
        selectedSolicitud = selectedLicencia.Solicitud.Id == 0
            ? await PatentesFormularioService.CrearNuevaSolicitudAsync()
            : selectedLicencia.Solicitud;
    }

    private async Task LoadAllAsync()
    {
        licencias = await PatentesService.GetAllAsync();
        filteredLicencias = licencias.OrderByDescending(x => x.FechaSolicitud).ToList();
    }

    private async Task LoadFilteredAsync()
    {
        DateTime? fechaVencimiento = null;
        if (DateTime.TryParse(filtro.FechaVencimientoHasta, out var parsed))
            fechaVencimiento = parsed;

        filteredLicencias = await PatentesService.SearchAsync(
            filtro.NumeroLicencia,
            filtro.Contribuyente,
            filtro.ActividadEconomica,
            filtro.Distrito,
            filtro.Estado,
            fechaVencimiento,
            filtro.Tipo);

        Snackbar.Add("Consulta de patentes actualizada.", Severity.Normal);
    }

    private async Task CreateNewAsync()
    {
        selectedSolicitud = await PatentesFormularioService.CrearNuevaSolicitudAsync();
        selectedLicencia = new LicenciaComercialDto();
        activePanelIndex = 2;
    }

    private async Task OpenSolicitudAsync(int id)
    {
        selectedLicencia = await PatentesService.GetByIdAsync(id);
        selectedSolicitud = selectedLicencia.Solicitud.Id == 0
            ? await PatentesFormularioService.CrearNuevaSolicitudAsync()
            : selectedLicencia.Solicitud;
        activePanelIndex = 2;
    }

    private async Task OpenMaintenanceAsync(int id)
    {
        selectedLicencia = await PatentesService.GetByIdAsync(id);
        selectedSolicitud = selectedLicencia.Solicitud.Id == 0
            ? await PatentesFormularioService.CrearNuevaSolicitudAsync()
            : selectedLicencia.Solicitud;
        activePanelIndex = 3;
    }

    private Task OnSolicitudChanged(SolicitudPatenteDto model)
    {
        selectedSolicitud = model;
        return Task.CompletedTask;
    }

    private Task OnLicenciaChanged(LicenciaComercialDto model)
    {
        selectedLicencia = model;
        return Task.CompletedTask;
    }

    private async Task AfterSolicitudSavedAsync(int id)
    {
        await LoadAllAsync();
        selectedLicencia = await PatentesService.GetByIdAsync(id);
        selectedSolicitud = selectedLicencia.Solicitud;
        activePanelIndex = 1;
    }

    private async Task AfterMaintenanceChangedAsync(int id)
    {
        await LoadAllAsync();
        selectedLicencia = await PatentesService.GetByIdAsync(id);
        selectedSolicitud = selectedLicencia.Solicitud;
        activePanelIndex = 3;
    }
}
