using BlazorApp.Models;
using BlazorApp.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Threading.Tasks;

namespace BlazorApp.Shared;

public partial class MantenimientoPatentePanel : ComponentBase
{
    [Inject] private IPatentesService PatentesService { get; set; } = default!;
    [Inject] private IAuditoriaService AuditoriaService { get; set; } = default!;
    [Inject] private IMaestroDatosService MaestroDatosService { get; set; } = default!;
    [Inject] private IPatentesValidacionService PatentesValidacionService { get; set; } = default!;
    [Inject] private IDialogService DialogService { get; set; } = default!;
    [Inject] private ISnackbar Snackbar { get; set; } = default!;

    [Parameter] public LicenciaComercialDto Model { get; set; } = new();
    [Parameter] public EventCallback<LicenciaComercialDto> ModelChanged { get; set; }
    [Parameter] public EventCallback<int> OnChanged { get; set; }

    private List<string> validationErrors = new();
    private List<string> distritos = new();
    private List<string> tiposPatente = new();
    private string motivoMovimiento = string.Empty;
    private string usuarioResponsable = "Analista Municipal";
    private string fechaMovimientoInput = DateTime.Today.ToString("yyyy-MM-dd");

    protected override async Task OnInitializedAsync()
    {
        distritos = await MaestroDatosService.GetDistritosAsync();
        tiposPatente = await MaestroDatosService.GetTiposPatenteAsync();
    }

    protected override void OnParametersSet()
    {
        usuarioResponsable = string.IsNullOrWhiteSpace(Model.Responsable) ? "Analista Municipal" : Model.Responsable;
    }

    private Task OnFechaChanged(string value)
    {
        fechaMovimientoInput = value ?? string.Empty;
        return Task.CompletedTask;
    }

    private async Task SaveBasicAsync()
    {
        validationErrors = PatentesValidacionService.ValidarMantenimiento(Model, motivoMovimiento, false).Errores;
        if (validationErrors.Any())
            return;

        var saved = await PatentesService.SaveLicenciaAsync(Model, usuarioResponsable);
        await AuditoriaService.RegistrarCambioAsync(saved.ContribuyenteId, new AuditoriaCambioDto
        {
            Usuario = usuarioResponsable,
            CampoModificado = "Mantenimiento patente",
            ValorAnterior = "Datos básicos anteriores",
            ValorNuevo = $"{saved.NombreComercial} · {saved.DireccionLocal}",
            Origen = "Patentes"
        });

        Model = saved;
        await ModelChanged.InvokeAsync(Model);
        Snackbar.Add("Mantenimiento básico guardado.", Severity.Success);
        await OnChanged.InvokeAsync(Model.Id);
    }

    private async Task SuspendAsync()
    {
        validationErrors = PatentesValidacionService.ValidarMantenimiento(Model, motivoMovimiento, true).Errores;
        if (validationErrors.Any())
            return;

        var dialog = await DialogService.ShowAsync<ConfirmDialog>("Confirmar suspensión", new DialogParameters<ConfirmDialog>
        {
            { x => x.Title, "Suspender licencia" },
            { x => x.Message, "La licencia quedará suspendida y se registrará el evento de auditoría." }
        });
        var result = await dialog.Result;
        if (result.Canceled)
            return;

        var previousStatus = Model.Estado;
        var saved = await PatentesService.SuspenderAsync(Model.Id, motivoMovimiento, ResolveFechaMovimiento(), usuarioResponsable);
        await AuditoriaService.RegistrarCambioAsync(saved.ContribuyenteId, new AuditoriaCambioDto
        {
            Usuario = usuarioResponsable,
            CampoModificado = "Estado licencia",
            ValorAnterior = previousStatus,
            ValorNuevo = "Suspendido",
            Origen = "Patentes"
        });

        Model = saved;
        await ModelChanged.InvokeAsync(Model);
        Snackbar.Add("Licencia suspendida.", Severity.Warning);
        await OnChanged.InvokeAsync(Model.Id);
    }

    private async Task CancelAsync()
    {
        validationErrors = PatentesValidacionService.ValidarMantenimiento(Model, motivoMovimiento, true).Errores;
        if (validationErrors.Any())
            return;

        var dialog = await DialogService.ShowAsync<ConfirmDialog>("Confirmar cancelación", new DialogParameters<ConfirmDialog>
        {
            { x => x.Title, "Cancelar licencia" },
            { x => x.Message, "La licencia quedará cancelada y se registrará el evento de auditoría." }
        });
        var result = await dialog.Result;
        if (result.Canceled)
            return;

        var previousStatus = Model.Estado;
        var saved = await PatentesService.CancelarAsync(Model.Id, motivoMovimiento, ResolveFechaMovimiento(), usuarioResponsable);
        await AuditoriaService.RegistrarCambioAsync(saved.ContribuyenteId, new AuditoriaCambioDto
        {
            Usuario = usuarioResponsable,
            CampoModificado = "Estado licencia",
            ValorAnterior = previousStatus,
            ValorNuevo = "Cancelado",
            Origen = "Patentes"
        });

        Model = saved;
        await ModelChanged.InvokeAsync(Model);
        Snackbar.Add("Licencia cancelada.", Severity.Error);
        await OnChanged.InvokeAsync(Model.Id);
    }

    private DateTime ResolveFechaMovimiento()
        => DateTime.TryParse(fechaMovimientoInput, out var parsed) ? parsed : DateTime.Today;

    private List<MovimientoPatenteDto> RecentMovements => Model.Movimientos.OrderByDescending(x => x.FechaHora).ToList();
}
