using BlazorApp.Models;
using System.Threading.Tasks;

namespace BlazorApp.Services;

public interface IContribuyentesService
{
    Task<List<ContribuyenteDto>> GetAllAsync();
    Task<ContribuyenteDto> GetByIdAsync(int id);
    Task<List<ContribuyenteDto>> SearchAsync(string? identificacion, string? nombre, string? tipoPersona, string? estado, string? distrito, string? actividadEconomica);
    Task SaveAsync(ContribuyenteDto contribuyente);
    Task SimularActualizacionTseAsync(ContribuyenteDto contribuyente);
    Task SimularActualizacionRegistroNacionalAsync(ContribuyenteDto contribuyente);
}
