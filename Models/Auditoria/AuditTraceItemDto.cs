namespace BlazorApp.Models.Auditoria;

public class AuditTraceItemDto
{
    public string Id { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string PreviousState { get; set; } = string.Empty;
    public string NewState { get; set; } = string.Empty;
    public string Module { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}