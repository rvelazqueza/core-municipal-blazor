namespace BlazorApp.Models;

public class MmRequirementDto
{
    public string Id { get; set; } = string.Empty;
    public string LocalId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string DocumentPlaceholder { get; set; } = string.Empty;
    public string Observation { get; set; } = string.Empty;
    public string ReceivedBy { get; set; } = string.Empty;
    public DateTime? ReceivedAt { get; set; }
}
