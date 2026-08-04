using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlazorApp.Helpers;
using BlazorApp.Models.Pagos;

namespace BlazorApp.Services.Pagos;

public class PagosMockService
{
    private static readonly object Gate = new();
    private static bool seeded;
    private static List<PaymentTypeDto> paymentTypes = new();
    private static List<MunicipalAccountDto> accounts = new();
    private static List<PaymentTransactionDto> transactions = new();
    private static List<LinkedAccountDto> linkedAccounts = new();
    private static List<PaymentHistoryEventDto> history = new();

    private readonly LocalStorageHelper _localStorage;
    private readonly IMaestroDatosService _maestroDatos;
    
    private const string ACCOUNTS_KEY = "coremunicipal.pagos.cuentas";
    private const string TRANSACTIONS_KEY = "coremunicipal.pagos.transacciones";
    private const string LINKED_ACCOUNTS_KEY = "coremunicipal.pagos.cuentas-vinculadas";
    private const string HISTORY_KEY = "coremunicipal.pagos.historial";

    public PagosMockService(LocalStorageHelper localStorage, IMaestroDatosService maestroDatos)
    {
        _localStorage = localStorage;
        _maestroDatos = maestroDatos;
        EnsureSeeded();
    }

    public Task<List<PaymentTypeDto>> GetPaymentTypesAsync() => Task.FromResult(paymentTypes.ToList());

    public async Task<List<MunicipalAccountDto>> GetAccountsAsync()
    {
        var stored = await _localStorage.GetItemAsync<List<MunicipalAccountDto>>(ACCOUNTS_KEY);
        if (stored != null && stored.Any())
        {
            accounts = stored;
        }
        return accounts.ToList();
    }

    public async Task<MunicipalAccountDto?> GetAccountByNumberAsync(string accountNumber)
    {
        await GetAccountsAsync();
        return accounts.FirstOrDefault(a => IdentificationHelper.AccountNumberMatches(a.AccountNumber, accountNumber));
    }

    public async Task<MunicipalAccountDto?> GetAccountByContributorIdAsync(string contributorId)
    {
        await GetAccountsAsync();
        return accounts.FirstOrDefault(a => IdentificationHelper.IdentificationMatches(a.ContributorId, contributorId));
    }

    public async Task<(bool ExistsInRuc, string ContributorName, MunicipalAccountDto? Account)> SearchByIdentificationAsync(string identification)
    {
        await GetAccountsAsync();
        
        var account = accounts.FirstOrDefault(a => IdentificationHelper.IdentificationMatches(a.ContributorId, identification));
        
        if (account != null)
        {
            return (true, account.ContributorName, account);
        }

        return (false, string.Empty, null);
    }

    public async Task<List<PaymentTransactionDto>> GetTransactionsAsync()
    {
        var stored = await _localStorage.GetItemAsync<List<PaymentTransactionDto>>(TRANSACTIONS_KEY);
        if (stored != null && stored.Any())
        {
            transactions = stored;
        }
        return transactions.OrderByDescending(t => t.PaymentDate).ToList();
    }

    public async Task<PaymentTransactionDto?> GetTransactionByReceiptAsync(string receiptNumber)
    {
        await GetTransactionsAsync();
        var transaction = transactions.FirstOrDefault(t => string.Equals(t.ReceiptNumber, receiptNumber, StringComparison.OrdinalIgnoreCase));
        return transaction;
    }

    public async Task<PaymentTransactionDto?> RegisterReceiptReprintAsync(string receiptNumber, string action, string user)
    {
        await GetTransactionsAsync();
        
        var transaction = transactions.FirstOrDefault(t => string.Equals(t.ReceiptNumber, receiptNumber, StringComparison.OrdinalIgnoreCase));
        if (transaction == null)
        {
            return null;
        }

        lock (Gate)
        {
            transaction.ReprintCount++;
            transaction.LastReprintedAt = DateTime.Now;

            _ = SaveTransactionsAsync();

            _ = AddHistoryEventAsync(new PaymentHistoryEventDto
            {
                User = user,
                Action = "Comprobante reimpreso",
                Description = $"Comprobante {transaction.ReceiptNumber} reimpreso",
                Reference = transaction.ReceiptNumber,
                Amount = transaction.TotalAmount,
                Observation = $"Tipo: {action}"
            });

            return transaction;
        }
    }

