namespace BlazorApp.Models;

public class ContribuyenteDto
{
    public int Id { get; set; }
    public string TipoPersona { get; set; } = "Fisica";
    public string TipoIdentificacion { get; set; } = "Cedula";
    public string NumeroIdentificacion { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string RazonSocial { get; set; } = string.Empty;
    public string Nacionalidad { get; set; } = "Costarricense";
    public string EstadoCivil { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public DireccionFiscalDto DireccionFiscal { get; set; } = new();
    public RepresentanteLegalDto RepresentanteLegal { get; set; } = new();
    public string ActividadEconomica { get; set; } = string.Empty;
    public string EstadoContribuyente { get; set; } = "Activo";
    public bool DatosIncompletos { get; set; }
    public bool PendienteCalidad { get; set; }
    public bool IndicadorDifunto { get; set; }
    public DateTime UltimaActualizacion { get; set; } = DateTime.Now;
    public List<TributoVinculadoDto> TributosVinculados { get; set; } = new();
    public List<BienInmuebleVinculadoDto> BienesInmueblesVinculados { get; set; } = new();
    public List<AuditoriaCambioDto> HistorialCambios { get; set; } = new();

    public string NombreCompleto => TipoPersona == "Juridica"
        ? RazonSocial
        : string.Join(" ", new[] { Nombre, Apellidos }.Where(x => !string.IsNullOrWhiteSpace(x)));
}
