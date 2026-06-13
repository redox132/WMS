using System.Collections.Generic;
using System.Linq;
using WMS.Models;
using WMS.Services;

namespace WMS.ViewModels.Pages;

public class ProductInsightRow
{
    public string  ProductName { get; set; } = "";
    public string  SKU         { get; set; } = "";
    public decimal TotalQty    { get; set; }
    public decimal NetAmount   { get; set; }
    public decimal GrossAmount { get; set; }
    public int     OrderCount  { get; set; }

    public string TotalQtyLabel    => TotalQty.ToString("N2");
    public string NetAmountLabel   => NetAmount.ToString("N2") + " PLN";
    public string GrossAmountLabel => GrossAmount.ToString("N2") + " PLN";
}

public class InsightsPageViewModel : ViewModelBase
{
    // ── Sales KPIs ────────────────────────────────────────────────────────────
    public decimal SalesTotalRevenue  { get; }
    public int     SalesOrderCount    { get; }
    public decimal SalesAvgOrderValue { get; }
    public string  SalesTopProduct    { get; }

    // ── Purchase KPIs ─────────────────────────────────────────────────────────
    public decimal PurchaseTotalCost      { get; }
    public int     PurchaseOrderCount     { get; }
    public decimal PurchaseAvgOrderValue  { get; }
    public string  PurchaseTopProduct     { get; }

    // ── Charts ───────────────────────────────────────────────────────────────
    public List<ChartBar> SalesTopProductsChart    { get; }
    public List<ChartBar> PurchaseTopProductsChart { get; }

    // ── Per-product tables ────────────────────────────────────────────────────
    public List<ProductInsightRow> SalesPerProduct    { get; }
    public List<ProductInsightRow> PurchasePerProduct { get; }

    private static readonly string[] Colors =
    {
        "#1565C0","#2E7D32","#E65100","#6A1B9A",
        "#0277BD","#558B2F","#F57F17","#B71C1C"
    };

    public InsightsPageViewModel()
    {
        var orders = AppServices.Orders.GetAll();

        var salesOrders    = orders.Where(o => o.Type == OrderType.Sales).ToList();
        var purchaseOrders = orders.Where(o => o.Type == OrderType.Purchase).ToList();

        // ── Sales KPIs ────────────────────────────────────────────────────────
        SalesOrderCount    = salesOrders.Count;
        SalesTotalRevenue  = salesOrders.Sum(o => o.GrossAmount);
        SalesAvgOrderValue = SalesOrderCount > 0 ? SalesTotalRevenue / SalesOrderCount : 0;

        SalesPerProduct = BuildProductRows(salesOrders);
        SalesTopProduct = SalesPerProduct.FirstOrDefault()?.ProductName ?? "—";

        SalesTopProductsChart = BuildChart(SalesPerProduct.Take(8).ToList(), o => (double)o.GrossAmount);

        // ── Purchase KPIs ─────────────────────────────────────────────────────
        PurchaseOrderCount    = purchaseOrders.Count;
        PurchaseTotalCost     = purchaseOrders.Sum(o => o.GrossAmount);
        PurchaseAvgOrderValue = PurchaseOrderCount > 0 ? PurchaseTotalCost / PurchaseOrderCount : 0;

        PurchasePerProduct = BuildProductRows(purchaseOrders);
        PurchaseTopProduct = PurchasePerProduct.FirstOrDefault()?.ProductName ?? "—";

        PurchaseTopProductsChart = BuildChart(PurchasePerProduct.Take(8).ToList(), o => (double)o.GrossAmount);
    }

    private static List<ProductInsightRow> BuildProductRows(List<Order> orders)
    {
        return orders
            .SelectMany(o => o.Lines.Select(l => new { l, o.Id }))
            .GroupBy(x => new { x.l.ProductId, x.l.ProductName, x.l.SKU })
            .Select(g => new ProductInsightRow
            {
                ProductName = g.Key.ProductName,
                SKU         = g.Key.SKU,
                TotalQty    = g.Sum(x => x.l.Quantity),
                NetAmount   = g.Sum(x => x.l.NetAmount),
                GrossAmount = g.Sum(x => x.l.GrossAmount),
                OrderCount  = g.Select(x => x.Id).Distinct().Count(),
            })
            .OrderByDescending(r => r.GrossAmount)
            .ToList();
    }

    private static List<ChartBar> BuildChart(List<ProductInsightRow> rows, System.Func<ProductInsightRow, double> valueSelector)
    {
        var max = rows.Any() ? rows.Max(valueSelector) : 1;
        return rows
            .Select((r, i) => new ChartBar(
                r.ProductName.Length > 24 ? r.ProductName[..24] + "…" : r.ProductName,
                valueSelector(r),
                max,
                Colors[i % Colors.Length]))
            .ToList();
    }
}
