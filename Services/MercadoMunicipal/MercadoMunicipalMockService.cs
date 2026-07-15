using System;
using System.Collections.Generic;
using System.Linq;
using BlazorApp.Models;

namespace BlazorApp.Services;

public class MercadoMunicipalMockService
{
    private static readonly List<MmTenantDto> tenants = BuildTenants();
    private static readonly List<MmLocalDto> locals = BuildLocals();
    private static readonly List<MmContractDto> contracts = BuildContracts();
    private static readonly List<MmRentDto> rents = BuildRents();
    private static readonly List<MmRequirementDto> requirements = BuildRequirements();
    private static readonly List<MmDocumentDto> documents = BuildDocuments();
    private static readonly List<MmTaxAccountMockDto> taxAccounts = BuildTaxAccounts();
    private static readonly List<MmReceivableDto> receivables = BuildReceivables();
    private static readonly List<MmEmissionDto> emissions = BuildEmissions();
    private static readonly List<MmPaymentMockDto> payments = BuildPayments();
    private static readonly List<MmCollectionDto> collections = BuildCollections();
    private static readonly List<MmModificationDto> modifications = BuildModifications();
    private static readonly List<MmTransferDto> transfers = BuildTransfers();
    private static readonly List<MmMergerSegregationDto> mergersSegregations = BuildMergersSegregations();
    private static readonly List<MmWarningDto> warnings = BuildWarnings();
    private static readonly List<MmSanctionDto> sanctions = BuildSanctions();
    private static readonly List<MmRemodelingPermitDto> remodelingPermits = BuildRemodelingPermits();
    private static readonly List<MmSpecialScheduleDto> specialSchedules = BuildSpecialSchedules();
    private static readonly List<MmCardDto> cards = BuildCards();
    private static readonly List<MmInspectionDto> inspections = BuildInspections();
    private static readonly List<MmCorrespondenceDto> correspondences = BuildCorrespondences();
    private static readonly List<MmNotificationDto> notifications = BuildNotifications();
    private static readonly List<MmReportDto> reports = BuildReports();
    private static readonly List<MmHistoryEventDto> history = BuildHistory();
    private static readonly List<MmAuditEventDto> audits = BuildAudits();
    private static readonly List<MmCatalogItemDto> catalogs = BuildCatalogs();
    private static readonly List<MmIntegrationStatusDto> integrations = BuildIntegrations();

    public List<MmLocalDto> GetLocals() => locals.ToList();
    public MmLocalDto? GetLocalById(string id) => locals.FirstOrDefault(x => x.Id.Equals(id, StringComparison.OrdinalIgnoreCase));
    public List<MmTenantDto> GetTenants() => tenants.ToList();
    public List<MmContractDto> GetContracts() => contracts.ToList();
    public List<MmRentDto> GetRents() => rents.ToList();
    public List<MmRequirementDto> GetRequirements() => requirements.ToList();
    public List<MmDocumentDto> GetDocuments() => documents.ToList();
    public List<MmTaxAccountMockDto> GetTaxAccounts() => taxAccounts.ToList();
    public List<MmReceivableDto> GetReceivables() => receivables.ToList();
    public List<MmEmissionDto> GetEmissions() => emissions.ToList();
    public List<MmPaymentMockDto> GetPayments() => payments.ToList();
    public List<MmCollectionDto> GetCollections() => collections.ToList();
    public List<MmModificationDto> GetModifications() => modifications.ToList();
    public List<MmTransferDto> GetTransfers() => transfers.ToList();
    public List<MmMergerSegregationDto> GetMergersSegregations() => mergersSegregations.ToList();
    public List<MmWarningDto> GetWarnings() => warnings.ToList();
    public List<MmSanctionDto> GetSanctions() => sanctions.ToList();
    public List<MmRemodelingPermitDto> GetRemodelingPermits() => remodelingPermits.ToList();
    public List<MmSpecialScheduleDto> GetSpecialSchedules() => specialSchedules.ToList();
    public List<MmCardDto> GetCards() => cards.ToList();
    public List<MmInspectionDto> GetInspections() => inspections.ToList();
    public List<MmCorrespondenceDto> GetCorrespondences() => correspondences.ToList();
    public List<MmNotificationDto> GetNotifications() => notifications.ToList();
    public List<MmReportDto> GetReports() => reports.ToList();
    public List<MmHistoryEventDto> GetHistory() => history.ToList();
    public List<MmAuditEventDto> GetAuditEvents() => audits.ToList();
    public List<MmCatalogItemDto> GetCatalogs() => catalogs.ToList();
    public List<MmIntegrationStatusDto> GetIntegrations() => integrations.ToList();

    private static List<MmTenantDto> BuildTenants()
    {
        var result = new List<MmTenantDto>();

        foreach (var index in Enumerable.Range(1, 15))
        {
            result.Add(new MmTenantDto
            {
                Id = $"TEN-{index:000}",
                Identification = $"1-0{index:03}-0{index:03}",
                Name = $"Inquilino Comercial {index:00}",
                PersonType = index % 3 == 0 ? "Jurídica" : "Física",
                Phone = $"222{index:0000}",
                Email = $"inquilino{index:00}@demo.mun",
                Address = $"Dirección mock #{index}, Mercado Municipal central.",
                RucStatus = index % 5 == 0 ? "Pendiente" : "Activo",
                DataQualityStatus = index % 4 == 0 ? "Observada" : "Validada",
                BusinessName = $"Negocio {index:00}",
                EconomicActivity = index % 2 == 0 ? "Abarrotes" : "Servicios varios",
                PhotoPlaceholder = $"https://placehold.co/600x380?text=Inquilino+{index:00}",
                Condition = index % 4 == 0 ? "Derecho de piso" : index % 3 == 0 ? "Ocasional" : "Fijo",
                LegalRepresentative = index % 3 == 0 ? $"Representante Legal {index:00}" : "No aplica"
            });
        }

        return result;
    }

