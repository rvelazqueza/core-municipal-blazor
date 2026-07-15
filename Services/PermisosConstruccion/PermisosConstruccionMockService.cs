using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using BlazorApp.Models.PermisosConstruccion;

namespace BlazorApp.Services.PermisosConstruccion;

public class PermisosConstruccionMockService
{
    private static readonly object SyncRoot = new();
    private static readonly List<PcPermitTypeDto> PermitTypes = BuildPermitTypes();
    private static readonly List<PcPermitDto> Permits = BuildPermits();
    private static readonly List<PcCatalogItemDto> Catalogs = BuildCatalogs();
    private static readonly List<PcIntegrationStatusDto> Integrations = BuildIntegrations();

    public Task<List<PcPermitTypeDto>> GetPermitTypesAsync() => Task.FromResult(PermitTypes.Select(item => CloneObject(item)).ToList());
    public Task<List<PcPermitDto>> GetPermitsAsync() => Task.FromResult(Permits.Select(item => CloneObject(item)).ToList());
    public Task<PcPermitDto?> GetPermitAsync(string id) => Task.FromResult(Permits.FirstOrDefault(x => x.Id == id) is PcPermitDto permit ? CloneObject(permit) : null);
    public Task<List<PcApplicantDto>> GetApplicantsAsync() => Task.FromResult(Permits.Select(x => CloneObject(x.Applicant)).GroupBy(x => x.Identification).Select(x => x.First()).ToList());
    public Task<List<PcOwnerDto>> GetOwnersAsync() => Task.FromResult(Permits.Select(x => CloneObject(x.Owner)).GroupBy(x => x.Identification).Select(x => x.First()).ToList());
    public Task<List<PcPropertyDto>> GetPropertiesAsync() => Task.FromResult(Permits.Select(x => CloneObject(x.Property)).GroupBy(x => x.IdPredial).Select(x => x.First()).ToList());
    public Task<List<PcProfessionalDto>> GetProfessionalsAsync() => Task.FromResult(Permits.Select(x => CloneObject(x.Professional)).GroupBy(x => x.Identification).Select(x => x.First()).ToList());
    public Task<List<PcRoadAlignmentDto>> GetRoadAlignmentsAsync() => Task.FromResult(Permits.Select(x => CloneObject(x.RoadAlignment)).ToList());
    public Task<List<PcLandUseDto>> GetLandUsesAsync() => Task.FromResult(Permits.Select(x => CloneObject(x.LandUse)).ToList());
    public Task<List<PcPublicServiceDto>> GetPublicServicesAsync() => Task.FromResult(Permits.SelectMany(x => x.PublicServices.Select(item => CloneObject(item))).ToList());
    public Task<List<PcMunicipalReviewDto>> GetMunicipalReviewsAsync() => Task.FromResult(Permits.Select(x => CloneObject(x.MunicipalReview)).ToList());
    public Task<List<PcQualityControlDto>> GetQualityControlsAsync() => Task.FromResult(Permits.Select(x => CloneObject(x.QualityControl)).ToList());
    public Task<List<PcAssessmentDto>> GetAssessmentsAsync() => Task.FromResult(Permits.Select(x => CloneObject(x.Assessment)).ToList());
    public Task<List<PcPaymentMockDto>> GetPaymentsAsync() => Task.FromResult(Permits.Select(x => CloneObject(x.Payment)).ToList());
    public Task<List<PcResolutionDto>> GetResolutionsAsync() => Task.FromResult(Permits.Select(x => CloneObject(x.Resolution)).ToList());
    public Task<List<PcCatalogItemDto>> GetCatalogItemsAsync() => Task.FromResult(Catalogs.Select(item => CloneObject(item)).ToList());
    public Task<List<PcIntegrationStatusDto>> GetIntegrationsAsync() => Task.FromResult(Integrations.Select(item => CloneObject(item)).ToList());
    public Task<List<PcRequirementDto>> GetRequirementCatalogAsync() => Task.FromResult(GetRequirementTemplate().Select(item => CloneObject(item)).ToList());
    public Task<List<PcDocumentDto>> GetDocumentCatalogAsync() => Task.FromResult(GetDocumentTemplate().Select(item => CloneObject(item)).ToList());
    public Task<List<PcHistoryEventDto>> GetHistoryTemplateAsync() => Task.FromResult(GetHistoryTemplate().Select(item => CloneObject(item)).ToList());
    public Task<List<PcAuditEventDto>> GetAuditTemplateAsync() => Task.FromResult(GetAuditTemplate().Select(item => CloneObject(item)).ToList());

    public Task<PcPermitDto> GetDraftPermitAsync()
    {
        var template = CloneObject(Permits.First());
        template.Id = Guid.NewGuid().ToString("N");
        template.CaseNumber = BuildCaseNumber(Permits.Count + 1);
        template.SystemCode = $"PC-{DateTime.Now:yyyy}-{Permits.Count + 1:000}";
        template.PermitStatus = "Borrador";
        template.ResolutionStatus = "Borrador";
        template.ResolutionResult = "Pendiente información";
        template.ProgressPercent = 15;
        template.CreatedAt = DateTime.Today;
        template.UpdatedAt = DateTime.Today;
        template.DueDate = DateTime.Today.AddDays(10);
        template.UserResponsible = "analista.demo";
        template.AssignedDepartment = template.UnitOwner;
        template.AssignedTeam = "Equipo técnico base";
        template.Observations = "Borrador inicial de permisos de construcción.";
        return Task.FromResult(template);
    }

    public Task<PcPermitDto> SaveDraftAsync(PcPermitDto permit)
    {
        lock (SyncRoot)
        {
            var existing = Permits.FirstOrDefault(x => x.Id == permit.Id);
            if (existing is not null)
                Permits.Remove(existing);
            permit.UpdatedAt = DateTime.Now;
            permit.PermitStatus = "Borrador";
            Permits.Insert(0, CloneObject(permit));
        }

        return Task.FromResult(permit);
    }

    public Task<PcPermitDto> RegisterPermitAsync(PcPermitDto permit)
    {
        lock (SyncRoot)
        {
            var existing = Permits.FirstOrDefault(x => x.Id == permit.Id);
            if (existing is not null)
                Permits.Remove(existing);

            permit.PermitStatus = permit.ResolutionResult == "Aprobado" ? "Aprobado" : permit.PermitStatus;
            permit.ResolutionStatus = "Generada";
            permit.FinishedAt = DateTime.Now;
            permit.UpdatedAt = DateTime.Now;
            permit.HistoryEvents.Insert(0, new PcHistoryEventDto
            {
                Date = DateTime.Now,
                User = "demo.user",
                Action = "Registro final",
                PreviousStatus = "En proceso",
                NewStatus = permit.PermitStatus,
                Department = permit.UnitOwner,
                Comment = "Permiso registrado.",
                Origin = "Plataforma de Construcción"
            });
            permit.AuditEvents.Insert(0, new PcAuditEventDto
            {
                Date = DateTime.Now,
                User = "demo.user",
                Action = "Finalizar permiso",
                Entity = permit.CaseNumber,
                Details = permit.ResolutionResult,
                Origin = "Plataforma de Construcción"
            });
            Permits.Insert(0, CloneObject(permit));
        }

        return Task.FromResult(permit);
    }

