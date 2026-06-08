using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorApp.Models;
using MudBlazor;

namespace BlazorApp.Shared;

public partial class PatenteCreateWizard
{
    private readonly List<string> origenesRequisito = ["Manual", "Plataforma de Servicios", "Mock", "No aplica"];

    private void EnsureRequirementDefaults()
    {
        SyncRequirementList();
        SyncRequirementsState();
    }

    private void SyncRequirementList()
    {
        var existing = (model.Requisitos ?? new List<RequisitoPatenteDto>())
            .Where(x => !string.IsNullOrWhiteSpace(x.Clave) || !string.IsNullOrWhiteSpace(x.Nombre))
            .ToDictionary(x => NormalizeRequirementKey(string.IsNullOrWhiteSpace(x.Clave) ? x.Nombre : x.Clave), StringComparer.OrdinalIgnoreCase);

        model.Requisitos =
        [
            BuildRequirement(existing, "identificacion", "Identificación del solicitante", model.EstadoIdentificacion, true,
                string.IsNullOrWhiteSpace(model.Identificacion) ? "Cédula o identificación mock pendiente" : $"ID mock: {model.Identificacion}",
                "Plataforma de Servicios"),
            BuildRequirement(existing, "personeria", "Personería jurídica", model.EstadoPersoneriaJuridica, IsJuridicalPerson(),
                "Certificación de personería jurídica mock", "Manual"),
            BuildRequirement(existing, "uso-suelo", "Uso de Suelo", model.EstadoUsoSueloChecklist, true,
                string.IsNullOrWhiteSpace(model.NumeroCertificadoUsoSuelo) ? "Certificado de Uso de Suelo mock pendiente" : $"Certificado mock: {model.NumeroCertificadoUsoSuelo}",
                "Mock"),
            BuildRequirement(existing, "permiso-sanitario", "Permiso sanitario de funcionamiento", model.EstadoPermisoSanitario, model.RequierePermisoSanitario,
                "Permiso sanitario mock", "Mock"),
            BuildRequirement(existing, "arrendamiento-propiedad", "Contrato de arrendamiento o documento de propiedad", model.EstadoArrendamientoPropiedad, true,
                IsActivityWithoutPhysicalLocal() ? "Referencia de actividad sin local físico" : "Contrato o propiedad mock del local", "Manual"),
            BuildRequirement(existing, "ccss", "CCSS", model.EstadoCcssChecklist, true,
                "Constancia CCSS mock", "Mock"),
            BuildRequirement(existing, "fodesaf", "FODESAF", model.EstadoFodesafChecklist, true,
                "Constancia FODESAF mock", "Mock"),
            BuildRequirement(existing, "ins", "INS", model.EstadoInsChecklist, true,
                "Póliza INS mock", "Mock"),
            BuildRequirement(existing, "declaracion-jurada", "Declaración jurada", model.EstadoDeclaracionJuradaChecklist, model.TieneDeclaracionJurada,
                $"Declaración jurada mock · {model.ModalidadDeclaracionJurada}", "Manual"),
            BuildRequirement(existing, "comprobante-pago", "Comprobante de pago mock", model.EstadoComprobantePago, true,
                $"Recibo mock {model.NumeroSolicitud}", "Mock"),
            BuildRequirement(existing, "croquis", IsActivityWithoutPhysicalLocal() ? "Croquis o referencia de ubicación" : "Croquis o ubicación del local", model.EstadoCroquis, true,
                IsActivityWithoutPhysicalLocal() ? "Ubicación referencial de actividad sin local físico" : "Croquis mock del local", "Manual"),
            BuildRequirement(existing, "patente-anterior", "Patente anterior", ResolveRequirementState(existing, "patente-anterior", IsRenewalRequest() ? "Pendiente" : "No aplica"), IsRenewalRequest(),
                "Patente anterior mock", "Mock"),
            BuildRequirement(existing, "exoneracion", "Documento de exoneración", ResolveRequirementState(existing, "exoneracion", IsExonerationRequest() ? "Pendiente" : "No aplica"), IsExonerationRequest(),
                "Documento de exoneración mock", "Manual"),
            BuildRequirement(existing, "concejo", "Autorización Concejo Municipal", ResolveRequirementState(existing, "concejo", RequiresConcejoAuthorization() ? "Pendiente" : "No aplica"), RequiresConcejoAuthorization(),
                "Acuerdo mock de Concejo Municipal", "Plataforma de Servicios"),
            BuildRequirement(existing, "mopt", "Autorización MOPT", ResolveRequirementState(existing, "mopt", RequiresMoptAuthorization() ? "Pendiente" : "No aplica"), RequiresMoptAuthorization(),
                "Autorización MOPT mock", "Plataforma de Servicios"),
            BuildRequirement(existing, "especial", ResolveSpecialRequirementName(), ResolveRequirementState(existing, "especial", RequiresSpecialRequirement() ? "Pendiente" : "No aplica"), RequiresSpecialRequirement(),
                ResolveSpecialRequirementPlaceholder(), "Manual")
        ];
    }

