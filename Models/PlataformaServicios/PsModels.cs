using System;
using System.Collections.Generic;

namespace BlazorApp.Models.PlataformaServicios;

public class PsProcedureTypeDto
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string ModuleName { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public bool RequiresRuc { get; set; }
    public bool RequiresFinca { get; set; }
    public bool RequiresPatent { get; set; }
    public bool RequiresDocuments { get; set; } = true;
    public int DeadlineDays { get; set; } = 10;
    public string InitialStatus { get; set; } = "Recibido";
    public string Priority { get; set; } = "Normal";
}

public class PsApplicantDto
{
    public string Identification { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string PersonType { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string FiscalAddress { get; set; } = string.Empty;
    public string PreferredNotification { get; set; } = string.Empty;
    public string RucStatus { get; set; } = "Pendiente validar RUC";
    public string RucQuality { get; set; } = "Información parcial";
    public bool ExistsInRuc { get; set; } = true;
}

public class PsProcedureObjectDto
{
    public string Kind { get; set; } = string.Empty;
    public string Summary { get; set; } = string.Empty;
    public string IdPredial { get; set; } = string.Empty;
    public string NumeroFinca { get; set; } = string.Empty;
    public string NumeroPlano { get; set; } = string.Empty;
    public string Distrito { get; set; } = string.Empty;
    public string Propietario { get; set; } = string.Empty;
    public string NumeroLicencia { get; set; } = string.Empty;
    public string NombreComercial { get; set; } = string.Empty;
    public string ActividadEconomica { get; set; } = string.Empty;
    public string UsoSuelo { get; set; } = string.Empty;
    public string NumeroLocal { get; set; } = string.Empty;
    public string Arrendatario { get; set; } = string.Empty;
    public string Contrato { get; set; } = string.Empty;
    public string EstadoCuentaMock { get; set; } = string.Empty;
    public string ProfesionalResponsable { get; set; } = string.Empty;
    public string TipoObra { get; set; } = string.Empty;
    public string Area { get; set; } = string.Empty;
    public string MontoEstimado { get; set; } = string.Empty;
    public string TipoActualizacionRuc { get; set; } = string.Empty;
    public string DatosAModificar { get; set; } = string.Empty;
    public string OrigenSolicitud { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string Observaciones { get; set; } = string.Empty;
    public string ReferenciaExterna { get; set; } = string.Empty;
}

public class PsRequirementDto
{
    public string Name { get; set; } = string.Empty;
    public string Status { get; set; } = "Pendiente";
    public string DocumentPlaceholder { get; set; } = string.Empty;
    public string Observation { get; set; } = string.Empty;
    public DateTime? ReceivedAt { get; set; }
    public string Receiver { get; set; } = string.Empty;
    public string Origin { get; set; } = "Plataforma de Servicios";
}

public class PsClassificationDto
{
    public string ModuleName { get; set; } = string.Empty;
    public string DepartmentName { get; set; } = string.Empty;
    public string Responsible { get; set; } = string.Empty;
    public string Team { get; set; } = string.Empty;
    public string Status { get; set; } = "En clasificación";
    public string EstimatedDeadline { get; set; } = string.Empty;
    public DateTime? MockDueDate { get; set; }
    public string Priority { get; set; } = "Normal";
    public string TimeStatus { get; set; } = "En tiempo";
    public string TransferMessage { get; set; } = string.Empty;
}

public class PsQualityControlDto
{
    public string Status { get; set; } = "Pendiente";
    public string Responsible { get; set; } = string.Empty;
    public string Observation { get; set; } = string.Empty;
    public string ReturnReason { get; set; } = string.Empty;
    public DateTime? Date { get; set; }
    public string Action { get; set; } = string.Empty;
}

public class PsTransferDto
{
    public string OriginDepartment { get; set; } = string.Empty;
    public string DestinationDepartment { get; set; } = string.Empty;
    public string OriginUser { get; set; } = string.Empty;
    public string DestinationUser { get; set; } = string.Empty;
    public DateTime TransferDate { get; set; }
    public string Reason { get; set; } = string.Empty;
    public string Status { get; set; } = "Pendiente";
}

public class PsResolutionDto
{
    public string Type { get; set; } = string.Empty;
    public DateTime? Date { get; set; }
    public string Responsible { get; set; } = string.Empty;
    public string Status { get; set; } = "Borrador";
    public string DigitalSignatureReference { get; set; } = string.Empty;
    public string PdfReference { get; set; } = string.Empty;
    public string Observation { get; set; } = string.Empty;
}

public class PsNotificationDto
{
    public string Type { get; set; } = string.Empty;
    public string Channel { get; set; } = string.Empty;
    public string Status { get; set; } = "Pendiente";
    public DateTime? Date { get; set; }
    public string Result { get; set; } = string.Empty;
    public string Observation { get; set; } = string.Empty;
}

public class PsDeadlineDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int DaysElapsed { get; set; }
    public int DaysRemaining { get; set; }
    public string Status { get; set; } = "En tiempo";
    public string SuspensionReason { get; set; } = string.Empty;
    public string Alert { get; set; } = string.Empty;
}

public class PsIntegrationStatusDto
{
    public string Name { get; set; } = string.Empty;
    public string Status { get; set; } = "Simulado";
    public string LastEvent { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string ActionText { get; set; } = string.Empty;
}

public class PsHistoryEventDto
{
    public DateTime Date { get; set; }
    public string User { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public string PreviousStatus { get; set; } = string.Empty;
    public string NewStatus { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;
    public string Origin { get; set; } = "Plataforma de Servicios";
}

public class PsAuditEventDto
{
    public string EventId { get; set; } = Guid.NewGuid().ToString("N");
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public string User { get; set; } = "demo.user";
    public string Action { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Severity { get; set; } = "Info";
}

public class PsReportDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Filters { get; set; } = string.Empty;
    public string Status { get; set; } = "Mock";
}

public class PsCatalogItemDto
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Status { get; set; } = "Preparado";
}

public class PsProcedureDto
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string CaseNumber { get; set; } = string.Empty;
    public string TypeCode { get; set; } = string.Empty;
    public string TypeName { get; set; } = string.Empty;
    public string ResolverModule { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string Responsible { get; set; } = string.Empty;
    public string Team { get; set; } = string.Empty;
    public string Channel { get; set; } = string.Empty;
    public string Status { get; set; } = "Borrador";
    public string Priority { get; set; } = "Normal";
    public DateTime EntryDate { get; set; } = DateTime.Today;
    public DateTime LimitDate { get; set; } = DateTime.Today.AddDays(10);
    public PsApplicantDto Applicant { get; set; } = new();
    public PsProcedureObjectDto Object { get; set; } = new();
    public List<PsRequirementDto> Requirements { get; set; } = new();
    public PsClassificationDto Classification { get; set; } = new();
    public PsQualityControlDto QualityControl { get; set; } = new();
    public List<PsTransferDto> Transfers { get; set; } = new();
    public PsResolutionDto Resolution { get; set; } = new();
    public List<PsNotificationDto> Notifications { get; set; } = new();
    public PsDeadlineDto Deadline { get; set; } = new();
    public List<PsIntegrationStatusDto> Integrations { get; set; } = new();
    public List<PsHistoryEventDto> History { get; set; } = new();
    public List<PsAuditEventDto> AuditEvents { get; set; } = new();
    public List<string> Alerts { get; set; } = new();
    public string Summary { get; set; } = string.Empty;
}