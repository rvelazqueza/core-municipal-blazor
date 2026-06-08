namespace BlazorApp.Models;

public class MmContractDto
{
    public string Id { get; set; } = string.Empty;
    public string LocalId { get; set; } = string.Empty;
    public string TenantId { get; set; } = string.Empty;
    public string ContractNumber { get; set; } = string.Empty;
    public string ContractType { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int DurationMonths { get; set; }
    public bool IsFiveYearTerm { get; set; }
    public string Status { get; set; } = string.Empty;
    public string ResponsibleUser { get; set; } = string.Empty;
    public string Obligations { get; set; } = string.Empty;
    public string Conditions { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public string DocumentPlaceholder { get; set; } = string.Empty;
    public bool HasDigitalSignature { get; set; }
}