    private RequisitoPatenteDto BuildRequirement(
        IReadOnlyDictionary<string, RequisitoPatenteDto> existing,
        string key,
        string name,
        string legacyState,
        bool applies,
        string placeholder,
        string defaultOrigin)
    {
        existing.TryGetValue(NormalizeRequirementKey(key), out var item);
        var estado = applies
            ? NormalizeRequirementState(string.IsNullOrWhiteSpace(item?.Estado) ? legacyState : item.Estado)
            : "No aplica";

        return new RequisitoPatenteDto
        {
            Clave = key,
            Nombre = name,
            Obligatorio = applies,
            Estado = estado,
            Cumplido = estado.Equals("Cumple", StringComparison.OrdinalIgnoreCase),
            Observacion = applies
                ? (string.IsNullOrWhiteSpace(item?.Observacion) ? BuildDefaultRequirementObservation(name, estado) : item!.Observacion)
                : "No aplica",
            DocumentoPlaceholder = applies
                ? (string.IsNullOrWhiteSpace(item?.DocumentoPlaceholder) ? placeholder : item!.DocumentoPlaceholder)
                : "No aplica",
            Origen = applies
                ? NormalizeRequirementOrigin(string.IsNullOrWhiteSpace(item?.Origen) ? defaultOrigin : item!.Origen)
                : "No aplica"
        };
    }

