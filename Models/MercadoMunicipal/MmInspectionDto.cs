namespace BlazorApp.Models;

public class MmInspectionDto
{
    public string Id { get; set; } = string.Empty;
    public string LocalId { get; set; } = string.Empty;
    public string TenantId { get; set; } = string.Empty;
    public string InspectionNumber { get; set; } = string.Empty;
    public string LocalNumber { get; set; } = string.Empty;
    public string TenantName { get; set; } = string.Empty;
    public string Inspector { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Result { get; set; } = string.Empty;
    public string EvidencePlaceholder { get; set; } = string.Empty;
}
