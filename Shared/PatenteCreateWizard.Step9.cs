using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MudBlazor;

namespace BlazorApp.Shared;

public partial class PatenteCreateWizard
{
    private readonly List<string> tiposTasacionMock = ["Analogía", "Declaración", "Transporte público", "Licores", "Temporal"];
    private readonly List<string> estadosTasacionMock = ["Pendiente", "Calculada", "Aprobada", "Puesta al cobro"];
    private readonly List<string> estadosCobroMock = ["Pendiente", "Pago verificado", "Exonerado", "No aplica"];
    private readonly List<string> estadosResolucionMock = ["Aprobada", "Prevenida", "Rechazada", "Pendiente firma", "Pendiente validación"];

    private void EnsureAssessmentDefaults()
    {
        model.TipoTasacion = tiposTasacionMock.Contains(model.TipoTasacion, StringComparer.OrdinalIgnoreCase)
            ? tiposTasacionMock.First(x => x.Equals(model.TipoTasacion, StringComparison.OrdinalIgnoreCase))
            : ResolveSuggestedAssessmentType();
        model.EstadoTasacion = estadosTasacionMock.Contains(model.EstadoTasacion, StringComparer.OrdinalIgnoreCase)
            ? estadosTasacionMock.First(x => x.Equals(model.EstadoTasacion, StringComparison.OrdinalIgnoreCase))
            : "Pendiente";
        model.EstadoCobro = estadosCobroMock.Contains(model.EstadoCobro, StringComparer.OrdinalIgnoreCase)
            ? estadosCobroMock.First(x => x.Equals(model.EstadoCobro, StringComparison.OrdinalIgnoreCase))
            : "Pendiente";
        model.EstadoResolucion = estadosResolucionMock.Contains(model.EstadoResolucion, StringComparer.OrdinalIgnoreCase)
            ? estadosResolucionMock.First(x => x.Equals(model.EstadoResolucion, StringComparison.OrdinalIgnoreCase))
            : "Pendiente validación";
        model.FechaInicioCobro = string.IsNullOrWhiteSpace(model.FechaInicioCobro) ? DateTime.Today.ToString("dd/MM/yyyy") : model.FechaInicioCobro;
        model.EstadoCuentaTributariaMock = string.IsNullOrWhiteSpace(model.EstadoCuentaTributariaMock) ? "Cuenta tributaria mock pendiente de emisión." : model.EstadoCuentaTributariaMock;
        model.ReferenciaCuentaPorCobrarMock = string.IsNullOrWhiteSpace(model.ReferenciaCuentaPorCobrarMock) ? BuildReceivableReference() : model.ReferenciaCuentaPorCobrarMock;
        model.ObservacionesResolucion = string.IsNullOrWhiteSpace(model.ObservacionesResolucion) ? "Resolución mock pendiente de generación." : model.ObservacionesResolucion;
        model.FirmaDigitalReferencial = string.IsNullOrWhiteSpace(model.FirmaDigitalReferencial) ? "Pendiente firma digital referencial." : model.FirmaDigitalReferencial;

        if (model.MontoAnualMock <= 0 || model.MontoTrimestralMock <= 0 || model.TimbreBiodiversidad <= 0)
            ApplyMockAssessmentAmounts();

        model.CobroProporcionalVisual = string.IsNullOrWhiteSpace(model.CobroProporcionalVisual)
            ? BuildProportionalChargeVisual()
            : model.CobroProporcionalVisual;
    }

    private void SyncAssessmentState()
    {
        EnsureAssessmentDefaults();
        model.CobroProporcionalVisual = BuildProportionalChargeVisual();

        if (model.EstadoTasacion.Equals("Puesta al cobro", StringComparison.OrdinalIgnoreCase))
        {
            if (model.EstadoCobro.Equals("Pendiente", StringComparison.OrdinalIgnoreCase))
                model.EstadoCuentaTributariaMock = $"Cuenta tributaria mock en cobro para {model.PeriodoFiscal}.";
        }

        if (model.EstadoCobro.Equals("Pago verificado", StringComparison.OrdinalIgnoreCase) && model.EstadoTasacion.Equals("Calculada", StringComparison.OrdinalIgnoreCase))
            model.EstadoTasacion = "Aprobada";

        var canAdjustWorkflow = currentStatus.Equals("En progreso", StringComparison.OrdinalIgnoreCase)
            || currentStatus.Equals("Pendiente validación", StringComparison.OrdinalIgnoreCase)
            || currentStatus.Equals("Completado", StringComparison.OrdinalIgnoreCase);

        if (model.EstadoResolucion.Equals("Rechazada", StringComparison.OrdinalIgnoreCase))
        {
            if (canAdjustWorkflow)
                model.EstadoFinalExpediente = "Rechazada";
            return;
        }

        if (model.EstadoResolucion.Equals("Prevenida", StringComparison.OrdinalIgnoreCase))
        {
            if (canAdjustWorkflow)
                model.EstadoFinalExpediente = "Prevenida";
            return;
        }

        if (HasApprovalBlockers())
        {
            if (model.EstadoResolucion.Equals("Aprobada", StringComparison.OrdinalIgnoreCase))
                model.EstadoResolucion = ResolveBlockedApprovalResolutionState();

            if (canAdjustWorkflow && !model.EstadoResolucion.Equals("Aprobada", StringComparison.OrdinalIgnoreCase))
                model.EstadoFinalExpediente = ResolveBlockedExpedienteState();

            return;
        }

        if (model.EstadoResolucion.Equals("Aprobada", StringComparison.OrdinalIgnoreCase))
        {
            if (canAdjustWorkflow)
                model.EstadoFinalExpediente = "Aprobada en modo demo";
            return;
        }

        if (RequiresPendingInspectionBlocker())
        {
            model.EstadoResolucion = "Pendiente validación";
            if (canAdjustWorkflow)
                model.EstadoFinalExpediente = "Pendiente validación";
        }
    }