    private void SyncRequirementsState()
    {
        SyncRequirementList();
        model.EstadoIdentificacion = GetRequirementState("identificacion", model.EstadoIdentificacion);
        model.EstadoPersoneriaJuridica = GetRequirementState("personeria", model.EstadoPersoneriaJuridica);
        model.EstadoUsoSueloChecklist = GetRequirementState("uso-suelo", model.EstadoUsoSueloChecklist);
        model.EstadoPermisoSanitario = GetRequirementState("permiso-sanitario", model.EstadoPermisoSanitario);
        model.EstadoArrendamientoPropiedad = GetRequirementState("arrendamiento-propiedad", model.EstadoArrendamientoPropiedad);
        model.EstadoCcssChecklist = GetRequirementState("ccss", model.EstadoCcssChecklist);
        model.EstadoFodesafChecklist = GetRequirementState("fodesaf", model.EstadoFodesafChecklist);
        model.EstadoInsChecklist = GetRequirementState("ins", model.EstadoInsChecklist);
        model.EstadoDeclaracionJuradaChecklist = GetRequirementState("declaracion-jurada", model.EstadoDeclaracionJuradaChecklist);
        model.EstadoComprobantePago = GetRequirementState("comprobante-pago", model.EstadoComprobantePago);
        model.EstadoCroquis = GetRequirementState("croquis", model.EstadoCroquis);

        var hasNoCumple = GetNonCompliantRequirementsCount() > 0;
        var hasPending = GetPendingRequirementsCount() > 0;
        var allApplicableAreCumple = model.Requisitos
            .Where(x => !x.Estado.Equals("No aplica", StringComparison.OrdinalIgnoreCase))
            .All(x => x.Estado.Equals("Cumple", StringComparison.OrdinalIgnoreCase));

        model.EstadoRequisitos = hasNoCumple
            ? "No cumple"
            : hasPending
                ? "Pendiente"
                : allApplicableAreCumple
                    ? "Cumple"
                    : "Pendiente";

        if (model.TieneDeclaracionJurada)
        {
            var estadoDj = model.EstadoDeclaracionJuradaChecklist;
            model.EstadoDeclaracionJurada = estadoDj.Equals("Cumple", StringComparison.OrdinalIgnoreCase)
                ? "Aceptada"
                : estadoDj;
        }
        else
        {
            model.EstadoDeclaracionJurada = "No";
        }

        var canAdjustWorkflow = currentStatus.Equals("En progreso", StringComparison.OrdinalIgnoreCase)
            || currentStatus.Equals("Pendiente validación", StringComparison.OrdinalIgnoreCase);

        if (hasNoCumple)
        {
            model.EstadoResolucion = "Pendiente validación";
            model.EstadoRevision = "Observado";
            if (canAdjustWorkflow)
                model.EstadoFinalExpediente = "Requiere revisión";
        }
        else if (hasPending)
        {
            model.EstadoResolucion = "Pendiente validación";
            if (!model.EstadoRevision.Equals("Observado", StringComparison.OrdinalIgnoreCase))
                model.EstadoRevision = "Pendiente";
            if (canAdjustWorkflow)
                model.EstadoFinalExpediente = "Pendiente validación";
        }
        else if (!HasUsoSueloWorkflowAlert())
        {
            if (model.EstadoResolucion.Equals("Pendiente validación", StringComparison.OrdinalIgnoreCase))
                model.EstadoResolucion = "Preparada";

            if (canAdjustWorkflow && (model.EstadoFinalExpediente.Equals("Pendiente validación", StringComparison.OrdinalIgnoreCase) || model.EstadoFinalExpediente.Equals("Requiere revisión", StringComparison.OrdinalIgnoreCase)))
                model.EstadoFinalExpediente = "Borrador";
        }
    }

    private async Task SyncRequirementsAfterEditAsync()
    {
        SyncRequirementsState();
        await RefreshWizardStateAsync(persist: false);
    }

    private async Task ValidateRequirementsMockAsync()
    {
        SyncRequirementsState();
        var severity = GetNonCompliantRequirementsCount() > 0 ? Severity.Warning : Severity.Success;
        var message = $"Validación mock de requisitos: {GetCompletedRequirementsCount()} completos, {GetPendingRequirementsCount()} pendientes y {GetNonCompliantRequirementsCount()} no cumplidos.";
        Snackbar.Add(message, severity);
        await RefreshWizardStateAsync(persist: false);
    }

    private async Task GeneratePreventionMockAsync()
    {
        SyncRequirementsState();
        model.EstadoResolucion = "Prevenida";
        if (!model.EstadoFinalExpediente.Equals("Cancelado", StringComparison.OrdinalIgnoreCase) && !model.EstadoFinalExpediente.Equals("Completado", StringComparison.OrdinalIgnoreCase))
            model.EstadoFinalExpediente = GetNonCompliantRequirementsCount() > 0 ? "Requiere revisión" : "Pendiente validación";

        model.Observaciones = $"Prevención mock generada por requisitos documentales el {DateTime.Now:dd/MM/yyyy HH:mm}.";
        Snackbar.Add("Prevención mock generada para seguimiento documental del expediente.", Severity.Warning);
        await RefreshWizardStateAsync(persist: false);
    }

    private int GetApplicableRequirementsCount()
        => model.Requisitos.Count(x => !x.Estado.Equals("No aplica", StringComparison.OrdinalIgnoreCase));

    private int GetCompletedRequirementsCount()
        => model.Requisitos.Count(x => x.Estado.Equals("Cumple", StringComparison.OrdinalIgnoreCase));

    private int GetPendingRequirementsCount()
        => model.Requisitos.Count(x => x.Estado.Equals("Pendiente", StringComparison.OrdinalIgnoreCase));

