namespace BlazorApp.Models;

public class PatQualityCaseDto
{
    public string CodigoCaso { get; set; } = string.Empty;
    public string Origen { get; set; } = string.Empty;
    public string Referencia { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string MotivoDevolucion { get; set; } = string.Empty;
    public string UsuarioResponsable { get; set; } = string.Empty;
    public string FuncionarioAsignado { get; set; } = string.Empty;
    public DateTime FechaRegistro { get; set; } = DateTime.Today;
}
