using System.Collections.Generic;
using System.Threading.Tasks;
using BlazorApp.Models;

namespace BlazorApp.Services;

public interface IWizardStateService
{
    Task<WizardStateDto?> GetStateAsync(string processKey);
    Task<WizardStateDto> SaveDraftAsync(WizardStateDto state);
    Task<WizardStateDto> UpdateStateAsync(WizardStateDto state);
    Task ClearStateAsync(string processKey);
    WizardStateDto GetInitialState(string moduleName, string processName, IEnumerable<WizardStepDto> steps, string? processId = null);
    Task<WizardStateDto> GoNextAsync(WizardStateDto state);
    Task<WizardStateDto> GoPreviousAsync(WizardStateDto state);
    Task<WizardStateDto> GoToStepAsync(WizardStateDto state, string stepId);
    Task<WizardStateDto> UpdateStepValidationAsync(WizardStateDto state, string stepId, IEnumerable<StepValidationResultDto> validations);
    Task<double> CalculateProgressAsync(WizardStateDto state);
    Task<WizardStateDto> MarkStepCompletedAsync(WizardStateDto state, string stepId, bool isCompleted = true);
    Task<WizardStateDto> SetStatusAsync(WizardStateDto state, string status);
}