    private int GetNonCompliantRequirementsCount()
        => model.Requisitos.Count(x => x.Estado.Equals("No cumple", StringComparison.OrdinalIgnoreCase));

    private int GetRequirementsCompletionPercent()
    {
        var applicable = GetApplicableRequirementsCount();
        if (applicable == 0)
            return 100;

        return (int)Math.Round(GetCompletedRequirementsCount() * 100d / applicable, 0);
    }

    private Color GetRequirementStatusColor(string estado)
    {
        if (estado.Equals("Cumple", StringComparison.OrdinalIgnoreCase))
            return Color.Success;

        if (estado.Equals("Pendiente", StringComparison.OrdinalIgnoreCase))
            return Color.Warning;

        if (estado.Equals("No cumple", StringComparison.OrdinalIgnoreCase))
            return Color.Error;

        return Color.Info;
    }

    private Severity GetRequirementsPendingAlertSeverity()
        => model.TieneDeclaracionJurada ? Severity.Info : Severity.Warning;

    private bool HasUsoSueloWorkflowAlert()
        => model.EstadoUsoSuelo.Equals("Pendiente", StringComparison.OrdinalIgnoreCase)
           || model.EstadoUsoSuelo.Equals("No conforme", StringComparison.OrdinalIgnoreCase)
           || model.EstadoUsoSuelo.Equals("Vencido", StringComparison.OrdinalIgnoreCase);

    private bool IsJuridicalPerson()
        => model.TipoPersona.Equals("Juridica", StringComparison.OrdinalIgnoreCase);

    private bool IsRenewalRequest()
        => model.TipoSolicitud.Contains("Renovación", StringComparison.OrdinalIgnoreCase);

    private bool IsExonerationRequest()
        => model.TipoSolicitud.Contains("Exoneración", StringComparison.OrdinalIgnoreCase);

    private bool RequiresConcejoAuthorization()
        => model.RequiereLicores || model.TipoLicencia.Equals("Espectáculos públicos", StringComparison.OrdinalIgnoreCase);

    private bool RequiresMoptAuthorization()
        => model.TipoLicencia.Equals("Ambulante", StringComparison.OrdinalIgnoreCase)
           || model.TipoLicencia.Equals("Estacionaria", StringComparison.OrdinalIgnoreCase);

    private bool RequiresSpecialRequirement()
        => model.TipoLicencia.Equals("Temporal", StringComparison.OrdinalIgnoreCase)
           || model.TipoLicencia.Equals("Días festivos", StringComparison.OrdinalIgnoreCase)
           || model.TipoLicencia.Equals("Licores", StringComparison.OrdinalIgnoreCase)
           || model.TipoLicencia.Equals("Espectáculos públicos", StringComparison.OrdinalIgnoreCase)
           || model.TipoLicencia.Equals("Extracción de materiales", StringComparison.OrdinalIgnoreCase);

    private string ResolveSpecialRequirementName()
    {
        if (model.TipoLicencia.Equals("Temporal", StringComparison.OrdinalIgnoreCase) || model.TipoLicencia.Equals("Días festivos", StringComparison.OrdinalIgnoreCase))
            return "Requisito especial: plan operativo temporal";

        if (model.TipoLicencia.Equals("Licores", StringComparison.OrdinalIgnoreCase))
            return "Requisito especial: constancia para expendio de licores";

        if (model.TipoLicencia.Equals("Espectáculos públicos", StringComparison.OrdinalIgnoreCase))
            return "Requisito especial: plan de seguridad del evento";

        if (model.TipoLicencia.Equals("Extracción de materiales", StringComparison.OrdinalIgnoreCase))
            return "Requisito especial: informe técnico de extracción";

        return "Requisito especial según tipo de licencia";
    }

