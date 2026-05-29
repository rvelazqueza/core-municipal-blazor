using BlazorApp.Models;
using System.Threading.Tasks;

namespace BlazorApp.Services;

public class UsoSueloMockService : IUsoSueloMockService
{
    public Task<List<string>> GetEstadosAsync() => Task.FromResult(new List<string> { "Pendiente", "Conforme", "No conforme" });

    public Task<UsoSueloVinculadoDto> ValidarAsync(string fincaOIdPredial, string numeroCertificado, string codigoCaecr)
    {
        if (string.IsNullOrWhiteSpace(fincaOIdPredial) || string.IsNullOrWhiteSpace(numeroCertificado))
        {
            return Task.FromResult(new UsoSueloVinculadoDto
            {
                NumeroCertificado = numeroCertificado,
                Estado = "Pendiente",
                EsConforme = false,
                FechaValidacion = DateTime.Today,
                Observaciones = "Debe indicar finca o ID predial y certificado para validar.",
                Fuente = "Uso de Suelo"
            });
        }

        var noConforme = numeroCertificado.StartsWith("NC", StringComparison.OrdinalIgnoreCase)
            || codigoCaecr == "5611" && fincaOIdPredial.Contains("SJ-002145", StringComparison.OrdinalIgnoreCase);

        return Task.FromResult(new UsoSueloVinculadoDto
        {
            NumeroCertificado = numeroCertificado,
            Estado = noConforme ? "No conforme" : "Conforme",
            EsConforme = !noConforme,
            FechaValidacion = DateTime.Today,
            Observaciones = noConforme
                ? "La actividad seleccionada no cumple con la conformidad de uso de suelo para el local indicado."
                : "Certificado validado conforme con la actividad económica y el local vinculado.",
            Fuente = "Uso de Suelo"
        });
    }
}
