using BlazorApp.Models;
using BlazorApp.Models.Auditoria;
using System.Threading.Tasks;

namespace BlazorApp.Services;

public interface IAuditoriaService
{
    Task<List<AuditoriaCambioDto>> GetRecentAsync();
    Task<List<AuditoriaCambioDto>> GetByContribuyenteAsync(int contribuyenteId);
    Task RegistrarCambioAsync(int contribuyenteId, AuditoriaCambioDto cambio);

    Task<List<AuditEventDto>> GetAuditEvents();
    Task<AuditEventDetailDto?> GetEventById(string id);
    Task<List<AuditEventDto>> GetEventsByModule(string module);
    Task<List<AuditEventDto>> GetEventsByExpedient(string expedientNumber);
    Task<List<AuditEventDto>> GetEventsByEntity(string entityName, string entityId);
    Task<List<AuditSecurityEventDto>> GetSecurityEvents();
    Task<List<AuditIntegrationEventDto>> GetIntegrationEvents();
    Task<List<AuditConfigurationEventDto>> GetConfigurationEvents();
    Task<List<AuditModuleSummaryDto>> GetModuleSummaries();
    Task<List<AuditEventDto>> GetFilteredEvents(AuditFilterDto filter);
    Task<List<AuditTraceItemDto>> GetTraceByExpedient(string expedientNumber);
    Task RegisterMockEventAsync(AuditExportRequestDto request);
}
