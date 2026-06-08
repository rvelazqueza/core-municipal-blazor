using System;
using System.Collections.Generic;
using System.Linq;
using BlazorApp.Models;

namespace BlazorApp.Shared;

public partial class BienInmuebleCreateWizard
{
    private List<WizardStepDto> BuildSteps()
    {
        return Enumerable.Range(0, TotalSteps)
            .Select(index =>
            {
                var validation = validationResults.ElementAtOrDefault(index) ?? new StepValidationResultDto();
                var isVisited = index <= currentStepIndex || currentStepIndex == TotalSteps - 1;
                return new WizardStepDto
                {
                    Index = index,
                    Title = stepTitles[index],
                    Subtitle = stepSubtitles[index],
                    Icon = stepIcons[index],
                    IsOptional = index == 4,
                    IsCompleted = index < currentStepIndex && validation.IsValid && !validation.HasWarnings,
                    HasErrors = validation.IsValid is false && isVisited,
                    HasWarnings = validation.HasWarnings && isVisited,
                    ValidationMessages = validation.Messages.ToList()
                };
            })
            .ToList();
    }

    private List<StepValidationResultDto> BuildValidationResults()
    {
        var results = Enumerable.Range(0, TotalSteps - 1).Select(BuildStepValidationResult).ToList();
        var allBlockingMessages = results.Where(x => !x.IsValid).SelectMany(x => x.Messages).Distinct().ToList();
        var allWarnings = results.Where(x => x.HasWarnings).SelectMany(x => x.Messages).Distinct().ToList();
        var reviewMessages = allBlockingMessages.Concat(allWarnings).Distinct().ToList();

        results.Add(new StepValidationResultDto
        {
            StepIndex = 5,
            StepTitle = stepTitles[5],
            IsValid = !results.Any(x => !x.IsValid),
            HasWarnings = results.Any(x => x.HasWarnings),
            Messages = reviewMessages
        });

        return results;
    }

    private List<string> BuildPendingValidations()
    {
        var relevantResults = currentStepIndex >= TotalSteps - 1
            ? validationResults
            : validationResults.Where(x => x.StepIndex <= currentStepIndex);

        return relevantResults
            .Where(x => !x.IsValid || x.HasWarnings)
            .SelectMany(x => x.Messages)
            .Distinct()
            .ToList();
    }

    private StepValidationResultDto BuildStepValidationResult(int stepIndex)
    {
        var blockingMessages = new List<string>();
        var warningMessages = new List<string>();
        var hasWarnings = false;

        switch (stepIndex)
        {
            case 0:
                if (string.IsNullOrWhiteSpace(Model.NumeroFinca) && string.IsNullOrWhiteSpace(Model.IdPredial))
                    blockingMessages.Add("El número de finca o el ID predial es obligatorio.");
                if (string.IsNullOrWhiteSpace(Model.TipoFinca))
                    blockingMessages.Add("El tipo de finca es obligatorio.");
                if (string.IsNullOrWhiteSpace(Model.EstadoRegistral))
                    blockingMessages.Add("El estado registral es obligatorio.");
                if (string.IsNullOrWhiteSpace(Model.Distrito))
                    blockingMessages.Add("El distrito es obligatorio.");
                if (string.IsNullOrWhiteSpace(Model.DireccionExacta))
                    blockingMessages.Add("La dirección exacta es obligatoria.");
                break;

            case 1:
                if (Model.PropietarioPrincipalId <= 0)
                    blockingMessages.Add("El propietario principal vinculado a RUC es obligatorio.");
                if (Model.PorcentajeDerecho < 0 || Model.PorcentajeDerecho > 100)
                    blockingMessages.Add("El porcentaje de derecho debe estar entre 0 y 100.");
                break;

            case 2:
                if (Model.AreaTerreno <= 0)
                    blockingMessages.Add("El área de terreno debe ser mayor que cero.");
                if (string.IsNullOrWhiteSpace(Model.UsoInmueble))
                    blockingMessages.Add("El uso del inmueble es obligatorio.");
                if (string.IsNullOrWhiteSpace(Model.ZonaHomogenea))
                    blockingMessages.Add("La zona homogénea es obligatoria.");
                if (string.IsNullOrWhiteSpace(Model.TipologiaConstructiva))
                    blockingMessages.Add("La tipología constructiva es obligatoria.");
                break;

            case 3:
                if (Model.ValorTerreno < 0)
                    blockingMessages.Add("El valor del terreno no puede ser negativo.");
                if (Model.ValorConstruccion < 0)
                    blockingMessages.Add("El valor de construcción no puede ser negativo.");
                break;

            case 4:
                if (!Model.RegistroPublicoSincronizado)
                {
                    warningMessages.Add(integrationWarnings[0]);
                    hasWarnings = true;
                }

                if (string.IsNullOrWhiteSpace(Model.UbicacionGis))
                {
                    warningMessages.Add(integrationWarnings[1]);
                    hasWarnings = true;
                }

                if (!Model.RemitidoHacienda)
                {
                    warningMessages.Add(integrationWarnings[2]);
                    hasWarnings = true;
                }

                if (string.IsNullOrWhiteSpace(Model.CuentaTributaria))
                {
                    warningMessages.Add(integrationWarnings[3]);
                    hasWarnings = true;
                }
                break;
        }

        return new StepValidationResultDto
        {
            StepIndex = stepIndex,
            StepTitle = stepTitles[stepIndex],
            IsValid = blockingMessages.Count == 0,
            HasWarnings = hasWarnings,
            Messages = blockingMessages.Concat(warningMessages).ToList()
        };
    }

