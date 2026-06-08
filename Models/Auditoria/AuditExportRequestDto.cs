namespace BlazorApp.Models.Auditoria;

public class AuditExportRequestDto
{
    public string RequestedBy { get; set; } = string.Empty;
    public string Module { get; set; } = "Auditoría";
    public string Format { get; set; } = string.Empty;
    public string ExpedientNumber { get; set; } = string.Empty;
    public string EntityName { get; set; } = string.Empty;
    public string EntityId { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}