namespace BlazorApp.Models.Auditoria;

public class AuditModuleSummaryDto
{
    public string Module { get; set; } = string.Empty;
    public int TotalEvents { get; set; }
    public int CriticalEvents { get; set; }
    public DateTime LatestEventDate { get; set; }
    public string LatestUserName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string LastAction { get; set; } = string.Empty;
}