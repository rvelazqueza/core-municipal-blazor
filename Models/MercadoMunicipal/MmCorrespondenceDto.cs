namespace BlazorApp.Models;

public class MmCorrespondenceDto
{
    public string Id { get; set; } = string.Empty;
    public string LocalId { get; set; } = string.Empty;
    public string ProcedureNumber { get; set; } = string.Empty;
    public string FileNumber { get; set; } = string.Empty;
    public string Channel { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public string Applicant { get; set; } = string.Empty;
    public string RelatedLocal { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public DateTime EntryDate { get; set; }
    public DateTime? ResponseDate { get; set; }
    public string ResolutionMock { get; set; } = string.Empty;
    public string AttachmentPlaceholder { get; set; } = string.Empty;
    public string Responsible { get; set; } = string.Empty;
}
