using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorApp.Models;
using BlazorApp.Models.Auditoria;

namespace BlazorApp.Services;

public class AuditoriaMockService : IAuditoriaService
{
    private static readonly object Sync = new();
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

    private static readonly List<AuditEventDto> AuditEvents = BuildGeneralEvents();
    private static readonly List<AuditSecurityEventDto> SecurityEvents = BuildSecurityEvents();
    private static readonly List<AuditIntegrationEventDto> IntegrationEvents = BuildIntegrationEvents();
    private static readonly List<AuditConfigurationEventDto> ConfigurationEvents = BuildConfigurationEvents();

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
        lock (Sync)
        {
            if (!Store.ContainsKey(contribuyenteId))
            {
                Store[contribuyenteId] = new List<AuditoriaCambioDto>();
            }

            Store[contribuyenteId].Insert(0, cambio);
            AuditEvents.Insert(0, new AuditEventDto
            {
                Id = $"AUD-{DateTime.Now:yyyyMMddHHmmssfff}",
                EventDate = cambio.FechaHora == default ? DateTime.Now : cambio.FechaHora,
                UserName = cambio.Usuario,
                UserRole = "Operador",
                Module = "RUC",
                Action = $"Cambio en {cambio.CampoModificado}",
                EventType = "Modificación",
                Severity = "Información",
                Status = "Exitoso",
                EntityName = "Contribuyente",
                EntityId = contribuyenteId.ToString(),
                ExpedientNumber = $"EXP-{contribuyenteId:000}",
                TaxpayerIdentification = $"1-234-{contribuyenteId:0000}",
                TaxpayerName = $"Contribuyente {contribuyenteId}",
                PreviousValue = cambio.ValorAnterior,
                NewValue = cambio.ValorNuevo,
                Source = cambio.Origen,
                Channel = "Formulario",
                IpAddress = "10.0.0.15",
                Device = "PC institucional",
                Browser = "Edge",
                SessionId = $"LEG-{contribuyenteId:0000}",
                CorrelationId = $"COR-LEG-{contribuyenteId:0000}",
                RequestId = $"REQ-LEG-{contribuyenteId:0000}",
                ResultMessage = "Cambio registrado en bitácora mock."
            });
        }

