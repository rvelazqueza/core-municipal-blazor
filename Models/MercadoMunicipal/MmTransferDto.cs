namespace BlazorApp.Models;

public class MmTransferDto
{
    public string Id { get; set; } = string.Empty;
    public string LocalId { get; set; } = string.Empty;
    public string RequestNumber { get; set; } = string.Empty;
    public string LocalNumber { get; set; } = string.Empty;
    public string LocalAccount { get; set; } = string.Empty;
    public string CurrentTaxpayer { get; set; } = string.Empty;
    public string PreviousTaxpayer { get; set; } = string.Empty;
    public string NewTaxpayer { get; set; } = string.Empty;
    public DateTime RequestDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string CouncilAgreementMock { get; set; } = string.Empty;
    public string ThirdPartyReceiptMock { get; set; } = string.Empty;
    public string ReceivablesUpdateMock { get; set; } = string.Empty;
    public string OwnershipHistory { get; set; } = string.Empty;
}
