using System;
using System.IO;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;

namespace WMS.ViewModels.Pages;

public partial class ProductColumnConfig : ObservableObject
{
    private static readonly string _filePath =
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "product_columns.json");

    // Always visible: Name
    [ObservableProperty] private bool _showBarcode       = true;
    [ObservableProperty] private bool _showSku           = true;
    [ObservableProperty] private bool _showCategory      = false;
    [ObservableProperty] private bool _showUnit          = true;
    [ObservableProperty] private bool _showStock         = true;
    [ObservableProperty] private bool _showMinStock      = false;
    [ObservableProperty] private bool _showPurchasePrice = true;
    [ObservableProperty] private bool _showSalePrice     = true;
    [ObservableProperty] private bool _showVatRate       = false;
    [ObservableProperty] private bool _showLocation      = false;
    [ObservableProperty] private bool _showStatus        = true;
    [ObservableProperty] private bool _showIsService     = false;
    [ObservableProperty] private bool _showWeight        = false;

    public static ProductColumnConfig Load()
    {
        try
        {
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                return JsonSerializer.Deserialize<ProductColumnConfig>(json) ?? new();
            }
        }
        catch { }
        return new();
    }

    public void Save()
    {
        try
        {
            var json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_filePath, json);
        }
        catch { }
    }
}