    public async Task<List<LinkedAccountDto>> GetLinkedAccountsAsync()
    {
        var stored = await _localStorage.GetItemAsync<List<LinkedAccountDto>>(LINKED_ACCOUNTS_KEY);
        if (stored != null && stored.Any())
        {
            linkedAccounts = stored;
        }
        return linkedAccounts.OrderByDescending(a => a.IsDefault).ThenByDescending(a => a.LinkedDate).ToList();
    }

    public async Task<List<PaymentHistoryEventDto>> GetHistoryAsync()
    {
        var stored = await _localStorage.GetItemAsync<List<PaymentHistoryEventDto>>(HISTORY_KEY);
        if (stored != null && stored.Any())
        {
            history = stored;
        }
        return history.OrderByDescending(h => h.Date).ToList();
    }

    public async Task<PaymentTransactionDto> RegisterPaymentAsync(PaymentTransactionDto transaction)
    {
        await GetTransactionsAsync();
        await GetAccountsAsync();

        lock (Gate)
        {
            if (string.IsNullOrWhiteSpace(transaction.Id))
            {
                transaction.Id = Guid.NewGuid().ToString("N");
            }

            transaction.ReceiptNumber = string.IsNullOrWhiteSpace(transaction.ReceiptNumber)
                ? $"REC-{DateTime.Now:yyyy}-{transactions.Count + 1:00000}"
                : transaction.ReceiptNumber;

            transaction.PaymentDate = transaction.PaymentDate == default ? DateTime.Now : transaction.PaymentDate;
            transaction.Status = string.IsNullOrWhiteSpace(transaction.Status) ? "Completado" : transaction.Status;
            transaction.ReprintCount = 0;

            var account = accounts.FirstOrDefault(a => a.AccountNumber == transaction.AccountNumber);
            if (account != null)
            {
                var previousBalance = account.CurrentBalance;

                foreach (var item in transaction.Items)
                {
                    var obligation = account.Obligations.FirstOrDefault(o => o.Id == item.ObligationId);
                    if (obligation != null)
                    {
                        obligation.PaidAmount += item.Amount;
                        obligation.PendingAmount = Math.Max(0, obligation.TotalAmount - obligation.PaidAmount);
                        if (obligation.PendingAmount == 0)
                        {
                            obligation.Status = "Pagado";
                        }
                        else if (obligation.PaidAmount > 0)
                        {
                            obligation.Status = "Pago parcial";
                        }
                    }
                }

                account.PendingAmount = account.Obligations.Sum(o => o.PendingAmount);
                account.OverdueAmount = account.Obligations.Where(o => o.IsOverdue && o.PendingAmount > 0).Sum(o => o.PendingAmount);
                account.CurrentBalance = account.PendingAmount;
                account.LastPaymentDate = DateTime.Now;

                var movement = new AccountMovementDto
                {
                    Date = transaction.PaymentDate,
                    Type = "Pago",
                    Description = $"Pago recibido - {transaction.PaymentMethod}",
                    Reference = transaction.ReceiptNumber,
                    Debit = 0,
                    Credit = transaction.TotalAmount,
                    Balance = account.CurrentBalance,
                    Status = "Registrado"
                };

                account.Movements.Insert(0, movement);

                _ = _localStorage.SetItemAsync(ACCOUNTS_KEY, accounts);
            }

            transactions.Insert(0, transaction);
            _ = _localStorage.SetItemAsync(TRANSACTIONS_KEY, transactions);

            _ = AddHistoryEventAsync(new PaymentHistoryEventDto
            {
                Action = "Pago completado",
                Description = $"Pago registrado - Comprobante {transaction.ReceiptNumber}",
                Reference = transaction.ReceiptNumber,
                Amount = transaction.TotalAmount,
                Observation = $"Método: {transaction.PaymentMethod}"
            });

            return transaction;
        }
    }

