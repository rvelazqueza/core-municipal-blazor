using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorApp.Helpers;
using BlazorApp.Models.PlataformaServicios;

namespace BlazorApp.Services.PlataformaServicios;

public class PlataformaServiciosMockService
{
    private static readonly object Gate = new();
    private static bool seeded;
    private static List<PsProcedureTypeDto> procedureTypes = new();
    private static List<PsProcedureDto> procedures = new();
    private static List<PsReportDto> reports = new();
    private static List<PsCatalogItemDto> integrations = new();

    private readonly LocalStorageHelper _localStorage;
    private const string PROCEDURES_KEY = "coremunicipal.plataforma.tramites";
    private const string DRAFT_KEY = "coremunicipal.plataforma.borrador";

    public PlataformaServiciosMockService(LocalStorageHelper localStorage)
    {
        _localStorage = localStorage;
        EnsureSeeded();
    }

    public Task<List<PsProcedureTypeDto>> GetProcedureTypesAsync() => Task.FromResult(procedureTypes.ToList());

    public async Task<List<PsProcedureDto>> GetProceduresAsync()
    {
        var stored = await _localStorage.GetItemAsync<List<PsProcedureDto>>(PROCEDURES_KEY);
        if (stored != null && stored.Any())
        {
            procedures = stored;
        }
        return procedures.OrderByDescending(item => item.EntryDate).ToList();
    }

    public async Task<PsProcedureDto?> GetProcedureAsync(string id)
    {
        await GetProceduresAsync();
        var procedure = procedures.FirstOrDefault(item => string.Equals(item.Id, id, StringComparison.OrdinalIgnoreCase) || string.Equals(item.CaseNumber, id, StringComparison.OrdinalIgnoreCase));
        return procedure;
    }

    public Task<List<PsReportDto>> GetReportsAsync() => Task.FromResult(reports.ToList());

    public Task<List<PsCatalogItemDto>> GetIntegrationsAsync() => Task.FromResult(integrations.ToList());

    public async Task<PsProcedureDto> RegisterProcedureAsync(PsProcedureDto procedure)
    {
        await GetProceduresAsync();

        lock (Gate)
        {
            if (string.IsNullOrWhiteSpace(procedure.Id))
            {
                procedure.Id = Guid.NewGuid().ToString("N");
            }

            procedure.CaseNumber = string.IsNullOrWhiteSpace(procedure.CaseNumber)
                ? $"PS-{DateTime.Now:yyyy}-{procedures.Count + 1:0000}"
                : procedure.CaseNumber;

            procedure.EntryDate = procedure.EntryDate == default ? DateTime.Now : procedure.EntryDate;
            procedure.LimitDate = procedure.LimitDate == default ? DateTime.Now.AddDays(10) : procedure.LimitDate;
            procedure.Status = string.IsNullOrWhiteSpace(procedure.Status) ? "Recibido" : procedure.Status;

            procedure.Classification ??= new PsClassificationDto();
            procedure.QualityControl ??= new PsQualityControlDto();
            procedure.Resolution ??= new PsResolutionDto();
            procedure.Deadline ??= new PsDeadlineDto();

            procedure.History.Insert(0, new PsHistoryEventDto
            {
                Date = DateTime.Now,
                User = "system.user",
                Action = "Registro final",
                PreviousStatus = "Borrador",
                NewStatus = procedure.Status,
                Department = procedure.Department,
                Comment = "El trámite fue registrado correctamente.",
                Origin = "Plataforma de Servicios"
            });

            procedure.AuditEvents.Insert(0, new PsAuditEventDto
            {
                Action = "Registro final",
                Description = $"Se creó el expediente {procedure.CaseNumber} desde la Plataforma de Servicios.",
                Severity = "Success"
            });

            var existingIndex = procedures.FindIndex(item => string.Equals(item.Id, procedure.Id, StringComparison.OrdinalIgnoreCase));
            if (existingIndex >= 0)
            {
                procedures[existingIndex] = procedure;
            }
            else
            {
                procedures.Insert(0, procedure);
            }

            _ = _localStorage.SetItemAsync(PROCEDURES_KEY, procedures);
            return procedure;
        }
    }

