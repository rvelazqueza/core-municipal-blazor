namespace BlazorApp.Models.Auditoria;

public class AuditEventDetailDto : AuditEventDto
{
    public string FieldChanged { get; set; } = string.Empty;
    public string Observation { get; set; } = string.Empty;
    public string TechnicalMessage { get; set; } = string.Empty;
    public string IntegrationRelated { get; set; } = string.Empty;
    public string PreviousState { get; set; } = string.Empty;
    public string NewState { get; set; } = string.Empty;
    public string ActionTaken { get; set; } = string.Empty;
}