using System;
using System.IO;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;

namespace WMS.ViewModels.Pages;

public partial class OrderColumnConfig : ObservableObject
{
    private static readonly string _filePath =
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "order_columns.json");

    [ObservableProperty] private bool _showCustomer     = true;
    [ObservableProperty] private bool _showDate         = true;
    [ObservableProperty] private bool _showDueDate      = false;
    [ObservableProperty] private bool _showDeliveryDate = false;
    [ObservableProperty] private bool _showPayment      = false;
    [ObservableProperty] private bool _showCurrency     = false;
    [ObservableProperty] private bool _showNetAmount    = false;
    [ObservableProperty] private bool _showGrossAmount  = true;
    [ObservableProperty] private bool _showDiscount     = false;
    [ObservableProperty] private bool _showTracking     = false;
    [ObservableProperty] private bool _showStatus       = true;

    public static OrderColumnConfig Load()
    {
        try { if (File.Exists(_filePath)) return JsonSerializer.Deserialize<OrderColumnConfig>(File.ReadAllText(_filePath)) ?? new(); } catch { }
        return new();
    }
    public void Save()
    {
        try { File.WriteAllText(_filePath, JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true })); } catch { }
    }
}