    private static List<PcPermitTypeDto> BuildPermitTypes() =>
    [
        new() { Code = "APC", Name = "Licencia de construcción APC", ModuleResolver = "Urbanismo", RequiresRuc = true, RequiresProperty = true, RequiresProfessional = true, RequiresApc = true, RequiresDocuments = true, RequiresInspection = true, InitialStatus = "Recibido" },
        new() { Code = "OM", Name = "Obra menor", ModuleResolver = "Urbanismo", RequiresRuc = true, RequiresProperty = true, RequiresProfessional = false, RequiresApc = true, RequiresDocuments = true, RequiresInspection = false, InitialStatus = "Recibido" },
        new() { Code = "MOD", Name = "Modificación", ModuleResolver = "Urbanismo", RequiresRuc = true, RequiresProperty = true, RequiresProfessional = true, RequiresApc = true, RequiresDocuments = true, RequiresInspection = true, InitialStatus = "En revisión" },
        new() { Code = "PRR", Name = "Prórroga", ModuleResolver = "Urbanismo", RequiresRuc = true, RequiresProperty = true, RequiresProfessional = false, RequiresApc = true, RequiresDocuments = true, RequiresInspection = false, InitialStatus = "Recibido" },
        new() { Code = "ANU", Name = "Anulación", ModuleResolver = "Control de Calidad", RequiresRuc = true, RequiresProperty = false, RequiresProfessional = false, RequiresApc = false, RequiresDocuments = true, RequiresInspection = false, InitialStatus = "Recibido" },
        new() { Code = "REG", Name = "Regularización", ModuleResolver = "Urbanismo", RequiresRuc = true, RequiresProperty = true, RequiresProfessional = true, RequiresApc = true, RequiresDocuments = true, RequiresInspection = true, InitialStatus = "En clasificación" },
        new() { Code = "INS", Name = "Inspección inicial", ModuleResolver = "Inspecciones", RequiresRuc = true, RequiresProperty = true, RequiresProfessional = false, RequiresApc = false, RequiresDocuments = false, RequiresInspection = true, InitialStatus = "Recibido" },
        new() { Code = "FISC", Name = "Fiscalización de obra", ModuleResolver = "Control de Calidad", RequiresRuc = true, RequiresProperty = true, RequiresProfessional = false, RequiresApc = false, RequiresDocuments = true, RequiresInspection = true, InitialStatus = "En revisión" }
    ];

    private static List<PcPermitDto> BuildPermits()
    {
        var applicants = BuildApplicants();
        var owners = BuildOwners();
        var properties = BuildProperties();
        var professionals = BuildProfessionals();
        var permitTypes = BuildPermitTypes();
        var permits = new List<PcPermitDto>();

        for (var i = 1; i <= 12; i++)
        {
            var permitType = permitTypes[(i - 1) % permitTypes.Count];
            var applicant = applicants[(i - 1) % applicants.Count];
            var owner = owners[(i - 1) % owners.Count];
            var property = properties[(i - 1) % properties.Count];
            var professional = professionals[(i - 1) % professionals.Count];
            var workAmount = 125000m + (i * 18500m);
            permits.Add(new PcPermitDto
            {
                Id = Guid.NewGuid().ToString("N"),
                CaseNumber = BuildCaseNumber(i),
                SystemCode = $"PC-{DateTime.Now:yyyy}-{i:000}",
                CfiaApcCode = $"APC-{DateTime.Now:yy}{i:0000}",
                ApcRCode = $"APCR-{DateTime.Now:yy}{i:0000}",
                TypeCode = permitType.Code,
                TypeName = permitType.Name,
                Channel = i % 3 == 0 ? "Plataforma de Servicios" : i % 3 == 1 ? "APC/CFIA" : "Presencial",
                UnitOwner = permitType.ModuleResolver,
                PermitStatus = i % 6 == 0 ? "Aprobado" : i % 5 == 0 ? "En prevención" : i % 4 == 0 ? "Pendiente inspección" : "En revisión técnica",
                ApcStatus = i % 4 == 0 ? "Aprobado" : "Pendiente",
                PaymentStatus = i % 5 == 0 ? "Pagado" : i % 3 == 0 ? "Al cobro" : "Pendiente",
                InspectionStatus = i % 4 == 0 ? "Aprobada" : "Pendiente",
                ResolutionStatus = i % 6 == 0 ? "Generada" : "Borrador",
                ResolutionResult = i % 6 == 0 ? "Aprobado" : "Pendiente información",
                Priority = i % 4 == 0 ? "Alta" : "Normal",
                District = property.District,
                ProgressPercent = i % 6 == 0 ? 95 : 35 + i * 4,
                WorkAmount = workAmount,
                UserResponsible = i % 2 == 0 ? "analista.urbanismo" : "tecnico.calidad",
                AssignedDepartment = permitType.ModuleResolver,
                AssignedTeam = i % 2 == 0 ? "Equipo Norte" : "Equipo Centro",
                Observations = $"Expediente mock {i} preparado para seguimiento integral.",
                CreatedAt = DateTime.Today.AddDays(-i * 2),
                UpdatedAt = DateTime.Today.AddDays(-i),
                DueDate = DateTime.Today.AddDays(8 - i),
                Applicant = applicant,
                Owner = owner,
                Property = property,
                Professional = professional,
                TechnicalWork = new PcTechnicalWorkDto
                {
                    WorkType = i % 2 == 0 ? "Obra mayor" : "Obra menor",
                    ProjectType = i % 2 == 0 ? "Edificación residencial" : "Ampliación menor",
                    ProjectName = $"Proyecto demo {i}",
                    Description = $"Descripción demo del permiso {i}.",
                    Destination = "Uso mixto",
                    RelatedEconomicActivity = "Construcción",
                    FundingSource = "Privado",
                    ExistingArea = 180 + i * 10,
                    NewArea = 80 + i * 5,
                    TotalArea = 260 + i * 12,
                    WorkAmount = workAmount,
                    EstimatedTaxAmount = workAmount * 0.015m,
                    InitialProgress = i * 5,
                    ConstructionSystem = "Mampostería confinada",
                    RoadMaterial = "Asfalto",
                    StartDate = DateTime.Today.AddDays(10 + i),
                    EndDate = DateTime.Today.AddMonths(6).AddDays(i),
                    RequiresWater = true,
                    RequiresSewer = i % 2 == 0,
                    GeneratedServicesAccount = $"SC-{i:0000}",
                    Regularization = i % 5 == 0,
                    RegularizationOrigin = "Regularización visual",
                    RegularizationFinalAct = "Acta demo",
                    Observations = i % 2 == 0 ? "Proyecto con control constructivo reforzado." : "Seguimiento técnico estándar."
                },
                Apc = new PcApcStatusDto
                {
                    CfiaApcCode = $"APC-{DateTime.Now:yy}{i:0000}",
                    ApcStatus = i % 4 == 0 ? "Aprobado" : "Pendiente",
                    ApcRStatus = i % 3 == 0 ? "Presentado" : "Pendiente",
                    ReceptionDate = DateTime.Today.AddDays(-(i + 2)),
                    PreventionDate = i % 5 == 0 ? DateTime.Today.AddDays(-i) : null,
                    CorrectionDate = i % 5 == 0 ? DateTime.Today.AddDays(-(i - 1)) : null,
                    ApprovalDate = i % 4 == 0 ? DateTime.Today.AddDays(-1) : null,
                    SealedAt = DateTime.Today.AddDays(-i),
                    LastSyncAt = DateTime.Today.AddHours(-i),
                    MockSyncStatus = i % 4 == 0 ? "Sincronizado" : "Operativo"
                },
                Requirements = GetRequirementTemplate(),
                Documents = GetDocumentTemplate(),
                RoadAlignment = new PcRoadAlignmentDto
                {
                    Required = true,
                    RequestDate = DateTime.Today.AddDays(-i),
                    SignalDate = DateTime.Today.AddDays(i),
                    ManagementNumber = $"ALI-{DateTime.Now:yy}{i:0000}",
                    Responsible = "Ing. Urbanismo Demo",
                    Status = i % 3 == 0 ? "Aprobado" : "Pendiente",
                    Observations = i % 3 == 0 ? "Señalamiento conforme." : "Pendiente visita de campo.",
                    ImagePlaceholder = "Alineamiento visual"
                },
                LandUse = new PcLandUseDto
                {
                    Required = true,
                    CertificateNumber = $"USO-{DateTime.Now:yy}{i:0000}",
                    Status = i % 4 == 0 ? "Aprobado" : "Pendiente",
                    AllowedActivity = permitType.Name,
                    Compatibility = i % 4 == 0 ? "Compatible" : "En estudio",
                    EmittedAt = i % 4 == 0 ? DateTime.Today.AddDays(-2) : null,
                    ValidUntil = DateTime.Today.AddMonths(6),
                    Observations = i % 4 == 0 ? "Uso autorizado." : "Pendiente validación urbanística."
                },
                PublicServices = BuildPublicServices(i),
                Inspections = BuildInspections(i),
                WorkProgressEntries = BuildWorkProgress(i),
                BiUpdates = BuildBiUpdates(i),
                GisTasks = BuildGisTasks(i),
                Correspondences = BuildCorrespondences(i),
                Reports = BuildReports(i),
                MunicipalReview = new PcMunicipalReviewDto
                {
                    Technician = "Técnico demo",
                    UrbanismAnalyst = "Analista demo",
                    Status = i % 5 == 0 ? "Aprobado" : "Pendiente",
                    TechnicalCriterion = "Procede con observaciones menores.",
                    Observations = "Observación mock para demo.",
                    ReviewedAt = DateTime.Today.AddDays(-1)
                },
                QualityControl = new PcQualityControlDto
                {
                    Status = i % 4 == 0 ? "Aprobado" : "Pendiente",
                    Responsible = "Control de calidad demo",
                    Inconsistencies = i % 2 == 0 ? "Sin inconsistencias relevantes" : "Observaciones menores",
                    RucChecked = true,
                    PropertyChecked = true,
                    TributaryAccountChecked = i % 3 != 0,
                    DocumentsChecked = true,
                    ReturnReason = i % 5 == 0 ? "Revisar soporte documental complementario." : string.Empty,
                    RequiresInspection = true,
                    Inspector = "Inspector municipal",
                    InspectionStatus = i % 3 == 0 ? "Agendada" : "Pendiente",
                    InspectionResult = i % 4 == 0 ? "Aprobada" : "Pendiente"
                },
                Assessment = new PcAssessmentDto
                {
                    CaseNumber = BuildCaseNumber(i),
                    ConstructionTaxPercent = 1.5m,
                    TotalWorkValue = workAmount,
                    ConstructionTax = workAmount * 0.015m,
                    Fine = i % 5 == 0 ? 2500 : 0,
                    Interest = i % 3 == 0 ? 1250 : 0,
                    UrbanServices = 750,
                    TotalToPay = (workAmount * 0.015m) + (i % 5 == 0 ? 2500 : 0) + (i % 3 == 0 ? 1250 : 0) + 750,
                    Status = i % 5 == 0 ? "Al cobro" : "Pendiente"
                },
                Payment = new PcPaymentMockDto
                {
                    Status = i % 5 == 0 ? "Verificado" : "Pendiente",
                    PaymentMethod = i % 2 == 0 ? "Banco" : "Caja",
                    InsurancePolicyStatus = i % 4 == 0 ? "Verificada" : i % 3 == 0 ? "Presentada" : "Pendiente",
                    ReceiptMock = $"REC-{i:0000}",
                    Verified = i % 5 == 0,
                    Infructuous = false,
                    ReversalGenerated = false
                },
                Resolution = new PcResolutionDto
                {
                    Result = i % 6 == 0 ? "Aprobado" : "Pendiente información",
                    PermitNumber = $"LIC-{DateTime.Now:yy}{i:0000}",
                    ApprovalDate = i % 6 == 0 ? DateTime.Today.AddDays(-1) : null,
                    ApprovedBy = "Jefatura demo",
                    ResolutionTerm = i % 2 == 0 ? "10 días ordinario" : "30 días excepcional",
                    NotificationSimulated = i % 6 == 0 ? "Enviada" : "Pendiente",
                    DigitalSignatureRef = $"FIRMA-{i:0000}",
                    LicenseMock = $"LIC-{i:0000}",
                    ApcSyncStatus = i % 4 == 0 ? "Sincronizado" : "Simulado"
                },
                HistoryEvents = GetHistoryTemplate(),
                AuditEvents = GetAuditTemplate(),
                Integrations = BuildIntegrations()
            });
        }

        return permits;
    }