    public async Task<PsProcedureDto?> GetDraftAsync()
    {
        return await _localStorage.GetItemAsync<PsProcedureDto>(DRAFT_KEY);
    }

    public async Task SaveDraftAsync(PsProcedureDto draft)
    {
        await _localStorage.SetItemAsync(DRAFT_KEY, draft);
    }

    public async Task DeleteDraftAsync()
    {
        await _localStorage.RemoveItemAsync(DRAFT_KEY);
    }

    public async Task<PsProcedureDto> UpdateProcedureAsync(PsProcedureDto procedure)
    {
        await GetProceduresAsync();

        lock (Gate)
        {
            var existingIndex = procedures.FindIndex(item => string.Equals(item.Id, procedure.Id, StringComparison.OrdinalIgnoreCase));
            if (existingIndex >= 0)
            {
                procedures[existingIndex] = procedure;
            }
            else
            {
                procedures.Insert(0, procedure);
            }

            _ = _localStorage.SetItemAsync(PROCEDURES_KEY, procedures);
            return procedure;
        }
    }

    private static void EnsureSeeded()
    {
        if (seeded)
        {
            return;
        }

        lock (Gate)
        {
            if (seeded)
            {
                return;
            }

            procedureTypes = new List<PsProcedureTypeDto>
            {
                // Facturación y cobros
                new() { Id = "1", Code = "ALTO-ALC", Name = "Alto Monto Alcantarillado Sanitario o Reclamo por Cobro Tanque Séptico", Description = "Estudio solicitado cuando el contribuyente presenta dudas sobre el cobro del servicio de alcantarillado sanitario o tanque séptico.", Category = "Facturación y cobros", RequiresServiceAccount = true, TargetDepartment = "Acueductos", EstimatedDays = 5, Active = true },
                new() { Id = "2", Code = "ALTO-AGUA", Name = "Alto monto agua", Description = "Inspección para analizar dudas sobre el consumo facturado cuando la lectura presenta una desviación significativa sobre el límite superior o promedio del abonado.", Category = "Facturación y cobros", RequiresServiceAccount = true, RequiresInspection = true, RequiresMeter = true, TargetDepartment = "Acueductos", EstimatedDays = 5, Active = true },
                new() { Id = "3", Code = "ALTO-BASURA", Name = "Alto Monto de Basura", Description = "Estudio para verificar el uso de la propiedad o las unidades servidas según la actividad económica utilizada para calcular recolección y tratamiento de residuos.", Category = "Facturación y cobros", RequiresProperty = true, RequiresInspection = true, TargetDepartment = "Catastro", EstimatedDays = 7, Active = true },
                new() { Id = "4", Code = "ALTO-VIAS", Name = "Alto monto limpieza de vías", Description = "Verificación del valor imponible del inmueble utilizado para determinar el cobro del servicio de limpieza de vías.", Category = "Facturación y cobros", RequiresProperty = true, TargetDepartment = "Catastro", EstimatedDays = 5, Active = true },
                new() { Id = "5", Code = "ALTO-PARQUES", Name = "Alto monto mantenimiento de parques", Description = "Verificación del valor imponible del inmueble utilizado para determinar el cobro del servicio de mantenimiento de parques.", Category = "Facturación y cobros", RequiresProperty = true, TargetDepartment = "Catastro", EstimatedDays = 5, Active = true },
                new() { Id = "6", Code = "CTA-DUP", Name = "Cuentas duplicadas", Description = "Estudio solicitado cuando el contribuyente considera que existe duplicidad en el cobro de servicios.", Category = "Facturación y cobros", RequiresServiceAccount = true, TargetDepartment = "Acueductos", EstimatedDays = 5, Active = true },
                new() { Id = "7", Code = "REC-TANQUE", Name = "Reclamo tanque séptico", Description = "Consulta sobre el origen y fundamento del cobro por tanque séptico.", Category = "Facturación y cobros", RequiresServiceAccount = true, TargetDepartment = "Acueductos", EstimatedDays = 3, Active = true },
                new() { Id = "8", Code = "REC-SERV", Name = "Reclamo por no prestación de servicio", Description = "Estudio solicitado cuando el contribuyente considera que uno o varios servicios cobrados no se están brindando.", Category = "Facturación y cobros", RequiresServiceAccount = true, TargetDepartment = "Tesorería", EstimatedDays = 5, Active = true },
                new() { Id = "9", Code = "SEG-RUBROS", Name = "Segregación de rubros", Description = "Separación de los diferentes rubros o servicios que integran la factura municipal.", Category = "Facturación y cobros", RequiresServiceAccount = true, TargetDepartment = "Tesorería", EstimatedDays = 4, Active = true },
                new() { Id = "10", Code = "VER-MEDIDAS", Name = "Verificación de medidas", Description = "Verificación de las medidas utilizadas para determinar el cobro del servicio de limpieza de vías.", Category = "Facturación y cobros", RequiresProperty = true, RequiresInspection = true, TargetDepartment = "Catastro", EstimatedDays = 7, Active = true },
                // Agua potable y medidores
                new() { Id = "11", Code = "AMP-PLAZO", Name = "Ampliación de plazo de reparación de fuga", Description = "Solicitud de un plazo adicional al otorgado por la administración para reparar una fuga interna.", Category = "Agua potable y medidores", RequiresServiceAccount = true, RequiresMeter = true, TargetDepartment = "Acueductos", EstimatedDays = 2, Active = true },
                new() { Id = "12", Code = "CAM-CAT", Name = "Cambio de categoría", Description = "Cambio de categoría de los servicios según el uso de suelo autorizado en el Plan Regulador.", Category = "Agua potable y medidores", RequiresServiceAccount = true, RequiresProperty = true, TargetDepartment = "Acueductos", EstimatedDays = 5, Active = true },
                new() { Id = "13", Code = "CAM-MED", Name = "Cambio de medidor", Description = "Sustitución del hidrómetro utilizado para medir el consumo de agua potable.", Category = "Agua potable y medidores", RequiresServiceAccount = true, RequiresMeter = true, RequiresInspection = true, TargetDepartment = "Acueductos", EstimatedDays = 10, Active = true },
                new() { Id = "14", Code = "FIJO-MED", Name = "Cambio de tipo de fijo a medido", Description = "Instalación de medidor en una cuenta que actualmente se factura mediante tarifa fija.", Category = "Agua potable y medidores", RequiresServiceAccount = true, RequiresInspection = true, TargetDepartment = "Acueductos", EstimatedDays = 10, Active = true },
                new() { Id = "15", Code = "DESCON", Name = "Desconexión del servicio", Description = "Eliminación total de la conexión del servicio de agua de la red municipal.", Category = "Agua potable y medidores", RequiresServiceAccount = true, RequiresAccountUpToDate = true, RequiresInspection = true, Requirements = new List<string> { "Cuenta al día", "Documento de identidad del titular" }, TargetDepartment = "Acueductos", EstimatedDays = 5, Active = true },
                new() { Id = "16", Code = "ELIM-PEND", Name = "Eliminación del pendiente", Description = "Solicitud para eliminar un cobro de agua aplicado a construcciones nuevas o independizaciones donde el servicio no ha sido instalado.", Category = "Agua potable y medidores", RequiresServiceAccount = true, TargetDepartment = "Acueductos", EstimatedDays = 5, Active = true },
                new() { Id = "17", Code = "FUGA-MED", Name = "Fuga en medidor", Description = "Inspección para determinar una salida o escape accidental en el conducto por donde circula el agua.", Category = "Agua potable y medidores", RequiresServiceAccount = true, RequiresMeter = true, RequiresInspection = true, TargetDepartment = "Acueductos", EstimatedDays = 5, Active = true },
                new() { Id = "18", Code = "INDEP-PAJA", Name = "Independización de paja de agua", Description = "Solicitud de una conexión adicional para separar el servicio.", Category = "Agua potable y medidores", RequiresServiceAccount = true, RequiresProperty = true, RequiresInspection = true, TargetDepartment = "Acueductos", EstimatedDays = 15, Active = true },
                new() { Id = "19", Code = "LOC-MED", Name = "Localizar medidor", Description = "Solicitud para determinar la ubicación física del medidor.", Category = "Agua potable y medidores", RequiresServiceAccount = true, RequiresMeter = true, RequiresInspection = true, TargetDepartment = "Acueductos", EstimatedDays = 3, Active = true },
                new() { Id = "20", Code = "MED-INV", Name = "Medidor invertido", Description = "Inspección para determinar una posible confusión entre cuentas y medidores en propiedades que poseen más de un servicio de agua autorizado.", Category = "Agua potable y medidores", RequiresServiceAccount = true, RequiresMeter = true, RequiresInspection = true, TargetDepartment = "Acueductos", EstimatedDays = 5, Active = true },
                new() { Id = "21", Code = "PAJA-CONST", Name = "Paja de agua adicional con permiso de construcción", Description = "Solicitud de conexión adicional asociada a un permiso de construcción vigente.", Category = "Agua potable y medidores", RequiresBuildingPermit = true, RequiresProperty = true, RequiresInspection = true, TargetDepartment = "Acueductos", EstimatedDays = 10, Active = true },
                new() { Id = "22", Code = "MED-NIVEL", Name = "Poner medidor a nivel", Description = "Solicitud para colocar la caja del medidor al nivel de la acera.", Category = "Agua potable y medidores", RequiresServiceAccount = true, RequiresMeter = true, RequiresInspection = true, TargetDepartment = "Acueductos", EstimatedDays = 10, Active = true },
                new() { Id = "23", Code = "TOMA-LECT", Name = "Toma de lectura", Description = "Inspección para realizar una nueva lectura y validar si la lectura registrada por la municipalidad es correcta.", Category = "Agua potable y medidores", RequiresServiceAccount = true, RequiresMeter = true, RequiresInspection = true, TargetDepartment = "Acueductos", EstimatedDays = 3, Active = true },
                new() { Id = "24", Code = "TRASL-SERV", Name = "Traslado o reubicación del servicio", Description = "Reubicación de la caja y el medidor de un lugar a otro.", Category = "Agua potable y medidores", RequiresServiceAccount = true, RequiresMeter = true, RequiresInspection = true, TargetDepartment = "Acueductos", EstimatedDays = 15, Active = true },
                new() { Id = "25", Code = "VER-FUGA", Name = "Verificación de fuga interna reparada", Description = "Reinspección para confirmar que el suscriptor reparó una fuga interna previamente detectada.", Category = "Agua potable y medidores", RequiresServiceAccount = true, RequiresMeter = true, RequiresInspection = true, TargetDepartment = "Acueductos", EstimatedDays = 5, Active = true },
                // Propiedad y titularidad
                new() { Id = "26", Code = "CAM-TIT", Name = "Cambio de nombre del titular de la cuenta", Description = "Actualización del titular cuando la propiedad ha sido traspasada en el Registro Nacional.", Category = "Propiedad y titularidad", RequiresServiceAccount = true, Requirements = new List<string> { "Documento de identidad", "Cuando existan varios derechos, autorización expresa", "Nombre, identificación y firma de propietarios", "Copias de documentos de identificación vigentes" }, TargetDepartment = "Tesorería", EstimatedDays = 3, Active = true },
                new() { Id = "27", Code = "DEMOL", Name = "Demolición", Description = "Inspección para determinar la eliminación física parcial o total de un inmueble.", Category = "Propiedad y titularidad", RequiresProperty = true, RequiresInspection = true, TargetDepartment = "Catastro", EstimatedDays = 5, Active = true },
                new() { Id = "28", Code = "INM-DESH", Name = "Inmueble sin uso, deshabitado o inhabitable", Description = "Inspección para determinar que el inmueble no está siendo utilizado.", Category = "Propiedad y titularidad", RequiresProperty = true, RequiresInspection = true, TargetDepartment = "Catastro", EstimatedDays = 5, Active = true },
                new() { Id = "29", Code = "LOTE-BALD", Name = "Lote baldío", Description = "Inspección para determinar que la propiedad no cuenta con edificación o construcción.", Category = "Propiedad y titularidad", RequiresProperty = true, RequiresInspection = true, TargetDepartment = "Catastro", EstimatedDays = 5, Active = true },
                new() { Id = "30", Code = "SEG-PROP", Name = "Segregación de propiedad", Description = "Verificación de conexiones en una finca segregada para identificar la relación entre la finca original y la nueva finca.", Category = "Propiedad y titularidad", RequiresProperty = true, RequiresInspection = true, TargetDepartment = "Catastro", EstimatedDays = 7, Active = true }
            };

            var applicantPool = new[]
            {
                new PsApplicantDto { Identification = "1-2345-6789", Name = "Comercial El Parque S.A.", PersonType = "Jurídica", Phone = "2222-1111", Email = "contacto@elparque.cr", FiscalAddress = "San José, Barrio Escalante", PreferredNotification = "Correo", RucStatus = "Validado", RucQuality = "Completo", ExistsInRuc = true },
                new PsApplicantDto { Identification = "2-1111-2222", Name = "María Pérez López", PersonType = "Física", Phone = "8888-4444", Email = "maria.perez@mail.cr", FiscalAddress = "Heredia centro", PreferredNotification = "Plataforma digital", RucStatus = "Pendiente validar RUC", RucQuality = "Parcial", ExistsInRuc = false },
                new PsApplicantDto { Identification = "3-3333-4444", Name = "Constructora del Norte S.R.L.", PersonType = "Jurídica", Phone = "2277-3344", Email = "info@cnorte.cr", FiscalAddress = "Alajuela, Grecia", PreferredNotification = "SMS", RucStatus = "Validado", RucQuality = "Completo", ExistsInRuc = true },
                new PsApplicantDto { Identification = "4-5555-6666", Name = "Inversiones del Valle S.A.", PersonType = "Jurídica", Phone = "2299-1010", Email = "contacto@inversionesvalle.cr", FiscalAddress = "Cartago, Centro", PreferredNotification = "Correo", RucStatus = "Validado", RucQuality = "Completo", ExistsInRuc = true },
                new PsApplicantDto { Identification = "5-7777-8888", Name = "Juan Carlos Ramírez", PersonType = "Física", Phone = "8877-6655", Email = "juan.ramirez@mail.cr", FiscalAddress = "Puntarenas centro", PreferredNotification = "SMS", RucStatus = "Pendiente validar RUC", RucQuality = "Parcial", ExistsInRuc = false },
                new PsApplicantDto { Identification = "6-9999-0000", Name = "Mercados del Pacífico S.A.", PersonType = "Jurídica", Phone = "2211-3344", Email = "contacto@mercados.cr", FiscalAddress = "Puntarenas, Barranca", PreferredNotification = "Plataforma digital", RucStatus = "Validado", RucQuality = "Completo", ExistsInRuc = true },
                new PsApplicantDto { Identification = "7-1212-3434", Name = "Servicios Urbanos del Este", PersonType = "Jurídica", Phone = "2288-9988", Email = "servicios@este.cr", FiscalAddress = "Limón centro", PreferredNotification = "Correo", RucStatus = "Validado", RucQuality = "Completo", ExistsInRuc = true },
                new PsApplicantDto { Identification = "8-4545-6767", Name = "Rosa Vega Mena", PersonType = "Física", Phone = "8899-2233", Email = "rosa.vega@mail.cr", FiscalAddress = "Guanacaste, Liberia", PreferredNotification = "Correo", RucStatus = "Pendiente validar RUC", RucQuality = "Parcial", ExistsInRuc = false }
            };

            var objects = new[]
            {
                new PsProcedureObjectDto { Kind = "Finca", Summary = "Regularización predial mock", IdPredial = "IDP-001", NumeroFinca = "5-123456", NumeroPlano = "PL-1020", Distrito = "Central", Propietario = "Comercial El Parque S.A." },
                new PsProcedureObjectDto { Kind = "Patente", Summary = "Apertura de negocio mock", NumeroLicencia = "PAT-2024-018", NombreComercial = "Café Central", ActividadEconomica = "Restaurante", UsoSuelo = "Conforme" },
                new PsProcedureObjectDto { Kind = "Permiso de construcción", Summary = "Obra menor mock", NumeroFinca = "4-654321", ProfesionalResponsable = "Ing. Andrea Mora", TipoObra = "Ampliación", Area = "120 m2", MontoEstimado = "₡45,000,000" },
                new PsProcedureObjectDto { Kind = "Mercado Municipal", Summary = "Local comercial mock", NumeroLocal = "L-12", Arrendatario = "María Pérez López", Contrato = "CM-2025-011", EstadoCuentaMock = "Al día" },
                new PsProcedureObjectDto { Kind = "RUC", Summary = "Actualización de datos registrales", TipoActualizacionRuc = "Cambio de dirección", DatosAModificar = "Dirección fiscal y teléfono", OrigenSolicitud = "Portal municipal" }
            };

            var channels = new[] { "Presencial", "MIMUNIENCASA", "Portal municipal", "VUI", "Correo", "Teléfono", "Gestión interna" };
            var statuses = new[] { "Recibido", "En clasificación", "En revisión", "Trasladado", "En análisis", "Prevenido", "Resuelto", "Archivado" };
            var departments = new[] { "Atención inicial", "Catastro", "Patentes", "Ingeniería", "Mercado", "Secretaría", "Tesorería" };
            var priorities = new[] { "Normal", "Alta", "Urgente" };
            var timeStatuses = new[] { "En tiempo", "Próximo a vencer", "Vencido" };
            var resolutions = new[] { "Aprobación", "Prevención", "Traslado", "Archivo", "Rechazo" };
            var notificationChannels = new[] { "Correo", "SMS", "Domicilio fiscal", "Plataforma digital" };

            procedures = Enumerable.Range(1, 15).Select(index =>
            {
                var type = procedureTypes[(index - 1) % procedureTypes.Count];
                var applicant = applicantPool[(index - 1) % applicantPool.Length];
                var objectDto = objects[(index - 1) % objects.Length];
                var status = statuses[(index - 1) % statuses.Length];
                var priority = priorities[(index - 1) % priorities.Length];
                var channel = channels[(index - 1) % channels.Length];
                var department = departments[(index - 1) % departments.Length];
                var timeStatus = timeStatuses[(index - 1) % timeStatuses.Length];

                var procedure = new PsProcedureDto
                {
                    Id = Guid.NewGuid().ToString("N"),
                    CaseNumber = $"PS-{DateTime.Now:yyyy}-{index:0000}",
                    TypeCode = type.Code,
                    TypeName = type.Name,
                    ResolverModule = type.ModuleName,
                    Department = department,
                    Responsible = $"Funcionario {index:00}",
                    Team = $"{department} 1",
                    Channel = channel,
                    Status = status,
                    Priority = priority,
                    EntryDate = DateTime.Now.AddDays(-index),
                    LimitDate = DateTime.Now.AddDays(type.DeadlineDays - index),
                    Applicant = applicant,
                    Object = objectDto,
                    Summary = $"Expediente demo #{index} para {type.Name}",
                    Requirements = BuildRequirements(index),
                    Classification = new PsClassificationDto
                    {
                        ModuleName = type.ModuleName,
                        DepartmentName = department,
                        Responsible = $"Funcionario {index:00}",
                        Team = $"{department} 1",
                        Status = status,
                        EstimatedDeadline = $"{type.DeadlineDays} días",
                        MockDueDate = DateTime.Now.AddDays(type.DeadlineDays - index),
                        Priority = priority,
                        TimeStatus = timeStatus,
                        TransferMessage = "Traslado simulado al módulo resolutor."
                    },
                    QualityControl = new PsQualityControlDto
                    {
                        Status = index % 4 == 0 ? "Devuelto" : index % 5 == 0 ? "Aprobado" : "Pendiente",
                        Responsible = "Control de calidad demo",
                        Observation = index % 4 == 0 ? "Subsanación requerida." : "Revisión visual simulada.",
                        ReturnReason = index % 4 == 0 ? "Documento incompleto" : string.Empty,
                        Date = DateTime.Now.AddDays(-index / 2),
                        Action = "Control simulado"
                    },
                    Resolution = new PsResolutionDto
                    {
                        Type = resolutions[(index - 1) % resolutions.Length],
                        Date = DateTime.Now.AddDays(-Math.Max(1, index / 2)),
                        Responsible = "Secretaría demo",
                        Status = index % 3 == 0 ? "Notificada" : index % 2 == 0 ? "Generada" : "Pendiente firma",
                        DigitalSignatureReference = $"FIRMA-{index:000}",
                        PdfReference = $"PDF-{index:000}",
                        Observation = "Resolución mock para demostración."
                    },
                    Deadline = new PsDeadlineDto
                    {
                        StartDate = DateTime.Now.AddDays(-index),
                        EndDate = DateTime.Now.AddDays(type.DeadlineDays - index),
                        DaysElapsed = index,
                        DaysRemaining = type.DeadlineDays - index,
                        Status = timeStatus,
                        SuspensionReason = timeStatus == "Vencido" ? "Esperando criterio externo." : string.Empty,
                        Alert = timeStatus == "Próximo a vencer" ? "Atención prioritaria requerida." : string.Empty
                    },
                    Integrations = BuildIntegrations(),
                    Notifications = new List<PsNotificationDto>
                    {
                        new() { Type = "Prevención", Channel = notificationChannels[index % notificationChannels.Length], Status = "Enviada", Date = DateTime.Now.AddDays(-1), Result = "Recibida", Observation = "Mensaje registrado." },
                        new() { Type = "Resolución", Channel = notificationChannels[(index + 1) % notificationChannels.Length], Status = "Pendiente", Date = DateTime.Now, Result = "Pendiente", Observation = "Pendiente de envío." }
                    },
                    History = BuildHistory(index, department, channel, status),
                    AuditEvents = BuildAudit(index)
                };

                if (type.RequiresFinca)
                {
                    procedure.Object.Kind = "Finca";
                    procedure.Object.IdPredial = $"IDP-{index:000}";
                    procedure.Object.NumeroFinca = $"{index}-100{index}";
                    procedure.Object.NumeroPlano = $"PL-{index:000}";
                    procedure.Object.Distrito = $"Distrito {((index - 1) % 4) + 1}";
                }

                if (type.RequiresPatent)
                {
                    procedure.Object.Kind = "Patente";
                    procedure.Object.NumeroLicencia = $"PAT-2024-{index:000}";
                    procedure.Object.NombreComercial = $"Comercio Demo {index:00}";
                    procedure.Object.ActividadEconomica = "Actividad comercial";
                    procedure.Object.UsoSuelo = index % 3 == 0 ? "No conforme" : "Conforme";
                }

                return procedure;
            }).ToList();

            reports = new List<PsReportDto>
            {
                new() { Name = "Trámites por estado", Description = "Distribución de expedientes según el estado operativo.", Filters = "Estado, canal, prioridad" },
                new() { Name = "Trámites por departamento", Description = "Volumen de trabajo por unidad resolutora.", Filters = "Departamento, responsable" },
                new() { Name = "Trámites por canal", Description = "Recepción presencial, portal o ventanillas digitales.", Filters = "Canal, fecha" },
                new() { Name = "Expedientes vencidos", Description = "Casos en riesgo o vencidos con seguimiento mock.", Filters = "Fecha límite, prioridad" },
                new() { Name = "Resoluciones emitidas", Description = "Resultado de resoluciones simuladas.", Filters = "Estado resolución, módulo" },
                new() { Name = "Notificaciones", Description = "Historial de notificaciones simuladas.", Filters = "Medio, estado" },
                new() { Name = "Productividad", Description = "Atención referencial por funcionario.", Filters = "Funcionario, período" },
                new() { Name = "Integraciones", Description = "Estado referencial de módulos del Core Municipal.", Filters = "Integración, estado" }
            };

            integrations = new List<PsCatalogItemDto>
            {
                new() { Code = "RUC", Name = "RUC", Description = "Consulta de contribuyentes referencial.", Status = "Preparado" },
                new() { Code = "BI", Name = "Bienes Inmuebles", Description = "Vínculo visual con catastro y finca.", Status = "Simulado" },
                new() { Code = "PAT", Name = "Patentes", Description = "Vínculo visual con licencias.", Status = "Simulado" },
                new() { Code = "PERM", Name = "Permisos de Construcción", Description = "Criterio técnico mock.", Status = "Preparado" },
                new() { Code = "MERC", Name = "Mercado Municipal", Description = "Local y arrendamiento referencial.", Status = "Preparado" },
                new() { Code = "USO", Name = "Uso de Suelo", Description = "Validación referencial.", Status = "Simulado" },
                new() { Code = "COBRO", Name = "Cobro", Description = "Estado de cuenta y resolución mock.", Status = "Referencial" },
                new() { Code = "CTA", Name = "Cuenta Tributaria", Description = "Estado de cuenta mock.", Status = "Preparado" },
                new() { Code = "NOT", Name = "Notificaciones", Description = "Alertas simuladas.", Status = "Preparado" },
                new() { Code = "AUD", Name = "Auditoría", Description = "Trazabilidad mock del expediente.", Status = "Preparado" }
            };

            seeded = true;
        }
    }

