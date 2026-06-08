using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorApp.Models;

namespace BlazorApp.Shared;

public partial class PatenteCreateWizard
{
    private List<StepValidationResultDto> BuildValidationResults()
    {
        var results = Enumerable.Range(0, wizardSteps.Count - 1).Select(BuildStepValidationResult).ToList();
        var pendingSteps = results.Where(x => !x.IsValid).Select(x => x.StepTitle).ToList();

        var reviewMessages = new List<string>();
        if (pendingSteps.Any())
        {
            reviewMessages.Add($"Antes de finalizar, complete los pasos pendientes: {string.Join(", ", pendingSteps)}.");
        }
        else if (HasApprovalBlockers())
        {
            reviewMessages.Add("El expediente puede registrarse en modo demo, pero no finalizar como aprobado debido a bloqueos críticos.");
            reviewMessages.AddRange(GetApprovalBlockingMessages());
            reviewMessages.Add($"Resultado sugerido de finalización: {GetFinalizationOutcomePreview()}.");
        }
        else
        {
            reviewMessages.Add("El expediente demo cumple con la estructura mínima para registrar la solicitud y puede finalizar como Aprobada en modo demo.");
        }

        results.Add(new StepValidationResultDto
        {
            StepIndex = wizardSteps.Count - 1,
            StepTitle = wizardSteps[^1].Title,
            IsValid = !pendingSteps.Any(),
            HasWarnings = !pendingSteps.Any() && HasApprovalBlockers(),
            Messages = reviewMessages
        });

        return results;
    }

    private StepValidationResultDto ValidateCurrentStep() => BuildStepValidationResult(currentStepIndex);

    private bool CanFinalizeWizard()
    {
        if (currentStepIndex != wizardSteps.Count - 1)
            return false;

        return BuildValidationResults().Take(wizardSteps.Count - 1).All(x => x.IsValid);
    }

    private void RegisterValidationMessages(IEnumerable<StepValidationResultDto> results)
    {
        validationMessages.Clear();
        validationMessages.AddRange(results
            .SelectMany(x => x.Messages)
            .Where(message => !string.IsNullOrWhiteSpace(message))
            .Distinct());
    }

    private Task UpdateSummaryAsync(bool saveDraft = false, bool persist = true) => RefreshWizardStateAsync(saveDraft, persist);

