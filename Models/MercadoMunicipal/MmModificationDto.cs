namespace BlazorApp.Models;

public class MmModificationDto
{
    public string Id { get; set; } = string.Empty;
    public string LocalId { get; set; } = string.Empty;
    public string RequestNumber { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public string MovementType { get; set; } = string.Empty;
    public string Justification { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string PreviousValue { get; set; } = string.Empty;
    public string NewValue { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string User { get; set; } = string.Empty;
    public bool RequiresInspection { get; set; }
    public bool RequiresCouncilApproval { get; set; }
    public string Result { get; set; } = string.Empty;
    public string CommercialImpact { get; set; } = string.Empty;
    public string GisImpact { get; set; } = string.Empty;
    public string FiscalizationImpact { get; set; } = string.Empty;
    public string CollectionImpact { get; set; } = string.Empty;
    public string TaxAccountImpact { get; set; } = string.Empty;
    public string NotificationImpact { get; set; } = string.Empty;
}
