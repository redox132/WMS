using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Avalonia.Controls;
using Avalonia.Threading;
using NetSparkleUpdater;
using NetSparkleUpdater.Events;

namespace WMS.Views;

public partial class UpdateDialog : Window
{
    private readonly List<AppCastItem> _updates;
    private string? _downloadPath;
    private bool _readyToInstall;

    public UpdateDialog(List<AppCastItem> updates)
    {
        InitializeComponent();
        _updates = updates;

        var version    = updates.Count > 0 ? updates[0].Version : "?";
        var current    = App.Sparkle.Configuration?.InstalledVersion ?? "1.0.0";
        MessageText.Text = $"Version {version} is available (you have {current}).\n\nClick Download to get the update.";

        App.Sparkle.DownloadMadeProgress += OnDownloadProgress;
        App.Sparkle.DownloadFinished     += OnDownloadFinished;
        App.Sparkle.DownloadHadError     += OnDownloadError;
    }

    private async void Action_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (_readyToInstall)
        {
            ActionButton.IsEnabled  = false;
            CancelButton.IsEnabled  = false;
            MessageText.Text        = "Applying update…";
            Detach();
            ApplyUpdate(_downloadPath!);
            return;
        }

        ActionButton.IsEnabled = false;
        ProgressBar.IsVisible  = true;
        ProgressText.IsVisible = true;
        MessageText.Text       = "Downloading…";

        await App.Sparkle.InitAndBeginDownload(_updates[0]);
    }

    private void ApplyUpdate(string zipPath)
    {
        try
        {
            var appDir  = AppDomain.CurrentDomain.BaseDirectory;
            var tempDir = Path.Combine(Path.GetTempPath(), "WMS_update_" + Guid.NewGuid().ToString("N")[..8]);

            ZipFile.ExtractToDirectory(zipPath, tempDir, overwriteFiles: true);

            var sourceDir = tempDir;

            // Write a shell script that swaps files after the app quits
            var scriptPath = Path.Combine(Path.GetTempPath(), "wms_apply_update.sh");
            var scriptLines = new System.Text.StringBuilder();
            scriptLines.AppendLine("#!/bin/bash");
            scriptLines.AppendLine("sleep 1"); // wait for app to quit

            foreach (var file in Directory.EnumerateFiles(sourceDir, "*", SearchOption.AllDirectories))
            {
                var relative = Path.GetRelativePath(sourceDir, file);
                var dest     = Path.Combine(appDir, relative).Replace("'", "'\\''");
                var src      = file.Replace("'", "'\\''");
                scriptLines.AppendLine($"mkdir -p '{Path.GetDirectoryName(dest)}'");
                scriptLines.AppendLine($"cp -f '{src}' '{dest}'");
            }

            scriptLines.AppendLine($"chmod +x '{Path.Combine(appDir, "WMS").Replace("'", "'\\''")}'");
            scriptLines.AppendLine($"open '{appDir.TrimEnd('/').Replace("MacOS", "").TrimEnd('/')}'.app");
            scriptLines.AppendLine($"rm -- \"$0\"");

            File.WriteAllText(scriptPath, scriptLines.ToString());
            System.Diagnostics.Process.Start("chmod", $"+x \"{scriptPath}\"")?.WaitForExit();

            // Launch the script and quit
            System.Diagnostics.Process.Start("/bin/bash", $"\"{scriptPath}\"");

            Dispatcher.UIThread.Post(() =>
            {
                MessageText.Text       = "Update ready! The app will restart automatically.";
                ActionButton.Content   = "Quit & Install";
                ActionButton.IsEnabled = true;
                ActionButton.Click    -= Action_Click;
                ActionButton.Click    += (_, _) =>
                {
                    Close();
                    if (Avalonia.Application.Current?.ApplicationLifetime is
                        Avalonia.Controls.ApplicationLifetimes.IClassicDesktopStyleApplicationLifetime life)
                        life.Shutdown();
                };
            });
        }
        catch (Exception ex)
        {
            Dispatcher.UIThread.Post(() =>
            {
                MessageText.Text       = $"Install failed: {ex.Message}";
                ActionButton.IsEnabled = true;
            });
        }
    }

    private void Cancel_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Detach();
        Close();
    }

    private void Detach()
    {
        App.Sparkle.DownloadMadeProgress -= OnDownloadProgress;
        App.Sparkle.DownloadFinished     -= OnDownloadFinished;
        App.Sparkle.DownloadHadError     -= OnDownloadError;
    }

    private void OnDownloadProgress(object sender, AppCastItem item, ItemDownloadProgressEventArgs e)
    {
        Dispatcher.UIThread.Post(() =>
        {
            ProgressBar.Value = e.ProgressPercentage;
            ProgressText.Text = $"{e.ProgressPercentage}%";
        });
    }

    private void OnDownloadFinished(AppCastItem item, string path)
    {
        Dispatcher.UIThread.Post(() =>
        {
            _downloadPath          = path;
            _readyToInstall        = true;
            ProgressBar.Value      = 100;
            ProgressText.Text      = "100%";
            MessageText.Text       = "Download complete. Click Install to apply the update.";
            ActionButton.Content   = "Install";
            ActionButton.IsEnabled = true;
        });
    }

    private void OnDownloadError(AppCastItem item, string path, Exception exception)
    {
        Dispatcher.UIThread.Post(() =>
        {
            MessageText.Text       = $"Download failed: {exception.Message}";
            ProgressBar.IsVisible  = false;
            ProgressText.IsVisible = false;
            ActionButton.IsEnabled = true;
        });
    }
}
