namespace BlazorApp.Models;

public class MmWarningDto
{
    public string Id { get; set; } = string.Empty;
    public string LocalId { get; set; } = string.Empty;
    public string TenantId { get; set; } = string.Empty;
    public string WarningNumber { get; set; } = string.Empty;
    public string LocalNumber { get; set; } = string.Empty;
    public string TenantName { get; set; } = string.Empty;
    public string WarningType { get; set; } = string.Empty;
    public DateTime AppliedDate { get; set; }
    public DateTime ComplianceDeadline { get; set; }
    public string Status { get; set; } = string.Empty;
    public string DocumentMock { get; set; } = string.Empty;
    public string NotificationMock { get; set; } = string.Empty;
}
