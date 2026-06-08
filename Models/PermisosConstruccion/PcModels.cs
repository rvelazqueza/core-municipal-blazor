using System;
using System.Collections.Generic;

namespace BlazorApp.Models.PermisosConstruccion;

public class PcPermitDto
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string CaseNumber { get; set; } = string.Empty;
    public string SystemCode { get; set; } = string.Empty;
    public string CfiaApcCode { get; set; } = string.Empty;
    public string ApcRCode { get; set; } = string.Empty;
    public string TypeCode { get; set; } = string.Empty;
    public string TypeName { get; set; } = string.Empty;
    public string Channel { get; set; } = string.Empty;
    public string UnitOwner { get; set; } = string.Empty;
    public string PermitStatus { get; set; } = "Recibido";
    public string ApcStatus { get; set; } = "Pendiente";
    public string PaymentStatus { get; set; } = "Pendiente";
    public string InspectionStatus { get; set; } = "Pendiente";
    public string ResolutionStatus { get; set; } = "Borrador";
    public string ResolutionResult { get; set; } = "Pendiente información";
    public string Priority { get; set; } = "Normal";
    public string District { get; set; } = string.Empty;
    public decimal ProgressPercent { get; set; } = 10;
    public decimal WorkAmount { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.Today;
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime? FinishedAt { get; set; }
    public string UserResponsible { get; set; } = "analista.demo";
    public string AssignedDepartment { get; set; } = "Urbanismo";
    public string AssignedTeam { get; set; } = "Equipo técnico";
    public string Observations { get; set; } = string.Empty;
    public PcApplicantDto Applicant { get; set; } = new();
    public PcOwnerDto Owner { get; set; } = new();
    public PcPropertyDto Property { get; set; } = new();
    public PcProfessionalDto Professional { get; set; } = new();
    public PcTechnicalWorkDto TechnicalWork { get; set; } = new();
    public PcApcStatusDto Apc { get; set; } = new();
    public List<PcRequirementDto> Requirements { get; set; } = new();
    public List<PcDocumentDto> Documents { get; set; } = new();
    public PcRoadAlignmentDto RoadAlignment { get; set; } = new();
    public PcLandUseDto LandUse { get; set; } = new();
    public List<PcPublicServiceDto> PublicServices { get; set; } = new();
    public List<PcInspectionDto> Inspections { get; set; } = new();
    public List<PcWorkProgressDto> WorkProgressEntries { get; set; } = new();
    public List<PcBiUpdateDto> BiUpdates { get; set; } = new();
    public List<PcGisStatusDto> GisTasks { get; set; } = new();
    public List<PcCorrespondenceDto> Correspondences { get; set; } = new();
    public List<PcReportDto> Reports { get; set; } = new();
    public PcMunicipalReviewDto MunicipalReview { get; set; } = new();
    public PcQualityControlDto QualityControl { get; set; } = new();
    public PcAssessmentDto Assessment { get; set; } = new();
    public PcPaymentMockDto Payment { get; set; } = new();
    public PcResolutionDto Resolution { get; set; } = new();
    public List<PcHistoryEventDto> HistoryEvents { get; set; } = new();
    public List<PcAuditEventDto> AuditEvents { get; set; } = new();
    public List<PcIntegrationStatusDto> Integrations { get; set; } = new();
}

public class PcPermitTypeDto
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string ModuleResolver { get; set; } = "Urbanismo";
    public bool RequiresRuc { get; set; } = true;
    public bool RequiresProperty { get; set; } = true;
    public bool RequiresProfessional { get; set; } = true;
    public bool RequiresApc { get; set; } = true;
    public bool RequiresDocuments { get; set; } = true;
    public bool RequiresInspection { get; set; } = true;
    public string InitialStatus { get; set; } = "Recibido";
    public string Priority { get; set; } = "Normal";
}

public class PcApplicantDto
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string Identification { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string PersonType { get; set; } = "Jurídica";
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string RucStatus { get; set; } = "Validado";
    public string PreferredNotification { get; set; } = "Correo";
    public bool ExistsInRuc { get; set; } = true;
}

