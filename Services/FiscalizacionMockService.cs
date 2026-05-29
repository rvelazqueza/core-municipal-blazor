using BlazorApp.Models;
using System.Threading.Tasks;

namespace BlazorApp.Services;

public class FiscalizacionMockService : IFiscalizacionService
{
    private static readonly List<FiscalizacionBienDto> Pending = new()
    {
        new() { NumeroCaso = "FIS-2025-004", Inspector = "Marvin Soto", Estado = "Programada", FechaProgramada = DateTime.Today.AddDays(6), Hallazgos = "Revisar ampliación reportada por catastro." },
        new() { NumeroCaso = "FIS-2025-015", Inspector = "Daniel Ureña", Estado = "Programada", FechaProgramada = DateTime.Today.AddDays(12), Hallazgos = "Validar área declarada y uso del suelo." }
    };

    public Task<List<FiscalizacionBienDto>> GetPendientesAsync()
    {
        return Task.FromResult(Pending.Select(x => new FiscalizacionBienDto
        {
            NumeroCaso = x.NumeroCaso,
            Inspector = x.Inspector,
            Estado = x.Estado,
            Resultado = x.Resultado,
            FechaProgramada = x.FechaProgramada,
            FechaCierre = x.FechaCierre,
            Hallazgos = x.Hallazgos
        }).ToList());
    }

    public Task<FiscalizacionBienDto> ProgramarAsync(BienInmuebleDto bien)
    {
        var fiscalizacion = new FiscalizacionBienDto
        {
            NumeroCaso = $"FIS-{DateTime.Today.Year}-{(Pending.Count + 20).ToString("000")}",
            Inspector = "Equipo territorial",
            Estado = "Programada",
            FechaProgramada = DateTime.Today.AddDays(10),
            Hallazgos = "Programación generada desde expediente de finca."
        };

        Pending.Add(fiscalizacion);
        return Task.FromResult(fiscalizacion);
    }

    public Task<FiscalizacionBienDto> RegistrarResultadoAsync(BienInmuebleDto bien, string resultado)
    {
        var latest = bien.Fiscalizaciones.OrderByDescending(x => x.FechaProgramada).FirstOrDefault();
        if (latest is null)
        {
            latest = new FiscalizacionBienDto
            {
                NumeroCaso = $"FIS-{DateTime.Today.Year}-{(Pending.Count + 50).ToString("000")}",
                Inspector = "Equipo territorial",
                Estado = "Finalizada",
                FechaProgramada = DateTime.Today,
                FechaCierre = DateTime.Today,
                Resultado = resultado,
                Hallazgos = "Registro generado automáticamente."
            };
        }
        else
        {
            latest.Estado = "Finalizada";
            latest.Resultado = resultado;
            latest.FechaCierre = DateTime.Today;
        }

        return Task.FromResult(latest);
    }
}
