using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorApp.Models;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BlazorApp.Shared;

public partial class PatenteCreateWizard
{
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;

    private string GetDisplayValue(string? value, string fallback = "Pendiente")
        => string.IsNullOrWhiteSpace(value) ? fallback : value;

    private string GetGeneralWorkflowState()
    {
        if (currentStatus.Equals("Completado", StringComparison.OrdinalIgnoreCase))
            return GetDisplayValue(model.EstadoFinalExpediente, currentStatus);

        if (!string.IsNullOrWhiteSpace(model.EstadoFinalExpediente)
            && !model.EstadoFinalExpediente.Equals("Borrador", StringComparison.OrdinalIgnoreCase))
        {
            return model.EstadoFinalExpediente;
        }

        return currentStatus;
    }

    private string GetFinalizationOutcomePreview()
    {
        if (model.EstadoResolucion.Equals("Rechazada", StringComparison.OrdinalIgnoreCase))
            return "Rechazada";

        if (model.EstadoResolucion.Equals("Prevenida", StringComparison.OrdinalIgnoreCase))
            return "Prevenida";

        return HasApprovalBlockers()
            ? ResolveBlockedExpedienteState()
            : "Aprobada";
    }

    private Severity GetFinalizationAlertSeverity()
        => HasApprovalBlockers() ? Severity.Warning : Severity.Success;

    private string GetFinalizationAlertMessage()
        => HasApprovalBlockers()
            ? "la solicitud no puede finalizar como aprobada y quedará Prevenida o Pendiente validación según corresponda."
            : "La solicitud cumple las reglas de cierre y puede finalizar como Aprobada.";

    private string GetRucMockSummary()
        => model.PendienteRegistroContribuyente
            ? "RUC pendiente de creación o consulta del contribuyente."
            : $"RUC {GetDisplayValue(model.CalidadDatosRuc, "Pendiente")} · contribuyente {GetDisplayValue(model.EstadoContribuyente, "Pendiente")}.";

    private string GetBienesInmueblesMockSummary()
    {
        if (IsActivityWithoutPhysicalLocal())
            return "Bienes Inmuebles no aplica por tratarse de actividad sin local físico.";

        return $"Bienes Inmuebles vinculado a ID Predial {GetDisplayValue(model.IdPredial)} y finca {GetDisplayValue(string.IsNullOrWhiteSpace(model.NumeroFinca) ? model.FincaOIdPredial : model.NumeroFinca)}.";
    }

    private string GetUsoSueloMockSummary()
        => $"Uso de Suelo {GetDisplayValue(model.EstadoUsoSuelo)} · certificado {GetDisplayValue(model.NumeroCertificadoUsoSuelo, "Pendiente")}.";

    private string GetCobroMockSummary()
        => $"Cobro {GetDisplayValue(model.EstadoCobro)} · referencia {GetDisplayValue(model.ReferenciaCuentaPorCobrarMock)}.";

    private string GetCuentaTributariaMockSummary()
        => GetDisplayValue(model.EstadoCuentaTributariaMock, "Cuenta tributaria pendiente de emisión.");

    private string GetNotificacionesMockSummary()
        => model.NotificacionSimulada
            ? "Notificación registrada en el expediente."
            : "Notificación pendiente de envío.";

    private string GetAuditoriaMockSummary()
        => $"Auditoría lista para registrar el cierre en estado {GetFinalizationOutcomePreview()}.";

    private void ApplyFinalizationOutcome()
    {
        SyncAssessmentState();

        if (model.EstadoResolucion.Equals("Rechazada", StringComparison.OrdinalIgnoreCase))
        {
            model.EstadoFinalExpediente = "Rechazada";
            model.NotificacionSimulada = true;
            if (string.IsNullOrWhiteSpace(model.ObservacionesResolucion))
                model.ObservacionesResolucion = "Solicitud rechazada.";
            if (string.IsNullOrWhiteSpace(model.FirmaDigitalReferencial))
                model.FirmaDigitalReferencial = "Rechazo referencial sin firma digital real.";
            return;
        }

        if (HasApprovalBlockers())
        {
            model.EstadoResolucion = ResolveBlockedApprovalResolutionState();
            model.EstadoFinalExpediente = ResolveBlockedExpedienteState();
            model.NotificacionSimulada = true;
            model.ObservacionesResolucion = $"Finalización condicionada. Bloqueos críticos: {GetApprovalBlockersSummary()}";
            model.FirmaDigitalReferencial = "Finalización referencial sin firma digital real por bloqueos del expediente.";
            return;
        }

        if (model.EstadoResolucion.Equals("Prevenida", StringComparison.OrdinalIgnoreCase))
        {
            model.EstadoFinalExpediente = "Prevenida";
            model.NotificacionSimulada = true;
            if (string.IsNullOrWhiteSpace(model.ObservacionesResolucion))
                model.ObservacionesResolucion = "Prevención emitida.";
            if (string.IsNullOrWhiteSpace(model.FirmaDigitalReferencial))
                model.FirmaDigitalReferencial = "Prevención referencial sin firma digital real.";
            return;
        }

        model.EstadoTasacion = model.EstadoTasacion.Equals("Puesta al cobro", StringComparison.OrdinalIgnoreCase)
            ? model.EstadoTasacion
            : "Aprobada";
        if (!model.EstadoCobro.Equals("Exonerado", StringComparison.OrdinalIgnoreCase) && !model.EstadoCobro.Equals("No aplica", StringComparison.OrdinalIgnoreCase))
            model.EstadoCobro = "Pago verificado";
        model.EstadoCuentaTributariaMock = "Cuenta tributaria verificada para cierre.";
        model.EstadoResolucion = "Aprobada";
        model.EstadoFinalExpediente = "Aprobada";
        model.NotificacionSimulada = true;
        model.ObservacionesResolucion = "Solicitud de patente aprobada.";
        model.FirmaDigitalReferencial = $"Firma digital registrada el {DateTime.Now:dd/MM/yyyy HH:mm}.";
    }

    private string? ResolveCreatedExpedienteRoute(SolicitudPatenteDto solicitud)
        => null;

    private async Task NavigateAfterFinalizationAsync(SolicitudPatenteDto solicitud)
    {
        var expedienteRoute = ResolveCreatedExpedienteRoute(solicitud);
        if (!string.IsNullOrWhiteSpace(expedienteRoute))
        {
            NavigationManager.NavigateTo(expedienteRoute);
            return;
        }

        Snackbar.Add("No existe una ruta de expediente configurada. Regresando a la consulta de Patentes.", Severity.Info);
        await Task.Yield();
        NavigationManager.NavigateTo("/patentes");
    }

    private List<string> BuildIntegrationSummaryItems()
        =>
        [
            $"RUC: {GetRucMockSummary()}",
            $"Bienes Inmuebles: {GetBienesInmueblesMockSummary()}",
            $"Uso de Suelo: {GetUsoSueloMockSummary()}",
            $"Cobro: {GetCobroMockSummary()}",
            $"Cuenta Tributaria: {GetCuentaTributariaMockSummary()}",
            $"Notificaciones: {GetNotificacionesMockSummary()}",
            $"Auditoría: {GetAuditoriaMockSummary()}"
        ];
}