    private StepValidationResultDto BuildStepValidationResult(int stepIndex)
    {
        var result = new StepValidationResultDto
        {
            StepIndex = stepIndex,
            StepTitle = wizardSteps[stepIndex].Title,
            IsValid = true
        };

        switch (stepIndex)
        {
            case 0:
                ValidateRequired(result, model.TipoSolicitud, "Seleccione el tipo de solicitud.");
                ValidateRequired(result, model.TipoLicencia, "Seleccione el tipo de licencia.");
                ValidateRequired(result, model.CanalIngreso, "Seleccione el canal de ingreso.");
                ValidateRequired(result, model.NumeroExpediente, "Ingrese el número de expediente mock.");
                ValidateRequired(result, model.EstadoInicial, "Defina el estado inicial del expediente.");
                ValidateRequired(result, model.ModalidadDeclaracionJurada, "Indique la modalidad de declaración jurada.");
                break;
            case 1:
                if (model.ContribuyenteId is null && !model.PendienteRegistroContribuyente)
                {
                    result.IsValid = false;
                    result.Messages.Add("Seleccione un contribuyente existente o marque el contribuyente como pendiente de registro.");
                }

                ValidateRequired(result, model.Identificacion, "Ingrese la identificación del solicitante.");
                ValidateRequired(result, model.ContribuyenteNombre, "Ingrese el nombre o razón social.");
                ValidateRequired(result, model.TipoPersona, "Indique el tipo de persona.");
                ValidateRequired(result, model.Telefono, "Registre el teléfono del contribuyente.");
                ValidateRequired(result, model.DireccionFiscal, "Registre la dirección fiscal.");
                ValidateRequired(result, model.EstadoContribuyente, "Defina el estado del contribuyente.");
                ValidateRequired(result, model.CalidadDatosRuc, "Defina la calidad de los datos del RUC.");

                if (string.IsNullOrWhiteSpace(model.Correo))
                {
                    result.HasWarnings = true;
                    result.Messages.Add("Se recomienda registrar un correo para notificaciones.");
                }

                if (string.IsNullOrWhiteSpace(model.MedioNotificacion))
                {
                    result.HasWarnings = true;
                    result.Messages.Add("Se recomienda definir el medio de notificación.");
                }

                if (model.PendienteRegistroContribuyente)
                {
                    result.HasWarnings = true;
                    result.Messages.Add("El contribuyente quedará marcado como pendiente de registro en el RUC mock.");
                }

                if (model.EstadoContribuyente.Equals("Suspendido", StringComparison.OrdinalIgnoreCase) || model.CalidadDatosRuc.Equals("Revisar", StringComparison.OrdinalIgnoreCase))
                {
                    result.HasWarnings = true;
                    result.Messages.Add("El contribuyente requiere revisión o está en estado no óptimo para demo.");
                }
                break;
            case 2:
                ValidateRequired(result, model.Distrito, "Seleccione el distrito.");
                if (IsActivityWithoutPhysicalLocal())
                {
                    result.HasWarnings = true;
                    result.Messages.Add("Actividad sin local físico.");
                }
                else
                {
                    var hasLocationReference = !string.IsNullOrWhiteSpace(model.IdPredial)
                                               || !string.IsNullOrWhiteSpace(model.FincaOIdPredial)
                                               || !string.IsNullOrWhiteSpace(model.DireccionLocal);
                    if (!hasLocationReference)
                    {
                        result.IsValid = false;
                        result.Messages.Add("Registre ID Predial, finca o dirección del local.");
                    }

                    ValidateRequired(result, model.NombreComercialLocal, "Seleccione o registre el nombre comercial del local.");
                    ValidateRequired(result, model.CuentaServiciosMunicipales, "Seleccione la cuenta de servicios municipales.");
                }

                if (string.IsNullOrWhiteSpace(model.EstadoGis) || model.EstadoGis.Equals("Pendiente", StringComparison.OrdinalIgnoreCase))
                {
                    result.HasWarnings = true;
                    result.Messages.Add("El estado GIS está pendiente en el demo.");
                }
                break;
            case 3:
                ValidateRequired(result, model.NombreComercial, "Indique el nombre comercial.");
                ValidateRequired(result, model.ActividadEconomica, "Indique la actividad económica.");
                ValidateRequired(result, model.CategoriaActividad, "Indique la categoría de la actividad.");
                if (string.IsNullOrWhiteSpace(model.ActividadPrincipal))
                {
                    result.HasWarnings = true;
                    result.Messages.Add("La actividad principal puede completarse en una siguiente iteración.");
                }

                if (model.RequiereLicores)
                {
                    result.HasWarnings = true;
                    result.Messages.Add("Revisión especial de licencias de licores en modo demo.");
                }
                break;
            case 4:
                ValidateRequired(result, model.EstadoUsoSuelo, "Defina el estado de Uso de Suelo.");
                ValidateRequired(result, model.Zonificacion, "Registre la zonificación.");
                ValidateRequired(result, model.ActividadEconomica, "Complete la actividad solicitada para el análisis de Uso de Suelo.");
                ValidateRequired(result, model.ActividadesAutorizadas, "Registre las actividades autorizadas mock.");
                ValidateRequired(result, model.CompatibilidadUsoSuelo, "Registre la compatibilidad.");
                ValidateRequired(result, model.ResultadoUsoSuelo, "Defina el resultado de la validación de Uso de Suelo.");

                if (model.EstadoUsoSuelo.Equals("Conforme", StringComparison.OrdinalIgnoreCase))
                {
                    ValidateRequired(result, model.NumeroCertificadoUsoSuelo, "Registre el certificado de Uso de Suelo.");
                    ValidateRequired(result, model.CertificadoUsoSuelo, "Registre el certificado de Uso de Suelo.");
                }
                else if (model.EstadoUsoSuelo.Equals("Pendiente", StringComparison.OrdinalIgnoreCase))
                {
                    result.HasWarnings = true;
                    result.Messages.Add("Uso de Suelo pendiente: el expediente queda en pendiente de validación para la resolución demo.");
                }
                else if (model.EstadoUsoSuelo.Equals("No conforme", StringComparison.OrdinalIgnoreCase) || model.EstadoUsoSuelo.Equals("Vencido", StringComparison.OrdinalIgnoreCase))
                {
                    result.HasWarnings = true;
                    result.Messages.Add("Uso de Suelo no conforme o vencido: se requiere revisión antes de una aprobación directa.");
                }

                if (IsActivityWithoutPhysicalLocal())
                {
                    result.HasWarnings = true;
                    result.Messages.Add("La validación de Uso de Suelo puede ser referencial para actividades sin local físico.");
                }
                break;
            case 5:
                ValidateRequired(result, model.EstadoRequisitos, "Defina el estado de requisitos.");
                ValidateChecklistState(result, model.EstadoIdentificacion, "Identificación");
                ValidateChecklistState(result, model.EstadoPersoneriaJuridica, "Personería jurídica");
                ValidateChecklistState(result, model.EstadoUsoSueloChecklist, "Uso de Suelo");
                ValidateChecklistState(result, model.EstadoPermisoSanitario, "Permiso sanitario");
                ValidateChecklistState(result, model.EstadoArrendamientoPropiedad, "Arrendamiento o propiedad");
                ValidateChecklistState(result, model.EstadoCcssChecklist, "CCSS");
                ValidateChecklistState(result, model.EstadoFodesafChecklist, "FODESAF");
                ValidateChecklistState(result, model.EstadoInsChecklist, "INS");
                ValidateChecklistState(result, model.EstadoDeclaracionJuradaChecklist, "Declaración jurada");
                ValidateChecklistState(result, model.EstadoComprobantePago, "Comprobante de pago mock");
                ValidateChecklistState(result, model.EstadoCroquis, "Croquis");

                if (!model.Requisitos.Any())
                {
                    result.IsValid = false;
                    result.Messages.Add("Debe existir al menos un requisito en el checklist demo.");
                }

                if (GetNonCompliantRequirementsCount() > 0)
                {
                    result.HasWarnings = true;
                    result.Messages.Add("Hay requisitos en estado No cumple; la resolución quedará condicionada a revisión.");
                }

                if (ContainsPendingChecklist())
                {
                    result.HasWarnings = true;
                    result.Messages.Add(model.TieneDeclaracionJurada
                        ? "La declaración jurada permite continuar con requisitos pendientes dentro del plazo mock."
                        : "Existen requisitos pendientes; en modo demo se permite continuar y el expediente queda pendiente de validación.");
                }

                if (model.TieneDeclaracionJurada)
                {
                    ValidateRequired(result, model.FechaInicioActividad, "Registre la fecha de inicio de actividad para la declaración jurada.");

                    if (model.DeclaracionJuradaDiasHabiles <= 0)
                    {
                        result.HasWarnings = true;
                        result.Messages.Add("Plazo vencido, requiere gestión administrativa.");
                    }
                    else if (model.DeclaracionJuradaDiasHabiles <= 10)
                    {
                        result.HasWarnings = true;
                        result.Messages.Add("Plazo próximo a vencer.");
                    }
                }
                break;
            case 6:
                ValidateExternalStatus(result, model.EstadoHacienda, "Hacienda");
                ValidateExternalStatus(result, model.EstadoCcss, "CCSS");
                ValidateExternalStatus(result, model.EstadoFodesaf, "FODESAF");
                ValidateExternalStatus(result, model.EstadoIns, "INS");
                ValidateRequired(result, model.FechaInicioActividad, "Registre la fecha de inicio de actividad.");
                ValidateRequired(result, model.RegimenTributario, "Registre el régimen tributario.");
                ValidateRequired(result, model.PeriodoFiscal, "Registre el período fiscal.");
                ValidateRequired(result, model.ActividadesEnOtrosCantones, "Registre actividades en otros cantones.");
                break;
            case 7:
                if (!model.SolicitanteAlDia || !model.DuenoPropiedadAlDia)
                    result.HasWarnings = true;

                ValidateRequired(result, model.ResultadoMorosidad, "Indique el resultado de morosidad.");
                ValidateRequired(result, model.ResultadoRevision, "Indique el resultado de revisión.");

                if (model.SolicitarInspeccion && string.IsNullOrWhiteSpace(model.InspectorAsignado))
                {
                    result.IsValid = false;
                    result.Messages.Add("Asigne un inspector para continuar.");
                }

                if (result.HasWarnings)
                    result.Messages.Add("Existen alertas de morosidad o revisión que no bloquean el demo.");
                break;
            case 8:
                ValidateRequired(result, model.TipoTasacion, "Defina el tipo de tasación.");
                ValidateRequired(result, model.EstadoTasacion, "Defina el estado de tasación.");
                ValidateRequired(result, model.FechaInicioCobro, "Registre la fecha de inicio del cobro mock.");
                ValidateRequired(result, model.EstadoCobro, "Defina el estado de cobro.");
                ValidateRequired(result, model.EstadoCuentaTributariaMock, "Registre el estado de cuenta tributaria mock.");
                ValidateRequired(result, model.ReferenciaCuentaPorCobrarMock, "Registre la referencia de cuenta por cobrar mock.");
                ValidateRequired(result, model.EstadoResolucion, "Defina el estado de resolución.");
                ValidateRequired(result, model.ObservacionesResolucion, "Registre las observaciones de resolución.");
                ValidateRequired(result, model.FirmaDigitalReferencial, "Registre la firma digital referencial.");

                if (model.MontoAnualMock < 0 || model.MontoTrimestralMock < 0 || model.TimbreBiodiversidad < 0 || model.PublicidadExterior < 0 || model.Multa < 0 || model.Intereses < 0)
                {
                    result.IsValid = false;
                    result.Messages.Add("Los montos mock no pueden ser negativos.");
                }

                if (HasApprovalBlockers())
                {
                    result.HasWarnings = true;
                    result.Messages.AddRange(GetApprovalBlockingMessages());

                    if (model.EstadoResolucion.Equals("Aprobada", StringComparison.OrdinalIgnoreCase))
                    {
                        result.IsValid = false;
                        result.Messages.Add("No es posible aprobar directamente. El expediente queda Pendiente validación o Prevenido en modo demo.");
                    }
                }

                if (model.EstadoResolucion.Equals("Pendiente firma", StringComparison.OrdinalIgnoreCase) && !model.NotificacionSimulada)
                {
                    result.HasWarnings = true;
                    result.Messages.Add("La resolución mock está pendiente de firma y notificación simulada.");
                }
                break;
        }

        if (!result.Messages.Any())
            result.Messages.Add("Paso validado correctamente.");

        return result;
    }

