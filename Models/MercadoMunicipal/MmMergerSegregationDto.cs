namespace BlazorApp.Models;

public class MmMergerSegregationDto
{
    public string Id { get; set; } = string.Empty;
    public string LocalId { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string SourceLocal { get; set; } = string.Empty;
    public string TargetLocal { get; set; } = string.Empty;
    public decimal PreviousArea { get; set; }
    public decimal NewArea { get; set; }
    public int AffectedReceivables { get; set; }
    public bool RequiresCouncilAgreement { get; set; }
    public string Status { get; set; } = string.Empty;
}
