using System.IO;
using System.Reflection;
using Avalonia;
using Avalonia.Styling;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WMS.Models;
using WMS.Services;

namespace WMS.ViewModels.Pages;

public partial class SettingsPageViewModel : ViewModelBase
{
    public string DatabasePath { get; } =
        Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "wms.db");

    public string AppVersion { get; } =
        Assembly.GetExecutingAssembly().GetName().Version?.ToString(3) ?? "1.0.0";

    [ObservableProperty] private bool   _isDarkTheme;
    [ObservableProperty] private string _saveMessage = "";

    // ── Company fields ───────────────────────────────────────────────────────
    [ObservableProperty] private string _companyName     = "";
    [ObservableProperty] private string _legalForm       = "";
    [ObservableProperty] private string _shortName       = "";
    [ObservableProperty] private string _taxId           = "";
    [ObservableProperty] private string _vatNumber       = "";
    [ObservableProperty] private string _regon           = "";
    [ObservableProperty] private string _krs             = "";
    [ObservableProperty] private string _street          = "";
    [ObservableProperty] private string _city            = "";
    [ObservableProperty] private string _postalCode      = "";
    [ObservableProperty] private string _state           = "";
    [ObservableProperty] private string _country         = "PL";
    [ObservableProperty] private string _phone           = "";
    [ObservableProperty] private string _email           = "";
    [ObservableProperty] private string _website         = "";
    [ObservableProperty] private string _bankName        = "";
    [ObservableProperty] private string _bankAccount     = "";
    [ObservableProperty] private string _bankSwift       = "";
    [ObservableProperty] private string _currency        = "PLN";
    [ObservableProperty] private int    _paymentTermDays = 14;
    [ObservableProperty] private string _invoiceFooter   = "";

    public SettingsPageViewModel()
    {
        _isDarkTheme = Application.Current?.RequestedThemeVariant == ThemeVariant.Dark;
        Load();
    }

    private void Load()
    {
        var s       = AppServices.CompanySettings.Load();
        CompanyName     = s.CompanyName;
        LegalForm       = s.LegalForm       ?? "";
        ShortName       = s.ShortName       ?? "";
        TaxId           = s.TaxId           ?? "";
        VatNumber       = s.VatNumber       ?? "";
        Regon           = s.Regon           ?? "";
        Krs             = s.Krs             ?? "";
        Street          = s.Street          ?? "";
        City            = s.City            ?? "";
        PostalCode      = s.PostalCode      ?? "";
        State           = s.State           ?? "";
        Country         = s.Country         ?? "PL";
        Phone           = s.Phone           ?? "";
        Email           = s.Email           ?? "";
        Website         = s.Website         ?? "";
        BankName        = s.BankName        ?? "";
        BankAccount     = s.BankAccount     ?? "";
        BankSwift       = s.BankSwift       ?? "";
        Currency        = s.Currency;
        PaymentTermDays = s.PaymentTermDays;
        InvoiceFooter   = s.InvoiceFooter   ?? "";
    }

    [RelayCommand]
    private void SaveCompany()
    {
        SaveMessage = "";

        if (string.IsNullOrWhiteSpace(CompanyName))
        {
            SaveMessage = "Company name is required.";
            return;
        }

        AppServices.CompanySettings.Save(new CompanySettings
        {
            CompanyName     = CompanyName.Trim(),
            LegalForm       = Nullable(LegalForm),
            ShortName       = Nullable(ShortName),
            TaxId           = Nullable(TaxId),
            VatNumber       = Nullable(VatNumber),
            Regon           = Nullable(Regon),
            Krs             = Nullable(Krs),
            Street          = Nullable(Street),
            City            = Nullable(City),
            PostalCode      = Nullable(PostalCode),
            State           = Nullable(State),
            Country         = string.IsNullOrWhiteSpace(Country) ? "PL" : Country.Trim(),
            Phone           = Nullable(Phone),
            Email           = Nullable(Email),
            Website         = Nullable(Website),
            BankName        = Nullable(BankName),
            BankAccount     = Nullable(BankAccount),
            BankSwift       = Nullable(BankSwift),
            Currency        = string.IsNullOrWhiteSpace(Currency) ? "PLN" : Currency.Trim(),
            PaymentTermDays = PaymentTermDays,
            InvoiceFooter   = Nullable(InvoiceFooter),
        });

        SaveMessage = "Saved successfully.";
    }

    partial void OnIsDarkThemeChanged(bool value)
    {
        if (Application.Current is { } app)
            app.RequestedThemeVariant = value ? ThemeVariant.Dark : ThemeVariant.Light;
    }

    private static string? Nullable(string s) => string.IsNullOrWhiteSpace(s) ? null : s.Trim();
}