    private async Task SimulateTasacionMockAsync()
    {
        model.TipoTasacion = ResolveSuggestedAssessmentType();
        ApplyMockAssessmentAmounts();
        model.EstadoTasacion = "Calculada";
        model.FechaInicioCobro = DateTime.Today.ToString("dd/MM/yyyy");
        model.CobroProporcionalVisual = BuildProportionalChargeVisual();
        model.EstadoCuentaTributariaMock = $"Cuenta tributaria mock calculada para {model.PeriodoFiscal}.";
        model.ReferenciaCuentaPorCobrarMock = BuildReceivableReference();

        if (!model.EstadoCobro.Equals("Exonerado", StringComparison.OrdinalIgnoreCase) && !model.EstadoCobro.Equals("No aplica", StringComparison.OrdinalIgnoreCase))
            model.EstadoCobro = "Pendiente";

        Snackbar.Add($"Tasación mock simulada. Monto anual {FormatCurrency(model.MontoAnualMock)}.", Severity.Success);
        await RefreshWizardStateAsync(persist: false);
    }

    private async Task GenerateResolutionMockAsync()
    {
        EnsureAssessmentDefaults();

        if (HasApprovalBlockers())
        {
            model.EstadoResolucion = ResolveBlockedApprovalResolutionState();
            model.ObservacionesResolucion = $"Resolución mock condicionada. Bloqueos detectados: {GetApprovalBlockersSummary()}";
            model.FirmaDigitalReferencial = "Firma digital referencial no aplicable mientras existan bloqueos.";
            Snackbar.Add("Resolución mock generada con condicionantes del expediente demo.", Severity.Warning);
        }
        else
        {
            model.EstadoResolucion = "Pendiente firma";
            model.ObservacionesResolucion = "Resolución mock generada y lista para firma referencial.";
            model.FirmaDigitalReferencial = "Pendiente firma digital referencial.";
            Snackbar.Add("Resolución mock generada correctamente.", Severity.Success);
        }

        model.NotificacionSimulada = false;
        await RefreshWizardStateAsync(persist: false);
    }

    private async Task SendPreventionMockAsync()
    {
        model.EstadoResolucion = "Prevenida";
        model.EstadoFinalExpediente = "Prevenida";
        model.ObservacionesResolucion = $"Prevención mock generada el {DateTime.Now:dd/MM/yyyy HH:mm}.";
        model.NotificacionSimulada = true;
        model.FirmaDigitalReferencial = "Prevención emitida sin firma digital real.";
        Snackbar.Add("Prevención mock enviada al expediente demo.", Severity.Warning);
        await RefreshWizardStateAsync(persist: false);
    }

    private async Task ApproveSolicitudMockAsync()
    {
        if (HasApprovalBlockers())
        {
            model.EstadoResolucion = ResolveBlockedApprovalResolutionState();
            model.EstadoFinalExpediente = ResolveBlockedExpedienteState();
            model.ObservacionesResolucion = $"No es posible aprobar directamente. Bloqueos detectados: {GetApprovalBlockersSummary()}";
            model.FirmaDigitalReferencial = "No se emite firma digital referencial por bloqueos del expediente.";
            Snackbar.Add("No es posible aprobar directamente. El expediente queda Pendiente validación o Prevenido en modo demo.", Severity.Warning);
            await RefreshWizardStateAsync(persist: false);
            return;
        }

        ApplyMockAssessmentAmounts();
        model.EstadoTasacion = "Aprobada";
        if (!model.EstadoCobro.Equals("Exonerado", StringComparison.OrdinalIgnoreCase) && !model.EstadoCobro.Equals("No aplica", StringComparison.OrdinalIgnoreCase))
            model.EstadoCobro = "Pago verificado";
        model.EstadoCuentaTributariaMock = "Cuenta tributaria mock verificada para el expediente demo.";
        model.EstadoResolucion = "Aprobada";
        model.EstadoFinalExpediente = "Aprobada en modo demo";
        model.NotificacionSimulada = true;
        model.FirmaDigitalReferencial = $"Firma digital referencial registrada el {DateTime.Now:dd/MM/yyyy HH:mm}.";
        model.ObservacionesResolucion = "Solicitud aprobada en modo demo.";
        Snackbar.Add("Solicitud aprobada en modo demo.", Severity.Success);
        await RefreshWizardStateAsync(persist: false);
    }

