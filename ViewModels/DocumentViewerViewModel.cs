using System.Collections.Generic;
using WMS.Models;
using WMS.Services;

namespace WMS.ViewModels;

public class DetailRow
{
    public string Label { get; init; } = "";
    public string Value { get; init; } = "";
}

public class DocumentViewerViewModel
{
    private readonly WarehouseDocument _doc;
    private readonly CompanySettings   _company;

    public DocumentViewerViewModel(WarehouseDocument doc)
    {
        _doc     = doc;
        _company = AppServices.CompanySettings.Load();
    }

    // ── Document identity ────────────────────────────────────────────────────
    public string Number          => _doc.Number;
    public string TypeLabel       => _doc.Type.ToString();
    public string TypeDescription => _doc.TypeDescription;
    public string TypeColor       => _doc.TypeColor;
    public string StatusLabel     => _doc.StatusLabel;
    public string StatusColor     => _doc.StatusColor;
    public string DateFormatted   => _doc.Date.ToLocalTime().ToString("dd MMM yyyy  HH:mm");

    // ── Movement labels ──────────────────────────────────────────────────────
    public string FromLabel => _doc.Type switch
    {
        DocumentType.PZ or DocumentType.PW => _doc.ContractorName ?? "—",
        _                                  => _doc.WarehouseFromName ?? "—",
    };

    public string ToLabel => _doc.Type switch
    {
        DocumentType.WZ or DocumentType.RW => _doc.ContractorName ?? "—",
        _                                  => _doc.WarehouseToName ?? "—",
    };

    public string ContractorLabel => _doc.ContractorName ?? "—";
    public string WarehouseLabel  => _doc.WarehouseFromName ?? _doc.WarehouseToName ?? "—";
    public string? ExternalRef    => _doc.ExternalRef;
    public bool HasExternalRef    => !string.IsNullOrWhiteSpace(_doc.ExternalRef);

    // ── Detail rows (left panel) ─────────────────────────────────────────────
    public List<DetailRow> DetailRows
    {
        get
        {
            var rows = new List<DetailRow>
            {
                new() { Label = "Document Type",  Value = _doc.TypeDescription },
                new() { Label = "Date",            Value = DateFormatted },
                new() { Label = "Status",          Value = _doc.StatusLabel },
            };

            if (!string.IsNullOrWhiteSpace(_doc.ContractorName))
                rows.Add(new() { Label = "Contractor",    Value = _doc.ContractorName });
            if (!string.IsNullOrWhiteSpace(_doc.WarehouseFromName))
                rows.Add(new() { Label = "From Warehouse", Value = _doc.WarehouseFromName });
            if (!string.IsNullOrWhiteSpace(_doc.WarehouseToName))
                rows.Add(new() { Label = "To Warehouse",   Value = _doc.WarehouseToName });
            if (!string.IsNullOrWhiteSpace(_doc.ExternalRef))
                rows.Add(new() { Label = "External Ref",   Value = _doc.ExternalRef });
            if (!string.IsNullOrWhiteSpace(_doc.LinkedOrderNumber))
                rows.Add(new() { Label = "Linked Order",   Value = _doc.LinkedOrderNumber });
            if (!string.IsNullOrWhiteSpace(_doc.LinkedInvoiceNumber))
                rows.Add(new() { Label = "Invoice",        Value = _doc.LinkedInvoiceNumber });
            if (!string.IsNullOrWhiteSpace(_doc.Carrier))
                rows.Add(new() { Label = "Carrier",        Value = _doc.Carrier });
            if (!string.IsNullOrWhiteSpace(_doc.TrackingNumber))
                rows.Add(new() { Label = "Tracking",       Value = _doc.TrackingNumber });
            if (!string.IsNullOrWhiteSpace(_doc.ConfirmedBy))
                rows.Add(new() { Label = "Confirmed By",   Value = _doc.ConfirmedBy });
            if (_doc.ConfirmedAt.HasValue)
                rows.Add(new() { Label = "Confirmed At",   Value = _doc.ConfirmedAt.Value.ToLocalTime().ToString("dd MMM yyyy HH:mm") });
            if (!string.IsNullOrWhiteSpace(_doc.CreatedBy))
                rows.Add(new() { Label = "Created By",     Value = _doc.CreatedBy });

            return rows;
        }
    }

    // ── Lines & totals ───────────────────────────────────────────────────────
    public List<DocumentLine> Lines       => _doc.Lines;
    public decimal            NetAmount   => _doc.NetAmount;
    public decimal            VatAmount   => _doc.VatAmount;
    public decimal            GrossAmount => _doc.GrossAmount;

    // ── Notes ────────────────────────────────────────────────────────────────
    public string? Notes    => string.IsNullOrWhiteSpace(_doc.Notes) ? _doc.InternalNotes : _doc.Notes;
    public bool    HasNotes => !string.IsNullOrWhiteSpace(Notes);

    // ── Company (receipt right panel) ────────────────────────────────────────
    public string CompanyName => string.IsNullOrWhiteSpace(_company.CompanyName)
        ? "Your Company"
        : $"{_company.CompanyName}{(!string.IsNullOrWhiteSpace(_company.LegalForm) ? " " + _company.LegalForm : "")}";

    public string CompanyAddress
    {
        get
        {
            var parts = new List<string>();
            if (!string.IsNullOrWhiteSpace(_company.Street))     parts.Add(_company.Street);
            if (!string.IsNullOrWhiteSpace(_company.PostalCode) || !string.IsNullOrWhiteSpace(_company.City))
                parts.Add($"{_company.PostalCode} {_company.City}".Trim());
            if (!string.IsNullOrWhiteSpace(_company.Country) && _company.Country != "PL")
                parts.Add(_company.Country);
            return string.Join(", ", parts);
        }
    }

    public string CompanyTaxLine
    {
        get
        {
            var parts = new List<string>();
            if (!string.IsNullOrWhiteSpace(_company.TaxId))    parts.Add($"NIP: {_company.TaxId}");
            if (!string.IsNullOrWhiteSpace(_company.VatNumber)) parts.Add($"VAT: {_company.VatNumber}");
            return string.Join("  ·  ", parts);
        }
    }

    public string CompanyContactLine
    {
        get
        {
            var parts = new List<string>();
            if (!string.IsNullOrWhiteSpace(_company.Phone))   parts.Add(_company.Phone);
            if (!string.IsNullOrWhiteSpace(_company.Email))   parts.Add(_company.Email);
            if (!string.IsNullOrWhiteSpace(_company.Website)) parts.Add(_company.Website);
            return string.Join("  ·  ", parts);
        }
    }

    public string? InvoiceFooter    => _company.InvoiceFooter;
    public bool    HasInvoiceFooter => !string.IsNullOrWhiteSpace(_company.InvoiceFooter);
}