    private static List<PcApplicantDto> BuildApplicants() =>
    [
        new() { Identification = "1-1111-1111", Name = "Constructora Alpha S.A.", PersonType = "Jurídica", Email = "alpha@demo.cr", Phone = "2222-1111", Address = "San José centro", RucStatus = "Validado", PreferredNotification = "Correo", ExistsInRuc = true },
        new() { Identification = "1-2222-2222", Name = "Inversiones Beta S.A.", PersonType = "Jurídica", Email = "beta@demo.cr", Phone = "2222-2222", Address = "Heredia centro", RucStatus = "Validado", PreferredNotification = "Plataforma digital", ExistsInRuc = true },
        new() { Identification = "1-3333-3333", Name = "Desarrollos Gamma S.A.", PersonType = "Jurídica", Email = "gamma@demo.cr", Phone = "2222-3333", Address = "Alajuela centro", RucStatus = "Pendiente validar", PreferredNotification = "Correo", ExistsInRuc = false },
        new() { Identification = "1-4444-4444", Name = "Arquitectura Delta S.A.", PersonType = "Jurídica", Email = "delta@demo.cr", Phone = "2222-4444", Address = "Cartago centro", RucStatus = "Validado", PreferredNotification = "SMS", ExistsInRuc = true },
        new() { Identification = "1-5555-5555", Name = "Servicios Urbanos Épsilon", PersonType = "Jurídica", Email = "epsilon@demo.cr", Phone = "2222-5555", Address = "Puntarenas centro", RucStatus = "Validado", PreferredNotification = "Correo", ExistsInRuc = true },
        new() { Identification = "1-6666-6666", Name = "Logística Zeta S.A.", PersonType = "Jurídica", Email = "zeta@demo.cr", Phone = "2222-6666", Address = "Limón centro", RucStatus = "Pendiente validar", PreferredNotification = "Plataforma digital", ExistsInRuc = false },
        new() { Identification = "1-7777-7777", Name = "Consultoría Eta S.A.", PersonType = "Jurídica", Email = "eta@demo.cr", Phone = "2222-7777", Address = "Guanacaste centro", RucStatus = "Validado", PreferredNotification = "Correo", ExistsInRuc = true },
        new() { Identification = "1-8888-8888", Name = "Desarrollos Theta S.A.", PersonType = "Jurídica", Email = "theta@demo.cr", Phone = "2222-8888", Address = "San Carlos", RucStatus = "Validado", PreferredNotification = "Correo", ExistsInRuc = true }
    ];

    private static List<PcOwnerDto> BuildOwners() =>
    [
        new() { Identification = "2-1111-1111", Name = "Terrenos del Norte S.A.", Relationship = "Propietario", OwnershipPercentage = 100, AuthorizationMock = "No aplica" },
        new() { Identification = "2-2222-2222", Name = "Fideicomiso Centro", Relationship = "Propietario", OwnershipPercentage = 100, AuthorizationMock = "No aplica" },
        new() { Identification = "2-3333-3333", Name = "Grupo Horizonte S.A.", Relationship = "Copropietario", OwnershipPercentage = 75, AuthorizationMock = "Autorización registrada" },
        new() { Identification = "2-4444-4444", Name = "Corporación Delta S.A.", Relationship = "Propietario", OwnershipPercentage = 100, AuthorizationMock = "No aplica" },
        new() { Identification = "2-5555-5555", Name = "Desarrollos Urbanos S.A.", Relationship = "Propietario", OwnershipPercentage = 100, AuthorizationMock = "No aplica" },
        new() { Identification = "2-6666-6666", Name = "Finca Beta S.A.", Relationship = "Propietario", OwnershipPercentage = 100, AuthorizationMock = "No aplica" },
        new() { Identification = "2-7777-7777", Name = "Inmobiliaria Central", Relationship = "Propietario", OwnershipPercentage = 100, AuthorizationMock = "No aplica" },
        new() { Identification = "2-8888-8888", Name = "Patrimonio Costarricense", Relationship = "Propietario", OwnershipPercentage = 100, AuthorizationMock = "No aplica" }
    ];

