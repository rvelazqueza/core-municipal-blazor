namespace BlazorApp.Models;

public class MmRentDto
{
    public string Id { get; set; } = string.Empty;
    public string LocalId { get; set; } = string.Empty;
    public decimal MonthlyAmount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public string Periodicity { get; set; } = string.Empty;
    public int StartMonth { get; set; }
    public int EndMonth { get; set; }
    public int StartYear { get; set; }
    public int EndYear { get; set; }
    public int DueDay { get; set; }
    public decimal WeightingFactor { get; set; }
    public decimal PricePerM2 { get; set; }
    public decimal AreaM2 { get; set; }
    public decimal MockCalculatedAmount { get; set; }
    public bool HasDiscount { get; set; }
    public bool IsSuspended { get; set; }
    public string Notes { get; set; } = string.Empty;
}
