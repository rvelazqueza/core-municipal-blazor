using System;

namespace BlazorApp.Models.PermisosConstruccion;

public class PcInspectionDto
{
    public string Number { get; set; } = string.Empty;
    public string RelatedCaseNumber { get; set; } = string.Empty;
    public string RelatedProperty { get; set; } = string.Empty;
    public string RelatedPerson { get; set; } = string.Empty;
    public DateTime? InspectionDate { get; set; }
    public DateTime? AssignedDate { get; set; }
    public string Inspector { get; set; } = string.Empty;
    public string InspectionType { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public string Purpose { get; set; } = string.Empty;
    public string Status { get; set; } = "Solicitada";
    public string Result { get; set; } = "Pendiente";
    public decimal ProgressPercent { get; set; }
    public string EvidencePlaceholder { get; set; } = string.Empty;
    public string DocumentsPlaceholder { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}