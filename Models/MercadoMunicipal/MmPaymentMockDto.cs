namespace BlazorApp.Models;

public class MmPaymentMockDto
{
    public string Id { get; set; } = string.Empty;
    public string LocalId { get; set; } = string.Empty;
    public string LocalAccount { get; set; } = string.Empty;
    public DateTime PaymentDate { get; set; }
    public decimal Amount { get; set; }
    public decimal AppliedAmount { get; set; }
    public decimal Balance { get; set; }
    public string CollectorEntity { get; set; } = string.Empty;
    public string PaymentMethod { get; set; } = string.Empty;
    public string ReceiptNumber { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string PaymentArrangement { get; set; } = string.Empty;
    public string ModifiedByUser { get; set; } = string.Empty;
}
