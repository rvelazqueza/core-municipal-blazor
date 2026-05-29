using BlazorApp.Models;
using System.Threading.Tasks;

namespace BlazorApp.Services;

public interface IBienesInmueblesService
{
    Task<List<BienInmuebleDto>> GetAllAsync();
    Task<BienInmuebleDto> GetByIdAsync(int id);
    Task<List<BienInmuebleDto>> SearchAsync(string? numeroFinca, string? idPredial, string? propietario, string? distrito, string? estado, decimal? valorFiscalMinimo, string? condicion);
    Task SaveAsync(BienInmuebleDto bien);
    Task SimularRegistroPublicoAsync(BienInmuebleDto bien);
    Task SimularRemisionHaciendaAsync(BienInmuebleDto bien);
    Task SimularGisAsync(BienInmuebleDto bien);
    Task SimularCobroAsync(BienInmuebleDto bien);
}
