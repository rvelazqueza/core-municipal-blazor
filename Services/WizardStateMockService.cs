using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorApp.Models;

namespace BlazorApp.Services;

public class WizardStateMockService : IWizardStateService
{
    private static readonly ConcurrentDictionary<string, WizardStateDto> Store = new();

    public Task<WizardStateDto?> GetStateAsync(string processKey)
    {
        if (string.IsNullOrWhiteSpace(processKey))
            return Task.FromResult<WizardStateDto?>(null);

        Store.TryGetValue(processKey, out var state);
        return Task.FromResult(state is null ? null : Clone(state));
    }

    public Task<WizardStateDto> SaveDraftAsync(WizardStateDto state)
    {
        var draft = Clone(state);
        draft.Status = "Borrador";
        draft.LastSavedAt = DateTime.Now;
        draft.UpdatedAt = draft.LastSavedAt;
        draft.IsDirty = false;
        draft.TotalSteps = ResolveTotalSteps(draft);
        draft.ProgressPercent = ResolveProgress(draft);
        draft.ProgressPercentage = (decimal)draft.ProgressPercent;
        draft.DraftState = BuildDraftState(draft, "Guardado", false, "Borrador guardado correctamente.");
        Store[ResolveKey(draft)] = draft;
        return Task.FromResult(Clone(draft));
    }

    public Task<WizardStateDto> UpdateStateAsync(WizardStateDto state)
    {
        var updated = NormalizeState(Clone(state));
        Store[ResolveKey(updated)] = updated;
        return Task.FromResult(Clone(updated));
    }

    public Task ClearStateAsync(string processKey)
    {
        if (!string.IsNullOrWhiteSpace(processKey))
            Store.TryRemove(processKey, out _);

        return Task.CompletedTask;
    }

    public WizardStateDto GetInitialState(string moduleName, string processName, IEnumerable<WizardStepDto> steps, string? processId = null)
    {
        var orderedSteps = steps.OrderBy(step => step.Order == 0 ? step.Index : step.Order).ToList();
        for (var index = 0; index < orderedSteps.Count; index++)
        {
            orderedSteps[index].Index = index;
            orderedSteps[index].Order = index;
            orderedSteps[index].IsActive = index == 0;
            orderedSteps[index].Status = index == 0 ? "En progreso" : "Pendiente";
            if (string.IsNullOrWhiteSpace(orderedSteps[index].Id))
                orderedSteps[index].Id = $"step-{index + 1}";
        }

        var resolvedProcessId = processId ?? Guid.NewGuid().ToString("N");
        var state = new WizardStateDto
        {
            ProcessId = resolvedProcessId,
            ProcessKey = resolvedProcessId,
            ProcessName = processName,
            ProcessTitle = processName,
            ModuleName = moduleName,
            Status = "En progreso",
            StartedAt = DateTime.Now,
            UpdatedAt = DateTime.Now,
            CurrentStepIndex = 0,
            CurrentStepId = orderedSteps.FirstOrDefault()?.Id ?? string.Empty,
            TotalSteps = orderedSteps.Count,
            Steps = orderedSteps,
            DraftState = new DraftStateDto
            {
                DraftId = $"draft-{Guid.NewGuid():N}",
                ProcessId = resolvedProcessId,
                ModuleName = moduleName,
                ProcessName = processName,
                Status = "Sin guardar",
                HasUnsavedChanges = false,
                Message = "Proceso iniciado en modo demo."
            }
        };

        state.ProgressPercent = ResolveProgress(state);
        state.ProgressPercentage = (decimal)state.ProgressPercent;
        return state;
    }

    public Task<WizardStateDto> GoNextAsync(WizardStateDto state)
    {
        var updated = NormalizeState(Clone(state));
        var currentStep = GetCurrentStep(updated);
        if (currentStep is not null && HasBlockingErrors(currentStep))
            return Task.FromResult(Clone(updated));

        if (currentStep is not null)
        {
            currentStep.IsCompleted = true;
            if (!currentStep.HasErrors)
                currentStep.Status = currentStep.HasWarnings ? "Con alertas" : "Completado";
        }

        if (updated.CurrentStepIndex < updated.TotalSteps - 1)
            updated.CurrentStepIndex++;

        SyncActiveStep(updated);
        updated.Status = "En progreso";
        updated.IsDirty = true;
        updated.UpdatedAt = DateTime.Now;
        Store[ResolveKey(updated)] = updated;
        return Task.FromResult(Clone(updated));
    }

