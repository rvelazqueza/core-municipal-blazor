using BlazorApp.Models;
using System.Threading.Tasks;

namespace BlazorApp.Services;

public interface IValoracionService
{
    Task RecalcularAsync(BienInmuebleDto bien);
    Task<ValoracionDto> GenerarNuevaValoracionAsync(BienInmuebleDto bien);
}
