using System.Collections.Generic;
using WMS.Services;

namespace WMS.ViewModels.Pages;

public record ReportCard(string Title, string Description, string Icon, string Color);

public partial class ReportsPageViewModel : ViewModelBase
{
    public List<ReportCard> Reports { get; } = new()
    {
        new("Inventory Valuation",  "Total stock value broken down by category and product", "📦", "#1565C0"),
        new("Low Stock Alert",      "Products below minimum stock level requiring reorder",   "⚠️", "#B71C1C"),
        new("Stock Movement",       "Inbound and outbound movements over a selected period",  "↕️", "#2E7D32"),
        new("Dead Stock",           "Products with no movement for more than 90 days",        "💤", "#616161"),
        new("Supplier Performance", "Delivery times and order accuracy by supplier",          "🏭", "#6A1B9A"),
        new("Order Summary",        "Sales and purchase order totals by period",              "📋", "#E65100"),
        new("Customer Activity",    "Top customers by order volume and value",                "👥", "#0277BD"),
        new("Reorder Suggestions",  "Auto-generated list of products that need restocking",   "🔄", "#558B2F"),
    };

    // Snapshot totals shown at the top
    public decimal TotalStockValue { get; }
    public int     LowStockCount   { get; }
    public int     OpenOrdersCount { get; }

    public ReportsPageViewModel()
    {
        var products = AppServices.Products.GetAll();
        TotalStockValue = 0;
        foreach (var p in products)
            TotalStockValue += p.StockLevel * p.SalePrice1;

        LowStockCount   = AppServices.Products.CountLowStock();
        OpenOrdersCount = AppServices.Orders.CountOpen();
    }
}