    private static void ValidateExternalStatus(StepValidationResultDto result, string estado, string entidad)
    {
        if (string.IsNullOrWhiteSpace(estado) || estado.Equals("Pendiente", StringComparison.OrdinalIgnoreCase))
        {
            result.IsValid = false;
            result.Messages.Add($"Actualice el estado de {entidad}.");
            return;
        }

        if (estado.Equals("Observado", StringComparison.OrdinalIgnoreCase))
        {
            result.HasWarnings = true;
            result.Messages.Add($"{entidad} se encuentra observado en el demo.");
        }
    }

    private static void ValidateChecklistState(StepValidationResultDto result, string estado, string etiqueta)
    {
        if (string.IsNullOrWhiteSpace(estado))
        {
            result.IsValid = false;
            result.Messages.Add($"Defina el estado de {etiqueta}.");
            return;
        }

        if (estado.Equals("No cumple", StringComparison.OrdinalIgnoreCase))
        {
            result.HasWarnings = true;
            result.Messages.Add($"{etiqueta} está marcado como no cumple.");
            return;
        }

        if (estado.Equals("Pendiente", StringComparison.OrdinalIgnoreCase))
            result.HasWarnings = true;
    }

    private bool IsActivityWithoutPhysicalLocal()
        => !model.RequiereLocalFisico || model.TipoUbicacion.Equals("Actividad sin local físico", StringComparison.OrdinalIgnoreCase);

