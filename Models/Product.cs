namespace WMS.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string SKU { get; set; } = "";
    public string Barcode { get; set; } = "";
    public string Unit { get; set; } = "szt.";
    public string Category { get; set; } = "";
    public decimal PurchasePrice { get; set; }
    public decimal SalePrice { get; set; }
    public decimal StockLevel { get; set; }
    public decimal MinStockLevel { get; set; }
    public bool IsActive { get; set; } = true;

    public bool IsLowStock => StockLevel <= MinStockLevel;

    public string StockStatus => IsLowStock ? "Niski stan" : "OK";
    public string StockLevelColor => IsLowStock ? "#E05252" : "Transparent";
    public string StatusBadgeColor => IsLowStock ? "#B71C1C" : "#2E7D32";
}
