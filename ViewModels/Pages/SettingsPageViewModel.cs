using System.IO;
using System.Reflection;
using Avalonia;
using Avalonia.Styling;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace WMS.ViewModels.Pages;

public partial class SettingsPageViewModel : ViewModelBase
{
    public string DatabasePath { get; } =
        Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "wms.db");

    public string AppVersion { get; } =
        Assembly.GetExecutingAssembly().GetName().Version?.ToString(3) ?? "1.0.0";

    [ObservableProperty] private bool _isDarkTheme;

    public SettingsPageViewModel()
    {
        _isDarkTheme = Application.Current?.RequestedThemeVariant == ThemeVariant.Dark;
    }

    partial void OnIsDarkThemeChanged(bool value)
    {
        if (Application.Current is { } app)
            app.RequestedThemeVariant = value ? ThemeVariant.Dark : ThemeVariant.Light;
    }
}
