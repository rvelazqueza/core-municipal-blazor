using BlazorApp.Models;
using System.Threading.Tasks;

namespace BlazorApp.Services;

public class ActividadEconomicaMockService : IActividadEconomicaService
{
    private static readonly List<ActividadEconomicaDto> Data = new()
    {
        new() { CodigoCaecr = "5611", Descripcion = "Restaurantes y sodas", Categoria = "Servicios", RequiereUsoSuelo = true },
        new() { CodigoCaecr = "4711", Descripcion = "Supermercados y abastecedores", Categoria = "Comercio", RequiereUsoSuelo = true },
        new() { CodigoCaecr = "4741", Descripcion = "Venta de equipos y accesorios tecnológicos", Categoria = "Comercio", RequiereUsoSuelo = true },
        new() { CodigoCaecr = "9602", Descripcion = "Salones de belleza y barberías", Categoria = "Servicios", RequiereUsoSuelo = true },
        new() { CodigoCaecr = "6201", Descripcion = "Desarrollo de software y servicios informáticos", Categoria = "Profesional", RequiereUsoSuelo = false },
        new() { CodigoCaecr = "4752", Descripcion = "Ferreterías y materiales de construcción", Categoria = "Comercio", RequiereUsoSuelo = true }
    };

    public Task<List<ActividadEconomicaDto>> GetAllAsync() => Task.FromResult(Data.Select(Clone).ToList());

    public Task<ActividadEconomicaDto?> GetByCodeAsync(string codigoCaecr)
        => Task.FromResult(Data.Where(x => x.CodigoCaecr == codigoCaecr).Select(Clone).FirstOrDefault());

    public Task<List<ActividadEconomicaDto>> SearchAsync(string term)
    {
        var query = Data.AsEnumerable();
        if (!string.IsNullOrWhiteSpace(term))
        {
            query = query.Where(x => x.CodigoCaecr.Contains(term, StringComparison.OrdinalIgnoreCase)
                || x.Descripcion.Contains(term, StringComparison.OrdinalIgnoreCase)
                || x.Categoria.Contains(term, StringComparison.OrdinalIgnoreCase));
        }

        return Task.FromResult(query.Select(Clone).ToList());
    }

    private static ActividadEconomicaDto Clone(ActividadEconomicaDto item) => new()
    {
        CodigoCaecr = item.CodigoCaecr,
        Descripcion = item.Descripcion,
        Categoria = item.Categoria,
        RequiereUsoSuelo = item.RequiereUsoSuelo
    };
}
