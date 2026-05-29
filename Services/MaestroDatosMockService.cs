using System.Threading.Tasks;

namespace BlazorApp.Services;

public class MaestroDatosMockService : IMaestroDatosService
{
    public Task<List<string>> GetTiposPersonaAsync() => Task.FromResult(new List<string> { "Fisica", "Juridica" });
    public Task<List<string>> GetTiposIdentificacionAsync() => Task.FromResult(new List<string> { "Cedula", "DIMEX", "Pasaporte", "Cedula juridica" });
    public Task<List<string>> GetEstadosContribuyenteAsync() => Task.FromResult(new List<string> { "Activo", "Inactivo", "Difunto", "Pendiente revision" });
    public Task<List<string>> GetDistritosAsync() => Task.FromResult(new List<string> { "Carmen", "Merced", "Hospital", "Mata Redonda", "Pavas", "San Francisco" });
    public Task<List<string>> GetActividadesEconomicasAsync() => Task.FromResult(new List<string> { "Servicios profesionales", "Comercio", "Construccion", "Alquileres", "Industria", "Transporte" });
    public Task<List<string>> GetProvinciasAsync() => Task.FromResult(new List<string> { "San Jose", "Alajuela", "Cartago", "Heredia", "Guanacaste", "Puntarenas", "Limon" });
    public Task<List<string>> GetCantonesAsync() => Task.FromResult(new List<string> { "Central", "Escazu", "Desamparados", "Curridabat", "Montes de Oca" });
    public Task<List<string>> GetEstadosCivilesAsync() => Task.FromResult(new List<string> { "Soltero", "Casado", "Divorciado", "Viudo", "Union libre" });
    public Task<List<string>> GetEstadosBienInmuebleAsync() => Task.FromResult(new List<string> { "Activo", "Inactivo", "Pendiente revision", "En fiscalizacion" });
    public Task<List<string>> GetCondicionesBienInmuebleAsync() => Task.FromResult(new List<string> { "Al dia", "Con revision", "Omiso", "Pendiente GIS", "Exonerada" });
    public Task<List<string>> GetTiposFincaAsync() => Task.FromResult(new List<string> { "Individual", "Horizontal", "Agrícola", "Industrial", "Comercial" });
    public Task<List<string>> GetEstadosRegistralesAsync() => Task.FromResult(new List<string> { "Inscrita", "Pendiente revision", "En estudio", "Suspendida" });
    public Task<List<string>> GetUsosInmuebleAsync() => Task.FromResult(new List<string> { "Residencial", "Comercial", "Industrial", "Mixto", "Agropecuario" });
    public Task<List<string>> GetZonasHomogeneasAsync() => Task.FromResult(new List<string> { "Zona A-1", "Zona B-3", "Zona C-2", "Zona R-1" });
    public Task<List<string>> GetTipologiasConstructivasAsync() => Task.FromResult(new List<string> { "Residencial media", "Comercial alta", "Mixta básica", "Rural ligera" });
    public Task<List<string>> GetEstadosPatenteAsync() => Task.FromResult(new List<string> { "Borrador", "En revision", "Prevenido", "Aprobado", "Rechazado", "Suspendido", "Cancelado" });
    public Task<List<string>> GetTiposPatenteAsync() => Task.FromResult(new List<string> { "Comercial", "Industrial", "Temporal" });
    public Task<List<string>> GetEstadosUsoSueloAsync() => Task.FromResult(new List<string> { "Pendiente", "Conforme", "No conforme" });
}
