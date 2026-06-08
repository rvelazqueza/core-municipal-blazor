namespace BlazorApp.Models;

public class MmHistoryEventDto
{
    public string Id { get; set; } = string.Empty;
    public string LocalId { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string User { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string PreviousStatus { get; set; } = string.Empty;
    public string NewStatus { get; set; } = string.Empty;
    public string PreviousValue { get; set; } = string.Empty;
    public string NewValue { get; set; } = string.Empty;
    public string SourceDocument { get; set; } = string.Empty;
    public string SourceProcess { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;
}
