using System;

namespace BlazorApp.Models.PermisosConstruccion;

public class PcBiUpdateDto
{
    public string Status { get; set; } = "Pendiente";
    public string PropertyReference { get; set; } = string.Empty;
    public string UpdateType { get; set; } = "Nueva construcción";
    public string Origin { get; set; } = "Permiso aprobado";
    public DateTime? RequestedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string Notes { get; set; } = string.Empty;
}