    private string ResolveSpecialRequirementPlaceholder()
    {
        if (model.TipoLicencia.Equals("Temporal", StringComparison.OrdinalIgnoreCase) || model.TipoLicencia.Equals("Días festivos", StringComparison.OrdinalIgnoreCase))
            return "Plan operativo temporal mock";

        if (model.TipoLicencia.Equals("Licores", StringComparison.OrdinalIgnoreCase))
            return "Constancia mock para licencia de licores";

        if (model.TipoLicencia.Equals("Espectáculos públicos", StringComparison.OrdinalIgnoreCase))
            return "Plan de seguridad mock del evento";

        if (model.TipoLicencia.Equals("Extracción de materiales", StringComparison.OrdinalIgnoreCase))
            return "Informe técnico mock de extracción";

        return "Documento especial mock";
    }

    private string GetRequirementState(string key, string fallback)
        => model.Requisitos.FirstOrDefault(x => x.Clave.Equals(key, StringComparison.OrdinalIgnoreCase))?.Estado ?? fallback;

    private static string NormalizeRequirementKey(string value)
        => value.Trim().ToLowerInvariant();

    private string ResolveRequirementState(IReadOnlyDictionary<string, RequisitoPatenteDto> existing, string key, string fallback)
    {
        return existing.TryGetValue(NormalizeRequirementKey(key), out var item) && !string.IsNullOrWhiteSpace(item.Estado)
            ? item.Estado
            : fallback;
    }

    private string NormalizeRequirementState(string? estado)
    {
        if (string.IsNullOrWhiteSpace(estado))
            return "Pendiente";

        return estadosCumplimiento.Contains(estado, StringComparer.OrdinalIgnoreCase)
            ? estadosCumplimiento.First(x => x.Equals(estado, StringComparison.OrdinalIgnoreCase))
            : "Pendiente";
    }

    private string NormalizeRequirementOrigin(string? origen)
    {
        if (string.IsNullOrWhiteSpace(origen))
            return "Mock";

        return origenesRequisito.Contains(origen, StringComparer.OrdinalIgnoreCase)
            ? origenesRequisito.First(x => x.Equals(origen, StringComparison.OrdinalIgnoreCase))
            : "Mock";
    }

    private static string BuildDefaultRequirementObservation(string name, string estado)
    {
        if (estado.Equals("Cumple", StringComparison.OrdinalIgnoreCase))
            return $"{name} validado en modo demo.";

        if (estado.Equals("No cumple", StringComparison.OrdinalIgnoreCase))
            return $"{name} observado en modo demo.";

        if (estado.Equals("No aplica", StringComparison.OrdinalIgnoreCase))
            return "No aplica.";

        return $"{name} pendiente de verificación mock.";
    }

    private string GetDeclaracionJuradaDeadlineMessage()
    {
        if (!model.TieneDeclaracionJurada)
            return "No aplica.";

        if (model.DeclaracionJuradaDiasHabiles <= 0)
            return "Plazo vencido, requiere gestión administrativa.";

        if (model.DeclaracionJuradaDiasHabiles <= 10)
            return "Plazo próximo a vencer.";

        return "Requisitos pendientes dentro del plazo.";
    }

    private Severity GetDeclaracionJuradaDeadlineSeverity()
    {
        if (!model.TieneDeclaracionJurada)
            return Severity.Normal;

        if (model.DeclaracionJuradaDiasHabiles <= 0)
            return Severity.Error;

        if (model.DeclaracionJuradaDiasHabiles <= 10)
            return Severity.Warning;

        return Severity.Info;
    }

    private int GetDeclaracionJuradaDeadlinePercent()
    {
        if (!model.TieneDeclaracionJurada)
            return 100;

        var remaining = Math.Clamp(model.DeclaracionJuradaDiasHabiles, 0, 60);
        return (int)Math.Round(remaining * 100d / 60d, 0);
    }

    private static List<RequisitoPatenteDto> CloneRequirements(IEnumerable<RequisitoPatenteDto>? source)
        => (source ?? Enumerable.Empty<RequisitoPatenteDto>()).Select(item => new RequisitoPatenteDto
        {
            Clave = item.Clave,
            Nombre = item.Nombre,
            Obligatorio = item.Obligatorio,
            Cumplido = item.Cumplido,
            Estado = item.Estado,
            Observacion = item.Observacion,
            DocumentoPlaceholder = item.DocumentoPlaceholder,
            Origen = item.Origen
        }).ToList();
}
