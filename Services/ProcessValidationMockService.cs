using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using BlazorApp.Models;

namespace BlazorApp.Services;

public class ProcessValidationMockService : IProcessValidationService
{
    public Task<List<StepValidationResultDto>> ValidateStepAsync(string moduleName, string processName, string stepId, IReadOnlyDictionary<string, string?> payload, IReadOnlyCollection<string>? requiredFields = null)
        => Task.FromResult(Validate(stepId, payload, requiredFields));

    public Task<List<StepValidationResultDto>> ValidateProcessAsync(string moduleName, string processName, IReadOnlyDictionary<string, string?> payload, IReadOnlyCollection<string>? requiredFields = null)
        => Task.FromResult(Validate("process", payload, requiredFields));

    private static List<StepValidationResultDto> Validate(string stepId, IReadOnlyDictionary<string, string?> payload, IReadOnlyCollection<string>? requiredFields)
    {
        var results = new List<StepValidationResultDto>();
        var required = requiredFields?.Select(item => item.Trim()).Where(item => !string.IsNullOrWhiteSpace(item)).ToHashSet(StringComparer.OrdinalIgnoreCase)
                       ?? new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var field in required)
        {
            if (!payload.TryGetValue(field, out var value) || string.IsNullOrWhiteSpace(value))
                results.Add(Build(stepId, field, $"El campo {field} es obligatorio.", "Error", false));
        }

        foreach (var entry in payload)
        {
            var key = entry.Key ?? string.Empty;
            var value = entry.Value ?? string.Empty;
            var normalizedKey = key.Trim().ToLowerInvariant();

            if ((normalizedKey.Contains("correo") || normalizedKey.Contains("email")) && !string.IsNullOrWhiteSpace(value) && !value.Contains('@'))
                results.Add(Build(stepId, key, "El correo debe tener un formato válido.", "Error", false));

            if ((normalizedKey.Contains("telefono") || normalizedKey.Contains("phone")) && !string.IsNullOrWhiteSpace(value) && value.Count(char.IsDigit) < 8)
                results.Add(Build(stepId, key, "El teléfono debe tener al menos 8 dígitos.", "Warning", true, true));

            if ((normalizedKey.Contains("identificacion") || normalizedKey.Contains("identification")) && string.IsNullOrWhiteSpace(value) && required.Contains(key))
                results.Add(Build(stepId, key, "La identificación es obligatoria.", "Error", false));

            if ((normalizedKey.Contains("direccion") || normalizedKey.Contains("address")) && string.IsNullOrWhiteSpace(value) && required.Contains(key))
                results.Add(Build(stepId, key, "La dirección es obligatoria.", "Error", false));

            if (normalizedKey.Contains("morosidad") && (value.Equals("Observada", StringComparison.OrdinalIgnoreCase) || value.Equals("Morosa", StringComparison.OrdinalIgnoreCase)))
                results.Add(Build(stepId, key, "La morosidad se registra como advertencia en el estándar reusable.", "Warning", true, true));

            if (normalizedKey.Contains("usosuelo") && (value.Equals("No conforme", StringComparison.OrdinalIgnoreCase) || value.Equals("Vencido", StringComparison.OrdinalIgnoreCase)))
                results.Add(Build(stepId, key, "El estado de uso de suelo puede bloquear el avance si el módulo así lo define.", "Error", false));

            if (normalizedKey.Contains("cuentatributaria") && value.Equals("Pendiente", StringComparison.OrdinalIgnoreCase))
                results.Add(Build(stepId, key, "La cuenta tributaria mock está pendiente.", "Warning", true, true));

            if (normalizedKey.Contains("gis") && value.Equals("Pendiente", StringComparison.OrdinalIgnoreCase))
                results.Add(Build(stepId, key, "El GIS mock está pendiente de validación.", "Warning", true, true));
        }

        TryValidateDateRange(stepId, payload, results);

        if (!results.Any())
            results.Add(Build(stepId, "summary", "Validación mock correcta.", "Success", true));

        return results;
    }

    private static void TryValidateDateRange(string stepId, IReadOnlyDictionary<string, string?> payload, List<StepValidationResultDto> results)
    {
        if (!TryGetDate(payload, "FechaInicio", out var startDate) || !TryGetDate(payload, "FechaFin", out var endDate))
            return;

        if (startDate <= endDate)
            return;

        results.Add(Build(stepId, "FechaFin", "La fecha inicio debe ser menor o igual a la fecha fin.", "Error", false));
    }

    private static bool TryGetDate(IReadOnlyDictionary<string, string?> payload, string key, out DateTime value)
    {
        value = default;
        if (!payload.TryGetValue(key, out var rawValue) || string.IsNullOrWhiteSpace(rawValue))
            return false;

        return DateTime.TryParse(rawValue, CultureInfo.InvariantCulture, DateTimeStyles.None, out value)
            || DateTime.TryParse(rawValue, CultureInfo.CurrentCulture, DateTimeStyles.None, out value);
    }

    private static StepValidationResultDto Build(string stepId, string field, string message, string severity, bool isValid, bool hasWarnings = false)
    {
        return new StepValidationResultDto
        {
            StepId = stepId,
            Field = field,
            Message = message,
            Severity = severity,
            IsValid = isValid,
            HasWarnings = hasWarnings,
            Messages = new List<string> { message }
        };
    }
}