    private static List<PcPropertyDto> BuildProperties() =>
    [
        new() { IdPredial = "P-0001", PropertyNumber = "FN-1001", PlanNumber = "CP-1001", District = "Catedral", Sector = "Centro", Block = "A", ExactAddress = "Avenida 1, Calle 3", LandArea = 500, CurrentUse = "Comercial", FiscalValue = 12000000, GisStatus = "Vigente", TributaryAccountStatus = "Al día", BiUpdateStatus = "Pendiente actualización", ServicesAccount = "SC-0001", Gravames = "Sin gravámenes" },
        new() { IdPredial = "P-0002", PropertyNumber = "FN-1002", PlanNumber = "CP-1002", District = "San Francisco", Sector = "Este", Block = "B", ExactAddress = "Frente al parque", LandArea = 420, CurrentUse = "Residencial", FiscalValue = 9800000, ServicesAccount = "SC-0002", Gravames = "Hipoteca referencial" },
        new() { IdPredial = "P-0003", PropertyNumber = "FN-1003", PlanNumber = "CP-1003", District = "Pavas", Sector = "Oeste", Block = "C", ExactAddress = "Diagonal al mercado", LandArea = 650, CurrentUse = "Mixto", FiscalValue = 15000000, ServicesAccount = "SC-0003", Gravames = "Sin gravámenes" },
        new() { IdPredial = "P-0004", PropertyNumber = "FN-1004", PlanNumber = "CP-1004", District = "Escazú", Sector = "Sur", Block = "D", ExactAddress = "Ruta 27", LandArea = 800, CurrentUse = "Industrial", FiscalValue = 22000000, ServicesAccount = "SC-0004", Gravames = "Sin gravámenes" },
        new() { IdPredial = "P-0005", PropertyNumber = "FN-1005", PlanNumber = "CP-1005", District = "Desamparados", Sector = "Norte", Block = "E", ExactAddress = "Barrio La Paz", LandArea = 330, CurrentUse = "Residencial", FiscalValue = 7600000, ServicesAccount = "SC-0005", Gravames = "Embargo preventivo" },
        new() { IdPredial = "P-0006", PropertyNumber = "FN-1006", PlanNumber = "CP-1006", District = "Goicoechea", Sector = "Centro", Block = "F", ExactAddress = "Cerca de municipalidad", LandArea = 610, CurrentUse = "Comercial", FiscalValue = 18400000, ServicesAccount = "SC-0006", Gravames = "Sin gravámenes" },
        new() { IdPredial = "P-0007", PropertyNumber = "FN-1007", PlanNumber = "CP-1007", District = "Alajuela", Sector = "Aeropuerto", Block = "G", ExactAddress = "Ruta principal", LandArea = 900, CurrentUse = "Mixto", FiscalValue = 25000000, ServicesAccount = "SC-0007", Gravames = "Sin gravámenes" },
        new() { IdPredial = "P-0008", PropertyNumber = "FN-1008", PlanNumber = "CP-1008", District = "Cartago", Sector = "Centro", Block = "H", ExactAddress = "Calle central", LandArea = 470, CurrentUse = "Residencial", FiscalValue = 10500000, ServicesAccount = "SC-0008", Gravames = "Sin gravámenes" }
    ];

    private static List<PcProfessionalDto> BuildProfessionals() =>
    [
        new() { Identification = "3-1111-1111", Name = "Ing. Ana Solís", LicenseNumber = "CFIA-1001", Email = "ana@demo.cr", Phone = "8000-1001", Specialty = "Arquitectura", CfiaStatus = "Activo", MockValidation = "Validado" },
        new() { Identification = "3-2222-2222", Name = "Ing. Carlos Ruiz", LicenseNumber = "CFIA-1002", Email = "carlos@demo.cr", Phone = "8000-1002", Specialty = "Ingeniería civil", CfiaStatus = "Activo", MockValidation = "Validado" },
        new() { Identification = "3-3333-3333", Name = "Ing. Laura Campos", LicenseNumber = "CFIA-1003", Email = "laura@demo.cr", Phone = "8000-1003", Specialty = "Estructural", CfiaStatus = "Activo", MockValidation = "Validado" },
        new() { Identification = "3-4444-4444", Name = "Ing. Pablo Vega", LicenseNumber = "CFIA-1004", Email = "pablo@demo.cr", Phone = "8000-1004", Specialty = "Topografía", CfiaStatus = "Activo", MockValidation = "Validado" },
        new() { Identification = "3-5555-5555", Name = "Ing. Sofía Mora", LicenseNumber = "CFIA-1005", Email = "sofia@demo.cr", Phone = "8000-1005", Specialty = "Urbanismo", CfiaStatus = "Activo", MockValidation = "Validado" },
        new() { Identification = "3-6666-6666", Name = "Ing. Diego León", LicenseNumber = "CFIA-1006", Email = "diego@demo.cr", Phone = "8000-1006", Specialty = "Construcción", CfiaStatus = "Activo", MockValidation = "Validado" }
    ];

    private static List<PcRequirementDto> GetRequirementTemplate() =>
    [
        new() { Code = "REQ-01", Name = "Plano catastrado", State = "Presentado", DocumentPlaceholder = "PL-001", Observation = "OK", ReceivedAt = DateTime.Today.AddDays(-3), ReceivedBy = "demo.user", Origin = "Plataforma de Servicios" },
        new() { Code = "REQ-02", Name = "Plano constructivo", State = "Presentado", DocumentPlaceholder = "PL-002", Observation = "OK", ReceivedAt = DateTime.Today.AddDays(-3), ReceivedBy = "demo.user", Origin = "APC/CFIA simulado" },
        new() { Code = "REQ-03", Name = "Formulario APC", State = "Presentado", DocumentPlaceholder = "APC-003", Observation = "OK", ReceivedAt = DateTime.Today.AddDays(-3), ReceivedBy = "demo.user", Origin = "APC/CFIA simulado" },
        new() { Code = "REQ-04", Name = "Uso de suelo", State = "Presentado", DocumentPlaceholder = "USO-004", Observation = "OK", ReceivedAt = DateTime.Today.AddDays(-2), ReceivedBy = "demo.user", Origin = "MIMUNIENCASA" },
        new() { Code = "REQ-05", Name = "Alineamiento vial", State = "Pendiente", DocumentPlaceholder = "ALI-005", Observation = "Referencia mock", ReceivedAt = null, ReceivedBy = string.Empty, Origin = "Plataforma de Servicios" },
        new() { Code = "REQ-06", Name = "Disponibilidad agua potable", State = "Presentado", DocumentPlaceholder = "AG-006", Observation = "OK", ReceivedAt = DateTime.Today.AddDays(-1), ReceivedBy = "demo.user", Origin = "Acueductos" },
        new() { Code = "REQ-07", Name = "Disponibilidad alcantarillado", State = "Presentado", DocumentPlaceholder = "ALC-007", Observation = "OK", ReceivedAt = DateTime.Today.AddDays(-1), ReceivedBy = "demo.user", Origin = "Alcantarillado sanitario" },
        new() { Code = "REQ-08", Name = "Póliza INS", State = "Observado", DocumentPlaceholder = "INS-008", Observation = "Vigencia pendiente", ReceivedAt = DateTime.Today.AddDays(-1), ReceivedBy = "demo.user", Origin = "Presencial" },
        new() { Code = "REQ-09", Name = "Comprobante de pago", State = "Pendiente", DocumentPlaceholder = "PAGO-009", Observation = "Pendiente verificación", ReceivedAt = null, ReceivedBy = string.Empty, Origin = "Tesorería" },
        new() { Code = "REQ-10", Name = "Documento de propiedad", State = "Presentado", DocumentPlaceholder = "PROP-010", Observation = "OK", ReceivedAt = DateTime.Today.AddDays(-2), ReceivedBy = "demo.user", Origin = "Plataforma de Servicios" },
        new() { Code = "REQ-11", Name = "Autorización propietario", State = "Presentado", DocumentPlaceholder = "AUT-011", Observation = "OK", ReceivedAt = DateTime.Today.AddDays(-2), ReceivedBy = "demo.user", Origin = "Plataforma de Servicios" },
        new() { Code = "REQ-12", Name = "Viabilidad ambiental", State = "No aplica", DocumentPlaceholder = "VIA-012", Observation = "No aplica al alcance", ReceivedAt = null, ReceivedBy = string.Empty, Origin = "Referencial" },
        new() { Code = "REQ-13", Name = "Fotografías o croquis", State = "Presentado", DocumentPlaceholder = "FOT-013", Observation = "OK", ReceivedAt = DateTime.Today.AddDays(-2), ReceivedBy = "demo.user", Origin = "MIMUNIENCASA" },
        new() { Code = "REQ-14", Name = "Formulario obra menor", State = "No aplica", DocumentPlaceholder = "OM-014", Observation = "Aplica según tipo de trámite", ReceivedAt = null, ReceivedBy = string.Empty, Origin = "Sistema" },
        new() { Code = "REQ-15", Name = "Criterio técnico complementario", State = "Observado", DocumentPlaceholder = "CT-015", Observation = "Requiere revisión", ReceivedAt = DateTime.Today.AddDays(-1), ReceivedBy = "demo.user", Origin = "Control de calidad" }
    ];

