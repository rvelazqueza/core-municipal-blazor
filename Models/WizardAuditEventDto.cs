using System;

namespace BlazorApp.Models;

public class WizardAuditEventDto
{
    public string EventId { get; set; } = string.Empty;
    public string ProcessId { get; set; } = string.Empty;
    public string ModuleName { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string User { get; set; } = "demo.user";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string Severity { get; set; } = "Info";
}
