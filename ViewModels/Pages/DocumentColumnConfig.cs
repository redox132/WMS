using System;
using System.IO;
using System.Text.Json;
using CommunityToolkit.Mvvm.ComponentModel;

namespace WMS.ViewModels.Pages;

public partial class DocumentColumnConfig : ObservableObject
{
    private static readonly string _filePath =
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "document_columns.json");

    [ObservableProperty] private bool _showDate        = true;
    [ObservableProperty] private bool _showContractor  = true;
    [ObservableProperty] private bool _showMovement    = true;
    [ObservableProperty] private bool _showExternalRef = false;
    [ObservableProperty] private bool _showNetAmount   = false;
    [ObservableProperty] private bool _showGrossAmount = true;
    [ObservableProperty] private bool _showStatus      = true;
    [ObservableProperty] private bool _showCreatedBy   = false;

    public static DocumentColumnConfig Load()
    {
        try { if (File.Exists(_filePath)) return JsonSerializer.Deserialize<DocumentColumnConfig>(File.ReadAllText(_filePath)) ?? new(); } catch { }
        return new();
    }
    public void Save()
    {
        try { File.WriteAllText(_filePath, JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true })); } catch { }
    }
}