    private async Task RejectSolicitudMockAsync()
    {
        model.EstadoResolucion = "Rechazada";
        model.EstadoFinalExpediente = "Rechazada";
        model.ObservacionesResolucion = $"Solicitud rechazada en modo demo el {DateTime.Now:dd/MM/yyyy HH:mm}.";
        model.NotificacionSimulada = true;
        model.FirmaDigitalReferencial = "Rechazo registrado sin firma digital real.";
        Snackbar.Add("Solicitud rechazada en modo demo.", Severity.Error);
        await RefreshWizardStateAsync(persist: false);
    }

    private void ApplyMockAssessmentAmounts()
    {
        var baseAmount = ResolveMockBaseAmount();
        model.MontoAnualMock = baseAmount;
        model.MontoTrimestralMock = Math.Round(baseAmount / 4m, 2);
        model.TimbreBiodiversidad = ResolveBiodiversityAmount();
        model.PublicidadExterior = ResolveExteriorAdvertisingAmount();
        model.Multa = IsMorosityBlocking() ? 15000m : 0m;
        model.Intereses = IsMorosityBlocking() ? 4500m : 0m;
    }

    private decimal ResolveMockBaseAmount()
    {
        var tipoTasacion = ResolveSuggestedAssessmentType();
        decimal baseAmount = tipoTasacion switch
        {
            "Licores" => 285000m,
            "Temporal" => 98000m,
            "Transporte público" => 145000m,
            "Declaración" => 132000m,
            _ => 118000m
        };

        if (model.RiesgoActividad.Equals("Alto", StringComparison.OrdinalIgnoreCase))
            baseAmount += 25000m;
        else if (model.RiesgoActividad.Equals("Medio", StringComparison.OrdinalIgnoreCase))
            baseAmount += 12000m;

        if (model.RequiereLicores)
            baseAmount += 18000m;

        return baseAmount;
    }

    private decimal ResolveBiodiversityAmount()
    {
        if (model.TipoTasacion.Equals("Temporal", StringComparison.OrdinalIgnoreCase))
            return 4800m;

        if (model.RequiereLicores)
            return 9600m;

        return 7200m;
    }

    private decimal ResolveExteriorAdvertisingAmount()
    {
        if (IsActivityWithoutPhysicalLocal())
            return 0m;

        if (model.ActividadTemporal)
            return 3500m;

        return model.TipoLicencia.Equals("Ambulante", StringComparison.OrdinalIgnoreCase) ? 2500m : 9000m;
    }

    private string ResolveSuggestedAssessmentType()
    {
        if (model.RequiereLicores || model.TipoLicencia.Equals("Licores", StringComparison.OrdinalIgnoreCase))
            return "Licores";

        if (model.ActividadTemporal || model.TipoLicencia.Equals("Temporal", StringComparison.OrdinalIgnoreCase) || model.TipoLicencia.Equals("Días festivos", StringComparison.OrdinalIgnoreCase))
            return "Temporal";

        if (model.ActividadEconomica.Contains("transporte", StringComparison.OrdinalIgnoreCase) || model.TipoLicencia.Contains("Transporte", StringComparison.OrdinalIgnoreCase))
            return "Transporte público";

        if (model.TieneDeclaracionJurada)
            return "Declaración";

        return "Analogía";
    }

    private string BuildReceivableReference()
        => $"CXC-MOCK-{DateTime.Today:yyyyMMdd}-{model.NumeroSolicitud.Replace("-", string.Empty)}";

    private string BuildProportionalChargeVisual()
    {
        var proporcional = DateTime.Today.Day <= 10
            ? "Se visualizaría el 100% del trimestre demo."
            : DateTime.Today.Day <= 20
                ? "Se visualizaría un cobro proporcional del 75% del trimestre demo."
                : "Se visualizaría un cobro proporcional del 50% del trimestre demo.";

        return $"Inicio de cobro mock: {model.FechaInicioCobro}. {proporcional}";
    }

    private bool HasApprovalBlockers()
        => GetApprovalBlockingMessages().Count > 0;