    private static List<MmLocalDto> BuildLocals()
    {
        var result = new List<MmLocalDto>();

        foreach (var index in Enumerable.Range(1, 25))
        {
            var localType = index <= 12 ? "Fijo" : index <= 20 ? "Ocasional" : "Derecho de piso";
            var rentalType = index <= 12 ? "Permanente" : index <= 20 ? "Ocasional" : "Transitorio";
            var status = index <= 12
                ? "Alquilado"
                : index is 19 or 23
                    ? "En mantenimiento"
                    : index <= 20
                        ? "Desocupado"
                        : index % 2 == 0
                            ? "En remate"
                            : "Desocupado";

            result.Add(new MmLocalDto
            {
                Id = $"LOC-{index:000}",
                LocalNumber = index <= 12 ? $"F-{index:03}" : index <= 20 ? $"O-{index:03}" : $"D-{index:03}",
                AccountNumber = $"MM-ACCT-{index:04}",
                LocalType = localType,
                RentalType = rentalType,
                Status = status,
                Floor = $"Piso {((index - 1) % 3) + 1}",
                Zone = $"Zona {((index - 1) % 4) + 1}",
                Sector = $"Sector {((index - 1) % 5) + 1}",
                Aisle = $"Pasillo {((index - 1) % 6) + 1}",
                Location = $"Bloque {((index - 1) % 4) + 1} - Local {index:00}",
                RelatedProperty = $"Finca {1000 + index}",
                PatentNumber = index <= 15 ? $"PAT-{index:04}" : string.Empty,
                ServiceAccount = $"SERV-{index:05}",
                ElectricMeterNumber = $"MED-{index:04}",
                AreaM2 = 8 + (index * 1.5m),
                WeightingFactor = 1m + ((index - 1) % 5) * 0.1m,
                ProposedPrice = 180m + (index * 7m),
                RentAmount = 120m + (index * 9m),
                BusinessActivity = index % 2 == 0 ? "Alimentos y bebidas" : "Comercio general",
                BusinessName = index <= 15 ? $"Negocio {index:00}" : $"Local disponible {index:00}",
                TenantId = index <= 15 ? $"TEN-{index:000}" : string.Empty,
                HasMorosity = index is 4 or 7 or 13 or 18 or 22 or 24,
                LastUpdated = DateTime.Today.AddDays(-index),
                Observations = index % 4 == 0 ? "Se requiere seguimiento documental y revisión visual mock." : "Expediente base al día para consulta y edición controlada.",
                PhotoPlaceholder = $"https://placehold.co/800x420?text=Local+{index:00}"
            });
        }

        return result;
    }

    private static List<MmContractDto> BuildContracts()
    {
        var result = new List<MmContractDto>();

        foreach (var index in Enumerable.Range(1, 15))
        {
            var status = index <= 10 ? "Activo" : index <= 13 ? "Próximo a vencer" : "Vencido";
            var localId = $"LOC-{index:000}";
            var tenantId = $"TEN-{index:000}";
            var contractYear = 2024 + index;

            result.Add(new MmContractDto
            {
                Id = $"CON-{index:000}",
                LocalId = localId,
                TenantId = tenantId,
                ContractNumber = $"MM-CON-{contractYear:0000}-{index:03}",
                ContractType = index % 3 == 0 ? "Derecho de piso" : "Arrendamiento local fijo",
                StartDate = DateTime.Today.AddMonths(-index * 4),
                EndDate = DateTime.Today.AddMonths(24 - index),
                DurationMonths = index % 2 == 0 ? 60 : 24,
                IsFiveYearTerm = index <= 12,
                Status = status,
                ResponsibleUser = "Analista Mercado",
                Obligations = "Pago oportuno, conservación del espacio, cumplimiento sanitario y uso autorizado.",
                Conditions = "Uso comercial, mantenimiento ordinario y cumplimiento municipal.",
                Notes = index % 4 == 0 ? "Incluye revisión documental referencial." : "Contrato mock sin valor legal.",
                DocumentPlaceholder = $"Contrato_{index:00}.pdf",
                HasDigitalSignature = index % 2 == 0
            });
        }

        return result;
    }

    private static List<MmRentDto> BuildRents()
    {
        var result = new List<MmRentDto>();

        foreach (var index in Enumerable.Range(1, 15))
        {
            var area = 8m + (index * 1.5m);
            var factor = 1m + ((index - 1) % 5) * 0.1m;
            var pricePerM2 = 180m + (index * 7m);

            result.Add(new MmRentDto
            {
                Id = $"RNT-{index:000}",
                LocalId = $"LOC-{index:000}",
                MonthlyAmount = Math.Round(120m + (index * 9m), 2),
                Currency = "CRC",
                Periodicity = index % 3 == 0 ? "Anual" : "Mensual",
                StartMonth = ((index - 1) % 12) + 1,
                EndMonth = ((index + 3) % 12) + 1,
                StartYear = DateTime.Today.Year,
                EndYear = DateTime.Today.Year + 1,
                DueDay = 5 + ((index - 1) % 10),
                WeightingFactor = factor,
                PricePerM2 = pricePerM2,
                AreaM2 = area,
                MockCalculatedAmount = Math.Round(area * factor * pricePerM2, 2),
                HasDiscount = index % 4 == 0,
                IsSuspended = index % 6 == 0,
                Notes = "Monto calculado de forma visual para demo."
            });
        }

        return result;
    }

