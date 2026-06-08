namespace BlazorApp.Models;

public class MmSanctionDto
{
    public string Id { get; set; } = string.Empty;
    public string LocalId { get; set; } = string.Empty;
    public string TenantId { get; set; } = string.Empty;
    public string SanctionNumber { get; set; } = string.Empty;
    public string LocalNumber { get; set; } = string.Empty;
    public string TenantName { get; set; } = string.Empty;
    public string SanctionType { get; set; } = string.Empty;
    public DateTime AppliedDate { get; set; }
    public DateTime Deadline { get; set; }
    public string Status { get; set; } = string.Empty;
    public string RelatedWarningNumber { get; set; } = string.Empty;
    public string DocumentMock { get; set; } = string.Empty;
    public string NotificationMock { get; set; } = string.Empty;
}