    public Task<WizardStateDto> GoPreviousAsync(WizardStateDto state)
    {
        var updated = NormalizeState(Clone(state));
        if (updated.CurrentStepIndex > 0)
            updated.CurrentStepIndex--;

        SyncActiveStep(updated);
        updated.IsDirty = true;
        updated.UpdatedAt = DateTime.Now;
        Store[ResolveKey(updated)] = updated;
        return Task.FromResult(Clone(updated));
    }

    public Task<WizardStateDto> GoToStepAsync(WizardStateDto state, string stepId)
    {
        var updated = NormalizeState(Clone(state));
        var targetIndex = updated.Steps.FindIndex(step => step.Id == stepId);
        if (targetIndex < 0)
            return Task.FromResult(Clone(updated));

        if (targetIndex > updated.CurrentStepIndex)
        {
            var blockingStepExists = updated.Steps
                .Where(step => step.Index < targetIndex && !step.IsOptional)
                .Any(step => !step.IsCompleted || step.HasErrors);
            if (blockingStepExists)
                return Task.FromResult(Clone(updated));
        }

        updated.CurrentStepIndex = targetIndex;
        SyncActiveStep(updated);
        updated.IsDirty = true;
        updated.UpdatedAt = DateTime.Now;
        Store[ResolveKey(updated)] = updated;
        return Task.FromResult(Clone(updated));
    }

    public Task<WizardStateDto> UpdateStepValidationAsync(WizardStateDto state, string stepId, IEnumerable<StepValidationResultDto> validations)
    {
        var updated = NormalizeState(Clone(state));
        var step = updated.Steps.FirstOrDefault(item => item.Id == stepId);
        if (step is null)
            return Task.FromResult(Clone(updated));

        var stepValidations = validations.ToList();
        step.Validations = stepValidations;
        step.ValidationMessages = stepValidations
            .SelectMany(item => item.Messages.Any() ? item.Messages : new List<string> { item.Message })
            .Where(item => !string.IsNullOrWhiteSpace(item))
            .Distinct()
            .ToList();
        step.HasErrors = stepValidations.Any(item => !item.IsValid && !IsWarningOnly(item));
        step.HasWarnings = stepValidations.Any(item => item.HasWarnings || IsWarningOnly(item));
        step.Status = step.HasErrors ? "Con errores" : step.HasWarnings ? "Con alertas" : step.IsCompleted ? "Completado" : "Pendiente";

        updated.ValidationResults.RemoveAll(item => item.StepId == stepId || item.StepIndex == step.Index);
        updated.ValidationResults.Add(BuildStepSummary(step));
        updated.PendingValidations = updated.ValidationResults
            .Where(item => !item.IsValid || item.HasWarnings)
            .SelectMany(item => item.Messages)
            .Where(item => !string.IsNullOrWhiteSpace(item))
            .Distinct()
            .ToList();
        updated.IsDirty = true;
        updated.UpdatedAt = DateTime.Now;
        Store[ResolveKey(updated)] = updated;
        return Task.FromResult(Clone(updated));
    }

    public Task<double> CalculateProgressAsync(WizardStateDto state)
    {
        var updated = NormalizeState(Clone(state));
        return Task.FromResult(updated.ProgressPercent);
    }

    public Task<WizardStateDto> MarkStepCompletedAsync(WizardStateDto state, string stepId, bool isCompleted = true)
    {
        var updated = NormalizeState(Clone(state));
        var step = updated.Steps.FirstOrDefault(item => item.Id == stepId);
        if (step is null)
            return Task.FromResult(Clone(updated));

        step.IsCompleted = isCompleted;
        step.Status = isCompleted ? (step.HasWarnings ? "Con alertas" : "Completado") : "Pendiente";
        updated.UpdatedAt = DateTime.Now;
        updated.IsDirty = true;
        updated.ProgressPercent = ResolveProgress(updated);
        updated.ProgressPercentage = (decimal)updated.ProgressPercent;
        Store[ResolveKey(updated)] = updated;
        return Task.FromResult(Clone(updated));
    }

    public Task<WizardStateDto> SetStatusAsync(WizardStateDto state, string status)
    {
        var updated = NormalizeState(Clone(state));
        updated.Status = status;
        updated.UpdatedAt = DateTime.Now;
        if (status.Equals("Completado", StringComparison.OrdinalIgnoreCase))
            updated.FinishedAt = DateTime.Now;
        Store[ResolveKey(updated)] = updated;
        return Task.FromResult(Clone(updated));
    }

