namespace BlazorApp.Models;

public class MmCardDto
{
    public string Id { get; set; } = string.Empty;
    public string LocalId { get; set; } = string.Empty;
    public string TenantId { get; set; } = string.Empty;
    public string CardNumber { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Identification { get; set; } = string.Empty;
    public string Condition { get; set; } = string.Empty;
    public string PhotoPlaceholder { get; set; } = string.Empty;
    public DateTime IssueDate { get; set; }
    public DateTime ExpirationDate { get; set; }
    public string Status { get; set; } = string.Empty;
}