    private static List<MmRequirementDto> BuildRequirements()
    {
        var names = new[]
        {
            "Identificación del inquilino",
            "Personería jurídica, si aplica",
            "Contrato firmado",
            "Documento de adjudicación",
            "Acuerdo de Concejo Municipal",
            "Autorización de traspaso",
            "Comprobante de garantía",
            "Patente o actividad comercial",
            "Permiso sanitario",
            "Constancia de estar al día",
            "Fotografía del local",
            "Fotografía del inquilino",
            "Documentación de derecho de piso",
            "Documento de remodelación",
            "Otros requisitos"
        };

        var result = new List<MmRequirementDto>();
        var counter = 1;

        foreach (var localIndex in Enumerable.Range(1, 25))
        {
            foreach (var name in names)
            {
                result.Add(new MmRequirementDto
                {
                    Id = $"REQ-{counter:0000}",
                    LocalId = $"LOC-{localIndex:000}",
                    Name = name,
                    Status = counter % 5 == 0 ? "Pendiente" : counter % 4 == 0 ? "Observado" : counter % 3 == 0 ? "No aplica" : "Presentado",
                    DocumentPlaceholder = $"Documento_{localIndex:000}_{counter:0000}",
                    Observation = counter % 4 == 0 ? "Requiere subsanar observación visual." : "Revisión mock satisfactoria.",
                    ReceivedBy = counter % 2 == 0 ? "Ventanilla Mercado" : "Gestión Documental",
                    ReceivedAt = DateTime.Today.AddDays(-counter)
                });
                counter++;
            }
        }

        return result;
    }

    private static List<MmDocumentDto> BuildDocuments()
    {
        var names = new[]
        {
            "Expediente físico",
            "Contrato referencial",
            "Fotografía local",
            "Fotografía inquilino",
            "Orden interna",
            "Resolución mock",
            "Control de pagos mock",
            "Acta de inspección referencial"
        };

        var result = new List<MmDocumentDto>();
        var counter = 1;

        foreach (var localIndex in Enumerable.Range(1, 25))
        {
            foreach (var name in names)
            {
                result.Add(new MmDocumentDto
                {
                    Id = $"DOC-{counter:0000}",
                    LocalId = $"LOC-{localIndex:000}",
                    Name = name,
                    DocumentType = counter % 2 == 0 ? "PDF" : "Imagen",
                    Status = counter % 3 == 0 ? "Configurado" : "Disponible",
                    Placeholder = $"Archivo_{localIndex:000}_{counter:0000}"
                });
                counter++;
            }
        }

        return result;
    }

    private static List<MmTaxAccountMockDto> BuildTaxAccounts()
    {
        var result = new List<MmTaxAccountMockDto>();

        foreach (var index in Enumerable.Range(1, 15))
        {
            result.Add(new MmTaxAccountMockDto
            {
                Id = $"TAX-{index:000}",
                LocalId = $"LOC-{index:000}",
                AccountNumber = $"CTA-TRI-{index:04}",
                Period = $"{DateTime.Today:yyyy}-{((index - 1) % 12) + 1:00}",
                Month = ((index - 1) % 12) + 1,
                Debit = 1000m + (index * 120m),
                Credit = 350m + (index * 75m),
                Balance = 650m + (index * 45m),
                Status = index % 4 == 0 ? "Moroso" : index % 3 == 0 ? "Pendiente" : "Al día",
                LastPaymentDate = index % 3 == 0 ? DateTime.Today.AddDays(-(index * 2)) : DateTime.Today.AddDays(-(index + 1)),
                MorosityStatus = index % 4 == 0 ? "Con morosidad simulada" : "Sin morosidad"
            });
        }

        return result;
    }

    private static List<MmReceivableDto> BuildReceivables()
    {
        var result = new List<MmReceivableDto>();

        foreach (var index in Enumerable.Range(1, 20))
        {
            var localNumber = ((index - 1) % 10) + 1;
            var local = locals[localNumber - 1];
            var tenant = tenants[localNumber - 1];

            result.Add(new MmReceivableDto
            {
                Id = $"REC-{index:000}",
                LocalId = local.Id,
                EmissionNumber = $"EM-{((index - 1) % 6) + 1:000}",
                FiscalYear = DateTime.Today.Year,
                Month = ((index - 1) % 12) + 1,
                LocalAccount = local.AccountNumber,
                LocalType = local.LocalType,
                BusinessActivity = local.BusinessActivity,
                TaxpayerName = tenant.Name,
                TaxCode = index % 2 == 0 ? "Canon mercado" : "Alquiler local",
                SubTaxCode = index % 3 == 0 ? "Recargo administrativo" : "Rubro principal",
                Amount = 8500m + (index * 300m),
                Status = index % 5 == 0 ? "Pendiente" : index % 4 == 0 ? "Moroso" : "Generada",
                CreatedAt = DateTime.Today.AddDays(-(index + 2)),
                DueDate = DateTime.Today.AddDays(10 - index)
            });
        }

        return result;
    }

