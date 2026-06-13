using System;
using System.IO;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;

namespace WMS.ViewModels.Pages;

public partial class SupplierColumnConfig : ObservableObject
{
    private static readonly string _filePath =
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "supplier_columns.json");

    [ObservableProperty] private bool _showShortName   = false;
    [ObservableProperty] private bool _showTaxId       = true;
    [ObservableProperty] private bool _showEmail       = true;
    [ObservableProperty] private bool _showPhone       = false;
    [ObservableProperty] private bool _showCity        = true;
    [ObservableProperty] private bool _showCountry     = false;
    [ObservableProperty] private bool _showPaymentDays = true;
    [ObservableProperty] private bool _showWebsite     = false;
    [ObservableProperty] private bool _showBankAccount = false;
    [ObservableProperty] private bool _showIsActive    = true;

    public static SupplierColumnConfig Load()
    {
        try { if (File.Exists(_filePath)) return JsonSerializer.Deserialize<SupplierColumnConfig>(File.ReadAllText(_filePath)) ?? new(); } catch { }
        return new();
    }
    public void Save()
    {
        try { File.WriteAllText(_filePath, JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true })); } catch { }
    }
}