public class PcOwnerDto
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string Identification { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string PersonType { get; set; } = "Jurídica";
    public string Relationship { get; set; } = "Propietario";
    public decimal OwnershipPercentage { get; set; } = 100;
    public string AuthorizationMock { get; set; } = "No aplica";
}

public class PcPropertyDto
{
    public string IdPredial { get; set; } = string.Empty;
    public string PropertyNumber { get; set; } = string.Empty;
    public string PlanNumber { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public string Sector { get; set; } = string.Empty;
    public string Block { get; set; } = string.Empty;
    public string ExactAddress { get; set; } = string.Empty;
    public decimal LandArea { get; set; }
    public string CurrentUse { get; set; } = string.Empty;
    public decimal FiscalValue { get; set; }
    public string GisStatus { get; set; } = "Simulado";
    public string TributaryAccountStatus { get; set; } = "Al día";
    public string BiUpdateStatus { get; set; } = "Pendiente actualización";
    public string ServicesAccount { get; set; } = string.Empty;
    public string Gravames { get; set; } = "Sin gravámenes";
}

public class PcProfessionalDto
{
    public string Identification { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string LicenseNumber { get; set; } = string.Empty;
    public string College { get; set; } = "CFIA";
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Specialty { get; set; } = string.Empty;
    public string CfiaStatus { get; set; } = "Activo";
    public string MockValidation { get; set; } = "Pendiente";
}

public class PcTechnicalWorkDto
{
    public string WorkType { get; set; } = "Obra mayor";
    public string ProjectType { get; set; } = string.Empty;
    public string ProjectName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public string RelatedEconomicActivity { get; set; } = string.Empty;
    public string FundingSource { get; set; } = "Privado";
    public decimal ExistingArea { get; set; }
    public decimal NewArea { get; set; }
    public decimal TotalArea { get; set; }
    public decimal WorkAmount { get; set; }
    public decimal EstimatedTaxAmount { get; set; }
    public decimal InitialProgress { get; set; }
    public string ConstructionSystem { get; set; } = string.Empty;
    public string RoadMaterial { get; set; } = string.Empty;
    public DateTime? StartDate { get; set; } = DateTime.Today.AddDays(15);
    public DateTime? EndDate { get; set; } = DateTime.Today.AddMonths(6);
    public bool RequiresWater { get; set; } = true;
    public bool RequiresSewer { get; set; } = true;
    public string GeneratedServicesAccount { get; set; } = string.Empty;
    public bool Regularization { get; set; }
    public string RegularizationOrigin { get; set; } = string.Empty;
    public string RegularizationFinalAct { get; set; } = string.Empty;
    public string Observations { get; set; } = string.Empty;
}

public class PcApcStatusDto
{
    public string CfiaApcCode { get; set; } = string.Empty;
    public string ApcStatus { get; set; } = "Pendiente";
    public string ApcRStatus { get; set; } = "Pendiente";
    public DateTime? ReceptionDate { get; set; }
    public DateTime? PreventionDate { get; set; }
    public DateTime? CorrectionDate { get; set; }
    public DateTime? ApprovalDate { get; set; }
    public DateTime? SealedAt { get; set; }
    public DateTime? LastSyncAt { get; set; }
    public string MockSyncStatus { get; set; } = "Simulado";
}

public class PcRequirementDto
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string State { get; set; } = "Pendiente";
    public string DocumentPlaceholder { get; set; } = string.Empty;
    public string Observation { get; set; } = string.Empty;
    public DateTime? ReceivedAt { get; set; }
    public string ReceivedBy { get; set; } = string.Empty;
    public string Origin { get; set; } = "Plataforma de Servicios";
}

public class PcDocumentDto
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Placeholder { get; set; } = string.Empty;
    public string State { get; set; } = "Presentado";
    public DateTime? UploadedAt { get; set; }
    public string UploadedBy { get; set; } = string.Empty;
    public string Origin { get; set; } = "Plataforma de Servicios";
}

public class PcRoadAlignmentDto
{
    public bool Required { get; set; } = true;
    public DateTime? RequestDate { get; set; }
    public DateTime? SignalDate { get; set; }
    public string ManagementNumber { get; set; } = string.Empty;
    public string Responsible { get; set; } = string.Empty;
    public string Status { get; set; } = "Pendiente";
    public string Observations { get; set; } = string.Empty;
    public string ImagePlaceholder { get; set; } = "Imagen referencial";
}

