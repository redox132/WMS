using System;
using SQLite;

namespace WMS.Models;

[Table("DocumentLines")]
public class DocumentLine
{
    [PrimaryKey, AutoIncrement] public int Id { get; set; }
    [Indexed] public int DocumentId { get; set; }

    // ── Product ──────────────────────────────────────────────────────────────
    public int    ProductId   { get; set; }
    public string ProductName { get; set; } = "";  // snapshot
    public string SKU         { get; set; } = "";
    public string? Barcode    { get; set; }

    // ── Quantity & pricing ───────────────────────────────────────────────────
    public decimal Quantity  { get; set; }
    public string  Unit      { get; set; } = "pcs";
    public decimal UnitPrice { get; set; }
    public decimal VatRate   { get; set; } = 23;
    public decimal NetAmount   { get; set; }
    public decimal VatAmount   { get; set; }
    public decimal GrossAmount { get; set; }

    // ── Traceability ─────────────────────────────────────────────────────────
    public string?   WarehouseLocation { get; set; }
    public string?   BatchNumber       { get; set; }
    public string?   SerialNumber      { get; set; }
    public DateTime? ExpiryDate        { get; set; }

    public int? SortOrder { get; set; }

    // ── Computed ──────────────────────────────────────────────────────────────
    [Ignore] public decimal TotalPrice => GrossAmount;
}
