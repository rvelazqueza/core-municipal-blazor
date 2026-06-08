namespace BlazorApp.Models;

public class FiltroLicenciasPatenteDto
{
    public string NumeroLicencia { get; set; } = string.Empty;
    public string NumeroSolicitud { get; set; } = string.Empty;
    public string Expediente { get; set; } = string.Empty;
    public string Identificacion { get; set; } = string.Empty;
    public string Contribuyente { get; set; } = string.Empty;
    public string NombreComercial { get; set; } = string.Empty;
    public string ActividadEconomica { get; set; } = string.Empty;
    public string TipoLicencia { get; set; } = string.Empty;
    public string Tipo
    {
        get => TipoLicencia;
        set => TipoLicencia = value ?? string.Empty;
    }
    public string FechaVencimientoHasta { get; set; } = string.Empty;
    public string UsoSuelo { get; set; } = string.Empty;
    public string Distrito { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public string CanalIngreso { get; set; } = string.Empty;
}
