using System.Collections.Generic;

namespace BlazorApp.Models;

public class StepValidationResultDto
{
    public string StepId { get; set; } = string.Empty;
    public int StepIndex { get; set; }
    public string StepTitle { get; set; } = string.Empty;
    public string Field { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Severity { get; set; } = "Info";
    public bool IsValid { get; set; } = true;
    public bool HasWarnings { get; set; }
    public string Code { get; set; } = string.Empty;
    public List<string> Messages { get; set; } = new();
}
