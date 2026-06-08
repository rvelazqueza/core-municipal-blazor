using System;

namespace BlazorApp.Models;

public class DraftStateDto
{
    public string DraftId { get; set; } = string.Empty;
    public string ProcessId { get; set; } = string.Empty;
    public string ModuleName { get; set; } = string.Empty;
    public string ProcessName { get; set; } = string.Empty;
    public string Status { get; set; } = "Sin guardar";
    public DateTime? LastSavedAt { get; set; }
    public string SavedBy { get; set; } = "demo.user";
    public bool HasUnsavedChanges { get; set; }
    public string Message { get; set; } = string.Empty;
}
