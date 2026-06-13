using Avalonia.Controls;
using Avalonia.Threading;
using NetSparkleUpdater.Enums;
using WMS.ViewModels;

namespace WMS.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel();
    }

    private void ExitMenu_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Close();
    }

    private async void CheckForUpdates_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var result = await App.Sparkle.CheckForUpdatesQuietly();

        // Always continue on the UI thread before touching any window
        await Dispatcher.UIThread.InvokeAsync(async () =>
        {
            if (result?.Status == UpdateStatus.UpdateAvailable)
            {
                await new UpdateDialog(result.Updates).ShowDialog(this);
                return;
            }

            string message = result?.Status switch
            {
                UpdateStatus.UpdateNotAvailable => "You are up to date. No new version is available.",
                UpdateStatus.UserSkipped        => "Update was skipped.",
                _                               => "Could not check for updates. Please check your internet connection.",
            };

            await new Window
            {
                Title = "Software Update",
                Width = 380,
                Height = 130,
                CanResize = false,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Content = new TextBlock
                {
                    Text = message,
                    TextWrapping = Avalonia.Media.TextWrapping.Wrap,
                    Margin = new Avalonia.Thickness(24),
                    VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                    FontSize = 14,
                }
            }.ShowDialog(this);
        });
    }
}