    private static List<PcDocumentDto> GetDocumentTemplate() =>
    [
        new() { Code = "DOC-01", Name = "Plano catastrado", Placeholder = "PL-001", State = "Presentado", UploadedAt = DateTime.Today.AddDays(-3), UploadedBy = "demo.user", Origin = "APC/CFIA" },
        new() { Code = "DOC-02", Name = "Plano constructivo", Placeholder = "PL-002", State = "Presentado", UploadedAt = DateTime.Today.AddDays(-3), UploadedBy = "demo.user", Origin = "APC/CFIA" },
        new() { Code = "DOC-03", Name = "Formulario APC", Placeholder = "APC-003", State = "Presentado", UploadedAt = DateTime.Today.AddDays(-3), UploadedBy = "demo.user", Origin = "APC/CFIA" },
        new() { Code = "DOC-04", Name = "Uso de suelo", Placeholder = "USO-004", State = "Presentado", UploadedAt = DateTime.Today.AddDays(-2), UploadedBy = "demo.user", Origin = "MIMUNIENCASA" },
        new() { Code = "DOC-05", Name = "Alineamiento vial", Placeholder = "ALI-005", State = "Pendiente", UploadedAt = null, UploadedBy = string.Empty, Origin = "Urbanismo" },
        new() { Code = "DOC-06", Name = "Póliza INS", Placeholder = "INS-006", State = "Observado", UploadedAt = DateTime.Today.AddDays(-1), UploadedBy = "demo.user", Origin = "Seguros" },
        new() { Code = "DOC-07", Name = "Comprobante de pago", Placeholder = "PAGO-007", State = "Pendiente", UploadedAt = null, UploadedBy = string.Empty, Origin = "Tesorería" },
        new() { Code = "DOC-08", Name = "Autorización propietario", Placeholder = "AUT-008", State = "Presentado", UploadedAt = DateTime.Today.AddDays(-2), UploadedBy = "demo.user", Origin = "Plataforma de Servicios" },
        new() { Code = "DOC-09", Name = "Fotografías", Placeholder = "FOT-009", State = "Presentado", UploadedAt = DateTime.Today.AddDays(-2), UploadedBy = "demo.user", Origin = "Presencial" },
        new() { Code = "DOC-10", Name = "Formulario obra menor", Placeholder = "OM-010", State = "No aplica", UploadedAt = null, UploadedBy = string.Empty, Origin = "Urbanismo" }
    ];

    private static List<PcPublicServiceDto> BuildPublicServices(int index) =>
    [
        new() { ServiceType = "Agua potable", Required = true, Status = index % 2 == 0 ? "Aprobado" : "Pendiente", Administrator = "AyA / Acueducto local", Diameter = "3/4\"", Category = "Residencial" },
        new() { ServiceType = "Alcantarillado sanitario", Required = true, Status = index % 3 == 0 ? "Aprobado" : "Pendiente", Administrator = "Municipalidad / Operador", Diameter = "6\"", Category = "Mixto" }
    ];

    private static List<PcInspectionDto> BuildInspections(int index) =>
    [
        new() { Number = $"INS-{index:000}-01", RelatedCaseNumber = BuildCaseNumber(index), RelatedProperty = $"FN-10{index:00}", RelatedPerson = "Solicitante", InspectionDate = DateTime.Today.AddDays(-4), AssignedDate = DateTime.Today.AddDays(-5), Inspector = "Inspector municipal", InspectionType = "Inicial", Reason = "Verificación de campo", Purpose = "Control de expediente", Status = "Solicitada", Result = "Pendiente", ProgressPercent = 0, EvidencePlaceholder = "EVID-01", DocumentsPlaceholder = "DOC-01", Notes = "Pendiente asignación" },
        new() { Number = $"INS-{index:000}-02", RelatedCaseNumber = BuildCaseNumber(index), RelatedProperty = $"FN-10{index:00}", RelatedPerson = "Propietario", InspectionDate = DateTime.Today.AddDays(-3), AssignedDate = DateTime.Today.AddDays(-4), Inspector = "Inspector municipal", InspectionType = "Seguimiento", Reason = "Avance de obra", Purpose = "Verificación parcial", Status = "Asignada", Result = "Pendiente", ProgressPercent = 25, EvidencePlaceholder = "EVID-02", DocumentsPlaceholder = "DOC-02", Notes = "Asignada en agenda" },
        new() { Number = $"INS-{index:000}-03", RelatedCaseNumber = BuildCaseNumber(index), RelatedProperty = $"FN-10{index:00}", RelatedPerson = "Solicitante", InspectionDate = DateTime.Today.AddDays(-2), AssignedDate = DateTime.Today.AddDays(-2), Inspector = "Inspector municipal", InspectionType = "Técnica", Reason = "Revisión técnica", Purpose = "Inspección en campo", Status = "En campo", Result = "Observada", ProgressPercent = 50, EvidencePlaceholder = "EVID-03", DocumentsPlaceholder = "DOC-03", Notes = "Evidencia registrada" },
        new() { Number = $"INS-{index:000}-04", RelatedCaseNumber = BuildCaseNumber(index), RelatedProperty = $"FN-10{index:00}", RelatedPerson = "Propietario", InspectionDate = DateTime.Today.AddDays(-1), AssignedDate = DateTime.Today.AddDays(-1), Inspector = "Inspector municipal", InspectionType = "Control", Reason = "Control de calidad", Purpose = "Verificación documental", Status = "Registrada", Result = "Conforme", ProgressPercent = 80, EvidencePlaceholder = "EVID-04", DocumentsPlaceholder = "DOC-04", Notes = "Lista para cierre" },
        new() { Number = $"INS-{index:000}-05", RelatedCaseNumber = BuildCaseNumber(index), RelatedProperty = $"FN-10{index:00}", RelatedPerson = "Contribuyente", InspectionDate = DateTime.Today, AssignedDate = DateTime.Today.AddDays(-1), Inspector = "Inspector municipal", InspectionType = "Final", Reason = "Cierre de expediente", Purpose = "Finalización", Status = "Finalizada", Result = "Conforme", ProgressPercent = 100, EvidencePlaceholder = "EVID-05", DocumentsPlaceholder = "DOC-05", Notes = "Finalizada" },
        new() { Number = $"INS-{index:000}-06", RelatedCaseNumber = BuildCaseNumber(index), RelatedProperty = $"FN-10{index:00}", RelatedPerson = "Solicitante", InspectionDate = DateTime.Today.AddDays(-6), AssignedDate = DateTime.Today.AddDays(-7), Inspector = "Inspector municipal", InspectionType = "Prevención", Reason = "Seguimiento a prevención", Purpose = "Corregir observaciones", Status = "Solicitada", Result = "Requiere corrección", ProgressPercent = 15, EvidencePlaceholder = "EVID-06", DocumentsPlaceholder = "DOC-06", Notes = "Prevención pendiente" },
        new() { Number = $"INS-{index:000}-07", RelatedCaseNumber = BuildCaseNumber(index), RelatedProperty = $"FN-10{index:00}", RelatedPerson = "Propietario", InspectionDate = DateTime.Today.AddDays(-8), AssignedDate = DateTime.Today.AddDays(-8), Inspector = "Inspector municipal", InspectionType = "Especial", Reason = "Clausura", Purpose = "Control especial", Status = "Registrada", Result = "Clausura", ProgressPercent = 95, EvidencePlaceholder = "EVID-07", DocumentsPlaceholder = "DOC-07", Notes = "Clausura registrada" },
        new() { Number = $"INS-{index:000}-08", RelatedCaseNumber = BuildCaseNumber(index), RelatedProperty = $"FN-10{index:00}", RelatedPerson = "Solicitante", InspectionDate = DateTime.Today.AddDays(-9), AssignedDate = DateTime.Today.AddDays(-9), Inspector = "Inspector municipal", InspectionType = "Reinspección", Reason = "Verificación de subsanación", Purpose = "Cierre", Status = "Finalizada", Result = "Conforme", ProgressPercent = 100, EvidencePlaceholder = "EVID-08", DocumentsPlaceholder = "DOC-08", Notes = "Subsanación confirmada" }
    ];

