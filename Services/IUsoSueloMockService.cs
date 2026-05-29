using BlazorApp.Models;
using System.Threading.Tasks;

namespace BlazorApp.Services;

public interface IUsoSueloMockService
{
    Task<List<string>> GetEstadosAsync();
    Task<UsoSueloVinculadoDto> ValidarAsync(string fincaOIdPredial, string numeroCertificado, string codigoCaecr);
}