    private static List<MmEmissionDto> BuildEmissions()
    {
        var types = new[] { "Mensual", "Anual", "Manual", "Programada", "Mensual", "Anual" };
        var statuses = new[] { "Creada", "En control de calidad", "Aprobada", "Emitida mock", "Rechazada", "Aprobada" };

        return Enumerable.Range(1, 6).Select(index =>
        {
            var local = locals[index - 1];
            var rent = rents[index - 1];

            return new MmEmissionDto
            {
                Id = $"EMS-{index:000}",
                LocalId = local.Id,
                EmissionNumber = $"EM-{index:000}",
                FiscalYear = DateTime.Today.Year,
                Month = index,
                EmissionType = types[index - 1],
                LocalAccount = local.AccountNumber,
                LocalType = local.LocalType,
                TotalAmount = Math.Round(local.RentAmount + (index * 25m), 2),
                BusinessActivity = local.BusinessActivity,
                RentStatus = rent.IsSuspended ? "Cobro suspendido" : "Alquiler activo",
                Location = local.Location,
                Aisle = local.Aisle,
                WeightingFactor = local.WeightingFactor,
                AreaM2 = local.AreaM2,
                ProposedPrice = local.ProposedPrice,
                IncludedLocals = 8 + index,
                ExcludedLocals = index % 3,
                Inconsistencies = index % 2 == 0 ? "Sin inconsistencias" : "Revisión de período duplicado pendiente",
                QualityControlStatus = index % 2 == 0 ? "Control superado" : "Pendiente de validación",
                Status = statuses[index - 1],
                AutomaticEnabled = index % 2 == 0,
                ScheduledRule = index % 2 == 0 ? "Último día hábil del mes" : "Generación manual referencial",
                CreatedAt = DateTime.Today.AddDays(-(index * 3))
            };
        }).ToList();
    }

    private static List<MmPaymentMockDto> BuildPayments()
    {
        var methods = new[] { "Caja", "Banco", "Conectividad", "MIMUNIENCASA" };

        return Enumerable.Range(1, 10).Select(index =>
        {
            var localNumber = ((index - 1) % 10) + 1;
            var local = locals[localNumber - 1];

            return new MmPaymentMockDto
            {
                Id = $"PAY-{index:000}",
                LocalId = local.Id,
                LocalAccount = local.AccountNumber,
                PaymentDate = DateTime.Today.AddDays(-(index * 4)),
                Amount = 4500m + (index * 280m),
                AppliedAmount = 3000m + (index * 190m),
                Balance = Math.Max(0m, 1200m - (index * 35m)),
                CollectorEntity = index % 2 == 0 ? "Caja" : "Banco Nacional",
                PaymentMethod = methods[(index - 1) % methods.Length],
                ReceiptNumber = $"REC-{index:06}",
                Status = index % 4 == 0 ? "Reversado" : index % 3 == 0 ? "Pendiente" : "Aplicado",
                PaymentArrangement = index % 3 == 0 ? "Arreglo de pago referencial" : "Sin arreglo",
                ModifiedByUser = index % 2 == 0 ? "Tesorería Mock" : "Caja Mercado"
            };
        }).ToList();
    }

    private static List<MmCollectionDto> BuildCollections()
    {
        var statuses = new[] { "Al día", "Moroso", "En cobro administrativo", "Arreglo de pago", "Aplazamiento", "Cobro judicial" };
        var stages = new[] { "Seguimiento preventivo", "Cobro ordinario", "Cobro administrativo", "Convenio de pago", "Aplazamiento aprobado", "Cobro judicial referencial" };

        return Enumerable.Range(1, 6).Select(index => new MmCollectionDto
        {
            Id = $"COL-{index:000}",
            LocalId = $"LOC-{index:000}",
            PendingMonths = index + 1,
            OverdueBalance = 2000m + (index * 600m),
            Status = statuses[index - 1],
            LastAction = index % 2 == 0 ? "Envío a cobro administrativo" : "Seguimiento telefónico y recordatorio",
            ResponsibleUser = index % 2 == 0 ? "Analista Cobro" : "Coordinación Mercado",
            LastManagementDate = DateTime.Today.AddDays(-(index * 5)),
            AdministrativeStage = stages[index - 1]
        }).ToList();
    }

    private static List<MmHistoryEventDto> BuildHistory()
    {
        var origins = new[]
        {
            "Mercado",
            "Plataforma de Servicios",
            "RUC",
            "Patentes",
            "Comercial",
            "Cobro",
            "Cuenta Tributaria",
            "Tesorería",
            "GIS",
            "Notificaciones",
            "Sistema"
        };

        var documents = new[]
        {
            "Expediente físico",
            "Contrato referencial",
            "Boleta de recepción",
            "Acta de inspección",
            "Memo interno",
            "Pantalla de consulta",
            "Resolución mock",
            "Control de cobro mock"
        };

        var actions = new[]
        {
            "Creación de local",
            "Modificación de local",
            "Asignación de inquilino",
            "Creación de contrato",
            "Renovación de contrato",
            "Modificación de alquiler",
            "Consulta de expediente",
            "Registro de cuenta por cobrar",
            "Simulación de pago",
            "Registro de traspaso",
            "Registro de amonestación",
            "Registro de sanción",
            "Programación de inspección",
            "Notificación simulada",
            "Gestión de correspondencia"
        };

        return Enumerable.Range(1, 34).Select(index => new MmHistoryEventDto
        {
            Id = $"HIS-{index:000}",
            LocalId = $"LOC-{((index - 1) % 15) + 1:000}",
            Date = DateTime.Today.AddDays(-(index * 2)).AddHours((index % 8) + 7),
            User = index % 2 == 0 ? "Usuario Mercado" : "Sistema",
            Action = actions[(index - 1) % actions.Length],
            PreviousStatus = index % 2 == 0 ? "Borrador" : "Registrado",
            NewStatus = index % 2 == 0 ? "En revisión" : "Activo",
            PreviousValue = $"Valor anterior {index}",
            NewValue = $"Valor actual {index}",
            SourceDocument = documents[(index - 1) % documents.Length],
            SourceProcess = origins[(index - 1) % origins.Length],
            Comment = "Evento mock para trazabilidad del expediente de mercado."
        }).ToList();
    }