    private static List<PcWorkProgressDto> BuildWorkProgress(int index) =>
    [
        new() { ProgressPercent = 0, LastProgressDate = DateTime.Today.AddDays(-10), Inspector = "Inspector municipal", Status = "Sin iniciar", FineReference = 0, InterestReference = 0, BiTaskStatus = "No generada", Notes = "Inicio pendiente" },
        new() { ProgressPercent = 25, LastProgressDate = DateTime.Today.AddDays(-8), Inspector = "Inspector municipal", Status = "25%", FineReference = 0, InterestReference = 250, BiTaskStatus = "No generada", Notes = "Avance inicial" },
        new() { ProgressPercent = 50, LastProgressDate = DateTime.Today.AddDays(-6), Inspector = "Inspector municipal", Status = "50%", FineReference = 500, InterestReference = 500, BiTaskStatus = "No generada", Notes = "Avance intermedio" },
        new() { ProgressPercent = 80, LastProgressDate = DateTime.Today.AddDays(-4), Inspector = "Inspector municipal", Status = "80%", FineReference = 1000, InterestReference = 750, BiTaskStatus = "Generar tarea BI", Notes = "Regla visual activa" },
        new() { ProgressPercent = 95, LastProgressDate = DateTime.Today.AddDays(-2), Inspector = "Inspector municipal", Status = "En ejecución", FineReference = 1500, InterestReference = 900, BiTaskStatus = "Generada", Notes = "Avance alto" },
        new() { ProgressPercent = 100, LastProgressDate = DateTime.Today.AddDays(-1), Inspector = "Inspector municipal", Status = "Finalizada", FineReference = 1800, InterestReference = 1000, BiTaskStatus = "Actualizada", Notes = "Obra finalizada" }
    ];

    private static List<PcBiUpdateDto> BuildBiUpdates(int index) =>
    [
        new() { Status = "No requerida", PropertyReference = $"FN-10{index:00}", UpdateType = "Nueva construcción", Origin = "Permiso aprobado", RequestedAt = DateTime.Today.AddDays(-8), UpdatedAt = null, Notes = "No aplica por el momento" },
        new() { Status = "Pendiente", PropertyReference = $"FN-10{index:00}", UpdateType = "Ampliación", Origin = "Avance 80%", RequestedAt = DateTime.Today.AddDays(-6), UpdatedAt = null, Notes = "Generada por control de avance" },
        new() { Status = "Generada", PropertyReference = $"FN-10{index:00}", UpdateType = "Cambio de área", Origin = "Inspección", RequestedAt = DateTime.Today.AddDays(-4), UpdatedAt = null, Notes = "En cola referencial" },
        new() { Status = "Actualizada mock", PropertyReference = $"FN-10{index:00}", UpdateType = "Nueva construcción", Origin = "Regularización", RequestedAt = DateTime.Today.AddDays(-3), UpdatedAt = DateTime.Today.AddDays(-2), Notes = "BI actualizado en demo" },
        new() { Status = "Generada", PropertyReference = $"FN-10{index:00}", UpdateType = "Demolición", Origin = "Permiso aprobado", RequestedAt = DateTime.Today.AddDays(-1), UpdatedAt = null, Notes = "Pendiente validación" }
    ];

    private static List<PcGisStatusDto> BuildGisTasks(int index) =>
    [
        new() { Status = "Georreferenciado", Layer = "Catastro urbano", TaskNumber = $"GIS-{index:000}-01", LastUpdatedAt = DateTime.Today.AddDays(-7), PropertyReference = $"FN-10{index:00}", PermitReference = BuildCaseNumber(index), Relation = "Finca / permiso", Notes = "Capa alineada" },
        new() { Status = "Pendiente", Layer = "Alineamiento vial", TaskNumber = $"GIS-{index:000}-02", LastUpdatedAt = DateTime.Today.AddDays(-6), PropertyReference = $"FN-10{index:00}", PermitReference = BuildCaseNumber(index), Relation = "Alineamiento / permiso", Notes = "Pendiente validación" },
        new() { Status = "Inconsistente", Layer = "Uso de suelo", TaskNumber = $"GIS-{index:000}-03", LastUpdatedAt = DateTime.Today.AddDays(-5), PropertyReference = $"FN-10{index:00}", PermitReference = BuildCaseNumber(index), Relation = "Uso de suelo / permiso", Notes = "Diferencia referencial" },
        new() { Status = "Georreferenciado", Layer = "Servicios públicos", TaskNumber = $"GIS-{index:000}-04", LastUpdatedAt = DateTime.Today.AddDays(-4), PropertyReference = $"FN-10{index:00}", PermitReference = BuildCaseNumber(index), Relation = "Servicios / permiso", Notes = "Capa asociada" },
        new() { Status = "Pendiente", Layer = "Inspección", TaskNumber = $"GIS-{index:000}-05", LastUpdatedAt = DateTime.Today.AddDays(-3), PropertyReference = $"FN-10{index:00}", PermitReference = BuildCaseNumber(index), Relation = "Inspección / predio", Notes = "Pendiente revisión" },
        new() { Status = "Georreferenciado", Layer = "Expediente municipal", TaskNumber = $"GIS-{index:000}-06", LastUpdatedAt = DateTime.Today.AddDays(-1), PropertyReference = $"FN-10{index:00}", PermitReference = BuildCaseNumber(index), Relation = "Permiso / expediente", Notes = "Trazabilidad mock" }
    ];

    private static List<PcCorrespondenceDto> BuildCorrespondences(int index) =>
    [
        new() { ManagementNumber = $"COR-{index:000}-01", Type = "Interna", Channel = "Plataforma de Servicios", Applicant = "Solicitante", DestinationDepartment = "Urbanismo", Status = "Registrada", ResolutionMock = "Pendiente revisión", DeadlineMock = DateTime.Today.AddDays(4), NotificationMock = "Pendiente", Notes = "Ingreso interno" },
        new() { ManagementNumber = $"COR-{index:000}-02", Type = "Externa", Channel = "MIMUNIENCASA", Applicant = "Propietario", DestinationDepartment = "Inspecciones", Status = "En análisis", ResolutionMock = "En evaluación", DeadlineMock = DateTime.Today.AddDays(3), NotificationMock = "Pendiente", Notes = "Consulta externa" },
        new() { ManagementNumber = $"COR-{index:000}-03", Type = "Interna", Channel = "Presencial", Applicant = "Contribuyente", DestinationDepartment = "Control de calidad", Status = "Reasignada", ResolutionMock = "Reasignada a calidad", DeadlineMock = DateTime.Today.AddDays(2), NotificationMock = "Enviada", Notes = "Derivación interna" },
        new() { ManagementNumber = $"COR-{index:000}-04", Type = "Externa", Channel = "Correo", Applicant = "Solicitante", DestinationDepartment = "Tesorería", Status = "Atendida", ResolutionMock = "Atendida", DeadlineMock = DateTime.Today.AddDays(1), NotificationMock = "Enviada", Notes = "Respuesta por correo" },
        new() { ManagementNumber = $"COR-{index:000}-05", Type = "Interna", Channel = "Plataforma de Servicios", Applicant = "Propietario", DestinationDepartment = "Urbanismo", Status = "Resuelta", ResolutionMock = "Resuelta", DeadlineMock = DateTime.Today.AddDays(5), NotificationMock = "Enviada", Notes = "Cierre administrativo" },
        new() { ManagementNumber = $"COR-{index:000}-06", Type = "Externa", Channel = "MIMUNIENCASA", Applicant = "Tercero demo", DestinationDepartment = "Archivo", Status = "Notificada", ResolutionMock = "Notificada mock", DeadlineMock = DateTime.Today.AddDays(6), NotificationMock = "Enviada", Notes = "Cierre notificado" }
    ];

    private static List<PcReportDto> BuildReports(int index) =>
    [
        new() { Name = "Reporte de trámites por tipo", Description = "Resumen operativo por tipo de trámite.", MainFilters = "Tipo, estado, distrito", Category = "Trámite" },
        new() { Name = "Reporte por consecutivo de permiso", Description = "Consulta por número de permiso.", MainFilters = "Consecutivo, fechas", Category = "Identificador" },
        new() { Name = "Reporte por expediente Plataforma", Description = "Seguimiento por expediente digital.", MainFilters = "Expediente, canal", Category = "Expediente" },
        new() { Name = "Reporte por código CFIA", Description = "Consulta técnica por APC/CFIA.", MainFilters = "Código CFIA, APC-R", Category = "APC" },
        new() { Name = "Reporte por contribuyente", Description = "Búsqueda por nombre y cédula.", MainFilters = "Nombre, identificación", Category = "Contribuyente" },
        new() { Name = "Reporte por finca", Description = "Consulta por finca e ID predial.", MainFilters = "Finca, plano, predio", Category = "Predio" },
        new() { Name = "Reporte por avance de obra", Description = "Seguimiento del porcentaje de avance.", MainFilters = "Porcentaje, inspector", Category = "Fiscalización" },
        new() { Name = "Reporte por tasación", Description = "Detalle de valores y cobros.", MainFilters = "Monto, impuesto, multa", Category = "Cobro" },
        new() { Name = "Reporte por inspección", Description = "Estado y resultado de inspecciones.", MainFilters = "Estado, fecha, inspector", Category = "Inspección" },
        new() { Name = "Reporte por notificaciones", Description = "Resumen de notificaciones mock.", MainFilters = "Tipo, medio, estado", Category = "Comunicación" },
        new() { Name = "Reporte por clausuras", Description = "Trazabilidad referencial de clausuras.", MainFilters = "Estado, fecha, usuario", Category = "Control" },
        new() { Name = "Reporte por exoneraciones", Description = "Proyectos exonerados parcial o totalmente.", MainFilters = "Porcentaje, distrito", Category = "Beneficio" }
    ];

