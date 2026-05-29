using BlazorApp.Models;
using System.Threading.Tasks;

namespace BlazorApp.Services;

public interface IFiscalizacionService
{
    Task<List<FiscalizacionBienDto>> GetPendientesAsync();
    Task<FiscalizacionBienDto> ProgramarAsync(BienInmuebleDto bien);
    Task<FiscalizacionBienDto> RegistrarResultadoAsync(BienInmuebleDto bien, string resultado);
}
