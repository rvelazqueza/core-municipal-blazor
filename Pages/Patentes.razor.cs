using BlazorApp.Models;
using BlazorApp.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace BlazorApp.Pages;

public partial class Patentes : ComponentBase
{
    [Inject] private IPatentesService PatentesService { get; set; } = default!;
    [Inject] private ISnackbar Snackbar { get; set; } = default!;
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;

    private readonly List<BreadcrumbItem> breadcrumbs = new()
    {
        new("Inicio", href: "/"),
        new("Patentes", href: null, disabled: true)
    };

    private readonly FiltroLicenciasPatenteDto filtro = new();
    private PatentesModuleSnapshotDto snapshot = new();
    private List<LicenciaComercialDto> allLicencias = new();
    private List<LicenciaComercialDto> filteredLicencias = new();
    private List<string> tipoLicenciaOptions = new();
    private List<string> estadoOptions = new();
    private List<string> usoSueloOptions = new();
    private List<string> distritoOptions = new();
    private List<string> canalIngresoOptions = new();
    private List<KpiCardViewModel> kpiCards = new();
    private string resultSummary = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        await LoadAsync();
    }

    private async Task LoadAsync()
    {
        snapshot = await PatentesService.GetModuleSnapshotAsync();
        allLicencias = snapshot.LicenciasComerciales.OrderByDescending(x => x.FechaSolicitud).ToList();
        RefreshFilterOptions();
        BuildKpis();
        await ApplyFiltersInternalAsync(false);
    }

    private void RefreshFilterOptions()
    {
        tipoLicenciaOptions = allLicencias
            .Select(x => x.TipoPatente)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(x => x)
            .ToList();

        estadoOptions = allLicencias
            .Select(x => x.Estado)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(x => x)
            .ToList();

        usoSueloOptions = allLicencias
            .Select(x => GetUsoSueloLabel(x))
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(x => x)
            .ToList();

        distritoOptions = allLicencias
            .Select(x => x.Distrito)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(x => x)
            .ToList();

        canalIngresoOptions = allLicencias
            .Select(x => x.CanalIngreso)
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(x => x)
            .ToList();
    }

    private void BuildKpis()
    {
        var activeCommercial = allLicencias.Count(x => IsActiveLicense(x) && x.TipoPatente.Equals("Comercial", StringComparison.OrdinalIgnoreCase));
        var pendingRequests = snapshot.Solicitudes.Count(x => IsPendingState(x.Estado));
        var activeLiquor = snapshot.LicenciasLicores.Count(x => IsActiveState(x.EstadoVigencia));
        var activeTemporary = snapshot.LicenciasTemporales.Count(x => IsActiveState(x.EstadoResolucion));
        var pendingLandUse = allLicencias.Count(x => IsPendingLandUse(x));
        var pendingRequirements = allLicencias.Sum(x => x.Requisitos.Count(r => !r.Cumplido));
        var pendingDeclarations = snapshot.Declaraciones.Count(x => IsPendingState(x.Estado));
        var activeExemptions = snapshot.Exoneraciones.Count(x => IsActiveState(x.Estado));
        var openInspections = snapshot.Inspecciones.Count(x => IsOpenState(x.Estado));
        var openComplaints = snapshot.Denuncias.Count(x => IsOpenState(x.Estado));
        var expiringSoon = allLicencias.Count(x => x.FechaVencimiento.HasValue && x.FechaVencimiento.Value.Date >= DateTime.Today && x.FechaVencimiento.Value.Date <= DateTime.Today.AddDays(60));

        kpiCards = new List<KpiCardViewModel>
        {
            new("Licencias comerciales activas", activeCommercial.ToString(), "Vigentes con estado aprobado", Icons.Material.Filled.Storefront, Color.Primary),
            new("Solicitudes pendientes", pendingRequests.ToString(), "En revisión o borrador", Icons.Material.Filled.PendingActions, Color.Warning),
            new("Licencias de licores activas", activeLiquor.ToString(), "Vigencia especial aprobada", Icons.Material.Filled.LocalBar, Color.Secondary),
            new("Licencias temporales", activeTemporary.ToString(), "Eventos y permisos especiales", Icons.Material.Filled.Timer, Color.Info),
            new("Pendientes de Uso de Suelo", pendingLandUse.ToString(), "Revisión técnica en curso", Icons.Material.Filled.Map, Color.Warning),
            new("Requisitos pendientes", pendingRequirements.ToString(), "Documentación por completar", Icons.Material.Filled.Rule, Color.Error),
            new("Declaraciones pendientes", pendingDeclarations.ToString(), "Declaraciones sin cierre", Icons.Material.Filled.Description, Color.Warning),
            new("Exoneraciones activas", activeExemptions.ToString(), "Beneficios vigentes", Icons.Material.Filled.Redeem, Color.Success),
            new("Inspecciones abiertas", openInspections.ToString(), "Procesos en campo", Icons.Material.Filled.Badge, Color.Info),
            new("Denuncias abiertas", openComplaints.ToString(), "Casos en seguimiento", Icons.Material.Filled.ReportProblem, Color.Error),
            new("Licencias próximas a vencer", expiringSoon.ToString(), "Vencimiento estimado en 60 días", Icons.Material.Filled.Schedule, Color.Tertiary)
        };
    }

    private Task ApplyFiltersAsync()
        => ApplyFiltersInternalAsync(true);

    private Task ApplyFiltersInternalAsync(bool notify = true)
    {
        IEnumerable<LicenciaComercialDto> query = allLicencias;

        if (!string.IsNullOrWhiteSpace(filtro.NumeroLicencia))
            query = query.Where(x => x.NumeroLicencia.Contains(filtro.NumeroLicencia, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(filtro.NumeroSolicitud))
            query = query.Where(x => x.Solicitud.NumeroSolicitud.Contains(filtro.NumeroSolicitud, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(filtro.Expediente))
            query = query.Where(x => GetExpediente(x).Contains(filtro.Expediente, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(filtro.Identificacion))
            query = query.Where(x => x.Identificacion.Contains(filtro.Identificacion, StringComparison.OrdinalIgnoreCase) || x.ContribuyenteRuc.Contains(filtro.Identificacion, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(filtro.Contribuyente))
            query = query.Where(x => x.ContribuyenteNombre.Contains(filtro.Contribuyente, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(filtro.NombreComercial))
            query = query.Where(x => x.NombreComercial.Contains(filtro.NombreComercial, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(filtro.ActividadEconomica))
            query = query.Where(x => x.ActividadEconomica.Contains(filtro.ActividadEconomica, StringComparison.OrdinalIgnoreCase) || x.CodigoCaecr.Contains(filtro.ActividadEconomica, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(filtro.TipoLicencia))
            query = query.Where(x => x.TipoPatente.Equals(filtro.TipoLicencia, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(filtro.Estado))
            query = query.Where(x => x.Estado.Equals(filtro.Estado, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(filtro.UsoSuelo))
            query = query.Where(x => GetUsoSueloLabel(x).Equals(filtro.UsoSuelo, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(filtro.Distrito))
            query = query.Where(x => x.Distrito.Equals(filtro.Distrito, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(filtro.CanalIngreso))
            query = query.Where(x => x.CanalIngreso.Equals(filtro.CanalIngreso, StringComparison.OrdinalIgnoreCase));

        filteredLicencias = query.OrderByDescending(x => x.FechaSolicitud).ToList();
        resultSummary = $"{filteredLicencias.Count} de {allLicencias.Count} licencias mostradas con los filtros actuales.";
        if (notify)
        {
            Snackbar.Add("Consulta filtrada actualizada.", Severity.Normal);
        }
        return Task.CompletedTask;
    }

    private Task ClearFiltersAsync()
    {
        filtro.NumeroLicencia = string.Empty;
        filtro.NumeroSolicitud = string.Empty;
        filtro.Expediente = string.Empty;
        filtro.Identificacion = string.Empty;
        filtro.Contribuyente = string.Empty;
        filtro.NombreComercial = string.Empty;
        filtro.ActividadEconomica = string.Empty;
        filtro.TipoLicencia = string.Empty;
        filtro.Estado = string.Empty;
        filtro.UsoSuelo = string.Empty;
        filtro.Distrito = string.Empty;
        filtro.CanalIngreso = string.Empty;
        return ApplyFiltersInternalAsync();
    }

    private Task CreateNewAsync()
    {
        NavigationManager.NavigateTo("/patentes/nueva");
        return Task.CompletedTask;
    }

    private Task ApplyQuickTypeAsync(string tipoLicencia)
    {
        filtro.TipoLicencia = tipoLicencia;
        return ApplyFiltersInternalAsync();
    }

    private Task ApplyTemporalTypeAsync()
        => ApplyQuickTypeAsync("Temporal");

    private Task ApplyLiquorTypeAsync()
        => ApplyQuickTypeAsync("Licores");

    private Task OpenConfiguracionAsync()
    {
        NavigationManager.NavigateTo("/configuracion");
        return Task.CompletedTask;
    }

    private static string GetExpediente(LicenciaComercialDto licencia)
        => string.IsNullOrWhiteSpace(licencia.Expediente) ? $"EXP-{licencia.NumeroLicencia}" : licencia.Expediente;

    private static string GetLocalLabel(LicenciaComercialDto licencia)
        => string.IsNullOrWhiteSpace(licencia.FincaOIdPredial) || licencia.FincaOIdPredial.Equals("Sin local físico", StringComparison.OrdinalIgnoreCase)
            ? "Sin local físico"
            : $"{licencia.DireccionLocal} · {licencia.FincaOIdPredial}";

    private static string GetLastUpdate(LicenciaComercialDto licencia)
        => licencia.Movimientos.Any()
            ? licencia.Movimientos.OrderByDescending(x => x.FechaHora).First().FechaHora.ToString("dd/MM/yyyy HH:mm")
            : licencia.FechaAprobacion?.ToString("dd/MM/yyyy") ?? licencia.FechaSolicitud.ToString("dd/MM/yyyy");

    private static string GetUsoSueloLabel(LicenciaComercialDto licencia)
        => string.IsNullOrWhiteSpace(licencia.UsoSuelo?.Estado) ? "Pendiente" : licencia.UsoSuelo.Estado;

    private static Color GetEstadoColor(string estado)
    {
        if (estado.Contains("Aprob", StringComparison.OrdinalIgnoreCase) || estado.Contains("Vigente", StringComparison.OrdinalIgnoreCase))
            return Color.Success;

        if (estado.Contains("Revision", StringComparison.OrdinalIgnoreCase) || estado.Contains("Revisión", StringComparison.OrdinalIgnoreCase) || estado.Contains("Pend", StringComparison.OrdinalIgnoreCase))
            return Color.Warning;

        if (estado.Contains("Rech", StringComparison.OrdinalIgnoreCase) || estado.Contains("Suspen", StringComparison.OrdinalIgnoreCase) || estado.Contains("Cancel", StringComparison.OrdinalIgnoreCase))
            return Color.Error;

        return Color.Default;
    }

    private static Color GetUsoSueloColor(LicenciaComercialDto licencia)
    {
        var label = GetUsoSueloLabel(licencia);

        if (label.Contains("Conforme", StringComparison.OrdinalIgnoreCase))
            return Color.Success;

        if (label.Contains("Pend", StringComparison.OrdinalIgnoreCase))
            return Color.Warning;

        if (label.Contains("No conforme", StringComparison.OrdinalIgnoreCase))
            return Color.Error;

        return Color.Default;
    }

    private static Color GetCanalColor(string canalIngreso)
    {
        if (canalIngreso.Contains("digital", StringComparison.OrdinalIgnoreCase))
            return Color.Primary;

        if (canalIngreso.Contains("presencial", StringComparison.OrdinalIgnoreCase) || canalIngreso.Contains("ventanilla", StringComparison.OrdinalIgnoreCase))
            return Color.Secondary;

        return Color.Info;
    }

    private static bool IsActiveLicense(LicenciaComercialDto licencia)
        => licencia.Estado.Contains("Aprob", StringComparison.OrdinalIgnoreCase) || licencia.Estado.Contains("Vigente", StringComparison.OrdinalIgnoreCase);

    private static bool IsPendingLandUse(LicenciaComercialDto licencia)
        => licencia.PendienteUsoSuelo || GetUsoSueloLabel(licencia).Contains("Pend", StringComparison.OrdinalIgnoreCase);

    private static bool IsPendingState(string estado)
        => estado.Contains("Pend", StringComparison.OrdinalIgnoreCase) || estado.Contains("Revision", StringComparison.OrdinalIgnoreCase) || estado.Contains("Revisión", StringComparison.OrdinalIgnoreCase) || estado.Contains("Borr", StringComparison.OrdinalIgnoreCase);

    private static bool IsActiveState(string estado)
        => estado.Contains("Activa", StringComparison.OrdinalIgnoreCase) || estado.Contains("Vigente", StringComparison.OrdinalIgnoreCase) || estado.Contains("Aprob", StringComparison.OrdinalIgnoreCase);

    private static bool IsOpenState(string estado)
        => !estado.Contains("Cerr", StringComparison.OrdinalIgnoreCase) && !estado.Contains("Resuel", StringComparison.OrdinalIgnoreCase) && !estado.Contains("Atend", StringComparison.OrdinalIgnoreCase) && !estado.Contains("Archiv", StringComparison.OrdinalIgnoreCase);

    private sealed record KpiCardViewModel(string Label, string Value, string Description, string Icon, Color Color);
}