    private List<string> BuildSummaryItems()
    {
        return
        [
            $"Finca: {Fallback(Model.NumeroFinca, Fallback(Model.IdPredial, "Pendiente"))}",
            $"Tipo: {Fallback(Model.TipoFinca, "Sin definir")}",
            $"Distrito: {Fallback(Model.Distrito, "Sin definir")}",
            $"Propietario: {Fallback(Model.PropietarioPrincipal, "Sin definir")}",
            $"Área: {Model.AreaTerreno:N2} m²",
            $"Valor fiscal: {Model.ValorFiscalTotal:N2}"
        ];
    }

    private List<string> BuildIdentificationSummary()
    {
        return
        [
            $"Número de finca: {Fallback(Model.NumeroFinca, "Pendiente")}",
            $"ID predial: {Fallback(Model.IdPredial, "Pendiente")}",
            $"Tipo: {Fallback(Model.TipoFinca, "Pendiente")}",
            $"Estado registral: {Fallback(Model.EstadoRegistral, "Pendiente")}",
            $"Distrito: {Fallback(Model.Distrito, "Pendiente")}",
            $"Dirección: {Fallback(Model.DireccionExacta, "Pendiente")}"
        ];
    }

    private List<string> BuildOwnershipSummary()
    {
        return
        [
            $"Propietario: {Fallback(Model.PropietarioPrincipal, "Pendiente")}",
            $"Identificación: {Fallback(Model.PropietarioPrincipalIdentificacion, "Pendiente")}",
            $"Porcentaje de derecho: {Model.PorcentajeDerecho:N2}%"
        ];
    }

    private List<string> BuildCharacteristicsSummary()
    {
        return
        [
            $"Área de terreno: {Model.AreaTerreno:N2} m²",
            $"Uso del inmueble: {Fallback(Model.UsoInmueble, "Pendiente")}",
            $"Zona homogénea: {Fallback(Model.ZonaHomogenea, "Pendiente")}",
            $"Tipología constructiva: {Fallback(Model.TipologiaConstructiva, "Pendiente")}"
        ];
    }

    private List<string> BuildValuationSummary()
    {
        return
        [
            $"Valor terreno: {Model.ValorTerreno:N2}",
            $"Valor construcción: {Model.ValorConstruccion:N2}",
            $"Valor fiscal total: {Model.ValorFiscalTotal:N2}",
            $"Registro Público: {(Model.RegistroPublicoSincronizado ? "Simulado" : "Pendiente")}",
            $"GIS: {(string.IsNullOrWhiteSpace(Model.UbicacionGis) ? "Pendiente" : "Simulado")}",
            $"Hacienda: {(Model.RemitidoHacienda ? "Simulado" : "Pendiente")}",
            $"Cobro: {(string.IsNullOrWhiteSpace(Model.CuentaTributaria) ? "Pendiente" : "Simulado")}"
        ];
    }

    private static string Fallback(string? value, string fallback)
        => string.IsNullOrWhiteSpace(value) ? fallback : value;
}