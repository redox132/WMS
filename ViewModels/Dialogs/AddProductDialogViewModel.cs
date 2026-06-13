using System;
using CommunityToolkit.Mvvm.ComponentModel;
using WMS.Models;
using WMS.Services;

namespace WMS.ViewModels.Dialogs;

public partial class AddProductDialogViewModel : ObservableObject
{
    private readonly int? _editId;

    public string Title => _editId.HasValue ? "Edit Product" : "New Product";

    [ObservableProperty] private string _name = "";
    [ObservableProperty] private string _sku = "";
    [ObservableProperty] private string _categoryName = "";
    [ObservableProperty] private string _unit = "pcs";
    [ObservableProperty] private decimal _stockLevel;
    [ObservableProperty] private decimal _minStockLevel;
    [ObservableProperty] private decimal _purchasePrice;
    [ObservableProperty] private decimal _salePrice1;
    [ObservableProperty] private decimal _vatRateSale = 23;
    [ObservableProperty] private string _warehouseLocation = "";
    [ObservableProperty] private bool _isService;
    [ObservableProperty] private string _errorMessage = "";

    public AddProductDialogViewModel() { }

    public AddProductDialogViewModel(Product existing)
    {
        _editId           = existing.Id;
        Name              = existing.Name;
        Sku               = existing.SKU;
        CategoryName      = existing.CategoryName ?? "";
        Unit              = existing.Unit;
        StockLevel        = existing.StockLevel;
        MinStockLevel     = existing.MinStockLevel;
        PurchasePrice     = existing.PurchasePrice;
        SalePrice1        = existing.SalePrice1;
        VatRateSale       = existing.VatRateSale;
        WarehouseLocation = existing.WarehouseLocation ?? "";
        IsService         = existing.IsService;
    }

    public bool Save()
    {
        ErrorMessage = "";

        if (string.IsNullOrWhiteSpace(Name))       { ErrorMessage = "Name is required.";                    return false; }
        if (string.IsNullOrWhiteSpace(Sku))        { ErrorMessage = "SKU is required.";                     return false; }
        if (SalePrice1 < 0)                        { ErrorMessage = "Sale price cannot be negative.";        return false; }
        if (PurchasePrice < 0)                     { ErrorMessage = "Purchase price cannot be negative.";    return false; }
        if (VatRateSale < 0 || VatRateSale > 100)  { ErrorMessage = "VAT rate must be between 0 and 100.";  return false; }

        try
        {
            if (_editId.HasValue)
            {
                var existing = AppServices.Products.GetById(_editId.Value);
                if (existing == null) { ErrorMessage = "Product not found."; return false; }

                existing.Name              = Name.Trim();
                existing.SKU               = Sku.Trim();
                existing.CategoryName      = string.IsNullOrWhiteSpace(CategoryName) ? null : CategoryName.Trim();
                existing.Unit              = string.IsNullOrWhiteSpace(Unit) ? "pcs" : Unit.Trim();
                existing.StockLevel        = StockLevel;
                existing.MinStockLevel     = MinStockLevel;
                existing.PurchasePrice     = PurchasePrice;
                existing.SalePrice1        = SalePrice1;
                existing.VatRateSale       = VatRateSale;
                existing.WarehouseLocation = string.IsNullOrWhiteSpace(WarehouseLocation) ? null : WarehouseLocation.Trim();
                existing.IsService         = IsService;
                existing.UpdatedAt         = DateTime.UtcNow;
                AppServices.Products.Update(existing);
            }
            else
            {
                var product = new Product
                {
                    Name              = Name.Trim(),
                    SKU               = Sku.Trim(),
                    CategoryName      = string.IsNullOrWhiteSpace(CategoryName) ? null : CategoryName.Trim(),
                    Unit              = string.IsNullOrWhiteSpace(Unit) ? "pcs" : Unit.Trim(),
                    StockLevel        = StockLevel,
                    MinStockLevel     = MinStockLevel,
                    PurchasePrice     = PurchasePrice,
                    SalePrice1        = SalePrice1,
                    VatRateSale       = VatRateSale,
                    VatRatePurchase   = 23,
                    WarehouseLocation = string.IsNullOrWhiteSpace(WarehouseLocation) ? null : WarehouseLocation.Trim(),
                    IsService         = IsService,
                    IsActive          = true,
                };
                AppServices.Products.Insert(product);
            }
            return true;
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message.Contains("UNIQUE") ? "A product with this SKU already exists." : ex.Message;
            return false;
        }
    }
}