    private static List<MmAuditEventDto> BuildAudits()
    {
        var actions = new[]
        {
            "Creación de local",
            "Modificación de local",
            "Asignación de inquilino",
            "Creación de contrato",
            "Renovación",
            "Modificación de alquiler",
            "Consulta de expediente",
            "Aplicación de cobro administrativo",
            "Registro de modificación",
            "Aplicación de traspaso",
            "Registro de amonestación",
            "Registro de sanción",
            "Configuración referencial",
            "Firma digital mock",
            "Reporte mock exportado"
        };

        return Enumerable.Range(1, 30).Select(index => new MmAuditEventDto
        {
            Id = $"AUD-{index:000}",
            LocalId = $"LOC-{((index - 1) % 15) + 1:000}",
            Date = DateTime.Today.AddDays(-(index * 3)).AddHours((index % 9) + 8).AddMinutes(index * 2),
            User = index % 2 == 0 ? "Auditor Mercado" : "Sistema",
            Action = actions[(index - 1) % actions.Length],
            PreviousValue = $"Anterior {index}",
            CurrentValue = $"Actual {index}",
            SourceDocument = index % 2 == 0 ? "Expediente" : "Trámite",
            Process = "Mercado Municipal",
            Severity = index % 4 == 0 ? "Alta" : index % 3 == 0 ? "Media" : "Baja",
            OutputReference = index % 2 == 0 ? $"Reporte AUD-{index:000}" : $"Bitácora MM-{index:000}"
        }).ToList();
    }

    private static List<MmModificationDto> BuildModifications()
    {
        var movementTypes = new[]
        {
            "Agregar cuentas por cobrar",
            "Eliminar cuentas por cobrar",
            "Modificar cuentas por cobrar",
            "Traspaso de derecho de piso",
            "Cambio de actividad comercial",
            "Cambio de imagen del padrón fotográfico",
            "Cambio de estado del local",
            "Reasignación de local"
        };

        var results = new[] { "Aprobado", "Rechazado", "Pendiente" };

        return Enumerable.Range(1, 8).Select(index =>
        {
            var local = locals[index - 1];
            return new MmModificationDto
            {
                Id = $"MOD-{index:000}",
                LocalId = local.Id,
                RequestNumber = $"MM-MOD-{DateTime.Today.Year}-{index:000}",
                AccountNumber = local.AccountNumber,
                MovementType = movementTypes[index - 1],
                Justification = "Ajuste referencial del expediente para control administrativo mock.",
                Status = index % 3 == 0 ? "En análisis" : index % 2 == 0 ? "Aplicada" : "Registrada",
                PreviousValue = $"Valor previo {index}",
                NewValue = $"Valor actualizado {index}",
                Date = DateTime.Today.AddDays(-(index * 4)),
                User = index % 2 == 0 ? "Analista Mercado" : "Coordinación Mercado",
                RequiresInspection = index % 2 == 0,
                RequiresCouncilApproval = index % 3 == 0,
                Result = results[(index - 1) % results.Length],
                CommercialImpact = index % 2 == 0 ? "Actualizar actividad" : "Sin impacto",
                GisImpact = index % 3 == 0 ? "Actualizar localización" : "Sin impacto",
                FiscalizationImpact = index % 2 == 0 ? "Programar revisión" : "Sin impacto",
                CollectionImpact = index % 4 == 0 ? "Recalcular cobro" : "Sin impacto",
                TaxAccountImpact = index % 2 == 0 ? "Revisar cuenta" : "Sin impacto",
                NotificationImpact = index % 3 == 0 ? "Generar aviso" : "Sin impacto"
            };
        }).ToList();
    }

    private static List<MmTransferDto> BuildTransfers()
    {
        var statuses = new[] { "Registrado", "En análisis", "Pendiente Concejo", "Aprobado", "Aplicado" };

        return Enumerable.Range(1, 5).Select(index =>
        {
            var local = locals[index + 2];
            var currentTenant = tenants[index + 1];
            var previousTenant = tenants[Math.Max(0, index - 1)];
            var newTenant = tenants[index + 5];

            return new MmTransferDto
            {
                Id = $"TRA-{index:000}",
                LocalId = local.Id,
                RequestNumber = $"MM-TRA-{DateTime.Today.Year}-{index:000}",
                LocalNumber = local.LocalNumber,
                LocalAccount = local.AccountNumber,
                CurrentTaxpayer = currentTenant.Name,
                PreviousTaxpayer = previousTenant.Name,
                NewTaxpayer = newTenant.Name,
                RequestDate = DateTime.Today.AddDays(-(index * 7)),
                Status = statuses[index - 1],
                CouncilAgreementMock = index >= 3 ? $"Acuerdo CM-{index:000}" : "Pendiente de acuerdo",
                ThirdPartyReceiptMock = $"Recibo tercero RT-{index:000}",
                ReceivablesUpdateMock = index % 2 == 0 ? "Actualización mock generada" : "Pendiente de actualización",
                OwnershipHistory = $"Titular anterior: {previousTenant.Name} / Titular actual: {currentTenant.Name}"
            };
        }).ToList();
    }