    public async Task<LinkedAccountDto> LinkAccountAsync(LinkedAccountDto linkedAccount)
    {
        await GetLinkedAccountsAsync();

        lock (Gate)
        {
            if (string.IsNullOrWhiteSpace(linkedAccount.Id))
            {
                linkedAccount.Id = Guid.NewGuid().ToString("N");
            }

            linkedAccount.LinkedDate = linkedAccount.LinkedDate == default ? DateTime.Now : linkedAccount.LinkedDate;

            if (linkedAccount.IsDefault)
            {
                foreach (var acc in linkedAccounts)
                {
                    acc.IsDefault = false;
                }
            }

            var existingIndex = linkedAccounts.FindIndex(a => a.AccountNumber == linkedAccount.AccountNumber);
            if (existingIndex >= 0)
            {
                linkedAccounts[existingIndex] = linkedAccount;
            }
            else
            {
                linkedAccounts.Insert(0, linkedAccount);
            }

            _ = _localStorage.SetItemAsync(LINKED_ACCOUNTS_KEY, linkedAccounts);

            _ = AddHistoryEventAsync(new PaymentHistoryEventDto
            {
                Action = "Cuenta vinculada",
                Description = $"Cuenta {linkedAccount.AccountNumber} vinculada",
                Reference = linkedAccount.AccountNumber
            });

            return linkedAccount;
        }
    }

    public async Task<bool> UnlinkAccountAsync(string accountNumber)
    {
        await GetLinkedAccountsAsync();

        lock (Gate)
        {
            var removed = linkedAccounts.RemoveAll(a => a.AccountNumber == accountNumber);
            if (removed > 0)
            {
                _ = _localStorage.SetItemAsync(LINKED_ACCOUNTS_KEY, linkedAccounts);

                _ = AddHistoryEventAsync(new PaymentHistoryEventDto
                {
                    Action = "Cuenta desvinculada",
                    Description = $"Cuenta {accountNumber} desvinculada",
                    Reference = accountNumber
                });

                return true;
            }
            return false;
        }
    }

    private async Task SaveTransactionsAsync()
    {
        await _localStorage.SetItemAsync(TRANSACTIONS_KEY, transactions);
    }