public class PcLandUseDto
{
    public bool Required { get; set; } = true;
    public string CertificateNumber { get; set; } = string.Empty;
    public string Status { get; set; } = "Aprobado";
    public string AllowedActivity { get; set; } = string.Empty;
    public string Compatibility { get; set; } = "Compatible";
    public DateTime? EmittedAt { get; set; }
    public DateTime? ValidUntil { get; set; }
    public string Observations { get; set; } = string.Empty;
}

public class PcPublicServiceDto
{
    public string ServiceType { get; set; } = "Agua potable";
    public bool Required { get; set; } = true;
    public string Status { get; set; } = "Simulado";
    public string Administrator { get; set; } = string.Empty;
    public string Diameter { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
}

public class PcMunicipalReviewDto
{
    public string Technician { get; set; } = string.Empty;
    public string UrbanismAnalyst { get; set; } = string.Empty;
    public string Status { get; set; } = "Pendiente";
    public string TechnicalCriterion { get; set; } = string.Empty;
    public string Observations { get; set; } = string.Empty;
    public DateTime? ReviewedAt { get; set; } = DateTime.Today;
}

public class PcQualityControlDto
{
    public string Status { get; set; } = "Pendiente";
    public string Responsible { get; set; } = string.Empty;
    public string Inconsistencies { get; set; } = string.Empty;
    public bool RucChecked { get; set; } = true;
    public bool PropertyChecked { get; set; } = true;
    public bool TributaryAccountChecked { get; set; } = true;
    public bool DocumentsChecked { get; set; } = true;
    public string ReturnReason { get; set; } = string.Empty;
    public bool RequiresInspection { get; set; } = true;
    public string Inspector { get; set; } = string.Empty;
    public string InspectionStatus { get; set; } = "Pendiente";
    public string InspectionResult { get; set; } = "Pendiente";
}

public class PcAssessmentDto
{
    public string CaseNumber { get; set; } = string.Empty;
    public decimal ConstructionTaxPercent { get; set; } = 1.5m;
    public decimal TotalWorkValue { get; set; }
    public decimal ConstructionTax { get; set; }
    public decimal Fine { get; set; }
    public decimal Interest { get; set; }
    public decimal UrbanServices { get; set; }
    public decimal TotalToPay { get; set; }
    public string Status { get; set; } = "Pendiente";
}

public class PcPaymentMockDto
{
    public string Status { get; set; } = "Pendiente";
    public string PaymentMethod { get; set; } = "Caja";
    public string InsurancePolicyStatus { get; set; } = "Pendiente";
    public string ReceiptMock { get; set; } = string.Empty;
    public bool Verified { get; set; }
    public bool Infructuous { get; set; }
    public bool ReversalGenerated { get; set; }
}

public class PcResolutionDto
{
    public string Result { get; set; } = "Pendiente información";
    public string PermitNumber { get; set; } = string.Empty;
    public DateTime? ApprovalDate { get; set; }
    public string ApprovedBy { get; set; } = string.Empty;
    public string ResolutionTerm { get; set; } = "10 días ordinario";
    public string NotificationSimulated { get; set; } = "Pendiente";
    public string DigitalSignatureRef { get; set; } = string.Empty;
    public string LicenseMock { get; set; } = string.Empty;
    public string ApcSyncStatus { get; set; } = "Simulado";
}

public class PcHistoryEventDto
{
    public DateTime Date { get; set; } = DateTime.Now;
    public string User { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string PreviousStatus { get; set; } = string.Empty;
    public string NewStatus { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;
    public string Origin { get; set; } = "Sistema";
}

public class PcAuditEventDto
{
    public DateTime Date { get; set; } = DateTime.Now;
    public string User { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string Entity { get; set; } = string.Empty;
    public string Details { get; set; } = string.Empty;
    public string Origin { get; set; } = "Sistema";
}

public class PcCatalogItemDto
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = "Referencial";
    public string Category { get; set; } = string.Empty;
    public string Module { get; set; } = string.Empty;
}

public class PcIntegrationStatusDto
{
    public string Name { get; set; } = string.Empty;
    public string Status { get; set; } = "Simulado";
    public string LastEvent { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
}
