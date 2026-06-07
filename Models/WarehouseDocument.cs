using System;
using System.Collections.Generic;
using SQLite;

namespace WMS.Models;

public enum DocumentType   { WZ, PZ, MM, PW, RW }
public enum DocumentStatus { Draft, Confirmed, Cancelled }

[Table("WarehouseDocuments")]
public class WarehouseDocument
{
    // ── Identity ──────────────────────────────────────────────────────────────
    [PrimaryKey, AutoIncrement] public int Id { get; set; }
    [NotNull] public string Number { get; set; } = "";
    public DocumentType   Type   { get; set; }
    public DocumentStatus Status { get; set; } = DocumentStatus.Confirmed;

    // ── Dates ─────────────────────────────────────────────────────────────────
    public DateTime  Date        { get; set; } = DateTime.UtcNow;
    public DateTime? ConfirmedAt { get; set; }
    public string?   ConfirmedBy { get; set; }

    // ── Warehouse routing ─────────────────────────────────────────────────────
    public int?    WarehouseFromId   { get; set; }
    public string? WarehouseFromName { get; set; }
    public int?    WarehouseToId     { get; set; }
    public string? WarehouseToName   { get; set; }

    // ── Contractor ────────────────────────────────────────────────────────────
    public int?    ContractorId   { get; set; }
    public string? ContractorName { get; set; }

    // ── Linked documents ──────────────────────────────────────────────────────
    public int?    LinkedOrderId       { get; set; }
    public string? LinkedOrderNumber   { get; set; }
    public string? LinkedInvoiceNumber { get; set; }
    public string? ExternalRef         { get; set; }  // supplier delivery note no.

    // ── Transport ─────────────────────────────────────────────────────────────
    public string? Carrier        { get; set; }
    public string? TrackingNumber { get; set; }
    public string? VehiclePlate   { get; set; }
    public string? DriverName     { get; set; }

    // ── Totals ────────────────────────────────────────────────────────────────
    public decimal TotalQuantity { get; set; }
    public decimal NetAmount     { get; set; }
    public decimal VatAmount     { get; set; }
    public decimal GrossAmount   { get; set; }

    // ── Notes ─────────────────────────────────────────────────────────────────
    public string? Notes         { get; set; }
    public string? InternalNotes { get; set; }

    // ── Audit ─────────────────────────────────────────────────────────────────
    public string?   CreatedBy { get; set; }
    public DateTime  CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime  UpdatedAt { get; set; } = DateTime.UtcNow;

    // ── Computed (not stored) ──────────────────────────────────────────────────
    [Ignore] public List<DocumentLine> Lines { get; set; } = new();

    [Ignore] public string TypeDescription => Type switch
    {
        DocumentType.WZ => "Goods Issue (WZ)",
        DocumentType.PZ => "Goods Receipt (PZ)",
        DocumentType.MM => "Warehouse Transfer (MM)",
        DocumentType.PW => "Internal Receipt (PW)",
        DocumentType.RW => "Internal Issue (RW)",
        _               => ""
    };

    [Ignore] public string StatusLabel => Status switch
    {
        DocumentStatus.Draft     => "Draft",
        DocumentStatus.Confirmed => "Confirmed",
        DocumentStatus.Cancelled => "Cancelled",
        _                        => ""
    };

    [Ignore] public string StatusColor => Status switch
    {
        DocumentStatus.Draft     => "#616161",
        DocumentStatus.Confirmed => "#2E7D32",
        DocumentStatus.Cancelled => "#B71C1C",
        _                        => "#616161"
    };

    [Ignore] public string TypeColor => Type switch
    {
        DocumentType.WZ => "#1565C0",
        DocumentType.PZ => "#2E7D32",
        DocumentType.MM => "#E65100",
        DocumentType.PW => "#6A1B9A",
        DocumentType.RW => "#B71C1C",
        _               => "#616161"
    };

    [Ignore] public string MovementDescription
    {
        get
        {
            var from = WarehouseFromName ?? ContractorName ?? "—";
            var to   = WarehouseToName  ?? ContractorName ?? "—";
            return Type switch
            {
                DocumentType.WZ => $"{WarehouseFromName ?? "—"} → {ContractorName ?? "—"}",
                DocumentType.PZ => $"{ContractorName ?? "—"} → {WarehouseToName ?? "—"}",
                DocumentType.MM => $"{from} → {to}",
                DocumentType.PW => $"→ {WarehouseToName ?? "—"}",
                DocumentType.RW => $"{WarehouseFromName ?? "—"} →",
                _               => ""
            };
        }
    }
}
