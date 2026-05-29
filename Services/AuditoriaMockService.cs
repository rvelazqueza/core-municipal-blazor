using BlazorApp.Models;
using System.Threading.Tasks;

namespace BlazorApp.Services;

public class AuditoriaMockService : IAuditoriaService
{
    private static readonly Dictionary<int, List<AuditoriaCambioDto>> Store = new()
    {
        [1] = new List<AuditoriaCambioDto>
        {
            new() { Usuario = "mlopez", FechaHora = DateTime.Now.AddHours(-2), CampoModificado = "Correo", ValorAnterior = "", ValorNuevo = "ana.solano@correo.go.cr", Origen = "Formulario RUC" },
            new() { Usuario = "srivera", FechaHora = DateTime.Now.AddDays(-1), CampoModificado = "Estado", ValorAnterior = "Pendiente revision", ValorNuevo = "Activo", Origen = "Control de calidad" }
        },
        [2] = new List<AuditoriaCambioDto>
        {
            new() { Usuario = "tcalidad", FechaHora = DateTime.Now.AddHours(-6), CampoModificado = "Representante legal", ValorAnterior = "", ValorNuevo = "Carlos Vega", Origen = "Registro Nacional" }
        }
    };

    public Task<List<AuditoriaCambioDto>> GetRecentAsync()
    {
        var result = Store.Values.SelectMany(x => x).OrderByDescending(x => x.FechaHora).Take(8).ToList();
        return Task.FromResult(result);
    }

    public Task<List<AuditoriaCambioDto>> GetByContribuyenteAsync(int contribuyenteId)
    {
        var result = Store.TryGetValue(contribuyenteId, out var list)
            ? list.OrderByDescending(x => x.FechaHora).ToList()
            : new List<AuditoriaCambioDto>();
        return Task.FromResult(result);
    }

    public Task RegistrarCambioAsync(int contribuyenteId, AuditoriaCambioDto cambio)
    {
        if (!Store.ContainsKey(contribuyenteId))
        {
            Store[contribuyenteId] = new List<AuditoriaCambioDto>();
        }

        Store[contribuyenteId].Insert(0, cambio);
        return Task.CompletedTask;
    }
}
