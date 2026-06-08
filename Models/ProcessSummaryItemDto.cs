namespace BlazorApp.Models;

public class ProcessSummaryItemDto
{
    public string Label { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public string Severity { get; set; } = "Info";
    public string Group { get; set; } = string.Empty;
}
