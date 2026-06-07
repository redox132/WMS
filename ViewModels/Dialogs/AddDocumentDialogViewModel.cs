using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using WMS.Models;
using WMS.Services;

namespace WMS.ViewModels.Dialogs;

public partial class AddDocumentDialogViewModel : ObservableObject
{
    [ObservableProperty] private DocumentType _documentType = DocumentType.PZ;
    [ObservableProperty] private Warehouse? _selectedWarehouse;
    [ObservableProperty] private string _contractorName = "";
    [ObservableProperty] private string _externalRef = "";
    [ObservableProperty] private string _notes = "";
    [ObservableProperty] private string _errorMessage = "";

    public List<Warehouse> Warehouses { get; }
    public DocumentType[] DocumentTypeOptions { get; } = [DocumentType.WZ, DocumentType.PZ, DocumentType.MM, DocumentType.PW, DocumentType.RW];

    public AddDocumentDialogViewModel()
    {
        Warehouses = AppServices.Warehouses.GetAll().Where(w => w.IsActive).OrderBy(w => w.Name).ToList();
        SelectedWarehouse = Warehouses.FirstOrDefault();
    }

    public bool Save()
    {
        ErrorMessage = "";

        var now    = DateTime.UtcNow;
        var prefix = DocumentType.ToString();
        var number = $"{prefix}/{now:yyyy}/{now:MM}/{now:dd}/{now.Ticks % 10000:D4}";

        var doc = new WarehouseDocument
        {
            Number            = number,
            Type              = DocumentType,
            Status            = DocumentStatus.Draft,
            Date              = DateTime.UtcNow,
            WarehouseFromId   = DocumentType is DocumentType.WZ or DocumentType.MM or DocumentType.RW ? SelectedWarehouse?.Id : null,
            WarehouseFromName = DocumentType is DocumentType.WZ or DocumentType.MM or DocumentType.RW ? SelectedWarehouse?.Name : null,
            WarehouseToId     = DocumentType is DocumentType.PZ or DocumentType.MM or DocumentType.PW ? SelectedWarehouse?.Id : null,
            WarehouseToName   = DocumentType is DocumentType.PZ or DocumentType.MM or DocumentType.PW ? SelectedWarehouse?.Name : null,
            ContractorName    = string.IsNullOrWhiteSpace(ContractorName) ? null : ContractorName.Trim(),
            ExternalRef       = string.IsNullOrWhiteSpace(ExternalRef) ? null : ExternalRef.Trim(),
            Notes             = string.IsNullOrWhiteSpace(Notes) ? null : Notes.Trim(),
        };

        try
        {
            AppServices.Documents.Insert(doc);
            return true;
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            return false;
        }
    }
}
