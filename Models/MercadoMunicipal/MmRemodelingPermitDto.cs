namespace BlazorApp.Models;

public class MmRemodelingPermitDto
{
    public string Id { get; set; } = string.Empty;
    public string LocalId { get; set; } = string.Empty;
    public string ManagementNumber { get; set; } = string.Empty;
    public string LocalNumber { get; set; } = string.Empty;
    public string RemodelingType { get; set; } = string.Empty;
    public DateTime RequestDate { get; set; }
    public DateTime ExecutionDate { get; set; }
    public string AuthorizedSchedule { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string UrbanismTask { get; set; } = string.Empty;
    public string MayorOfficeTask { get; set; } = string.Empty;
    public string DocumentMock { get; set; } = string.Empty;
    public string NotificationMock { get; set; } = string.Empty;
}