    private bool ContainsPendingChecklist()
    {
        if (model.Requisitos.Any())
            return model.Requisitos.Any(x => x.Estado.Equals("Pendiente", StringComparison.OrdinalIgnoreCase));

        var checklist = new[]
        {
            model.EstadoIdentificacion,
            model.EstadoPersoneriaJuridica,
            model.EstadoUsoSueloChecklist,
            model.EstadoPermisoSanitario,
            model.EstadoArrendamientoPropiedad,
            model.EstadoCcssChecklist,
            model.EstadoFodesafChecklist,
            model.EstadoInsChecklist,
            model.EstadoDeclaracionJuradaChecklist,
            model.EstadoComprobantePago,
            model.EstadoCroquis
        };

        return checklist.Any(x => x.Equals("Pendiente", StringComparison.OrdinalIgnoreCase));
    }

    private bool IsMorosityObserved()
        => model.ResultadoMorosidad.Contains("observ", StringComparison.OrdinalIgnoreCase)
           || model.ResultadoMorosidad.Equals("Prevenido", StringComparison.OrdinalIgnoreCase)
           || model.ResultadoMorosidad.Equals("Observado", StringComparison.OrdinalIgnoreCase);

    private bool LegacyRequirementsReadyForApproval()
    {
        var requisitosOk = model.EstadoRequisitos.Equals("Completos", StringComparison.OrdinalIgnoreCase)
                           || model.EstadoRequisitos.Equals("Cumple", StringComparison.OrdinalIgnoreCase)
                           || model.EstadoRequisitos.Equals("No aplica", StringComparison.OrdinalIgnoreCase);
        var declaracionOk = model.EstadoDeclaracionJurada.Equals("Sí", StringComparison.OrdinalIgnoreCase)
                            || model.EstadoDeclaracionJurada.Equals("No", StringComparison.OrdinalIgnoreCase)
                            || model.EstadoDeclaracionJurada.Equals("Aceptada", StringComparison.OrdinalIgnoreCase)
                            || model.EstadoDeclaracionJurada.Equals("Cumple", StringComparison.OrdinalIgnoreCase);
        return requisitosOk && declaracionOk;
    }