    private async Task AddHistoryEventAsync(PaymentHistoryEventDto eventDto)
    {
        await GetHistoryAsync();
        history.Insert(0, eventDto);
        await _localStorage.SetItemAsync(HISTORY_KEY, history);
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

            paymentTypes = new List<PaymentTypeDto>
            {
                new() { Id = "1", Code = "PAT-TRI", Name = "Patentes municipales trimestral", Category = "Impuestos", Description = "Impuesto trimestral sobre actividades comerciales e industriales.", RequiresIdentification = true, RequiresAccount = true, AllowsPartialPayment = true, Department = "Patentes", Active = true },
                new() { Id = "2", Code = "PAT-ANU", Name = "Patentes municipales anual", Category = "Impuestos", Description = "Impuesto anual sobre actividades comerciales e industriales.", RequiresIdentification = true, RequiresAccount = true, AllowsPartialPayment = true, Department = "Patentes", Active = true },
                new() { Id = "3", Code = "BI-TRI", Name = "Bienes inmuebles trimestral", Category = "Impuestos", Description = "Impuesto trimestral sobre el valor de la propiedad inmueble.", RequiresIdentification = true, RequiresAccount = true, AllowsPartialPayment = true, Department = "Bienes Inmuebles", Active = true },
                new() { Id = "4", Code = "BI-ANU", Name = "Bienes inmuebles anual", Category = "Impuestos", Description = "Impuesto anual sobre el valor de la propiedad inmueble.", RequiresIdentification = true, RequiresAccount = true, AllowsPartialPayment = true, Department = "Bienes Inmuebles", Active = true },
                new() { Id = "5", Code = "ESP-PUB", Name = "Espectáculos públicos", Category = "Impuestos", Description = "Impuesto sobre espectáculos públicos y eventos.", RequiresIdentification = true, RequiresAccount = false, AllowsPartialPayment = false, Department = "Tesorería", Active = true },
                new() { Id = "6", Code = "EXT-MAT", Name = "Extracción de materiales", Category = "Impuestos", Description = "Impuesto sobre extracción de materiales y canteras.", RequiresIdentification = true, RequiresAccount = true, AllowsPartialPayment = true, Department = "Tesorería", Active = true },
                new() { Id = "7", Code = "SERV-MUN", Name = "Servicios municipales", Category = "Servicios", Description = "Tarifa por servicios municipales generales.", RequiresIdentification = true, RequiresAccount = true, AllowsPartialPayment = false, Department = "Tesorería", Active = true },
                new() { Id = "8", Code = "ALQ-MER", Name = "Alquiler de mercado", Category = "Servicios", Description = "Tarifa mensual por arrendamiento de locales en el mercado municipal.", RequiresIdentification = true, RequiresAccount = true, AllowsPartialPayment = false, Department = "Mercado", Active = true },
                new() { Id = "9", Code = "ALQ-MER-ANU", Name = "Alquiler de mercado anual", Category = "Servicios", Description = "Tarifa anual por arrendamiento de locales en el mercado municipal.", RequiresIdentification = true, RequiresAccount = true, AllowsPartialPayment = true, Department = "Mercado", Active = true },
                new() { Id = "10", Code = "AGUA", Name = "Agua potable", Category = "Servicios", Description = "Tarifa por consumo de agua potable.", RequiresIdentification = true, RequiresAccount = true, AllowsPartialPayment = false, Department = "Acueductos", Active = true },
                new() { Id = "11", Code = "ALC", Name = "Alcantarillado sanitario", Category = "Servicios", Description = "Tarifa por servicio de alcantarillado sanitario.", RequiresIdentification = true, RequiresAccount = true, AllowsPartialPayment = false, Department = "Acueductos", Active = true },
                new() { Id = "12", Code = "BAS", Name = "Recolección de residuos", Category = "Servicios", Description = "Tarifa por recolección y tratamiento de residuos sólidos.", RequiresIdentification = true, RequiresAccount = true, AllowsPartialPayment = false, Department = "Aseo de Vías", Active = true },
                new() { Id = "13", Code = "LIM", Name = "Limpieza de vías", Category = "Servicios", Description = "Tarifa para el mantenimiento y limpieza de vías públicas.", RequiresIdentification = true, RequiresAccount = true, AllowsPartialPayment = false, Department = "Aseo de Vías", Active = true },
                new() { Id = "14", Code = "PARQ", Name = "Mantenimiento de parques", Category = "Servicios", Description = "Tarifa para el mantenimiento de parques y zonas verdes.", RequiresIdentification = true, RequiresAccount = true, AllowsPartialPayment = false, Department = "Aseo de Vías", Active = true },
                new() { Id = "15", Code = "URB", Name = "Urbanismo", Category = "Servicios", Description = "Tarifas y derechos por servicios de urbanismo y zonificación.", RequiresIdentification = true, RequiresAccount = true, AllowsPartialPayment = true, Department = "Urbanismo", Active = true }
            };

            var contributorNames = new[]
            {
                "Comercial El Parque S.A.",
                "María Pérez López",
                "Constructora del Norte S.R.L.",
                "Inversiones del Valle S.A.",
                "Juan Carlos Ramírez Mora",
                "Mercados del Pacífico S.A.",
                "Servicios Urbanos del Este",
                "Rosa Vega Mena"
            };

            var contributorIds = new[]
            {
                "1-2345-6789",
                "1-1234-5678",
                "3-3333-4444",
                "4-5555-6666",
                "1-0987-6543",
                "6-9999-0000",
                "7-1212-3434",
                "1-5555-1111"
            };

            var addresses = new[]
            {
                "San José, Avenida Central, Calle 5",
                "Heredia, Residencial Los Laureles",
                "Alajuela, Barrio San José",
                "Cartago, Urbanización Valle Verde",
                "San José, Condominio Las Palmas",
                "Puntarenas, Barrio El Carmen",
                "Limón, Centro",
                "Guanacaste, Liberia Centro"
            };

            accounts = Enumerable.Range(1, 8).Select(index =>
            {
                var obligationsList = new List<AccountObligationDto>();
                var totalPending = 0m;
                var totalOverdue = 0m;

                var taxCodes = new[] { "PAT-TRI", "BI-TRI", "ESP-PUB" };
                foreach (var taxCode in taxCodes)
                {
                    var taxType = paymentTypes.First(pt => pt.Code == taxCode);
                    var amount = (index * 5000m) + (new Random(index + taxCode.GetHashCode()).Next(10000, 50000));
                    var dueDate = new DateTime(2026, (index % 3) + 1, 15);
                    var isOverdue = dueDate < DateTime.Now;

                    obligationsList.Add(new AccountObligationDto
                    {
                        AccountNumber = $"CTA-{index:0000}",
                        PaymentType = taxCode,
                        PaymentTypeName = taxType.Name,
                        Period = "2026-01",
                        TotalAmount = amount,
                        PendingAmount = amount,
                        PaidAmount = 0,
                        DueDate = dueDate,
                        Status = "Pendiente",
                        IsOverdue = isOverdue,
                        Reference = $"OBL-{taxCode}-{index:000}"
                    });

                    totalPending += amount;
                    if (isOverdue) totalOverdue += amount;
                }

                var serviceCodes = new[] { "AGUA", "ALC", "BAS", "LIM" };
                foreach (var svcCode in serviceCodes.Take(2 + (index % 2)))
                {
                    var svcType = paymentTypes.First(pt => pt.Code == svcCode);
                    var amount = (index * 2000m) + (new Random(index + svcCode.GetHashCode()).Next(5000, 15000));
                    var dueDate = new DateTime(2026, (index % 2) + 1, 10);
                    var isOverdue = dueDate < DateTime.Now;

                    obligationsList.Add(new AccountObligationDto
                    {
                        AccountNumber = $"CTA-{index:0000}",
                        PaymentType = svcCode,
                        PaymentTypeName = svcType.Name,
                        Period = "2026-01",
                        TotalAmount = amount,
                        PendingAmount = amount,
                        PaidAmount = 0,
                        DueDate = dueDate,
                        Status = "Pendiente",
                        IsOverdue = isOverdue,
                        Reference = $"OBL-{svcCode}-{index:000}"
                    });

                    totalPending += amount;
                    if (isOverdue) totalOverdue += amount;
                }

                return new MunicipalAccountDto
                {
                    AccountNumber = $"CTA-{index:0000}",
                    ContributorId = contributorIds[index - 1],
                    ContributorName = contributorNames[index - 1],
                    PropertyId = $"PROP-{index:000}",
                    PropertyAddress = addresses[index - 1],
                    FincaNumber = $"{index}-{100000 + index}",
                    AccountStatus = index % 7 == 0 ? "Inactiva" : "Activa",
                    CurrentBalance = totalPending,
                    PendingAmount = totalPending,
                    OverdueAmount = totalOverdue,
                    LastPaymentDate = index % 3 == 0 ? DateTime.Now.AddDays(-30 * index) : null,
                    Obligations = obligationsList
                };
            }).ToList();

            transactions = new List<PaymentTransactionDto>();
            
            // Seed initial transactions/receipts
            var seedTransactions = new List<PaymentTransactionDto>
            {
                new()
                {
                    Id = "TRX-001",
                    ReceiptNumber = "REC-2026-000001",
                    AccountNumber = "CTA-0001",
                    ContributorId = "1-2345-6789",
                    ContributorName = "Comercial El Parque S.A.",
                    PaymentDate = DateTime.Now.AddDays(-15),
                    PaymentMethod = "Tarjeta",
                    PaymentMethodData = new PaymentMethodDataDto { Method = "Tarjeta", CardBrand = "Visa", CardLastFour = "4532" },
                    TotalAmount = 45000m,
                    Status = "Completado",
                    CollectorUser = "usuario.municipal",
                    CashierStation = "Caja 01",
                    Items = new List<PaymentItemDto>
                    {
                        new() { PaymentType = "PAT-TRI", PaymentTypeName = "Patentes municipales trimestral", Period = "2026-01", Amount = 25000m, Reference = "OBL-PAT-TRI-001" },
                        new() { PaymentType = "AGUA", PaymentTypeName = "Agua potable", Period = "2026-01", Amount = 20000m, Reference = "OBL-AGUA-001" }
                    },
                    ReprintCount = 2,
                    LastReprintedAt = DateTime.Now.AddDays(-5)
                },
                new()
                {
                    Id = "TRX-002",
                    ReceiptNumber = "REC-2026-000002",
                    AccountNumber = "CTA-0002",
                    ContributorId = "1-1234-5678",
                    ContributorName = "María Pérez López",
                    PaymentDate = DateTime.Now.AddDays(-10),
                    PaymentMethod = "Transferencia bancaria",
                    PaymentMethodData = new PaymentMethodDataDto { Method = "Transferencia bancaria", Bank = "Banco Nacional de Costa Rica", Reference = "TR-2026-45678" },
                    TotalAmount = 38500m,
                    Status = "Completado",
                    CollectorUser = "usuario.municipal",
                    CashierStation = "Caja 02",
                    Items = new List<PaymentItemDto>
                    {
                        new() { PaymentType = "BI-TRI", PaymentTypeName = "Bienes inmuebles trimestral", Period = "2026-01", Amount = 28500m, Reference = "OBL-BI-TRI-002" },
                        new() { PaymentType = "BAS", PaymentTypeName = "Recolección de residuos", Period = "2026-01", Amount = 10000m, Reference = "OBL-BAS-002" }
                    },
                    ReprintCount = 0
                },
                new()
                {
                    Id = "TRX-003",
                    ReceiptNumber = "REC-2026-000003",
                    AccountNumber = "CTA-0002",
                    ContributorId = "1-1234-5678",
                    ContributorName = "María Pérez López",
                    PaymentDate = DateTime.Now.AddDays(-8),
                    PaymentMethod = "SINPE Móvil",
                    PaymentMethodData = new PaymentMethodDataDto { Method = "SINPE Móvil", SinpePhone = "8888-7777", Reference = "SIN-2026-12345" },
                    TotalAmount = 15000m,
                    Status = "Completado",
                    CollectorUser = "usuario.municipal",
                    CashierStation = "Caja 01",
                    Items = new List<PaymentItemDto>
                    {
                        new() { PaymentType = "AGUA", PaymentTypeName = "Agua potable", Period = "2026-01", Amount = 15000m, Reference = "OBL-AGUA-002" }
                    },
                    ReprintCount = 1,
                    LastReprintedAt = DateTime.Now.AddDays(-2)
                },
                new()
                {
                    Id = "TRX-004",
                    ReceiptNumber = "REC-2026-000004",
                    AccountNumber = "CTA-0003",
                    ContributorId = "3-3333-4444",
                    ContributorName = "Constructora del Norte S.R.L.",
                    PaymentDate = DateTime.Now.AddDays(-7),
                    PaymentMethod = "Efectivo",
                    PaymentMethodData = new PaymentMethodDataDto { Method = "Efectivo", Reference = "EF-2026-0004" },
                    TotalAmount = 52000m,
                    Status = "Completado",
                    CollectorUser = "usuario.municipal",
                    CashierStation = "Caja 03",
                    Items = new List<PaymentItemDto>
                    {
                        new() { PaymentType = "PAT-TRI", PaymentTypeName = "Patentes municipales trimestral", Period = "2026-01", Amount = 32000m, Reference = "OBL-PAT-TRI-003" },
                        new() { PaymentType = "ESP-PUB", PaymentTypeName = "Espectáculos públicos", Period = "2026-01", Amount = 20000m, Reference = "OBL-ESP-PUB-003" }
                    },
                    ReprintCount = 0
                },
                new()
                {
                    Id = "TRX-005",
                    ReceiptNumber = "REC-2026-000005",
                    AccountNumber = "CTA-0004",
                    ContributorId = "4-5555-6666",
                    ContributorName = "Inversiones del Valle S.A.",
                    PaymentDate = DateTime.Now.AddDays(-5),
                    PaymentMethod = "Tarjeta",
                    PaymentMethodData = new PaymentMethodDataDto { Method = "Tarjeta", CardBrand = "MasterCard", CardLastFour = "8899" },
                    TotalAmount = 67500m,
                    Status = "Completado",
                    CollectorUser = "usuario.municipal",
                    CashierStation = "Caja 01",
                    Items = new List<PaymentItemDto>
                    {
                        new() { PaymentType = "BI-TRI", PaymentTypeName = "Bienes inmuebles trimestral", Period = "2026-01", Amount = 42500m, Reference = "OBL-BI-TRI-004" },
                        new() { PaymentType = "AGUA", PaymentTypeName = "Agua potable", Period = "2026-01", Amount = 15000m, Reference = "OBL-AGUA-004" },
                        new() { PaymentType = "BAS", PaymentTypeName = "Recolección de residuos", Period = "2026-01", Amount = 10000m, Reference = "OBL-BAS-004" }
                    },
                    ReprintCount = 3,
                    LastReprintedAt = DateTime.Now.AddDays(-1)
                },
                new()
                {
                    Id = "TRX-006",
                    ReceiptNumber = "REC-2026-000006",
                    AccountNumber = "CTA-0005",
                    ContributorId = "1-0987-6543",
                    ContributorName = "Juan Carlos Ramírez Mora",
                    PaymentDate = DateTime.Now.AddDays(-3),
                    PaymentMethod = "Transferencia bancaria",
                    PaymentMethodData = new PaymentMethodDataDto { Method = "Transferencia bancaria", Bank = "Banco de Costa Rica", Reference = "TR-2026-99887" },
                    TotalAmount = 29000m,
                    Status = "Completado",
                    CollectorUser = "usuario.municipal",
                    CashierStation = "Caja 02",
                    Items = new List<PaymentItemDto>
                    {
                        new() { PaymentType = "PAT-TRI", PaymentTypeName = "Patentes municipales trimestral", Period = "2026-01", Amount = 20000m, Reference = "OBL-PAT-TRI-005" },
                        new() { PaymentType = "LIM", PaymentTypeName = "Limpieza de vías", Period = "2026-01", Amount = 9000m, Reference = "OBL-LIM-005" }
                    },
                    ReprintCount = 0
                },
                new()
                {
                    Id = "TRX-007",
                    ReceiptNumber = "REC-2026-000007",
                    AccountNumber = "CTA-0006",
                    ContributorId = "6-9999-0000",
                    ContributorName = "Mercados del Pacífico S.A.",
                    PaymentDate = DateTime.Now.AddDays(-2),
                    PaymentMethod = "SINPE Móvil",
                    PaymentMethodData = new PaymentMethodDataDto { Method = "SINPE Móvil", SinpePhone = "7777-6666", Reference = "SIN-2026-54321" },
                    TotalAmount = 58000m,
                    Status = "Completado",
                    CollectorUser = "usuario.municipal",
                    CashierStation = "Caja 01",
                    Items = new List<PaymentItemDto>
                    {
                        new() { PaymentType = "ALQ-MER", PaymentTypeName = "Alquiler de mercado", Period = "2026-01", Amount = 35000m, Reference = "OBL-ALQ-MER-006" },
                        new() { PaymentType = "AGUA", PaymentTypeName = "Agua potable", Period = "2026-01", Amount = 23000m, Reference = "OBL-AGUA-006" }
                    },
                    ReprintCount = 1,
                    LastReprintedAt = DateTime.Now.AddHours(-12)
                },
                new()
                {
                    Id = "TRX-008",
                    ReceiptNumber = "REC-2026-000008",
                    AccountNumber = "CTA-0001",
                    ContributorId = "1-2345-6789",
                    ContributorName = "Comercial El Parque S.A.",
                    PaymentDate = DateTime.Now.AddDays(-1),
                    PaymentMethod = "Efectivo",
                    PaymentMethodData = new PaymentMethodDataDto { Method = "Efectivo", Reference = "EF-2026-0008" },
                    TotalAmount = 18500m,
                    Status = "Completado",
                    CollectorUser = "usuario.municipal",
                    CashierStation = "Caja 03",
                    Items = new List<PaymentItemDto>
                    {
                        new() { PaymentType = "ALC", PaymentTypeName = "Alcantarillado sanitario", Period = "2026-01", Amount = 18500m, Reference = "OBL-ALC-001" }
                    },
                    ReprintCount = 0
                },
                new()
                {
                    Id = "TRX-009",
                    ReceiptNumber = "REC-2026-000009",
                    AccountNumber = "CTA-0003",
                    ContributorId = "3-3333-4444",
                    ContributorName = "Constructora del Norte S.R.L.",
                    PaymentDate = DateTime.Now.AddHours(-6),
                    PaymentMethod = "Tarjeta",
                    PaymentMethodData = new PaymentMethodDataDto { Method = "Tarjeta", CardBrand = "Visa", CardLastFour = "1122" },
                    TotalAmount = 24000m,
                    Status = "Completado",
                    CollectorUser = "usuario.municipal",
                    CashierStation = "Caja 01",
                    Items = new List<PaymentItemDto>
                    {
                        new() { PaymentType = "AGUA", PaymentTypeName = "Agua potable", Period = "2026-01", Amount = 14000m, Reference = "OBL-AGUA-003" },
                        new() { PaymentType = "BAS", PaymentTypeName = "Recolección de residuos", Period = "2026-01", Amount = 10000m, Reference = "OBL-BAS-003" }
                    },
                    ReprintCount = 0
                },
                new()
                {
                    Id = "TRX-010",
                    ReceiptNumber = "REC-2026-000010",
                    AccountNumber = "CTA-0002",
                    ContributorId = "1-1234-5678",
                    ContributorName = "María Pérez López",
                    PaymentDate = DateTime.Now.AddHours(-2),
                    PaymentMethod = "Transferencia bancaria",
                    PaymentMethodData = new PaymentMethodDataDto { Method = "Transferencia bancaria", Bank = "Banco Popular", Reference = "TR-2026-11223" },
                    TotalAmount = 12000m,
                    Status = "Completado",
                    CollectorUser = "usuario.municipal",
                    CashierStation = "Caja 02",
                    Items = new List<PaymentItemDto>
                    {
                        new() { PaymentType = "LIM", PaymentTypeName = "Limpieza de vías", Period = "2026-01", Amount = 12000m, Reference = "OBL-LIM-002" }
                    },
                    ReprintCount = 0
                }
            };
            
            transactions.AddRange(seedTransactions);
            
            linkedAccounts = new List<LinkedAccountDto>();
            history = new List<PaymentHistoryEventDto>();

            seeded = true;
        }
    }

    public async Task<List<AccountMovementDto>> GetAccountMovementsAsync(string accountNumber)
    {
        await GetAccountsAsync();
        var account = accounts.FirstOrDefault(a => IdentificationHelper.AccountNumberMatches(a.AccountNumber, accountNumber));
        
        if (account == null)
        {
            return new List<AccountMovementDto>();
        }

        if (!account.Movements.Any())
        {
            var seedMovements = new List<AccountMovementDto>
            {
                new() { Date = DateTime.Now.AddDays(-15), Type = "Cargo", Description = "Bienes inmuebles trimestral", Reference = "OBL-BI-TRI-001", Debit = 45000, Credit = 0, Balance = 45000, Status = "Aplicado" },
                new() { Date = DateTime.Now.AddDays(-10), Type = "Cargo", Description = "Agua potable", Reference = "OBL-AGUA-001", Debit = 12000, Credit = 0, Balance = 57000, Status = "Aplicado" }
            };
            account.Movements.AddRange(seedMovements);
            await _localStorage.SetItemAsync(ACCOUNTS_KEY, accounts);
        }

        return account.Movements.OrderByDescending(m => m.Date).ToList();
    }
}
