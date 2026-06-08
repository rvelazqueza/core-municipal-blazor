namespace BlazorApp.Models;

public class PatHistoryEntryDto
{
    public string NumeroLicencia { get; set; } = string.Empty;
    public string NumeroExpediente { get; set; } = string.Empty;
    public string CampoModificado { get; set; } = string.Empty;
    public string ValorAnterior { get; set; } = string.Empty;
    public string ValorNuevo { get; set; } = string.Empty;
    public string Proceso { get; set; } = string.Empty;
    public string Usuario { get; set; } = string.Empty;
    public DateTime FechaHora { get; set; } = DateTime.Now;
    public string EstadoControlCalidad { get; set; } = string.Empty;
    public string Origen { get; set; } = string.Empty;
}