    private static List<MmMergerSegregationDto> BuildMergersSegregations()
    {
        return new List<MmMergerSegregationDto>
        {
            new()
            {
                Id = "FSG-001",
                LocalId = "LOC-001",
                Type = "Fusión",
                SourceLocal = "F-001",
                TargetLocal = "F-002",
                PreviousArea = 19.00m,
                NewArea = 31.50m,
                AffectedReceivables = 2,
                RequiresCouncilAgreement = true,
                Status = "En análisis"
            },
            new()
            {
                Id = "FSG-002",
                LocalId = "LOC-004",
                Type = "Segregación",
                SourceLocal = "F-004",
                TargetLocal = "F-004A / F-004B",
                PreviousArea = 24.00m,
                NewArea = 12.00m,
                AffectedReceivables = 1,
                RequiresCouncilAgreement = true,
                Status = "Registrada"
            },
            new()
            {
                Id = "FSG-003",
                LocalId = "LOC-006",
                Type = "Fusión",
                SourceLocal = "F-006",
                TargetLocal = "F-007",
                PreviousArea = 34.00m,
                NewArea = 42.50m,
                AffectedReceivables = 3,
                RequiresCouncilAgreement = false,
                Status = "Aprobada"
            }
        };
    }

    private static List<MmWarningDto> BuildWarnings()
    {
        var warningTypes = new[]
        {
            "Incumplimiento de limpieza",
            "Uso no autorizado",
            "Mora en canon",
            "Falta de identificación",
            "Incumplimiento sanitario",
            "Cierre fuera de horario"
        };

        return Enumerable.Range(1, 6).Select(index =>
        {
            var local = locals[index - 1];
            var tenant = tenants[index - 1];
            return new MmWarningDto
            {
                Id = $"WAR-{index:000}",
                LocalId = local.Id,
                TenantId = tenant.Id,
                WarningNumber = $"AMO-{DateTime.Today.Year}-{index:000}",
                LocalNumber = local.LocalNumber,
                TenantName = tenant.Name,
                WarningType = warningTypes[index - 1],
                AppliedDate = DateTime.Today.AddDays(-(index * 6)),
                ComplianceDeadline = DateTime.Today.AddDays(5 + index),
                Status = index % 3 == 0 ? "Cumplida" : index % 2 == 0 ? "Notificada" : "Registrada",
                DocumentMock = $"Amonestacion_{index:000}.pdf",
                NotificationMock = index % 2 == 0 ? "Notificación enviada" : "Pendiente de notificación"
            };
        }).ToList();
    }

    private static List<MmSanctionDto> BuildSanctions()
    {
        var sanctionTypes = new[]
        {
            "Multa referencial",
            "Suspensión temporal",
            "Clausura administrativa mock",
            "Cobro adicional referencial"
        };

        return Enumerable.Range(1, 4).Select(index =>
        {
            var local = locals[index];
            var tenant = tenants[index];
            return new MmSanctionDto
            {
                Id = $"SAN-{index:000}",
                LocalId = local.Id,
                TenantId = tenant.Id,
                SanctionNumber = $"SNC-{DateTime.Today.Year}-{index:000}",
                LocalNumber = local.LocalNumber,
                TenantName = tenant.Name,
                SanctionType = sanctionTypes[index - 1],
                AppliedDate = DateTime.Today.AddDays(-(index * 9)),
                Deadline = DateTime.Today.AddDays(index + 7),
                Status = index % 2 == 0 ? "En seguimiento" : "Registrada",
                RelatedWarningNumber = $"AMO-{DateTime.Today.Year}-{index:000}",
                DocumentMock = $"Resolucion_Sancion_{index:000}.pdf",
                NotificationMock = index % 2 == 0 ? "Notificada mock" : "Pendiente mock"
            };
        }).ToList();
    }

    private static List<MmRemodelingPermitDto> BuildRemodelingPermits()
    {
        var types = new[] { "Mejora interna", "Cambio de fachada", "Adecuación sanitaria", "Reordenamiento de espacio" };

        return Enumerable.Range(1, 4).Select(index =>
        {
            var local = locals[index + 1];
            return new MmRemodelingPermitDto
            {
                Id = $"RMD-{index:000}",
                LocalId = local.Id,
                ManagementNumber = $"RMD-GST-{DateTime.Today.Year}-{index:000}",
                LocalNumber = local.LocalNumber,
                RemodelingType = types[index - 1],
                RequestDate = DateTime.Today.AddDays(-(index * 8)),
                ExecutionDate = DateTime.Today.AddDays(index * 4),
                AuthorizedSchedule = index % 2 == 0 ? "18:00 a 22:00" : "06:00 a 10:00",
                Status = index % 3 == 0 ? "Rechazada" : index % 2 == 0 ? "Aprobada" : "En revisión",
                UrbanismTask = index % 2 == 0 ? "Tarea Urbanismo generada" : "Pendiente Urbanismo",
                MayorOfficeTask = index % 2 == 0 ? "Visto bueno Alcaldía" : "Pendiente Alcaldía",
                DocumentMock = $"Permiso_Remodelacion_{index:000}.pdf",
                NotificationMock = "Notificación referencial"
            };
        }).ToList();
    }

    private static List<MmSpecialScheduleDto> BuildSpecialSchedules()
    {
        var permissionTypes = new[] { "Horario extraordinario", "Apertura temprana", "Cierre extendido", "Horario especial por mejoras" };
        var reasons = new[] { "Temporada especial", "Trabajos particulares", "Mejoras", "Otro" };

        return Enumerable.Range(1, 4).Select(index =>
        {
            var local = locals[index + 2];
            var tenant = tenants[index + 2];
            return new MmSpecialScheduleDto
            {
                Id = $"SCH-{index:000}",
                LocalId = local.Id,
                TenantId = tenant.Id,
                PermissionType = permissionTypes[index - 1],
                LocalNumber = local.LocalNumber,
                TenantName = tenant.Name,
                StartDate = DateTime.Today.AddDays(index),
                EndDate = DateTime.Today.AddDays(index + 15),
                Schedule = index % 2 == 0 ? "05:00 a 20:00" : "06:00 a 22:00",
                Reason = reasons[index - 1],
                Status = index % 2 == 0 ? "Aprobado" : "Registrado",
                DocumentMock = $"Horario_Especial_{index:000}.pdf",
                NotificationMock = index % 2 == 0 ? "Notificada mock" : "Pendiente"
            };
        }).ToList();
    }

