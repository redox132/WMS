using System;
using System.Collections.Generic;
using SQLite;

namespace WMS.Models;

public enum OrderType   { Sales, Purchase }
public enum OrderStatus { New, Confirmed, InProgress, Shipped, Delivered, Cancelled }
public enum PaymentStatus { Unpaid, PartiallyPaid, Paid, Refunded }

[Table("Orders")]
public class Order
{
    // ── Identity ────────────────────────────────────────────────────────────
    [PrimaryKey, AutoIncrement] public int Id { get; set; }
    [NotNull] public string Number { get; set; } = "";
    public OrderType    Type   { get; set; }
    public OrderStatus  Status { get; set; }

    // ── Dates ───────────────────────────────────────────────────────────────
    public DateTime  OrderDate    { get; set; } = DateTime.UtcNow;
    public DateTime? ConfirmedAt  { get; set; }
    public DateTime? DueDate      { get; set; }
    public DateTime? ShippingDate { get; set; }
    public DateTime? DeliveryDate { get; set; }

    // ── Customer / Supplier ─────────────────────────────────────────────────
    [Indexed] public int CustomerId { get; set; }
    public string CustomerName { get; set; } = ""; // snapshot

    // ── Addresses (snapshot at order time) ──────────────────────────────────
    public string? BillingName    { get; set; }
    public string? BillingStreet  { get; set; }
    public string? BillingCity    { get; set; }
    public string? BillingPostal  { get; set; }
    public string? BillingCountry { get; set; }

    public string? ShippingName    { get; set; }
    public string? ShippingStreet  { get; set; }
    public string? ShippingCity    { get; set; }
    public string? ShippingPostal  { get; set; }
    public string? ShippingCountry { get; set; }

    // ── Payment ─────────────────────────────────────────────────────────────
    public string        PaymentMethod { get; set; } = "Transfer";
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Unpaid;
    public DateTime?     PaymentDate   { get; set; }
    public string        Currency      { get; set; } = "PLN";
    public decimal       ExchangeRate  { get; set; } = 1;

    // ── Shipping ─────────────────────────────────────────────────────────────
    public string? ShippingMethod  { get; set; }
    public string? ShippingCarrier { get; set; }
    public string? TrackingNumber  { get; set; }
    public decimal ShippingCost    { get; set; }

    // ── Amounts ─────────────────────────────────────────────────────────────
    public decimal DiscountPercent { get; set; }
    public decimal DiscountAmount  { get; set; }
    public decimal NetAmount       { get; set; }
    public decimal VatAmount       { get; set; }
    public decimal GrossAmount     { get; set; }

    // ── References ───────────────────────────────────────────────────────────
    public int?    WarehouseId      { get; set; }
    public string? InvoiceNumber    { get; set; }
    public string? ExternalRef      { get; set; }    // PO number from customer
    public string? EcommerceOrderId { get; set; }    // platform order ID
    public string? EcommerceOrderNo { get; set; }    // human-readable platform number

    // ── Notes ────────────────────────────────────────────────────────────────
    public string? Notes         { get; set; }
    public string? InternalNotes { get; set; }

    // ── Audit ────────────────────────────────────────────────────────────────
    public string?   CreatedBy { get; set; }
    public DateTime  CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime  UpdatedAt { get; set; } = DateTime.UtcNow;

    // ── Computed (not stored) ─────────────────────────────────────────────────
    [Ignore] public List<OrderLine> Lines { get; set; } = new();

    [Ignore] public string TypeLabel => Type == OrderType.Sales ? "SO" : "PO";

    [Ignore] public string StatusLabel => Status switch
    {
        OrderStatus.New        => "New",
        OrderStatus.Confirmed  => "Confirmed",
        OrderStatus.InProgress => "In Progress",
        OrderStatus.Shipped    => "Shipped",
        OrderStatus.Delivered  => "Delivered",
        OrderStatus.Cancelled  => "Cancelled",
        _                      => ""
    };

    [Ignore] public string StatusColor => Status switch
    {
        OrderStatus.New        => "#616161",
        OrderStatus.Confirmed  => "#1565C0",
        OrderStatus.InProgress => "#E65100",
        OrderStatus.Shipped    => "#0277BD",
        OrderStatus.Delivered  => "#2E7D32",
        OrderStatus.Cancelled  => "#B71C1C",
        _                      => "#616161"
    };

    [Ignore] public string TypeColor => Type == OrderType.Sales ? "#1565C0" : "#6A1B9A";
}
