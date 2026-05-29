using System.Threading.Tasks;

namespace BlazorApp.Services;

public interface IMaestroDatosService
{
    Task<List<string>> GetTiposPersonaAsync();
    Task<List<string>> GetTiposIdentificacionAsync();
    Task<List<string>> GetEstadosContribuyenteAsync();
    Task<List<string>> GetDistritosAsync();
    Task<List<string>> GetActividadesEconomicasAsync();
    Task<List<string>> GetProvinciasAsync();
    Task<List<string>> GetCantonesAsync();
    Task<List<string>> GetEstadosCivilesAsync();
    Task<List<string>> GetEstadosBienInmuebleAsync();
    Task<List<string>> GetCondicionesBienInmuebleAsync();
    Task<List<string>> GetTiposFincaAsync();
    Task<List<string>> GetEstadosRegistralesAsync();
    Task<List<string>> GetUsosInmuebleAsync();
    Task<List<string>> GetZonasHomogeneasAsync();
    Task<List<string>> GetTipologiasConstructivasAsync();
    Task<List<string>> GetEstadosPatenteAsync();
    Task<List<string>> GetTiposPatenteAsync();
    Task<List<string>> GetEstadosUsoSueloAsync();
}
