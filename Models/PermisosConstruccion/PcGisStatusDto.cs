using System;

namespace BlazorApp.Models.PermisosConstruccion;

public class PcGisStatusDto
{
    public string Status { get; set; } = "Pendiente";
    public string Layer { get; set; } = string.Empty;
    public string TaskNumber { get; set; } = string.Empty;
    public DateTime? LastUpdatedAt { get; set; }
    public string PropertyReference { get; set; } = string.Empty;
    public string PermitReference { get; set; } = string.Empty;
    public string Relation { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}