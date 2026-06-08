using System;

namespace BlazorApp.Models.PermisosConstruccion;

public class PcCorrespondenceDto
{
    public string ManagementNumber { get; set; } = string.Empty;
    public string Type { get; set; } = "Interna";
    public string Channel { get; set; } = "Plataforma de Servicios";
    public string Applicant { get; set; } = string.Empty;
    public string DestinationDepartment { get; set; } = string.Empty;
    public string Status { get; set; } = "Registrada";
    public string ResolutionMock { get; set; } = string.Empty;
    public DateTime? DeadlineMock { get; set; }
    public string NotificationMock { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
}