    private int GetApprovalBlockersCount()
        => GetApprovalBlockingMessages().Count;

    private List<string> GetApprovalBlockingMessages()
    {
        var blockers = new List<string>();

        if (model.EstadoUsoSuelo.Equals("No conforme", StringComparison.OrdinalIgnoreCase)
            || model.EstadoUsoSuelo.Equals("Pendiente", StringComparison.OrdinalIgnoreCase)
            || model.EstadoUsoSuelo.Equals("Vencido", StringComparison.OrdinalIgnoreCase))
        {
            blockers.Add($"Uso de Suelo en estado {model.EstadoUsoSuelo}.");
        }

        if (GetNonCompliantRequirementsCount() > 0)
            blockers.Add("Existen requisitos en estado No cumple.");

        if (GetPendingRequirementsCount() > 0 && !model.TieneDeclaracionJurada)
            blockers.Add("Existen requisitos pendientes sin cobertura de Declaración Jurada.");

        if (model.TieneDeclaracionJurada && model.DeclaracionJuradaDiasHabiles <= 0)
            blockers.Add("La Declaración Jurada mock venció y requiere gestión administrativa.");

        if (IsMorosityBlocking())
            blockers.Add($"Morosidad en estado {model.ResultadoMorosidad}.");

        if (RequiresPendingInspectionBlocker())
            blockers.Add($"La inspección está pendiente en estado {model.EstadoInspeccion}.");

        return blockers.Distinct().ToList();
    }

    private bool IsMorosityBlocking()
        => model.ResultadoMorosidad.Contains("observ", StringComparison.OrdinalIgnoreCase)
           || model.ResultadoMorosidad.Contains("moroso", StringComparison.OrdinalIgnoreCase)
           || model.EstadoMorosidad.Contains("observ", StringComparison.OrdinalIgnoreCase)
           || model.EstadoMorosidad.Contains("moroso", StringComparison.OrdinalIgnoreCase);

    private bool RequiresPendingInspectionBlocker()
    {
        if (!model.RequiereInspeccion)
            return false;

        return !model.EstadoInspeccion.Equals("Finalizada", StringComparison.OrdinalIgnoreCase)
               && !model.EstadoInspeccion.Equals("Completada", StringComparison.OrdinalIgnoreCase)
               && !model.EstadoInspeccion.Equals("No requerida", StringComparison.OrdinalIgnoreCase);
    }

    private string ResolveBlockedApprovalResolutionState()
        => HasHardApprovalBlocker() ? "Prevenida" : "Pendiente validación";

    private string ResolveBlockedExpedienteState()
        => ResolveBlockedApprovalResolutionState().Equals("Prevenida", StringComparison.OrdinalIgnoreCase)
            ? "Prevenida"
            : "Pendiente validación";

    private bool HasHardApprovalBlocker()
        => model.EstadoUsoSuelo.Equals("No conforme", StringComparison.OrdinalIgnoreCase)
           || GetNonCompliantRequirementsCount() > 0
           || IsMorosityBlocking();

    private string GetApprovalBlockersSummary()
        => HasApprovalBlockers() ? string.Join(" ", GetApprovalBlockingMessages()) : "Sin bloqueos de aprobación.";

    private Color GetTasacionStatusColor(string estado)
    {
        if (estado.Equals("Aprobada", StringComparison.OrdinalIgnoreCase) || estado.Equals("Puesta al cobro", StringComparison.OrdinalIgnoreCase))
            return Color.Success;

        if (estado.Equals("Calculada", StringComparison.OrdinalIgnoreCase))
            return Color.Info;

        return Color.Warning;
    }

    private Color GetCobroStatusColor(string estado)
    {
        if (estado.Equals("Pago verificado", StringComparison.OrdinalIgnoreCase))
            return Color.Success;

        if (estado.Equals("Exonerado", StringComparison.OrdinalIgnoreCase) || estado.Equals("No aplica", StringComparison.OrdinalIgnoreCase))
            return Color.Info;

        return Color.Warning;
    }

    private Color GetResolucionStatusColor(string estado)
    {
        if (estado.Equals("Aprobada", StringComparison.OrdinalIgnoreCase))
            return Color.Success;

        if (estado.Equals("Rechazada", StringComparison.OrdinalIgnoreCase))
            return Color.Error;

        if (estado.Equals("Prevenida", StringComparison.OrdinalIgnoreCase) || estado.Equals("Pendiente validación", StringComparison.OrdinalIgnoreCase))
            return Color.Warning;

        return Color.Info;
    }

    private string FormatCurrency(decimal amount)
        => $"₡{amount:N2}";

    private decimal GetQuarterlyComponent(decimal annualAmount)
        => Math.Round(annualAmount / 4m, 2);
}
