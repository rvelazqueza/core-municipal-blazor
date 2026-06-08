using System.Threading.Tasks;
using BlazorApp.Models;

namespace BlazorApp.Services;

public interface IDraftService
{
    Task<DraftStateDto> SaveDraftAsync(WizardStateDto state, object? payload = null);
    Task<DraftStateDto?> GetDraftAsync(string draftId);
    Task DeleteDraftAsync(string draftId);
    Task<DraftStateDto> MarkDirtyAsync(WizardStateDto state);
    Task<DraftStateDto> GetDraftStatusAsync(string processId);
}
