namespace BlazorApp.Models;

public class LicenciaComercialDto
{
    public int Id { get; set; }
    public string NumeroLicencia { get; set; } = string.Empty;
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
    public string Distrito { get; set; } = string.Empty;
    public string Estado { get; set; } = "Borrador";
    public DateTime FechaSolicitud { get; set; } = DateTime.Today;
    public DateTime? FechaAprobacion { get; set; }
    public DateTime? FechaVencimiento { get; set; }
    public string Responsable { get; set; } = string.Empty;
    public string Observaciones { get; set; } = string.Empty;
    public bool PendienteUsoSuelo { get; set; }
    public bool EmisionSemestralPendiente { get; set; } = true;
    public bool NotificacionPendiente { get; set; }
    public bool CobroSincronizado { get; set; }
    public bool GisValidado { get; set; }
    public UsoSueloVinculadoDto UsoSuelo { get; set; } = new();
    public SolicitudPatenteDto Solicitud { get; set; } = new();
    public List<RequisitoPatenteDto> Requisitos { get; set; } = new();
    public List<MovimientoPatenteDto> Movimientos { get; set; } = new();
}
