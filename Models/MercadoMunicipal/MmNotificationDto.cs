namespace BlazorApp.Models;

public class MmNotificationDto
{
    public string Id { get; set; } = string.Empty;
    public string LocalId { get; set; } = string.Empty;
    public string NotificationNumber { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Medium { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string DigitalSignatureReference { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string TargetName { get; set; } = string.Empty;
}
