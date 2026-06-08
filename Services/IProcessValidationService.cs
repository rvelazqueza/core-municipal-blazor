using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorApp.Models;

namespace BlazorApp.Services;

public interface IProcessValidationService
{
    Task<List<StepValidationResultDto>> ValidateStepAsync(string moduleName, string processName, string stepId, IReadOnlyDictionary<string, string?> payload, IReadOnlyCollection<string>? requiredFields = null);
    Task<List<StepValidationResultDto>> ValidateProcessAsync(string moduleName, string processName, IReadOnlyDictionary<string, string?> payload, IReadOnlyCollection<string>? requiredFields = null);
}