    private static List<PsRequirementDto> BuildRequirements(int index)
    {
        return new List<PsRequirementDto>
        {
            new() { Name = "Formulario de ingreso", Status = "Presentado", DocumentPlaceholder = $"DOC-{index:000}-A", Receiver = "Recepción", Origin = "Plataforma de Servicios", ReceivedAt = DateTime.Now.AddDays(-1) },
            new() { Name = "Identificación del solicitante", Status = index % 4 == 0 ? "Observado" : "Presentado", DocumentPlaceholder = $"DOC-{index:000}-B", Receiver = "Recepción", Origin = "Presencial", Observation = index % 4 == 0 ? "Documento ilegible" : string.Empty, ReceivedAt = DateTime.Now.AddDays(-1) },
            new() { Name = "Constancia complementaria", Status = index % 3 == 0 ? "Pendiente" : "Presentado", DocumentPlaceholder = $"DOC-{index:000}-C", Receiver = "Recepción", Origin = "MIMUNIENCASA", Observation = index % 3 == 0 ? "Pendiente de adjunto" : string.Empty, ReceivedAt = DateTime.Now.AddDays(-2) }
        };
    }

    private static List<PsIntegrationStatusDto> BuildIntegrations()
    {
        return new List<PsIntegrationStatusDto>
        {
            new() { Name = "RUC", Status = "Preparado", LastEvent = "Consulta mock disponible", Description = "Vínculo con contribuyente", ActionText = "Ver" },
            new() { Name = "Bienes Inmuebles", Status = "Simulado", LastEvent = "Finca referencial", Description = "Relación predial visual", ActionText = "Abrir" },
            new() { Name = "Patentes", Status = "Simulado", LastEvent = "Licencia referencial", Description = "Licencia comercial mock", ActionText = "Abrir" },
            new() { Name = "Permisos de Construcción", Status = "Preparado", LastEvent = "Criterio técnico mock", Description = "Referencia de obra", ActionText = "Ver" },
            new() { Name = "Mercado Municipal", Status = "Preparado", LastEvent = "Local referencial", Description = "Arrendamiento mock", ActionText = "Ver" }
        };
    }

