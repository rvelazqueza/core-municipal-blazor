namespace BlazorApp.Models;

public class MovimientoPatenteDto
{
    public DateTime FechaHora { get; set; } = DateTime.Now;
    public string TipoMovimiento { get; set; } = string.Empty;
    public string Motivo { get; set; } = string.Empty;
    public string Usuario { get; set; } = string.Empty;
    public string EstadoResultante { get; set; } = string.Empty;
    public string Origen { get; set; } = string.Empty;
}
