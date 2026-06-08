using System.Collections.Generic;

namespace BlazorApp.Models;

public class WizardStepDto
{
    public string Id { get; set; } = string.Empty;
    public int Order { get; set; }
    public int Index { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public bool IsRequired { get; set; } = true;
    public bool IsOptional { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsActive { get; set; }
    public bool HasErrors { get; set; }
    public bool HasWarnings { get; set; }
    public bool IsSkipped { get; set; }
    public string Status { get; set; } = "Pendiente";
    public List<string> ValidationMessages { get; set; } = new();
    public List<StepValidationResultDto> Validations { get; set; } = new();
}
