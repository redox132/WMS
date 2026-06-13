using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using NetSparkleUpdater;
using NetSparkleUpdater.Enums;
using NetSparkleUpdater.SignatureVerifiers;
using WMS.Views;

namespace WMS;

public partial class App : Application
{
    public static SparkleUpdater Sparkle { get; private set; } = null!;

    // ── TODO: replace with your real values before shipping ─────────────────
    private const string AppCastUrl = "http://localhost:8080/appcast.xml";
    private const string PublicKey  = "bGb0JUbj6y6b5SQ0babN/3dlAA212rkggghSr+U0h7M=";
    // ────────────────────────────────────────────────────────────────────────

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        WMS.Services.AppServices.Initialise();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();

            Sparkle = new SparkleUpdater(
                AppCastUrl,
                new Ed25519Checker(SecurityMode.Strict, PublicKey))
            {
                UIFactory = null,
                RelaunchAfterUpdate = false,
            };

            Sparkle.StartLoop(false);
        }

        base.OnFrameworkInitializationCompleted();
    }
}
