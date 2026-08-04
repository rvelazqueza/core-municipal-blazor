using System;
using System.Collections.Generic;

namespace BlazorApp.Models.Pagos;

public class PaymentTypeDto
{
    public string Id { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public bool RequiresIdentification { get; set; } = true;
    public bool RequiresAccount { get; set; } = true;
    public bool AllowsPartialPayment { get; set; } = true;
    public string Department { get; set; } = "Tesorería";
    public bool Active { get; set; } = true;
}

public class PaymentMethodDataDto
{
    public string Method { get; set; } = string.Empty;
    public string CardBrand { get; set; } = string.Empty;
    public string CardLastFour { get; set; } = string.Empty;
    public string Bank { get; set; } = string.Empty;
    public string Reference { get; set; } = string.Empty;
    public string SinpePhone { get; set; } = string.Empty;
    public DateTime? PaymentDate { get; set; }
}

public class MunicipalAccountDto
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string AccountNumber { get; set; } = string.Empty;
    public string ContributorId { get; set; } = string.Empty;
    public string ContributorName { get; set; } = string.Empty;
    public string PropertyId { get; set; } = string.Empty;
    public string PropertyAddress { get; set; } = string.Empty;
    public string FincaNumber { get; set; } = string.Empty;
    public string AccountStatus { get; set; } = "Activa";
    public decimal CurrentBalance { get; set; }
    public decimal PendingAmount { get; set; }
    public decimal OverdueAmount { get; set; }
    public DateTime? LastPaymentDate { get; set; }
    public List<AccountObligationDto> Obligations { get; set; } = new();
    public List<AccountMovementDto> Movements { get; set; } = new();
}

public class AccountObligationDto
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string AccountNumber { get; set; } = string.Empty;
    public string PaymentType { get; set; } = string.Empty;
    public string PaymentTypeName { get; set; } = string.Empty;
    public string Period { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public decimal PendingAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public DateTime DueDate { get; set; }
    public string Status { get; set; } = "Pendiente";
    public bool IsOverdue { get; set; }
    public string Reference { get; set; } = string.Empty;
}

public class PaymentTransactionDto
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string ReceiptNumber { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public string ContributorId { get; set; } = string.Empty;
    public string ContributorName { get; set; } = string.Empty;
    public DateTime PaymentDate { get; set; } = DateTime.Now;
    public string PaymentMethod { get; set; } = string.Empty;
    public PaymentMethodDataDto? PaymentMethodData { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = "Completado";
    public string CollectorUser { get; set; } = "usuario.municipal";
    public string CashierStation { get; set; } = "Caja 01";
    public List<PaymentItemDto> Items { get; set; } = new();
    public string Reference { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public int ReprintCount { get; set; }
    public DateTime? LastReprintedAt { get; set; }
}

public class PaymentItemDto
{
    public string ObligationId { get; set; } = string.Empty;
    public string PaymentType { get; set; } = string.Empty;
    public string PaymentTypeName { get; set; } = string.Empty;
    public string Period { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Reference { get; set; } = string.Empty;
}

public class PaymentReceiptDto
{
    public string ReceiptNumber { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public string ContributorId { get; set; } = string.Empty;
    public string ContributorName { get; set; } = string.Empty;
    public string PropertyAddress { get; set; } = string.Empty;
    public DateTime PaymentDate { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public PaymentMethodDataDto? PaymentMethodData { get; set; }
    public decimal TotalAmount { get; set; }
    public List<PaymentItemDto> Items { get; set; } = new();
    public string CollectorUser { get; set; } = string.Empty;
    public string CashierStation { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public int ReprintCount { get; set; }
    public DateTime? LastReprintedAt { get; set; }
}

public class AccountMovementDto
{
    public DateTime Date { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Reference { get; set; } = string.Empty;
    public decimal Debit { get; set; }
    public decimal Credit { get; set; }
    public decimal Balance { get; set; }
    public string Status { get; set; } = string.Empty;
}

public class LinkedAccountDto
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public string AccountNumber { get; set; } = string.Empty;
    public string ContributorId { get; set; } = string.Empty;
    public string ContributorName { get; set; } = string.Empty;
    public string PropertyId { get; set; } = string.Empty;
    public string PropertyAddress { get; set; } = string.Empty;
    public string FincaNumber { get; set; } = string.Empty;
    public decimal CurrentBalance { get; set; }
    public DateTime? LastPaymentDate { get; set; }
    public DateTime LinkedDate { get; set; } = DateTime.Now;
    public bool IsDefault { get; set; }
}

public class PaymentHistoryEventDto
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public DateTime Date { get; set; } = DateTime.Now;
    public string User { get; set; } = "usuario.municipal";
    public string Action { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal? Amount { get; set; }
    public string Reference { get; set; } = string.Empty;
    public string Observation { get; set; } = string.Empty;
}
