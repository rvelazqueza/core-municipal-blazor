namespace BlazorApp.Models;

public class PatAuditEventDto
{
    public string Modulo { get; set; } = string.Empty;
    public string Accion { get; set; } = string.Empty;
    public string Usuario { get; set; } = string.Empty;
    public DateTime FechaHora { get; set; } = DateTime.Now;
    public string Descripcion { get; set; } = string.Empty;
    public string Referencia { get; set; } = string.Empty;
    public string Origen { get; set; } = string.Empty;
    public string Severidad { get; set; } = string.Empty;
}
