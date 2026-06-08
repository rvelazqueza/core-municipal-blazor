namespace BlazorApp.Models;

public class MmAuditEventDto
{
    public string Id { get; set; } = string.Empty;
    public string LocalId { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string User { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string PreviousValue { get; set; } = string.Empty;
    public string CurrentValue { get; set; } = string.Empty;
    public string SourceDocument { get; set; } = string.Empty;
    public string Process { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;
    public string OutputReference { get; set; } = string.Empty;
}