    private static List<PsHistoryEventDto> BuildHistory(int index, string department, string channel, string status)
    {
        return Enumerable.Range(1, 5).Select(step => new PsHistoryEventDto
        {
            Date = DateTime.Now.AddDays(-index - step),
            User = step == 1 ? "recepcion.demo" : step == 2 ? "clasificacion.demo" : "tramite.demo",
            Action = step == 1 ? "Recepción" : step == 2 ? "Clasificación" : step == 3 ? "Traslado" : "Seguimiento",
            PreviousStatus = step == 1 ? "Borrador" : "Recibido",
            NewStatus = step == 1 ? "Recibido" : status,
            Department = department,
            Comment = $"{channel} / evento {step}",
            Origin = step == 1 ? "Plataforma de Servicios" : "Sistema"
        }).ToList();
    }

    private static List<PsAuditEventDto> BuildAudit(int index)
    {
        return Enumerable.Range(1, 4).Select(step => new PsAuditEventDto
        {
            CreatedAt = DateTime.Now.AddDays(-step),
            User = "demo.user",
            Action = step == 1 ? "Crear" : step == 2 ? "Guardar borrador" : step == 3 ? "Derivar" : "Notificar",
            Description = $"Evento mock #{index:000}-{step}",
            Severity = step == 1 ? "Success" : "Info"
        }).ToList();
    }
}