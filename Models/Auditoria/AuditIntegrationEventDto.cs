namespace BlazorApp.Models.Auditoria;

public class AuditIntegrationEventDto : AuditEventDto
{
    public string ModuleOrigin { get; set; } = string.Empty;
    public string ModuleDestination { get; set; } = string.Empty;
    public string IntegrationState { get; set; } = string.Empty;
}