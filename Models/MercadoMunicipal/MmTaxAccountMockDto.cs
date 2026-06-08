namespace BlazorApp.Models;

public class MmTaxAccountMockDto
{
    public string Id { get; set; } = string.Empty;
    public string LocalId { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public string Period { get; set; } = string.Empty;
    public int Month { get; set; }
    public decimal Debit { get; set; }
    public decimal Credit { get; set; }
    public decimal Balance { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime? LastPaymentDate { get; set; }
    public string MorosityStatus { get; set; } = string.Empty;
}
