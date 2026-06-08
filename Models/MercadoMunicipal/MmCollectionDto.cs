namespace BlazorApp.Models;

public class MmCollectionDto
{
    public string Id { get; set; } = string.Empty;
    public string LocalId { get; set; } = string.Empty;
    public int PendingMonths { get; set; }
    public decimal OverdueBalance { get; set; }
    public string Status { get; set; } = string.Empty;
    public string LastAction { get; set; } = string.Empty;
    public string ResponsibleUser { get; set; } = string.Empty;
    public DateTime LastManagementDate { get; set; }
    public string AdministrativeStage { get; set; } = string.Empty;
}
