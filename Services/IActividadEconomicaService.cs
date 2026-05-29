using BlazorApp.Models;
using System.Threading.Tasks;

namespace BlazorApp.Services;

public interface IActividadEconomicaService
{
    Task<List<ActividadEconomicaDto>> GetAllAsync();
    Task<ActividadEconomicaDto?> GetByCodeAsync(string codigoCaecr);
    Task<List<ActividadEconomicaDto>> SearchAsync(string term);
}