    private static WizardStateDto NormalizeState(WizardStateDto state)
    {
        state.ProcessId = string.IsNullOrWhiteSpace(state.ProcessId) ? ResolveKey(state) : state.ProcessId;
        state.ProcessKey = ResolveKey(state);
        state.ProcessName = string.IsNullOrWhiteSpace(state.ProcessName) ? state.ProcessTitle : state.ProcessName;
        state.ProcessTitle = string.IsNullOrWhiteSpace(state.ProcessTitle) ? state.ProcessName : state.ProcessTitle;
        state.TotalSteps = ResolveTotalSteps(state);
        for (var index = 0; index < state.Steps.Count; index++)
        {
            state.Steps[index].Index = index;
            state.Steps[index].Order = index;
            if (string.IsNullOrWhiteSpace(state.Steps[index].Id))
                state.Steps[index].Id = $"step-{index + 1}";
            state.Steps[index].Status = ResolveStepStatus(state.Steps[index]);
        }

        SyncActiveStep(state);
        state.ProgressPercent = ResolveProgress(state);
        state.ProgressPercentage = (decimal)state.ProgressPercent;
        state.DraftState ??= BuildDraftState(state, "Sin guardar", state.IsDirty, string.Empty);
        return state;
    }

    private static void SyncActiveStep(WizardStateDto state)
    {
        if (state.CurrentStepIndex < 0)
            state.CurrentStepIndex = 0;
        if (state.CurrentStepIndex >= state.TotalSteps && state.TotalSteps > 0)
            state.CurrentStepIndex = state.TotalSteps - 1;

        for (var index = 0; index < state.Steps.Count; index++)
        {
            state.Steps[index].IsActive = index == state.CurrentStepIndex;
            state.Steps[index].Status = ResolveStepStatus(state.Steps[index]);
        }

        state.CurrentStepId = GetCurrentStep(state)?.Id ?? string.Empty;
    }

    private static WizardStepDto? GetCurrentStep(WizardStateDto state)
        => state.Steps.FirstOrDefault(step => step.Index == state.CurrentStepIndex);

    private static bool HasBlockingErrors(WizardStepDto step)
        => step.HasErrors || step.Validations.Any(item => !item.IsValid && !IsWarningOnly(item));

    private static bool IsWarningOnly(StepValidationResultDto result)
        => result.Severity.Equals("Warning", StringComparison.OrdinalIgnoreCase) || result.HasWarnings;

    private static StepValidationResultDto BuildStepSummary(WizardStepDto step)
    {
        var messages = step.ValidationMessages.Any() ? step.ValidationMessages : new List<string> { step.Title };
        return new StepValidationResultDto
        {
            StepId = step.Id,
            StepIndex = step.Index,
            StepTitle = step.Title,
            Severity = step.HasErrors ? "Error" : step.HasWarnings ? "Warning" : "Success",
            IsValid = !step.HasErrors,
            HasWarnings = step.HasWarnings,
            Message = messages.FirstOrDefault() ?? string.Empty,
            Messages = messages
        };
    }

    private static DraftStateDto BuildDraftState(WizardStateDto state, string status, bool hasUnsavedChanges, string message)
    {
        return new DraftStateDto
        {
            DraftId = string.IsNullOrWhiteSpace(state.DraftState?.DraftId) ? $"draft-{Guid.NewGuid():N}" : state.DraftState.DraftId,
            ProcessId = ResolveKey(state),
            ModuleName = state.ModuleName,
            ProcessName = string.IsNullOrWhiteSpace(state.ProcessName) ? state.ProcessTitle : state.ProcessName,
            Status = status,
            LastSavedAt = state.LastSavedAt,
            SavedBy = string.IsNullOrWhiteSpace(state.CreatedBy) ? "demo.user" : state.CreatedBy,
            HasUnsavedChanges = hasUnsavedChanges,
            Message = message
        };
    }

    private static string ResolveStepStatus(WizardStepDto step)
    {
        if (step.IsSkipped)
            return "Omitido";
        if (step.HasErrors)
            return "Con errores";
        if (step.HasWarnings)
            return "Con alertas";
        if (step.IsCompleted)
            return "Completado";
        if (step.IsActive)
            return "En progreso";
        return "Pendiente";
    }