    private static void ValidateRequired(StepValidationResultDto result, string value, string message)
    {
        if (!string.IsNullOrWhiteSpace(value))
            return;

        result.IsValid = false;
        result.Messages.Add(message);
    }

    private List<string> BuildSummaryItems()
    {
        var items = new List<string>
        {
            $"Solicitud demo: {model.NumeroSolicitud}",
            $"Expediente demo: {model.NumeroExpediente}",
            $"Tipo de solicitud: {GetDisplayValue(model.TipoSolicitud)}",
            $"Tipo de licencia: {GetDisplayValue(model.TipoLicencia)}",
            $"Canal de ingreso: {GetDisplayValue(model.CanalIngreso)}",
            $"Estado del trámite: {GetGeneralWorkflowState()}",
            $"Modalidad Declaración Jurada: {GetDisplayValue(model.ModalidadDeclaracionJurada)}",
            $"Solicitante: {(model.PendienteRegistroContribuyente ? "Pendiente de registro" : GetDisplayValue(model.ContribuyenteNombre))}",
            $"Identificación: {GetDisplayValue(model.Identificacion)}",
            $"Tipo de persona: {GetDisplayValue(model.TipoPersona)}",
            $"Correo / teléfono: {GetDisplayValue(model.Correo)} · {GetDisplayValue(model.Telefono)}",
            $"Medio de notificación: {GetDisplayValue(model.MedioNotificacion)}",
            $"Local/finca: {ResolveLocationSummary()}",
            $"Distrito: {GetDisplayValue(model.Distrito)}",
            $"Condición de ocupación: {GetDisplayValue(model.CondicionOcupacion)}",
            $"Actividad económica: {GetDisplayValue(model.ActividadEconomica)}",
            $"Actividad principal: {GetDisplayValue(model.ActividadPrincipal)}",
            $"CAECR/CIIU visual: {GetDisplayValue(model.CodigoCaecr)} / {GetDisplayValue(model.CodigoCiiuVisual)}",
            $"Riesgo / licores / sanitario / inspección: {GetDisplayValue(model.RiesgoActividad)} · {(model.RequiereLicores ? "Sí" : "No")} · {(model.RequierePermisoSanitario ? "Sí" : "No")} · {(model.RequiereInspeccion ? "Sí" : "No")}",
            $"Uso de Suelo: certificado {GetDisplayValue(model.NumeroCertificadoUsoSuelo)} · estado {GetDisplayValue(model.EstadoUsoSuelo)} · compatibilidad {GetDisplayValue(model.CompatibilidadUsoSuelo)} · resultado {GetDisplayValue(model.ResultadoUsoSuelo)}",
            $"Requisitos: {GetRequirementsCompletionPercent()}% completos · {GetCompletedRequirementsCount()} cumplidos · {GetPendingRequirementsCount()} pendientes · {GetNonCompliantRequirementsCount()} no cumplidos",
            $"Declaración Jurada: {(model.TieneDeclaracionJurada ? GetDeclaracionJuradaDeadlineMessage() : "No aplica.")}",
            $"Hacienda / CCSS / FODESAF / INS: {GetDisplayValue(model.EstadoHacienda)} / {GetDisplayValue(model.EstadoCcss)} / {GetDisplayValue(model.EstadoFodesaf)} / {GetDisplayValue(model.EstadoIns)}",
            $"Morosidad / inspección / revisión: {GetDisplayValue(model.ResultadoMorosidad)} / {GetDisplayValue(model.EstadoInspeccion)} / {GetDisplayValue(model.ResultadoRevision)}",
            $"Tasación mock: anual {FormatCurrency(model.MontoAnualMock)} · trimestral {FormatCurrency(model.MontoTrimestralMock)}",
            $"Cobro mock: {GetDisplayValue(model.EstadoCobro)} · cuenta {GetDisplayValue(model.EstadoCuentaTributariaMock)}",
            $"Resolución demo: {GetDisplayValue(model.EstadoResolucion)} · observaciones {GetDisplayValue(model.ObservacionesResolucion)}",
            $"Bloqueos de aprobación: {GetApprovalBlockersSummary()}",
            $"Resultado esperado de finalización: {GetFinalizationOutcomePreview()}"
        };

        items.AddRange(BuildIntegrationSummaryItems());

        if (HasApprovalBlockers())
            items.Add($"Alerta de cierre: no es posible aprobar directamente. Estado sugerido {GetFinalizationOutcomePreview()}.");

        return items;
    }

    private string ResolveLocationSummary()
    {
        if (IsActivityWithoutPhysicalLocal())
            return "Sin local físico";

        var finca = string.IsNullOrWhiteSpace(model.FincaOIdPredial) ? "Sin finca" : model.FincaOIdPredial;
        var direccion = string.IsNullOrWhiteSpace(model.DireccionLocal) ? "Dirección pendiente" : model.DireccionLocal;
        return $"{direccion} · {finca}";
    }

}
