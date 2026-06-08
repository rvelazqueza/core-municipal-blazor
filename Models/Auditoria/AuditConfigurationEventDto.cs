namespace BlazorApp.Models.Auditoria;

public class AuditConfigurationEventDto : AuditEventDto
{
    public string Parameter { get; set; } = string.Empty;
    public string Justification { get; set; } = string.Empty;
    public string AffectedModule { get; set; } = string.Empty;
    public string ConfigurationState { get; set; } = string.Empty;
}