    private static List<PcHistoryEventDto> GetHistoryTemplate() =>
    [
        new() { Date = DateTime.Now.AddDays(-12), User = "recepcion.demo", Action = "Creación de expediente", PreviousStatus = string.Empty, NewStatus = "Recibido", Department = "Plataforma de Servicios", Comment = "Ingreso inicial", Origin = "Plataforma de Servicios" },
        new() { Date = DateTime.Now.AddDays(-11), User = "clasificacion.demo", Action = "Clasificación", PreviousStatus = "Recibido", NewStatus = "En clasificación", Department = "Urbanismo", Comment = "Asignación preliminar", Origin = "Sistema" },
        new() { Date = DateTime.Now.AddDays(-10), User = "ruc.demo", Action = "Vinculación RUC", PreviousStatus = "En clasificación", NewStatus = "En revisión", Department = "RUC", Comment = "Validación mock", Origin = "Sistema" },
        new() { Date = DateTime.Now.AddDays(-9), User = "urbanismo.demo", Action = "Revisión técnica", PreviousStatus = "En revisión", NewStatus = "En análisis", Department = "Urbanismo", Comment = "Observaciones menores", Origin = "Módulo resolutor" },
        new() { Date = DateTime.Now.AddDays(-8), User = "inspeccion.demo", Action = "Inspección inicial", PreviousStatus = "En análisis", NewStatus = "Pendiente inspección", Department = "Inspecciones", Comment = "Agendada", Origin = "Control de calidad" },
        new() { Date = DateTime.Now.AddDays(-7), User = "tesoreria.demo", Action = "Tasación", PreviousStatus = "Pendiente inspección", NewStatus = "Al cobro", Department = "Tesorería", Comment = "Cálculo mock", Origin = "Sistema" },
        new() { Date = DateTime.Now.AddDays(-6), User = "caja.demo", Action = "Cobro", PreviousStatus = "Al cobro", NewStatus = "Pendiente pago", Department = "Cajas", Comment = "Pendiente verificación", Origin = "Sistema" },
        new() { Date = DateTime.Now.AddDays(-5), User = "revisor.demo", Action = "Control de calidad", PreviousStatus = "Pendiente pago", NewStatus = "Revisión final", Department = "Calidad", Comment = "OK", Origin = "Control de calidad" },
        new() { Date = DateTime.Now.AddDays(-4), User = "aprobador.demo", Action = "Aprobación mock", PreviousStatus = "Revisión final", NewStatus = "Aprobado", Department = "Dirección", Comment = "Aprobado en demo", Origin = "Sistema" },
        new() { Date = DateTime.Now.AddDays(-3), User = "notifica.demo", Action = "Notificación", PreviousStatus = "Aprobado", NewStatus = "Notificado", Department = "Plataforma", Comment = "Enviada", Origin = "Notificación" },
        new() { Date = DateTime.Now.AddDays(-2), User = "archivo.demo", Action = "Archivo", PreviousStatus = "Notificado", NewStatus = "Archivado", Department = "Archivo", Comment = "Cierre mock", Origin = "Sistema" },
        new() { Date = DateTime.Now.AddDays(-1), User = "consulta.demo", Action = "Consulta expediente", PreviousStatus = "Archivado", NewStatus = "Archivado", Department = "Plataforma", Comment = "Consulta demo", Origin = "Plataforma de Servicios" },
        new() { Date = DateTime.Now, User = "topografia.demo", Action = "Validación GIS", PreviousStatus = "Archivado", NewStatus = "Archivado", Department = "Topografía", Comment = "Revisión referencial", Origin = "GIS" },
        new() { Date = DateTime.Now, User = "fiscalizacion.demo", Action = "Avance de obra", PreviousStatus = "Archivado", NewStatus = "Archivado", Department = "Fiscalización", Comment = "Registro de avance", Origin = "Cobro" },
        new() { Date = DateTime.Now, User = "bienes.demo", Action = "Actualización BI", PreviousStatus = "Archivado", NewStatus = "Archivado", Department = "Bienes Inmuebles", Comment = "Actualización mock", Origin = "Bienes Inmuebles" },
        new() { Date = DateTime.Now, User = "correspondencia.demo", Action = "Correspondencia", PreviousStatus = "Archivado", NewStatus = "Archivado", Department = "Correspondencia", Comment = "Derivación demo", Origin = "Correspondencia" },
        new() { Date = DateTime.Now, User = "reportes.demo", Action = "Generación reporte", PreviousStatus = "Archivado", NewStatus = "Archivado", Department = "Reportería", Comment = "Exportación mock", Origin = "Sistema" }
    ];

    private static List<PcAuditEventDto> GetAuditTemplate() =>
    [
        new() { Date = DateTime.Now.AddDays(-12), User = "recepcion.demo", Action = "Crear trámite", Entity = "Expediente", Details = "Alta inicial", Origin = "Plataforma de Servicios" },
        new() { Date = DateTime.Now.AddDays(-11), User = "recepcion.demo", Action = "Guardar borrador", Entity = "Expediente", Details = "Borrador guardado", Origin = "Plataforma de Servicios" },
        new() { Date = DateTime.Now.AddDays(-10), User = "clasificacion.demo", Action = "Cambiar estado", Entity = "Expediente", Details = "Recibido a clasificación", Origin = "Sistema" },
        new() { Date = DateTime.Now.AddDays(-9), User = "ruc.demo", Action = "Consulta RUC", Entity = "RUC", Details = "Validación simulada", Origin = "RUC" },
        new() { Date = DateTime.Now.AddDays(-8), User = "urbanismo.demo", Action = "Traslado", Entity = "Expediente", Details = "Urbanismo", Origin = "Módulo resolutor" },
        new() { Date = DateTime.Now.AddDays(-7), User = "inspeccion.demo", Action = "Control de calidad", Entity = "Expediente", Details = "Inspección inicial", Origin = "Control de calidad" },
        new() { Date = DateTime.Now.AddDays(-6), User = "tesoreria.demo", Action = "Tasación mock", Entity = "Tasa", Details = "Cálculo demo", Origin = "Tesorería" },
        new() { Date = DateTime.Now.AddDays(-5), User = "caja.demo", Action = "Cobro mock", Entity = "Cobro", Details = "Pendiente pago", Origin = "Cajas" },
        new() { Date = DateTime.Now.AddDays(-4), User = "caja.demo", Action = "Pago mock", Entity = "Cobro", Details = "Verificado", Origin = "Cajas" },
        new() { Date = DateTime.Now.AddDays(-3), User = "aprobador.demo", Action = "Resolución mock", Entity = "Resolución", Details = "Aprobado", Origin = "Sistema" },
        new() { Date = DateTime.Now.AddDays(-2), User = "notifica.demo", Action = "Notificación mock", Entity = "Notificación", Details = "Enviada", Origin = "Notificación" },
        new() { Date = DateTime.Now.AddDays(-1), User = "consulta.demo", Action = "Consulta expediente", Entity = "Expediente", Details = "Detalle visual", Origin = "Plataforma de Servicios" },
        new() { Date = DateTime.Now, User = "inspecciones.demo", Action = "Inspección registrada", Entity = "Inspecciones", Details = "Inspección mock", Origin = "Inspecciones" },
        new() { Date = DateTime.Now, User = "fiscalizacion.demo", Action = "Fiscalización avance", Entity = "Avance", Details = "Avance 80%", Origin = "Fiscalización" },
        new() { Date = DateTime.Now, User = "gis.demo", Action = "Validación GIS", Entity = "GIS", Details = "Capa validada", Origin = "GIS" },
        new() { Date = DateTime.Now, User = "correspondencia.demo", Action = "Correspondencia", Entity = "Correspondencia", Details = "Gestión registral", Origin = "Correspondencia" },
        new() { Date = DateTime.Now, User = "reporteria.demo", Action = "Reporte mock", Entity = "Reportes", Details = "Exportación simulada", Origin = "Sistema" }
    ];

