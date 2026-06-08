using System;
using System.Collections.Generic;

namespace BlazorApp.Models;

public class WizardStateDto
{
    public string ProcessId { get; set; } = string.Empty;
    public string ProcessKey { get; set; } = string.Empty;
    public string ProcessCode { get; set; } = string.Empty;
    public string ModuleName { get; set; } = string.Empty;
    public string ProcessName { get; set; } = string.Empty;
    public string ProcessTitle { get; set; } = string.Empty;
    public string CurrentStepId { get; set; } = string.Empty;
    public int CurrentStepIndex { get; set; }
    public int TotalSteps { get; set; }
    public double ProgressPercent { get; set; }
    public decimal ProgressPercentage { get; set; }
    public string Status { get; set; } = "Borrador";
    public DateTime? StartedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? FinishedAt { get; set; }
    public DateTime? LastSavedAt { get; set; }
    public string CreatedBy { get; set; } = "demo.user";
    public bool IsDirty { get; set; }
    public bool IsReadOnly { get; set; }
    public DraftStateDto DraftState { get; set; } = new();
    public List<WizardStepDto> Steps { get; set; } = new();
    public List<StepValidationResultDto> ValidationResults { get; set; } = new();
    public List<string> SummaryItems { get; set; } = new();
    public List<string> PendingValidations { get; set; } = new();
    public List<ProcessSummaryItemDto> ProcessSummaryItems { get; set; } = new();
}
