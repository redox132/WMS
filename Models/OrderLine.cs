using SQLite;

namespace WMS.Models;

[Table("OrderLines")]
public class OrderLine
{
    [PrimaryKey, AutoIncrement] public int Id { get; set; }
    [Indexed] public int OrderId { get; set; }

    // ── Product reference ───────────────────────────────────────────────────
    public int    ProductId { get; set; }
    public string ProductName { get; set; } = "";   // denormalised snapshot
    public string SKU { get; set; } = "";
    public string? Barcode { get; set; }

    // ── Quantity & pricing ──────────────────────────────────────────────────
    public decimal Quantity { get; set; }
    public string  Unit { get; set; } = "pcs";
    public decimal UnitPrice { get; set; }          // net price per unit
    public decimal DiscountPercent { get; set; }
    public decimal VatRate { get; set; } = 23;

    // ── Calculated totals (denormalised for reporting) ───────────────────────
    public decimal NetAmount   { get; set; }
    public decimal VatAmount   { get; set; }
    public decimal GrossAmount { get; set; }

    // ── Warehouse / tracking ────────────────────────────────────────────────
    public string? WarehouseLocation { get; set; }
    public string? BatchNumber       { get; set; }
    public string? SerialNumber      { get; set; }
    public System.DateTime? ExpiryDate { get; set; }

    // ── E-commerce ──────────────────────────────────────────────────────────
    public string? EcommerceLineId { get; set; }

    public int? SortOrder { get; set; }

    // ── Computed ─────────────────────────────────────────────────────────────
    [Ignore] public decimal EffectiveUnitPrice => UnitPrice * (1 - DiscountPercent / 100);
    [Ignore] public decimal LineTotal          => GrossAmount;
}
