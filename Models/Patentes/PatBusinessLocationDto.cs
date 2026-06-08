namespace BlazorApp.Models;

public class PatBusinessLocationDto
{
    public int Id { get; set; }
    public string IdPredial { get; set; } = string.Empty;
    public string NumeroFinca { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public string Distrito { get; set; } = string.Empty;
    public string Propietario { get; set; } = string.Empty;
    public string CondicionOcupacion { get; set; } = string.Empty;
    public decimal AreaLocal { get; set; }
    public string CuentaServiciosMunicipales { get; set; } = string.Empty;
    public string NombreLocal { get; set; } = string.Empty;
    public string EstadoGis { get; set; } = string.Empty;
    public string EstadoCuentaTributaria { get; set; } = string.Empty;
    public bool SinLocalFisico { get; set; }
}