    private static List<MmCardDto> BuildCards()
    {
        return Enumerable.Range(1, 8).Select(index =>
        {
            var local = locals[index - 1];
            var tenant = tenants[index - 1];
            return new MmCardDto
            {
                Id = $"CRD-{index:000}",
                LocalId = local.Id,
                TenantId = tenant.Id,
                CardNumber = $"CAR-{DateTime.Today.Year}-{index:000}",
                Name = index % 2 == 0 ? $"Colaborador {index:00}" : tenant.Name,
                Identification = index % 2 == 0 ? $"2-0{index:03}-9{index:03}" : tenant.Identification,
                Condition = index % 2 == 0 ? "Colaborador" : "Inquilino",
                PhotoPlaceholder = $"https://placehold.co/140x180?text=Carn%C3%A9+{index:00}",
                IssueDate = DateTime.Today.AddMonths(-index),
                ExpirationDate = DateTime.Today.AddMonths(12 - index),
                Status = index % 4 == 0 ? "Vencido" : index % 3 == 0 ? "Próximo a vencer" : index % 5 == 0 ? "Inactivo" : "Vigente"
            };
        }).ToList();
    }

    private static List<MmInspectionDto> BuildInspections()
    {
        var reasons = new[]
        {
            "Estado del local",
            "Actividad no autorizada",
            "Morosidad",
            "Queja",
            "Mantenimiento",
            "Verificación contractual"
        };

        var results = new[]
        {
            "Conforme",
            "Requiere corrección",
            "Genera amonestación",
            "Genera sanción",
            "Requiere mantenimiento",
            "Conforme"
        };

        return Enumerable.Range(1, 6).Select(index =>
        {
            var local = locals[index - 1];
            var tenant = tenants[index - 1];
            return new MmInspectionDto
            {
                Id = $"INSP-{index:000}",
                LocalId = local.Id,
                TenantId = tenant.Id,
                InspectionNumber = $"INSP-MM-{DateTime.Today.Year}-{index:000}",
                LocalNumber = local.LocalNumber,
                TenantName = tenant.Name,
                Inspector = index % 2 == 0 ? "Inspector Mercado A" : "Inspector Mercado B",
                Date = DateTime.Today.AddDays(-(index * 5)),
                Reason = reasons[index - 1],
                Status = index % 5 == 0 ? "Finalizada" : index % 4 == 0 ? "Observada" : index % 3 == 0 ? "Realizada" : index % 2 == 0 ? "Asignada" : "Solicitada",
                Result = results[index - 1],
                EvidencePlaceholder = $"https://placehold.co/320x180?text=Evidencia+{index:00}"
            };
        }).ToList();
    }

    private static List<MmCorrespondenceDto> BuildCorrespondences()
    {
        var channels = new[] { "Plataforma de Servicios", "Trámite no presencial", "Presencial", "MIMUNIENCASA" };
        var types = new[] { "Reclamo", "Consulta", "Sugerencia", "Solicitud", "Proceso legal", "Correspondencia interna", "Correspondencia externa", "Solicitud" };

        return Enumerable.Range(1, 8).Select(index =>
        {
            var local = locals[index - 1];
            var tenant = tenants[index - 1];
            return new MmCorrespondenceDto
            {
                Id = $"COR-{index:000}",
                LocalId = local.Id,
                ProcedureNumber = $"TRM-{DateTime.Today.Year}-{index:000}",
                FileNumber = $"EXP-MM-{local.LocalNumber}-{index:000}",
                Channel = channels[(index - 1) % channels.Length],
                Type = types[index - 1],
                Applicant = tenant.Name,
                RelatedLocal = local.LocalNumber,
                Status = index % 3 == 0 ? "Resuelto" : index % 2 == 0 ? "En trámite" : "Ingresado",
                DueDate = DateTime.Today.AddDays(index + 6),
                EntryDate = DateTime.Today.AddDays(-(index * 3)),
                ResponseDate = index % 3 == 0 ? DateTime.Today.AddDays(-(index - 1)) : null,
                ResolutionMock = index % 3 == 0 ? $"Resolución TRM-{index:000}" : "Pendiente de resolución",
                AttachmentPlaceholder = $"Adjunto_{index:000}.zip",
                Responsible = index % 2 == 0 ? "Gestión Documental" : "Coordinación Mercado"
            };
        }).ToList();
    }

    private static List<MmNotificationDto> BuildNotifications()
    {
        var types = new[] { "Informativa", "Vencimiento de contrato", "Morosidad", "Amonestación", "Sanción", "Resolución", "Traspaso", "Emisión", "Cobro", "Inspección" };
        var media = new[] { "Correo", "SMS", "Domicilio fiscal", "Plataforma digital", "Impresión" };

        return Enumerable.Range(1, 10).Select(index =>
        {
            var local = locals[(index - 1) % 10];
            var tenant = tenants[(index - 1) % 10];
            return new MmNotificationDto
            {
                Id = $"NTF-{index:000}",
                LocalId = local.Id,
                NotificationNumber = $"NOT-{DateTime.Today.Year}-{index:000}",
                Type = types[index - 1],
                Medium = media[(index - 1) % media.Length],
                Status = index % 5 == 0 ? "Impresa" : index % 4 == 0 ? "Fallida" : index % 3 == 0 ? "Pendiente" : index % 2 == 0 ? "Enviada" : "Generada",
                DigitalSignatureReference = index % 2 == 0 ? "Firma referencial disponible" : "Pendiente firma mock",
                CreatedAt = DateTime.Today.AddDays(-(index * 2)),
                TargetName = tenant.Name
            };
        }).ToList();
    }

