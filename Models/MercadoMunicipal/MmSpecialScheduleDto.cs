namespace BlazorApp.Models;

public class MmSpecialScheduleDto
{
    public string Id { get; set; } = string.Empty;
    public string LocalId { get; set; } = string.Empty;
    public string TenantId { get; set; } = string.Empty;
    public string PermissionType { get; set; } = string.Empty;
    public string LocalNumber { get; set; } = string.Empty;
    public string TenantName { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Schedule { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string DocumentMock { get; set; } = string.Empty;
    public string NotificationMock { get; set; } = string.Empty;
}
