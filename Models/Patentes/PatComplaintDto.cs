namespace BlazorApp.Models;

public class PatComplaintDto
{
    public string NumeroExpediente { get; set; } = string.Empty;
    public string CanalIngreso { get; set; } = string.Empty;
    public bool Anonima { get; set; }
    public string TipoDenunciante { get; set; } = string.Empty;
    public string Motivo { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string NumeroLicencia { get; set; } = string.Empty;
    public string NombreComercial { get; set; } = string.Empty;
    public string InspectorAsignado { get; set; } = string.Empty;
    public string Resultado { get; set; } = string.Empty;
}
