using System;
using SQLite;

namespace WMS.Models;

[Table("Products")]
public class Product
{
    // ── Identity ────────────────────────────────────────────────────────────
    [PrimaryKey, AutoIncrement] public int Id { get; set; }
    [NotNull] public string Name { get; set; } = "";
    public string? ShortDescription { get; set; }
    public string? Description { get; set; }

    // ── Codes & Identifiers ─────────────────────────────────────────────────
    [Unique, NotNull] public string SKU { get; set; } = "";
    public string? Barcode { get; set; }         // primary barcode (EAN-13, UPC …)
    public string? Barcode2 { get; set; }         // secondary / supplier barcode
    public string? BarcodeType { get; set; }      // EAN13 | EAN8 | UPC | QR | CODE128
    public string? FiscalName { get; set; }       // name sent to fiscal cash register
    public string? PLUCode { get; set; }          // PLU for scales/cash drawers
    public string? SupplierSKU { get; set; }      // supplier's own part number

    // ── Classification ──────────────────────────────────────────────────────
    public int?   CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public string? Tags { get; set; }             // comma-separated
    public string? CountryOfOrigin { get; set; }  // ISO 3166-1 alpha-2
    public string? HSCode { get; set; }           // HS / CN code for customs & Intrastat

    // ── Units ───────────────────────────────────────────────────────────────
    public string Unit { get; set; } = "pcs";
    public string? Unit2 { get; set; }            // auxiliary unit (e.g. box)
    public decimal? UnitConversionFactor { get; set; } // 1 box = N pcs

    // ── Pricing (up to 5 levels, mirrors Subiekt GT) ────────────────────────
    public decimal PurchasePrice { get; set; }
    public decimal VatRatePurchase { get; set; } = 23;
    public decimal SalePrice1 { get; set; }       // base / retail
    public decimal? SalePrice2 { get; set; }      // wholesale
    public decimal? SalePrice3 { get; set; }
    public decimal? SalePrice4 { get; set; }
    public decimal? SalePrice5 { get; set; }      // VIP / special
    public decimal VatRateSale { get; set; } = 23;
    public decimal? MinSalePrice { get; set; }    // floor price

    // ── Stock ───────────────────────────────────────────────────────────────
    public decimal StockLevel { get; set; }
    public decimal MinStockLevel { get; set; }
    public decimal MaxStockLevel { get; set; }
    public decimal ReorderPoint { get; set; }
    public decimal ReorderQty { get; set; }
    public string StockMethod { get; set; } = "FIFO"; // FIFO | LIFO | ExpiryDate
    public bool   TrackExpiry { get; set; }
    public bool   TrackSerialNumbers { get; set; }
    public bool   TrackBatches { get; set; }
    public string? WarehouseLocation { get; set; } // bin / shelf / aisle

    // ── Physical ────────────────────────────────────────────────────────────
    public decimal? Weight { get; set; }
    public string?  WeightUnit { get; set; } = "kg";
    public decimal? Volume { get; set; }           // m³
    public decimal? DimensionLength { get; set; }  // cm
    public decimal? DimensionWidth  { get; set; }
    public decimal? DimensionHeight { get; set; }

    // ── Supplier ────────────────────────────────────────────────────────────
    public int? DefaultSupplierId { get; set; }
    public int? LeadTimeDays { get; set; }

    // ── E-commerce ──────────────────────────────────────────────────────────
    public bool   EcommerceEnabled { get; set; }
    public string? EcommerceId { get; set; }      // external platform ID
    public string? EcommerceSlug { get; set; }
    public string? ImageUrl { get; set; }
    public string? ImageUrl2 { get; set; }
    public string? ImageUrl3 { get; set; }

    // ── Flags ───────────────────────────────────────────────────────────────
    public bool IsActive  { get; set; } = true;
    public bool IsService { get; set; }
    public bool IsBundle  { get; set; }
    public bool IsVATExempt { get; set; }

    // ── Audit ───────────────────────────────────────────────────────────────
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // ── Computed (not stored) ───────────────────────────────────────────────
    [Ignore] public bool   IsLowStock        => MinStockLevel > 0 && StockLevel <= MinStockLevel;
    [Ignore] public string StockStatus       => IsLowStock ? "Low Stock" : "OK";
    [Ignore] public string StatusBadgeColor  => IsLowStock ? "#B71C1C" : "#2E7D32";
    [Ignore] public string StockLevelColor   => IsLowStock ? "#E05252" : "Transparent";
}
