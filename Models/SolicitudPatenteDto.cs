namespace BlazorApp.Models;

public class SolicitudPatenteDto
{
    public int Id { get; set; }
    public int? LicenciaId { get; set; }
    public string NumeroSolicitud { get; set; } = string.Empty;
    public int ContribuyenteId { get; set; }
    public string ContribuyenteRuc { get; set; } = string.Empty;
    public string Identificacion { get; set; } = string.Empty;
    public string ContribuyenteNombre { get; set; } = string.Empty;
    public string NombreComercial { get; set; } = string.Empty;
    public string DireccionLocal { get; set; } = string.Empty;
    public string FincaOIdPredial { get; set; } = string.Empty;
    public string ActividadEconomica { get; set; } = string.Empty;
    public string CodigoCaecr { get; set; } = string.Empty;
    public string TipoPatente { get; set; } = "Comercial";
    public string NumeroCertificadoUsoSuelo { get; set; } = string.Empty;
    public string EstadoUsoSuelo { get; set; } = "Pendiente";
    public bool UsoSueloConforme { get; set; }
    public DateTime FechaSolicitud { get; set; } = DateTime.Today;
    public string Responsable { get; set; } = string.Empty;
    public string Estado { get; set; } = "Borrador";
    public string Observaciones { get; set; } = string.Empty;
    public List<string> Adjuntos { get; set; } = new();
    public List<RequisitoPatenteDto> Requisitos { get; set; } = new();
    public UsoSueloVinculadoDto UsoSuelo { get; set; } = new();
    public string Distrito { get; set; } = string.Empty;
}
