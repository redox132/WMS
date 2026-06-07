using System;
using System.Collections.Generic;
using System.Linq;
using WMS.Models;
using WMS.Services;

namespace WMS.ViewModels.Pages;

public partial class DashboardPageViewModel : ViewModelBase
{
    // ── Greeting ─────────────────────────────────────────────────────────────
    public string Greeting  { get; } = $"Good {TimeOfDay()}, here's today's overview";
    public string TodayDate { get; } = DateTime.Now.ToString("dddd, d MMMM yyyy");

    // ── KPI Cards ────────────────────────────────────────────────────────────
    public int     TotalProducts  { get; }
    public int     OpenOrders     { get; }
    public int     TotalCustomers { get; }
    public int     LowStockItems  { get; }
    public decimal TotalStockValue { get; }

    // ── Low-Stock Alert List ─────────────────────────────────────────────────
    public List<Product> LowStockProducts { get; }

    // ── Mini Chart: stock units by category ──────────────────────────────────
    public List<ChartBar> StockByCategory { get; }

    // ── Recent Activity ──────────────────────────────────────────────────────
    public List<ActivityItem> RecentActivity { get; }

    // ── Quick Stats ──────────────────────────────────────────────────────────
    public int DocumentsToday { get; }
    public int OrdersThisWeek { get; }

    public DashboardPageViewModel()
    {
        var products  = AppServices.Products.GetAll();
        var orders    = AppServices.Orders.GetAll();
        var docs      = AppServices.Documents.GetAll();

        TotalProducts  = products.Count;
        OpenOrders     = AppServices.Orders.CountOpen();
        TotalCustomers = AppServices.Customers.CountActive();
        LowStockItems  = products.Count(p => p.IsLowStock);
        TotalStockValue = products.Sum(p => p.StockLevel * p.SalePrice1);

        DocumentsToday = docs.Count(d => d.Date.Date == DateTime.Today);
        OrdersThisWeek = orders.Count(o => o.OrderDate >= DateTime.Today.AddDays(-7));

        // Low-stock items (max 6 shown)
        LowStockProducts = products
            .Where(p => p.IsLowStock)
            .OrderBy(p => p.StockLevel)
            .Take(6)
            .ToList();

        // Mini chart: stock by category (top 6)
        var byCat    = products
            .GroupBy(p => p.CategoryName ?? "Other")
            .Select(g => new { Label = g.Key, Value = g.Sum(p => (double)p.StockLevel) })
            .OrderByDescending(x => x.Value).Take(6).ToList();
        var maxCat   = byCat.Any() ? byCat.Max(x => x.Value) : 1;
        var colors   = new[] { "#1565C0","#2E7D32","#E65100","#6A1B9A","#0277BD","#558B2F" };
        StockByCategory = byCat
            .Select((x, i) => new ChartBar(x.Label, x.Value, maxCat, colors[i % colors.Length]))
            .ToList();

        // Recent activity: last 5 docs + last 3 orders, sorted by date
        var activity = new List<ActivityItem>();
        foreach (var d in docs.OrderByDescending(d => d.Date).Take(5))
            activity.Add(new ActivityItem
            {
                Text = $"{d.Type}  {d.Number}  —  {d.MovementDescription}",
                Time = FormatRelative(d.Date),
                Color = d.TypeColor
            });
        foreach (var o in orders.OrderByDescending(o => o.OrderDate).Take(3))
            activity.Add(new ActivityItem
            {
                Text = $"{o.TypeLabel}  {o.Number}  —  {o.CustomerName}  ({o.GrossAmount:N2} {o.Currency})",
                Time = FormatRelative(o.OrderDate),
                Color = o.TypeColor
            });

        RecentActivity = activity.Take(8).ToList();
    }

    private static string TimeOfDay()
    {
        var h = DateTime.Now.Hour;
        if (h < 12) return "morning";
        if (h < 17) return "afternoon";
        return "evening";
    }

    private static string FormatRelative(DateTime dt)
    {
        var diff = DateTime.UtcNow - dt.ToUniversalTime();
        if (diff.TotalMinutes < 2)  return "just now";
        if (diff.TotalMinutes < 60) return $"{(int)diff.TotalMinutes} min ago";
        if (diff.TotalHours   < 24) return $"{(int)diff.TotalHours} hr ago";
        return dt.ToString("dd MMM");
    }
}
