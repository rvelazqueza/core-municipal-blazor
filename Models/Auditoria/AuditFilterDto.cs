using MudBlazor;

namespace BlazorApp.Models.Auditoria;

public class AuditFilterDto
{
    public DateRange DateRange { get; set; } = new(DateTime.Today.AddDays(-30), DateTime.Today);
    public string UserName { get; set; } = string.Empty;
    public string Module { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string EventType { get; set; } = string.Empty;
    public string Severity { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string TaxpayerIdentification { get; set; } = string.Empty;
    public string ExpedientNumber { get; set; } = string.Empty;
    public string EntityName { get; set; } = string.Empty;
    public string EntityId { get; set; } = string.Empty;
    public string IpAddress { get; set; } = string.Empty;
    public string Channel { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public string Result { get; set; } = string.Empty;
    public string FreeText { get; set; } = string.Empty;
}