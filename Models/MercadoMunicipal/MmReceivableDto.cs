namespace BlazorApp.Models;

public class MmReceivableDto
{
    public string Id { get; set; } = string.Empty;
    public string LocalId { get; set; } = string.Empty;
    public string EmissionNumber { get; set; } = string.Empty;
    public int FiscalYear { get; set; }
    public int Month { get; set; }
    public string LocalAccount { get; set; } = string.Empty;
    public string LocalType { get; set; } = string.Empty;
    public string BusinessActivity { get; set; } = string.Empty;
    public string TaxpayerName { get; set; } = string.Empty;
    public string TaxCode { get; set; } = string.Empty;
    public string SubTaxCode { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime DueDate { get; set; }
}