        return Task.CompletedTask;
    }

    public Task<List<AuditEventDto>> GetAuditEvents()
    {
        return Task.FromResult(GetAllEvents().OrderByDescending(x => x.EventDate).ToList());
    }

    public Task<AuditEventDetailDto?> GetEventById(string id)
    {
        var evt = GetAllEvents().FirstOrDefault(x => string.Equals(x.Id, id, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(evt is null ? null : ToDetail(evt));
    }

    public Task<List<AuditEventDto>> GetEventsByModule(string module)
    {
        return Task.FromResult(FilterEvents(x => string.Equals(x.Module, module, StringComparison.OrdinalIgnoreCase)));
    }

    public Task<List<AuditEventDto>> GetEventsByExpedient(string expedientNumber)
    {
        return Task.FromResult(FilterEvents(x => string.Equals(x.ExpedientNumber, expedientNumber, StringComparison.OrdinalIgnoreCase)));
    }

    public Task<List<AuditEventDto>> GetEventsByEntity(string entityName, string entityId)
    {
        return Task.FromResult(FilterEvents(x =>
            string.Equals(x.EntityName, entityName, StringComparison.OrdinalIgnoreCase) &&
            string.Equals(x.EntityId, entityId, StringComparison.OrdinalIgnoreCase)));
    }

    public Task<List<AuditSecurityEventDto>> GetSecurityEvents()
    {
        return Task.FromResult(SecurityEvents.OrderByDescending(x => x.EventDate).ToList());
    }

    public Task<List<AuditIntegrationEventDto>> GetIntegrationEvents()
    {
        return Task.FromResult(IntegrationEvents.OrderByDescending(x => x.EventDate).ToList());
    }

    public Task<List<AuditConfigurationEventDto>> GetConfigurationEvents()
    {
        return Task.FromResult(ConfigurationEvents.OrderByDescending(x => x.EventDate).ToList());
    }

    public Task<List<AuditModuleSummaryDto>> GetModuleSummaries()
    {
        var summaries = GetAllEvents()
            .GroupBy(x => x.Module)
            .Select(group => new AuditModuleSummaryDto
            {
                Module = group.Key,
                TotalEvents = group.Count(),
                CriticalEvents = group.Count(x => IsCritical(x.Severity)),
                LatestEventDate = group.Max(x => x.EventDate),
                LatestUserName = group.OrderByDescending(x => x.EventDate).First().UserName,
                Status = group.OrderByDescending(x => x.EventDate).First().Status,
                LastAction = group.OrderByDescending(x => x.EventDate).First().Action
            })
            .OrderByDescending(x => x.TotalEvents)
            .ToList();

        return Task.FromResult(summaries);
    }

    public Task<List<AuditEventDto>> GetFilteredEvents(AuditFilterDto filter)
    {
        var filtered = FilterEvents(evt =>
        {
            if (filter.DateRange?.Start is DateTime start)
            {
                if (evt.EventDate.Date < start.Date)
                {
                    return false;
                }
            }

            if (filter.DateRange?.End is DateTime end)
            {
                if (evt.EventDate.Date > end.Date)
                {
                    return false;
                }
            }

            if (!IsMatch(evt.UserName, filter.UserName)) return false;
            if (!IsMatch(evt.Module, filter.Module)) return false;
            if (!IsMatch(evt.Action, filter.Action)) return false;
            if (!IsMatch(evt.EventType, filter.EventType)) return false;
            if (!IsMatch(evt.Severity, filter.Severity)) return false;
            if (!IsMatch(evt.Status, filter.Status)) return false;
            if (!IsMatch(evt.TaxpayerIdentification, filter.TaxpayerIdentification)) return false;
            if (!IsMatch(evt.ExpedientNumber, filter.ExpedientNumber)) return false;
            if (!IsMatch(evt.EntityName, filter.EntityName)) return false;
            if (!IsMatch(evt.EntityId, filter.EntityId)) return false;
            if (!IsMatch(evt.IpAddress, filter.IpAddress)) return false;
            if (!IsMatch(evt.Channel, filter.Channel)) return false;
            if (!IsMatch(evt.Source, filter.Source)) return false;
            if (!IsMatch(evt.ResultMessage, filter.Result)) return false;

            if (!string.IsNullOrWhiteSpace(filter.FreeText))
            {
                var haystack = string.Join(" | ", new[]
                {
                    evt.Id, evt.UserName, evt.UserRole, evt.Module, evt.Action, evt.EventType, evt.Severity, evt.Status,
                    evt.EntityName, evt.EntityId, evt.ExpedientNumber, evt.TaxpayerIdentification, evt.TaxpayerName,
                    evt.PreviousValue, evt.NewValue, evt.Source, evt.Channel, evt.IpAddress, evt.Device, evt.Browser,
                    evt.SessionId, evt.CorrelationId, evt.RequestId, evt.ResultMessage
                });

                if (haystack.Contains(filter.FreeText, StringComparison.OrdinalIgnoreCase) == false)
                {
                    return false;
                }
            }

            return true;
        });

        return Task.FromResult(filtered);
    }

    public Task<List<AuditTraceItemDto>> GetTraceByExpedient(string expedientNumber)
    {
        var trace = GetAllEvents()
            .Where(x => string.Equals(x.ExpedientNumber, expedientNumber, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(x => x.EventDate)
            .Take(10)
            .Select(x => new AuditTraceItemDto
            {
                Id = x.Id,
                EventDate = x.EventDate,
                UserName = x.UserName,
                Action = x.Action,
                PreviousState = string.IsNullOrWhiteSpace(x.PreviousValue) ? x.Status : x.PreviousValue,
                NewState = string.IsNullOrWhiteSpace(x.NewValue) ? x.Status : x.NewValue,
                Module = x.Module,
                Comment = x.ResultMessage,
                Severity = x.Severity,
                Status = x.Status
            })
            .ToList();

        return Task.FromResult(trace);
    }

    public Task RegisterMockEventAsync(AuditExportRequestDto request)
    {
        lock (Sync)
        {
            AuditEvents.Insert(0, new AuditEventDto
            {
                Id = $"AUD-{DateTime.Now:yyyyMMddHHmmssfff}",
                EventDate = DateTime.Now,
                UserName = string.IsNullOrWhiteSpace(request.RequestedBy) ? "auditor.demo" : request.RequestedBy,
                UserRole = "Auditor",
                Module = string.IsNullOrWhiteSpace(request.Module) ? "Auditoría" : request.Module,
                Action = "Exportación mock de auditoría solicitada",
                EventType = "Exportación mock",
                Severity = "Información",
                Status = "Operativo",
                EntityName = string.IsNullOrWhiteSpace(request.EntityName) ? "Auditoría" : request.EntityName,
                EntityId = request.EntityId,
                ExpedientNumber = request.ExpedientNumber,
                TaxpayerIdentification = string.Empty,
                TaxpayerName = string.Empty,
                PreviousValue = string.Empty,
                NewValue = request.Description,
                Source = "Panel de exportación",
                Channel = request.Format,
                IpAddress = "127.0.0.1",
                Device = "Desktop demo",
                Browser = "Edge",
                SessionId = $"EXP-{Guid.NewGuid():N}".Substring(0, 12),
                CorrelationId = $"COR-{Guid.NewGuid():N}".Substring(0, 12),
                RequestId = $"REQ-{Guid.NewGuid():N}".Substring(0, 12),
                ResultMessage = "Exportación disponible en implementación backend."
            });
        }

        return Task.CompletedTask;
    }

    private static List<AuditEventDto> GetAllEvents()
    {
        return AuditEvents
            .Concat(SecurityEvents.Cast<AuditEventDto>())
            .Concat(IntegrationEvents.Cast<AuditEventDto>())
            .Concat(ConfigurationEvents.Cast<AuditEventDto>())
            .ToList();
    }

    private static List<AuditEventDto> FilterEvents(Func<AuditEventDto, bool> predicate)
    {
        return GetAllEvents()
            .Where(predicate)
            .OrderByDescending(x => x.EventDate)
            .ToList();
    }

    private static AuditEventDetailDto ToDetail(AuditEventDto evt)
    {
        return new AuditEventDetailDto
        {
            Id = evt.Id,
            EventDate = evt.EventDate,
            UserName = evt.UserName,
            UserRole = evt.UserRole,
            Module = evt.Module,
            Action = evt.Action,
            EventType = evt.EventType,
            Severity = evt.Severity,
            Status = evt.Status,
            EntityName = evt.EntityName,
            EntityId = evt.EntityId,
            ExpedientNumber = evt.ExpedientNumber,
            TaxpayerIdentification = evt.TaxpayerIdentification,
            TaxpayerName = evt.TaxpayerName,
            PreviousValue = evt.PreviousValue,
            NewValue = evt.NewValue,
            Source = evt.Source,
            Channel = evt.Channel,
            IpAddress = evt.IpAddress,
            Device = evt.Device,
            Browser = evt.Browser,
            SessionId = evt.SessionId,
            CorrelationId = evt.CorrelationId,
            RequestId = evt.RequestId,
            ResultMessage = evt.ResultMessage,
            FieldChanged = string.IsNullOrWhiteSpace(evt.PreviousValue) && string.IsNullOrWhiteSpace(evt.NewValue)
                ? evt.Action
                : evt.Action,
            Observation = $"Evento mock del módulo {evt.Module}.",
            TechnicalMessage = evt.ResultMessage,
            IntegrationRelated = evt.Module == "Integraciones" ? evt.Source : string.Empty,
            PreviousState = evt.PreviousValue,
            NewState = evt.NewValue,
            ActionTaken = evt.Action
        };
    }

    private static bool IsMatch(string source, string filter)
    {
        return string.IsNullOrWhiteSpace(filter) || source.Contains(filter, StringComparison.OrdinalIgnoreCase);
    }

    private static bool IsCritical(string severity)
    {
        return severity.Equals("Crítico", StringComparison.OrdinalIgnoreCase)
            || severity.Equals("Error", StringComparison.OrdinalIgnoreCase)
            || severity.Equals("Advertencia", StringComparison.OrdinalIgnoreCase);
    }

    private static List<AuditEventDto> BuildGeneralEvents()
    {
        var modules = new[]
        {
            "Login", "Dashboard", "RUC", "Bienes Inmuebles", "Patentes",
            "Plataforma de Servicios", "Permisos de Construcción", "Mercado Municipal",
            "Notificaciones", "Cobro mock", "Cuenta Tributaria mock"
        };

        var eventTypes = new[]
        {
            "Acceso", "Autenticación", "Consulta", "Creación", "Modificación",
            "Eliminación lógica", "Cambio de estado", "Validación", "Borrador", "Finalización de proceso"
        };

        var actions = new[]
        {
            "Consulta de expediente", "Creación de registro", "Modificación de registro", "Guardado de borrador",
            "Cambio de estado", "Validación de datos", "Finalización de trámite", "Simulación de integración",
            "Generación de notificación mock", "Simulación de pago mock"
        };

        var severities = new[] { "Información", "Éxito", "Advertencia", "Error", "Crítico" };
        var statuses = new[] { "Exitoso", "Fallido", "Pendiente", "Observado", "Operativo", "Bloqueado" };
        var channels = new[] { "Web", "Backoffice", "API mock", "Móvil", "Plataforma digital" };
        var sources = new[] { "Formulario", "Flujo institucional", "Servicio mock", "Proceso batch", "Revisión manual" };

        var list = new List<AuditEventDto>();
        var seed = 0;

        for (var expedientIndex = 1; expedientIndex <= 8; expedientIndex++)
        {
            var expedient = $"EXP-{expedientIndex:000}";
            for (var step = 1; step <= 10; step++)
            {
                var module = modules[(seed + step) % modules.Length];
                var action = actions[(seed + step) % actions.Length];
                var eventType = eventTypes[(seed + step) % eventTypes.Length];
                var severity = severities[(seed + step) % severities.Length];
                var status = statuses[(seed + step) % statuses.Length];
                var channel = channels[(seed + step) % channels.Length];
                var source = sources[(seed + step) % sources.Length];
                var result = status switch
                {
                    "Fallido" => "Acción con validación fallida simulada.",
                    "Observado" => "Evento observado para revisión posterior.",
                    "Pendiente" => "Proceso pendiente de confirmación.",
                    "Bloqueado" => "Evento bloqueado por validación mock.",
                    _ => "Evento registrado correctamente en bitácora mock."
                };

                list.Add(new AuditEventDto
                {
                    Id = $"AUD-{expedientIndex:000}-{step:00}",
                    EventDate = DateTime.Now.AddMinutes(-((expedientIndex * 20) + step * 7)),
                    UserName = $"usuario{((expedientIndex + step) % 12) + 1:00}",
                    UserRole = module switch
                    {
                        "Login" => "Usuario",
                        "Dashboard" => "Supervisor",
                        "RUC" => "Analista",
                        "Bienes Inmuebles" => "Inspector",
                        "Patentes" => "Gestor",
                        "Plataforma de Servicios" => "Atención",
                        "Permisos de Construcción" => "Técnico",
                        "Mercado Municipal" => "Administrador",
                        "Notificaciones" => "Operador",
                        "Cobro mock" => "Caja",
                        _ => "Auditor"
                    },
                    Module = module,
                    Action = action,
                    EventType = eventType,
                    Severity = severity,
                    Status = status,
                    EntityName = module switch
                    {
                        "Login" or "Dashboard" => "Sesión",
                        "RUC" => "Contribuyente",
                        "Bienes Inmuebles" => "Finca",
                        "Patentes" => "Patente",
                        "Plataforma de Servicios" => "Trámite",
                        "Permisos de Construcción" => "Expediente",
                        "Mercado Municipal" => "Local",
                        "Notificaciones" => "Notificación",
                        "Cobro mock" or "Cuenta Tributaria mock" => "Cuenta",
                        _ => "Configuración"
                    },
                    EntityId = $"{module[..Math.Min(3, module.Length)].ToUpperInvariant()}-{expedientIndex:000}-{step:00}",
                    ExpedientNumber = expedient,
                    TaxpayerIdentification = $"1-234-{expedientIndex:000}-{step:00}",
                    TaxpayerName = $"Contribuyente {expedientIndex:000}-{step:00}",
                    PreviousValue = step % 2 == 0 ? "Valor anterior mock" : string.Empty,
                    NewValue = step % 3 == 0 ? "Valor nuevo mock" : $"Estado {status}",
                    Source = source,
                    Channel = channel,
                    IpAddress = $"192.168.{expedientIndex}.{step}",
                    Device = step % 2 == 0 ? "Laptop institucional" : "Desktop demo",
                    Browser = step % 2 == 0 ? "Chrome" : "Edge",
                    SessionId = $"SES-{expedientIndex:000}-{step:00}",
                    CorrelationId = $"COR-{expedientIndex:000}-{step:00}",
                    RequestId = $"REQ-{expedientIndex:000}-{step:00}",
                    ResultMessage = result
                });
                seed++;
            }
        }

        for (var index = 1; index <= 8; index++)
        {
            list.Add(new AuditEventDto
            {
                Id = $"NOT-{index:000}",
                EventDate = DateTime.Now.AddMinutes(-(400 + index * 11)),
                UserName = $"notif{index:00}",
                UserRole = "Operador",
                Module = "Notificaciones",
                Action = "Generación de notificación mock",
                EventType = "Notificación simulada",
                Severity = index % 3 == 0 ? "Advertencia" : "Información",
                Status = index % 2 == 0 ? "Exitoso" : "Operativo",
                EntityName = "Notificación",
                EntityId = $"NOT-{index:000}",
                ExpedientNumber = $"EXP-{((index - 1) % 8) + 1:000}",
                TaxpayerIdentification = $"1-234-NOT-{index:000}",
                TaxpayerName = $"Contribuyente notificación {index:000}",
                Source = "Canal notificaciones",
                Channel = "Plataforma digital",
                IpAddress = $"172.20.0.{index}",
                Device = "Servidor mock",
                Browser = "System",
                SessionId = $"NOT-SES-{index:000}",
                CorrelationId = $"NOT-COR-{index:000}",
                RequestId = $"NOT-REQ-{index:000}",
                ResultMessage = "Notificación registrada."
            });
        }

        for (var index = 1; index <= 8; index++)
        {
            list.Add(new AuditEventDto
            {
                Id = $"EXP-{index:000}",
                EventDate = DateTime.Now.AddMinutes(-(500 + index * 13)),
                UserName = "auditor.demo",
                UserRole = "Auditor",
                Module = "Auditoría",
                Action = "Exportación mock de auditoría solicitada",
                EventType = "Exportación mock",
                Severity = "Información",
                Status = "Operativo",
                EntityName = "Auditoría",
                EntityId = $"EXP-REQ-{index:000}",
                ExpedientNumber = $"EXP-{index:000}",
                TaxpayerIdentification = string.Empty,
                TaxpayerName = string.Empty,
                Source = "Panel de auditoría",
                Channel = (index % 4) switch
                {
                    0 => "Excel",
                    1 => "PDF",
                    2 => "Word",
                    _ => "Impresión"
                },
                IpAddress = "127.0.0.1",
                Device = "Desktop demo",
                Browser = "Edge",
                SessionId = $"EXP-SES-{index:000}",
                CorrelationId = $"EXP-COR-{index:000}",
                RequestId = $"EXP-REQ-{index:000}",
                ResultMessage = "Exportación disponible en implementación backend."
            });
        }

        return list;
    }

    private static List<AuditSecurityEventDto> BuildSecurityEvents()
    {
        var items = new[]
        {
            ("Inicio de sesión exitoso", "Exitoso", "Información", "Acceso concedido"),
            ("Inicio de sesión fallido", "Fallido", "Advertencia", "Intento inválido"),
            ("Cierre de sesión", "Exitoso", "Información", "Sesión cerrada"),
            ("MFA validado exitoso", "Exitoso", "Información", "Segundo factor validado"),
            ("MFA validado fallido", "Fallido", "Error", "Segundo factor rechazado"),
            ("Firma digital aplicada", "Operativo", "Información", "Firma aplicada"),
            ("Cambio de contraseña", "Exitoso", "Información", "Actualización confirmada"),
            ("Recuperación de contraseña", "Exitoso", "Información", "Proceso de recuperación"),
            ("Acceso a módulo", "Exitoso", "Información", "Módulo autorizado"),
            ("Intento de acceso no autorizado mock", "Bloqueado", "Crítico", "Acceso bloqueado")
        };

        return items.Select((item, index) => new AuditSecurityEventDto
        {
            Id = $"SEC-{index + 1:000}",
            EventDate = DateTime.Now.AddHours(-(index + 1)),
            UserName = index % 2 == 0 ? "admin.demo" : "auditor.demo",
            UserRole = index % 2 == 0 ? "Administrador" : "Auditor",
            Module = "Seguridad",
            Action = item.Item1,
            EventType = "Seguridad",
            Severity = item.Item3,
            Status = item.Item2,
            EntityName = "Usuario",
            EntityId = $"USR-{index + 1:000}",
            ExpedientNumber = string.Empty,
            TaxpayerIdentification = string.Empty,
            TaxpayerName = string.Empty,
            PreviousValue = string.Empty,
            NewValue = string.Empty,
            Source = "Seguridad institucional mock",
            Channel = "Backoffice",
            IpAddress = $"10.10.0.{index + 1}",
            Device = index % 2 == 0 ? "Laptop" : "Desktop",
            Browser = index % 2 == 0 ? "Edge" : "Chrome",
            SessionId = $"SEC-SES-{index + 1:000}",
            CorrelationId = $"SEC-COR-{index + 1:000}",
            RequestId = $"SEC-REQ-{index + 1:000}",
            ResultMessage = item.Item4,
            SecurityActionTaken = item.Item4
        }).ToList();
    }

    private static List<AuditIntegrationEventDto> BuildIntegrationEvents()
    {
        var items = new[]
        {
            ("RUC", "Seguridad", "RUC", "Operativo", "Consulta sincronizada"),
            ("Bienes Inmuebles", "Bienes Inmuebles", "Catastro", "Referencial", "Consulta de finca recibida"),
            ("Patentes", "Patentes", "Patentes", "Preparado", "Intercambio listo para despliegue"),
            ("Plataforma de Servicios", "Plataforma de Servicios", "Trámites", "Operativo", "Expediente consultado"),
            ("Permisos de Construcción", "Permisos de Construcción", "APC / CFIA", "Operativo", "Estado de expediente sincronizado"),
            ("Mercado Municipal", "Mercado Municipal", "Mercado", "Preparado", "Gestión de local referencial"),
            ("GIS", "GIS", "Mapas", "Referencial", "Ubicación geográfica consultada"),
            ("Cobro", "Cobro", "Caja", "Operativo", "Consulta de cobro registrada"),
            ("Cuenta Tributaria", "Cuenta Tributaria mock", "Tesorería", "Preparado", "Cuenta tributaria preparada"),
            ("Tesorería", "Cobro", "Tesorería", "Operativo", "Conciliación registrada"),
            ("Cajas", "Cobro", "Cajas", "Operativo", "Movimiento de caja registrado"),
            ("Conectividad", "Integraciones", "Infraestructura", "Referencial", "Conectividad validada"),
            ("Notificaciones", "Notificaciones", "Canal digital", "Operativo", "Plantilla notificada"),
            ("MIMUNIENCASA", "Plataforma de Servicios", "MIMUNIENCASA", "Preparado", "Canal remoto listo"),
            ("VUI", "Plataforma de Servicios", "VUI", "Fallido mock", "Interoperabilidad en revisión")
        };

        return items.Select((item, index) => new AuditIntegrationEventDto
        {
            Id = $"INT-{index + 1:000}",
            EventDate = DateTime.Now.AddMinutes(-(index + 1) * 15),
            UserName = index % 2 == 0 ? "integ.demo" : "admin.demo",
            UserRole = "Integraciones",
            Module = "Integraciones",
            Action = $"Integración {item.Item1}",
            EventType = "Integración simulada",
            Severity = item.Item4 == "Operativo" ? "Información" : "Advertencia",
            Status = item.Item4,
            EntityName = "Integración",
            EntityId = $"INT-{index + 1:000}",
            ExpedientNumber = string.Empty,
            TaxpayerIdentification = string.Empty,
            TaxpayerName = string.Empty,
            PreviousValue = string.Empty,
            NewValue = string.Empty,
            Source = item.Item2,
            Channel = "Servicio mock",
            IpAddress = $"172.16.0.{index + 1}",
            Device = "Gateway mock",
            Browser = "System",
            SessionId = $"INT-SES-{index + 1:000}",
            CorrelationId = $"INT-COR-{index + 1:000}",
            RequestId = $"INT-REQ-{index + 1:000}",
            ResultMessage = item.Item5,
            ModuleOrigin = item.Item2,
            ModuleDestination = item.Item3,
            IntegrationState = item.Item4
        }).ToList();
    }

    private static List<AuditConfigurationEventDto> BuildConfigurationEvents()
    {
        var items = new[]
        {
            ("Cambio de parámetro", "RUC", "Parámetro de validación", "1", "2", "Ajuste del umbral de validación"),
            ("Cambio de catálogo", "Bienes Inmuebles", "Catálogo de estados", "Activo", "Suspendido", "Homologación de catálogo"),
            ("Cambio de estado", "Patentes", "Estado de patente", "Pendiente", "Activo", "Actualización de estado"),
            ("Alta de valor", "Mercado Municipal", "Factor de ponderación", "0.85", "0.90", "Nuevo valor referencial"),
            ("Inactivación de valor", "Configuración", "Catálogo de motivos", "Disponible", "Inactivo", "Depuración mock"),
            ("Cambio de regla mock", "Permisos de Construcción", "Regla de revisión", "Regla A", "Regla B", "Cambio operativo"),
            ("Cambio de permiso mock", "Seguridad", "Permiso de módulo", "Lectura", "Lectura/Escritura", "Permiso ampliado"),
            ("Cambio de rol mock", "Seguridad", "Rol institucional", "Operador", "Supervisor", "Ajuste de acceso"),
            ("Cambio de catálogo", "Plataforma de Servicios", "Catálogo de trámites", "General", "Prioritario", "Catálogo actualizado"),
            ("Cambio de parámetro", "Integraciones", "Timeout de integración", "30", "45", "Ajuste de tiempo de espera")
        };

        return items.Select((item, index) => new AuditConfigurationEventDto
        {
            Id = $"CFG-{index + 1:000}",
            EventDate = DateTime.Now.AddMinutes(-(index + 1) * 25),
            UserName = index % 2 == 0 ? "config.demo" : "admin.demo",
            UserRole = "Configurador",
            Module = "Configuración",
            Action = item.Item1,
            EventType = "Configuración",
            Severity = index % 3 == 0 ? "Advertencia" : "Información",
            Status = index % 2 == 0 ? "Exitoso" : "Simulado",
            EntityName = "Configuración",
            EntityId = $"CFG-{index + 1:000}",
            ExpedientNumber = string.Empty,
            TaxpayerIdentification = string.Empty,
            TaxpayerName = string.Empty,
            PreviousValue = item.Item4,
            NewValue = item.Item5,
            Source = "Catálogo mock",
            Channel = "Backoffice",
            IpAddress = $"192.168.50.{index + 1}",
            Device = "PC admin",
            Browser = "Edge",
            SessionId = $"CFG-SES-{index + 1:000}",
            CorrelationId = $"CFG-COR-{index + 1:000}",
            RequestId = $"CFG-REQ-{index + 1:000}",
            ResultMessage = item.Item6,
            Parameter = item.Item3,
            Justification = item.Item6,
            AffectedModule = item.Item2,
            ConfigurationState = index % 2 == 0 ? "Exitoso" : "Operativo"
        }).ToList();
    }
}