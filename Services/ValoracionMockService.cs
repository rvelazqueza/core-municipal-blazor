using BlazorApp.Models;
using System.Threading.Tasks;

namespace BlazorApp.Services;

public class ValoracionMockService : IValoracionService
{
    public Task RecalcularAsync(BienInmuebleDto bien)
    {
        bien.ValorFiscalTotal = bien.ValorTerreno + bien.ValorConstruccion;
        bien.FincaValorActualizado = true;
        bien.UltimaActualizacion = DateTime.Now;
        return Task.CompletedTask;
    }

    public Task<ValoracionDto> GenerarNuevaValoracionAsync(BienInmuebleDto bien)
    {
        var valoracion = new ValoracionDto
        {
            Periodo = DateTime.Today.Year.ToString(),
            ZonaHomogenea = bien.ZonaHomogenea,
            TipologiaConstructiva = bien.TipologiaConstructiva,
            ValorTerreno = bien.ValorTerreno,
            ValorConstruccion = bien.ValorConstruccion,
            ValorFiscalTotal = bien.ValorTerreno + bien.ValorConstruccion,
            FechaValoracion = DateTime.Today,
            Actualizado = true
        };

        return Task.FromResult(valoracion);
    }
}
