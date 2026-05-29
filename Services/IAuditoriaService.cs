using BlazorApp.Models;
using System.Threading.Tasks;

namespace BlazorApp.Services;

public interface IAuditoriaService
{
    Task<List<AuditoriaCambioDto>> GetRecentAsync();
    Task<List<AuditoriaCambioDto>> GetByContribuyenteAsync(int contribuyenteId);
    Task RegistrarCambioAsync(int contribuyenteId, AuditoriaCambioDto cambio);
}
