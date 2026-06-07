using System.Collections.Generic;
using System.Linq;
using WMS.Models;
using WMS.Services;

namespace WMS.ViewModels.Pages;

public partial class AnalyticsPageViewModel : ViewModelBase
{
    public List<ChartBar> StockByCategory    { get; }
    public List<ChartBar> OrdersByStatus     { get; }
    public List<ChartBar> TopProductsByValue { get; }

    // Summary KPIs
    public decimal TotalStockValue    { get; }
    public decimal AvgOrderValue      { get; }
    public int     TurnoverProducts   { get; }  // products with stock movement (WZ docs)
    public int     SuppliersCount     { get; }

    public AnalyticsPageViewModel()
    {
        var products  = AppServices.Products.GetAll();
        var orders    = AppServices.Orders.GetAll();
        var docs      = AppServices.Documents.GetAll();
        var customers = AppServices.Customers.GetAll();

        // ── KPIs ────────────────────────────────────────────────────────────────
        TotalStockValue  = products.Sum(p => p.StockLevel * p.SalePrice1);
        AvgOrderValue    = orders.Any() ? orders.Average(o => o.GrossAmount) : 0;
        TurnoverProducts = docs.Where(d => d.Type == DocumentType.WZ).SelectMany(d => d.Lines).Select(l => l.ProductId).Distinct().Count();
        SuppliersCount   = customers.Count(c => c.Type == CustomerType.Supplier || c.Type == CustomerType.Both);

        // ── Stock by category ────────────────────────────────────────────────────
        var byCat = products
            .GroupBy(p => p.CategoryName ?? "Uncategorised")
            .Select(g => new { Label = g.Key, Value = g.Sum(p => (double)p.StockLevel) })
            .OrderByDescending(x => x.Value)
            .Take(8)
            .ToList();

        var maxCat = byCat.Any() ? byCat.Max(x => x.Value) : 1;
        var catColors = new[] { "#1565C0","#2E7D32","#E65100","#6A1B9A","#B71C1C","#0277BD","#558B2F","#F57F17" };
        StockByCategory = byCat
            .Select((x, i) => new ChartBar(x.Label, x.Value, maxCat, catColors[i % catColors.Length]))
            .ToList();

        // ── Orders by status ─────────────────────────────────────────────────────
        var statusGroups = orders
            .GroupBy(o => o.StatusLabel)
            .Select(g => new { Label = g.Key, Value = (double)g.Count() })
            .OrderByDescending(x => x.Value)
            .ToList();

        var maxStatus = statusGroups.Any() ? statusGroups.Max(x => x.Value) : 1;
        var statusColors = new Dictionary<string, string>
        {
            ["New"]        = "#616161",
            ["Confirmed"]  = "#1565C0",
            ["In Progress"]= "#E65100",
            ["Shipped"]    = "#0277BD",
            ["Delivered"]  = "#2E7D32",
            ["Cancelled"]  = "#B71C1C",
        };
        OrdersByStatus = statusGroups
            .Select(x => new ChartBar(x.Label, x.Value, maxStatus,
                statusColors.TryGetValue(x.Label, out var c) ? c : "#616161"))
            .ToList();

        // ── Top 6 products by stock value ────────────────────────────────────────
        var topProds = products
            .OrderByDescending(p => p.StockLevel * p.SalePrice1)
            .Take(6)
            .ToList();
        var maxProdVal = topProds.Any() ? (double)(topProds[0].StockLevel * topProds[0].SalePrice1) : 1;
        TopProductsByValue = topProds
            .Select((p, i) => new ChartBar(
                p.Name.Length > 28 ? p.Name[..28] + "…" : p.Name,
                (double)(p.StockLevel * p.SalePrice1),
                maxProdVal,
                catColors[i % catColors.Length]))
            .ToList();
    }
}
