using System;

namespace BlazorApp.Models.PermisosConstruccion;

public class PcWorkProgressDto
{
    public decimal ProgressPercent { get; set; }
    public DateTime? LastProgressDate { get; set; }
    public string Inspector { get; set; } = string.Empty;
    public string Status { get; set; } = "Sin iniciar";
    public decimal FineReference { get; set; }
    public decimal InterestReference { get; set; }
    public string BiTaskStatus { get; set; } = "No generada";
    public string Notes { get; set; } = string.Empty;
}