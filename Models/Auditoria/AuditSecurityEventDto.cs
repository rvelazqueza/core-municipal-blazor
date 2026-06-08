namespace BlazorApp.Models.Auditoria;

public class AuditSecurityEventDto : AuditEventDto
{
    public string SecurityActionTaken { get; set; } = string.Empty;
}