    private static List<MmReportDto> BuildReports()
    {
        var names = new[]
        {
            "Padrón de inquilinos con fotografía",
            "Locales por estado",
            "Locales fijos",
            "Locales ocasionales",
            "Derechos de piso",
            "Locales por tipo de actividad",
            "Locales por tipo de inquilino",
            "Datos de locales",
            "Emisiones realizadas",
            "Monto total por emisión",
            "Pendientes de pago",
            "Histórico de pagos por cuenta",
            "Arreglos de pago referenciales",
            "Modificaciones por cuenta",
            "Traspasos históricos por local",
            "Amonestaciones y sanciones por cuenta",
            "Reporte de inspecciones",
            "Reporte de notificaciones"
        };

        return names.Select((name, index) => new MmReportDto
        {
            Id = $"RPT-{index + 1:000}",
            Name = name,
            Description = $"Consulta mock para {name.ToLowerInvariant()}.",
            Filters = index % 2 == 0 ? "Período, estado, local, tipo" : "Fecha, local, cuenta, responsable",
            Category = index % 3 == 0 ? "Cobro" : index % 2 == 0 ? "Operación" : "Control"
        }).ToList();
    }

    private static List<MmCatalogItemDto> BuildCatalogs()
    {
        var items = new List<MmCatalogItemDto>();

        var groups = new[]
        {
            ("CFG01", "Catálogo", "Tipos de local"),
            ("CFG02", "Catálogo", "Locales fijos"),
            ("CFG03", "Catálogo", "Locales ocasionales"),
            ("CFG04", "Catálogo", "Derechos de piso"),
            ("CFG05", "Catálogo", "Pisos"),
            ("CFG06", "Catálogo", "Zonas"),
            ("CFG07", "Catálogo", "Sectores"),
            ("CFG08", "Catálogo", "Pasillos"),
            ("CFG09", "Catálogo", "Estados de local"),
            ("CFG10", "Catálogo", "Estados de contrato"),
            ("CFG11", "Catálogo", "Tipos de contrato"),
            ("CFG12", "Catálogo", "Tipos de alquiler"),
            ("CFG13", "Catálogo", "Actividades comerciales"),
            ("CFG14", "Catálogo", "Tipos de inquilino"),
            ("CFG15", "Parámetro", "Factores de ponderación"),
            ("CFG16", "Parámetro", "Valores por metro cuadrado"),
            ("CFG17", "Parámetro", "Parámetros de emisión mensual"),
            ("CFG18", "Parámetro", "Parámetros de emisión anual"),
            ("CFG19", "Parámetro", "Parámetros de emisión automática"),
            ("CFG20", "Catálogo", "Tipos de modificación"),
            ("CFG21", "Catálogo", "Tipos de traspaso"),
            ("CFG22", "Catálogo", "Tipos de fusión/segregación"),
            ("CFG23", "Catálogo", "Motivos de amonestación"),
            ("CFG24", "Catálogo", "Tipos de sanción"),
            ("CFG25", "Catálogo", "Motivos de sanción"),
            ("CFG26", "Catálogo", "Tipos de inspección"),
            ("CFG27", "Catálogo", "Tipos de remodelación"),
            ("CFG28", "Catálogo", "Tipos de horario especial"),
            ("CFG29", "Catálogo", "Tipos de carné"),
            ("CFG30", "Catálogo", "Tipos de notificación"),
            ("CFG31", "Catálogo", "Responsables"),
            ("CFG32", "Catálogo", "Inspectores"),
            ("CFG33", "Catálogo", "Equipos de trabajo"),
            ("CFG34", "Catálogo", "Estados de cobro"),
            ("CFG35", "Catálogo", "Estados de cuenta")
        };

        foreach (var (code, group, name) in groups)
        {
            items.Add(new MmCatalogItemDto
            {
                Id = $"{code}-{name.Replace(" ", string.Empty)}",
                Code = code,
                Name = name,
                Group = group,
                Status = "Activo"
            });
        }

        return items;
    }

    private static List<MmIntegrationStatusDto> BuildIntegrations()
    {
        var names = new[]
        {
            "Plataforma de Servicios",
            "Registro Único de Contribuyentes",
            "Comercial",
            "Patentes",
            "Cobro Administrativo",
            "Conectividad",
            "Tesorería",
            "Cajas",
            "Reportes",
            "Control de Intereses",
            "Cuenta Tributaria",
            "GIS",
            "Compensación",
            "Notificaciones",
            "Seguridad",
            "Bitácoras / Históricos",
            "Contabilidad referencial",
            "Dirección de Urbanismo",
            "Fiscalización Tributaria"
        };

        var statuses = new[] { "Operativo", "Configurado", "Preparado", "No disponible" };

        return names.Select((name, index) => new MmIntegrationStatusDto
        {
            Id = $"INT-{index + 1:000}",
            Name = name,
            Status = statuses[index % statuses.Length],
            LastEvent = index % 2 == 0 ? "Sincronización mock exitosa" : "Evento pendiente de backend",
            Description = "Integración referencial sin consumo externo."
        }).ToList();
    }
}