    private static List<PcCatalogItemDto> BuildCatalogs() =>
    [
        new() { Code = "CAT-01", Name = "Estados de trámite", Description = "Recibido, revisión, prevención, resuelto", Status = "Preparado", Category = "Estados", Module = "Permisos de Construcción" },
        new() { Code = "CAT-02", Name = "Canales de ingreso", Description = "APC/CFIA, APC-R, Plataforma de Servicios", Status = "Preparado", Category = "Canales", Module = "Permisos de Construcción" },
        new() { Code = "CAT-03", Name = "Unidades propietarias", Description = "Urbanismo, Alineamientos, Inspecciones", Status = "Preparado", Category = "Organización", Module = "Permisos de Construcción" },
        new() { Code = "CAT-04", Name = "Tipos de obra", Description = "Obra mayor y obra menor", Status = "Preparado", Category = "Obra", Module = "Permisos de Construcción" },
        new() { Code = "CAT-05", Name = "Motivos de prevención", Description = "Documentación, criterio técnico, cobro", Status = "Preparado", Category = "Prevención", Module = "Permisos de Construcción" },
        new() { Code = "CAT-06", Name = "Estados APC", Description = "Pendiente, presentado, sellado, aprobado", Status = "Preparado", Category = "APC", Module = "Permisos de Construcción" },
        new() { Code = "CAT-07", Name = "Estados de pago", Description = "Pendiente, al cobro, pagado", Status = "Preparado", Category = "Cobro", Module = "Permisos de Construcción" },
        new() { Code = "CAT-08", Name = "Estados de inspección", Description = "Pendiente, agendada, aprobada", Status = "Preparado", Category = "Inspección", Module = "Permisos de Construcción" },
        new() { Code = "CAT-09", Name = "Estados de resolución", Description = "Borrador, generada, notificada", Status = "Preparado", Category = "Resolución", Module = "Permisos de Construcción" },
        new() { Code = "CAT-10", Name = "Estados de integración", Description = "Simulado, referencial, preparado", Status = "Preparado", Category = "Integraciones", Module = "Permisos de Construcción" },
        new() { Code = "CAT-11", Name = "Prioridades", Description = "Normal, alta, urgente", Status = "Preparado", Category = "Prioridad", Module = "Permisos de Construcción" },
        new() { Code = "CAT-12", Name = "Requisitos", Description = "Checklist básico del expediente", Status = "Preparado", Category = "Checklist", Module = "Permisos de Construcción" },
        new() { Code = "CAT-13", Name = "Tipos de trámite", Description = "APC, obra menor, modificación, prórroga", Status = "Preparado", Category = "Trámite", Module = "Permisos de Construcción" },
        new() { Code = "CAT-14", Name = "Reportes", Description = "Operativos y referenciales", Status = "Preparado", Category = "Reportes", Module = "Permisos de Construcción" },
        new() { Code = "CAT-15", Name = "Configuración", Description = "Catálogos mock para continuidad", Status = "Preparado", Category = "Configuración", Module = "Permisos de Construcción" },
        new() { Code = "CAT-16", Name = "Tipos de inspección", Description = "Inspección inicial, seguimiento, reinspección", Status = "Preparado", Category = "Inspecciones", Module = "Permisos de Construcción" },
        new() { Code = "CAT-17", Name = "Porcentajes de avance", Description = "0, 25, 50, 80 y 100%", Status = "Preparado", Category = "Avance", Module = "Permisos de Construcción" },
        new() { Code = "CAT-18", Name = "Correspondencia", Description = "Interna, externa y canales", Status = "Preparado", Category = "Correspondencia", Module = "Permisos de Construcción" },
        new() { Code = "CAT-19", Name = "Reportería", Description = "Reportes operativos mock", Status = "Preparado", Category = "Reportes", Module = "Permisos de Construcción" },
        new() { Code = "CAT-20", Name = "GIS y BI", Description = "Tareas geoespaciales y actualización BI", Status = "Preparado", Category = "Operación", Module = "Permisos de Construcción" }
    ];

    private static List<PcIntegrationStatusDto> BuildIntegrations() =>
    [
        new() { Name = "Plataforma de Servicios", Status = "Simulado", LastEvent = "Consulta expediente", Description = "Ingreso y seguimiento municipal", Category = "Canal", Icon = "Storefront" },
        new() { Name = "RUC", Status = "Referencial", LastEvent = "Validación mock", Description = "Identificación y razón social", Category = "Contribuyente", Icon = "Badge" },
        new() { Name = "Bienes Inmuebles", Status = "Referencial", LastEvent = "Consulta finca", Description = "Datos prediales y fiscales", Category = "Bien inmueble", Icon = "HomeWork" },
        new() { Name = "APC / CFIA", Status = "Simulado", LastEvent = "Sellado mock", Description = "Trámite técnico constructivo", Category = "APC", Icon = "Architecture" },
        new() { Name = "APC-R", Status = "Simulado", LastEvent = "Radicación mock", Description = "Registro complementario", Category = "APC", Icon = "EditNote" },
        new() { Name = "VUI", Status = "No incluido en MVP", LastEvent = "Referencia visual", Description = "Ventanilla única interoperable", Category = "Integración", Icon = "Hub" },
        new() { Name = "MIMUNIENCASA", Status = "Simulado", LastEvent = "Recepción mock", Description = "Ingreso no presencial", Category = "Canal digital", Icon = "House" },
        new() { Name = "GIS", Status = "Preparado", LastEvent = "Capa territorial", Description = "Mapa y georreferencia", Category = "Geoespacial", Icon = "Map" },
        new() { Name = "Movilidad", Status = "Referencial", LastEvent = "Vía pública", Description = "Alineamiento y tránsito", Category = "Movilidad", Icon = "Commute" },
        new() { Name = "Conectividad", Status = "Referencial", LastEvent = "Cobertura demo", Description = "Disponibilidad de servicios", Category = "Servicios", Icon = "Router" },
        new() { Name = "Comercial", Status = "Preparado", LastEvent = "Licencia referencial", Description = "Actividad económica", Category = "Comercial", Icon = "Store" },
        new() { Name = "Acueductos", Status = "Simulado", LastEvent = "Disponibilidad agua", Description = "Conexión y factibilidad", Category = "Servicios públicos", Icon = "WaterDrop" },
        new() { Name = "Alcantarillado Sanitario", Status = "Simulado", LastEvent = "Disponibilidad saneamiento", Description = "Saneamiento y descarga", Category = "Servicios públicos", Icon = "Plumbing" },
        new() { Name = "Cobro", Status = "Simulado", LastEvent = "Tasación y caja", Description = "Liquidación mock", Category = "Finanzas", Icon = "Payments" },
        new() { Name = "Tesorería", Status = "Preparado", LastEvent = "Validación de deuda", Description = "Control previo de liquidación", Category = "Finanzas", Icon = "AccountBalance" },
        new() { Name = "Cajas", Status = "Simulado", LastEvent = "Recepción de pago mock", Description = "Confirmación referencial de pagos", Category = "Finanzas", Icon = "PointOfSale" },
        new() { Name = "Contabilidad", Status = "Referencial", LastEvent = "Asiento visual", Description = "Registro contable no integrado", Category = "Finanzas", Icon = "ReceiptLong" },
        new() { Name = "Cuenta Tributaria", Status = "Preparado", LastEvent = "Consulta de estado", Description = "Balance y trazabilidad tributaria", Category = "Tributario", Icon = "Summarize" },
        new() { Name = "Notificaciones", Status = "Simulado", LastEvent = "Generación mock", Description = "Avisos por correo, SMS y portal", Category = "Comunicación", Icon = "Notifications" },
        new() { Name = "Seguridad", Status = "Preparado", LastEvent = "Bitácora de acceso", Description = "Control institucional y acceso", Category = "Control", Icon = "Security" },
        new() { Name = "Bitácoras / Históricos", Status = "Preparado", LastEvent = "Consulta expediente", Description = "Trazabilidad histórica completa", Category = "Control", Icon = "History" }
    ];

    private static string BuildCaseNumber(int index) => $"PC-{DateTime.Now:yyyy}-{index:0000}";

    private static T CloneObject<T>(T value) => JsonSerializer.Deserialize<T>(JsonSerializer.Serialize(value))!;
}