    private static int ResolveTotalSteps(WizardStateDto state)
        => state.TotalSteps > 0 ? state.TotalSteps : state.Steps.Count;

    private static string ResolveKey(WizardStateDto state)
        => !string.IsNullOrWhiteSpace(state.ProcessKey)
            ? state.ProcessKey
            : !string.IsNullOrWhiteSpace(state.ProcessId)
                ? state.ProcessId
                : Guid.NewGuid().ToString("N");

    private static double ResolveProgress(WizardStateDto state)
    {
        var totalSteps = ResolveTotalSteps(state);
        if (totalSteps <= 0)
            return 0;

        var completedSteps = state.Steps.Count(step => step.IsCompleted);
        var currentPosition = Math.Min(Math.Max(state.CurrentStepIndex + 1, 0), totalSteps);
        var progressBase = Math.Max(completedSteps, currentPosition);
        return Math.Round((double)progressBase / totalSteps * 100d, 0);
    }

    private static WizardStateDto Clone(WizardStateDto source)
    {
        return new WizardStateDto
        {
            ProcessId = source.ProcessId,
            ProcessKey = source.ProcessKey,
            ProcessCode = source.ProcessCode,
            ModuleName = source.ModuleName,
            ProcessName = source.ProcessName,
            ProcessTitle = source.ProcessTitle,
            CurrentStepId = source.CurrentStepId,
            CurrentStepIndex = source.CurrentStepIndex,
            TotalSteps = source.TotalSteps,
            ProgressPercent = source.ProgressPercent,
            ProgressPercentage = source.ProgressPercentage,
            Status = source.Status,
            StartedAt = source.StartedAt,
            UpdatedAt = source.UpdatedAt,
            FinishedAt = source.FinishedAt,
            LastSavedAt = source.LastSavedAt,
            CreatedBy = source.CreatedBy,
            IsDirty = source.IsDirty,
            IsReadOnly = source.IsReadOnly,
            SummaryItems = source.SummaryItems.ToList(),
            PendingValidations = source.PendingValidations.ToList(),
            ProcessSummaryItems = source.ProcessSummaryItems.Select(item => new ProcessSummaryItemDto
            {
                Label = item.Label,
                Value = item.Value,
                Icon = item.Icon,
                Severity = item.Severity,
                Group = item.Group
            }).ToList(),
            DraftState = new DraftStateDto
            {
                DraftId = source.DraftState.DraftId,
                ProcessId = source.DraftState.ProcessId,
                ModuleName = source.DraftState.ModuleName,
                ProcessName = source.DraftState.ProcessName,
                Status = source.DraftState.Status,
                LastSavedAt = source.DraftState.LastSavedAt,
                SavedBy = source.DraftState.SavedBy,
                HasUnsavedChanges = source.DraftState.HasUnsavedChanges,
                Message = source.DraftState.Message
            },
            Steps = source.Steps.Select(step => new WizardStepDto
            {
                Id = step.Id,
                Order = step.Order,
                Index = step.Index,
                Title = step.Title,
                Description = step.Description,
                Subtitle = step.Subtitle,
                Icon = step.Icon,
                IsRequired = step.IsRequired,
                IsOptional = step.IsOptional,
                IsCompleted = step.IsCompleted,
                IsActive = step.IsActive,
                HasErrors = step.HasErrors,
                HasWarnings = step.HasWarnings,
                IsSkipped = step.IsSkipped,
                Status = step.Status,
                ValidationMessages = step.ValidationMessages.ToList(),
                Validations = step.Validations.Select(validation => new StepValidationResultDto
                {
                    StepId = validation.StepId,
                    StepIndex = validation.StepIndex,
                    StepTitle = validation.StepTitle,
                    Field = validation.Field,
                    Message = validation.Message,
                    Severity = validation.Severity,
                    IsValid = validation.IsValid,
                    HasWarnings = validation.HasWarnings,
                    Code = validation.Code,
                    Messages = validation.Messages.ToList()
                }).ToList()
            }).ToList(),
            ValidationResults = source.ValidationResults.Select(result => new StepValidationResultDto
            {
                StepId = result.StepId,
                StepIndex = result.StepIndex,
                StepTitle = result.StepTitle,
                Field = result.Field,
                Message = result.Message,
                Severity = result.Severity,
                IsValid = result.IsValid,
                HasWarnings = result.HasWarnings,
                Code = result.Code,
                Messages = result.Messages.ToList()
            }).ToList()
        };
    }
}
