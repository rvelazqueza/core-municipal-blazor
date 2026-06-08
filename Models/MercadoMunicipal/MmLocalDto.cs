namespace BlazorApp.Models;

public class MmLocalDto
{
    public string Id { get; set; } = string.Empty;
    public string LocalNumber { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public string LocalType { get; set; } = string.Empty;
    public string RentalType { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Floor { get; set; } = string.Empty;
    public string Zone { get; set; } = string.Empty;
    public string Sector { get; set; } = string.Empty;
    public string Aisle { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string RelatedProperty { get; set; } = string.Empty;
    public string PatentNumber { get; set; } = string.Empty;
    public string ServiceAccount { get; set; } = string.Empty;
    public string ElectricMeterNumber { get; set; } = string.Empty;
    public decimal AreaM2 { get; set; }
    public decimal WeightingFactor { get; set; }
    public decimal ProposedPrice { get; set; }
    public decimal RentAmount { get; set; }
    public string BusinessActivity { get; set; } = string.Empty;
    public string BusinessName { get; set; } = string.Empty;
    public string TenantId { get; set; } = string.Empty;
    public bool HasMorosity { get; set; }
    public DateTime LastUpdated { get; set; }
    public string Observations { get; set; } = string.Empty;
    public string PhotoPlaceholder { get; set; } = string.Empty;
}
