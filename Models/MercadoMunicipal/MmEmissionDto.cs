namespace BlazorApp.Models;

public class MmEmissionDto
{
    public string Id { get; set; } = string.Empty;
    public string LocalId { get; set; } = string.Empty;
    public string EmissionNumber { get; set; } = string.Empty;
    public int FiscalYear { get; set; }
    public int Month { get; set; }
    public string EmissionType { get; set; } = string.Empty;
    public string LocalAccount { get; set; } = string.Empty;
    public string LocalType { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public string BusinessActivity { get; set; } = string.Empty;
    public string RentStatus { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string Aisle { get; set; } = string.Empty;
    public decimal WeightingFactor { get; set; }
    public decimal AreaM2 { get; set; }
    public decimal ProposedPrice { get; set; }
    public int IncludedLocals { get; set; }
    public int ExcludedLocals { get; set; }
    public string Inconsistencies { get; set; } = string.Empty;
    public string QualityControlStatus { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public bool AutomaticEnabled { get; set; }
    public string ScheduledRule { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
