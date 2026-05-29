namespace BlazorApp.Models;

public class AuditoriaCambioDto
{
    public string Usuario { get; set; } = string.Empty;
    public DateTime FechaHora { get; set; } = DateTime.Now;
    public string CampoModificado { get; set; } = string.Empty;
    public string ValorAnterior { get; set; } = string.Empty;
    public string ValorNuevo { get; set; } = string.Empty;
    public string Origen { get; set; } = string.Empty;
}
