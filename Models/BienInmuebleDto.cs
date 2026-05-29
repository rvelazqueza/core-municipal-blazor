namespace BlazorApp.Models;

public class BienInmuebleDto
{
    public int Id { get; set; }
    public string NumeroFinca { get; set; } = string.Empty;
    public string IdPredial { get; set; } = string.Empty;
    public string TipoFinca { get; set; } = "Individual";
    public string EstadoRegistral { get; set; } = "Inscrita";
    public string Distrito { get; set; } = string.Empty;
    public string DireccionExacta { get; set; } = string.Empty;
    public decimal AreaTerreno { get; set; }
    public int PropietarioPrincipalId { get; set; }
    public string PropietarioPrincipal { get; set; } = string.Empty;
    public string PropietarioPrincipalIdentificacion { get; set; } = string.Empty;
    public decimal PorcentajeDerecho { get; set; } = 100;
    public decimal ValorTerreno { get; set; }
    public decimal ValorConstruccion { get; set; }
    public decimal ValorFiscalTotal { get; set; }
    public string ZonaHomogenea { get; set; } = string.Empty;
    public string TipologiaConstructiva { get; set; } = string.Empty;
    public string UsoInmueble { get; set; } = string.Empty;
    public string Observaciones { get; set; } = string.Empty;
    public string Estado { get; set; } = "Activo";
    public string Condicion { get; set; } = "Al dia";
    public bool FincaValorActualizado { get; set; }
    public bool PendienteFiscalizacion { get; set; }
    public bool OmisoDetectado { get; set; }
    public bool RegistroPublicoSincronizado { get; set; }
    public bool RemitidoHacienda { get; set; }
    public string UbicacionGis { get; set; } = string.Empty;
    public string CuentaTributaria { get; set; } = string.Empty;
    public DateTime UltimaActualizacion { get; set; } = DateTime.Now;
    public List<DerechoPropiedadDto> DerechosPropiedad { get; set; } = new();
    public List<ValoracionDto> Valoraciones { get; set; } = new();
    public List<DeclaracionBienDto> Declaraciones { get; set; } = new();
    public List<ExoneracionDto> Exoneraciones { get; set; } = new();
    public List<FiscalizacionBienDto> Fiscalizaciones { get; set; } = new();
    public List<ImagenPredialDto> Imagenes { get; set; } = new();
    public List<AuditoriaCambioDto> HistorialCambios { get; set; } = new();
}
