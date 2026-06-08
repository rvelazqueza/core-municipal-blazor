using System;
using System.Collections.Generic;

namespace BlazorApp.Models;

public class MmManagementWizardViewModel
{
    public string SelectedReferenceLocalId { get; set; } = string.Empty;
    public string SelectedReferenceTenantId { get; set; } = string.Empty;

    public string ManagementType { get; set; } = string.Empty;
    public string IntakeChannel { get; set; } = string.Empty;
    public string MockFileNumber { get; set; } = string.Empty;
    public string InitialStatus { get; set; } = "Borrador";
    public string ResponsibleDepartment { get; set; } = string.Empty;

    public string LocalNumber { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public string LocalType { get; set; } = string.Empty;
    public string RentalType { get; set; } = string.Empty;
    public string LocalStatus { get; set; } = string.Empty;
    public string Floor { get; set; } = string.Empty;
    public string Zone { get; set; } = string.Empty;
    public string Sector { get; set; } = string.Empty;
    public string Aisle { get; set; } = string.Empty;
    public string Location { get; set; } = string.Empty;
    public string RelatedProperty { get; set; } = string.Empty;
    public string PatentNumber { get; set; } = string.Empty;
    public string ServiceAccount { get; set; } = string.Empty;
    public string ElectricMeterNumber { get; set; } = string.Empty;
    public decimal? AreaM2 { get; set; }
    public decimal? WeightingFactor { get; set; }
    public decimal? ProposedPrice { get; set; }
    public decimal? RentAmount { get; set; }
    public string PermittedBusinessActivity { get; set; } = string.Empty;
    public string CommercialName { get; set; } = string.Empty;
    public string LocalPhotoPlaceholder { get; set; } = "Fotografía local placeholder";
    public string LocalDocumentPlaceholder { get; set; } = "Documento local placeholder";

    public string LinkedRuc { get; set; } = string.Empty;
    public string Identification { get; set; } = string.Empty;
    public string TenantName { get; set; } = string.Empty;
    public string PersonType { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string RucStatus { get; set; } = string.Empty;
    public string DataQualityStatus { get; set; } = string.Empty;
    public string TenantPhotoPlaceholder { get; set; } = "Fotografía inquilino placeholder";
    public string EconomicActivity { get; set; } = string.Empty;
    public string BusinessName { get; set; } = string.Empty;
    public string LegalRepresentative { get; set; } = string.Empty;
    public string Condition { get; set; } = string.Empty;

    public string ContractNumber { get; set; } = string.Empty;
    public string ContractType { get; set; } = string.Empty;
    public DateTime? ContractStartDate { get; set; }
    public DateTime? ContractEndDate { get; set; }
    public int? ContractDurationMonths { get; set; }
    public bool IsFiveYearTerm { get; set; } = true;
    public string ContractStatus { get; set; } = string.Empty;
    public string MunicipalResponsible { get; set; } = string.Empty;
    public string MainConditions { get; set; } = string.Empty;
    public string TenantObligations { get; set; } = string.Empty;
    public string ContractObservations { get; set; } = string.Empty;
    public string ContractDocumentPlaceholder { get; set; } = "Contrato placeholder";
    public string SignatureReference { get; set; } = "Firma digital referencial";
    public bool ShowExpirationAlert { get; set; }

    public decimal? MonthlyAmount { get; set; }
    public string Currency { get; set; } = "CRC";
    public string ChargePeriodicity { get; set; } = string.Empty;
    public int? StartMonth { get; set; }
    public int? EndMonth { get; set; }
    public int? StartYear { get; set; }
    public int? EndYear { get; set; }
    public DateTime? ChargeDate { get; set; }
    public DateTime? ChargeDueDate { get; set; }
    public decimal? ChargeWeightingFactor { get; set; }
    public decimal? PricePerM2 { get; set; }
    public decimal? ChargeAreaM2 { get; set; }
    public decimal? MockCalculatedAmount { get; set; }
    public decimal? DepositGuarantee { get; set; }
    public bool AnnualAdjustment { get; set; }
    public bool IndividualAdjustment { get; set; }
    public bool CollectiveAdjustment { get; set; }
    public decimal? DiscountAmount { get; set; }
    public string DiscountReason { get; set; } = string.Empty;
    public bool ChargeSuspension { get; set; }
    public bool HasWaterService { get; set; }
    public bool HasElectricityService { get; set; }
    public bool HasCollectionService { get; set; }
    public bool HasMaintenanceService { get; set; }
    public bool HasOtherService { get; set; }
    public string ChargeObservations { get; set; } = string.Empty;

    public List<MmWizardRequirementItemViewModel> Requirements { get; set; } = new();

    public string LocalAccount { get; set; } = string.Empty;
    public string TaxAccountNumber { get; set; } = string.Empty;
    public string ReceivableChangeSummary { get; set; } = "Agregadas: 0 | Eliminadas: 0 | Modificadas: 0 | Pendientes: 0";
    public string AccountStatus { get; set; } = string.Empty;
    public decimal? AccountBalance { get; set; }
    public DateTime? LastPaymentDate { get; set; }
    public decimal? MonthlyQuotaGenerated { get; set; }
    public string MonthlyEmissionMock { get; set; } = string.Empty;
    public string AnnualEmissionMock { get; set; } = string.Empty;
    public string CollectionReferenceStatus { get; set; } = "Cobro administrativo referencial";
    public string TreasuryReferenceStatus { get; set; } = "Tesorería/Cajas/Conectividad referencial";
}

public class MmWizardRequirementItemViewModel
{
    public string Name { get; set; } = string.Empty;
    public string Status { get; set; } = "Pendiente";
    public string DocumentPlaceholder { get; set; } = string.Empty;
    public string Observation { get; set; } = string.Empty;
    public string ReceivedBy { get; set; } = string.Empty;
    public DateTime? ReceivedAt { get; set; }
}
