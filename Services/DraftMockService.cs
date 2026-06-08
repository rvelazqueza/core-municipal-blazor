using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using BlazorApp.Models;

namespace BlazorApp.Services;

public class DraftMockService : IDraftService
{
    private static readonly ConcurrentDictionary<string, DraftStateDto> Store = new();

    public Task<DraftStateDto> SaveDraftAsync(WizardStateDto state, object? payload = null)
    {
        var draft = new DraftStateDto
        {
            DraftId = string.IsNullOrWhiteSpace(state.DraftState?.DraftId) ? $"draft-{Guid.NewGuid():N}" : state.DraftState.DraftId,
            ProcessId = !string.IsNullOrWhiteSpace(state.ProcessId) ? state.ProcessId : state.ProcessKey,
            ModuleName = state.ModuleName,
            ProcessName = string.IsNullOrWhiteSpace(state.ProcessName) ? state.ProcessTitle : state.ProcessName,
            Status = "Guardado",
            LastSavedAt = DateTime.Now,
            SavedBy = string.IsNullOrWhiteSpace(state.CreatedBy) ? "demo.user" : state.CreatedBy,
            HasUnsavedChanges = false,
            Message = "Borrador guardado correctamente."
        };

        Store[draft.DraftId] = draft;
        return Task.FromResult(Clone(draft));
    }

    public Task<DraftStateDto?> GetDraftAsync(string draftId)
    {
        if (string.IsNullOrWhiteSpace(draftId))
            return Task.FromResult<DraftStateDto?>(null);

        Store.TryGetValue(draftId, out var draft);
        return Task.FromResult(draft is null ? null : Clone(draft));
    }

    public Task DeleteDraftAsync(string draftId)
    {
        if (!string.IsNullOrWhiteSpace(draftId))
            Store.TryRemove(draftId, out _);

        return Task.CompletedTask;
    }

    public Task<DraftStateDto> MarkDirtyAsync(WizardStateDto state)
    {
        var draft = new DraftStateDto
        {
            DraftId = string.IsNullOrWhiteSpace(state.DraftState?.DraftId) ? $"draft-{Guid.NewGuid():N}" : state.DraftState.DraftId,
            ProcessId = !string.IsNullOrWhiteSpace(state.ProcessId) ? state.ProcessId : state.ProcessKey,
            ModuleName = state.ModuleName,
            ProcessName = string.IsNullOrWhiteSpace(state.ProcessName) ? state.ProcessTitle : state.ProcessName,
            Status = "Sin guardar",
            LastSavedAt = state.LastSavedAt,
            SavedBy = string.IsNullOrWhiteSpace(state.CreatedBy) ? "demo.user" : state.CreatedBy,
            HasUnsavedChanges = true,
            Message = "Hay cambios pendientes de guardar."
        };

        Store[draft.DraftId] = draft;
        return Task.FromResult(Clone(draft));
    }

    public Task<DraftStateDto> GetDraftStatusAsync(string processId)
    {
        foreach (var entry in Store.Values)
        {
            if (entry.ProcessId.Equals(processId ?? string.Empty, StringComparison.OrdinalIgnoreCase))
                return Task.FromResult(Clone(entry));
        }

        return Task.FromResult(new DraftStateDto
        {
            ProcessId = processId,
            Status = "Sin guardar",
            HasUnsavedChanges = false,
            Message = "No existe borrador registrado."
        });
    }

    private static DraftStateDto Clone(DraftStateDto source)
    {
        return new DraftStateDto
        {
            DraftId = source.DraftId,
            ProcessId = source.ProcessId,
            ModuleName = source.ModuleName,
            ProcessName = source.ProcessName,
            Status = source.Status,
            LastSavedAt = source.LastSavedAt,
            SavedBy = source.SavedBy,
            HasUnsavedChanges = source.HasUnsavedChanges,
            Message = source.Message
        };
    }
}
