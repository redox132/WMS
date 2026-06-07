using System;
using CommunityToolkit.Mvvm.ComponentModel;
using WMS.Models;
using WMS.Services;

namespace WMS.ViewModels.Dialogs;

public partial class AddProductDialogViewModel : ObservableObject
{
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

    public bool Save()
    {
        ErrorMessage = "";

        if (string.IsNullOrWhiteSpace(Name))  { ErrorMessage = "Name is required."; return false; }
        if (string.IsNullOrWhiteSpace(Sku))   { ErrorMessage = "SKU is required.";  return false; }

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

        try
        {
            AppServices.Products.Insert(product);
            return true;
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message.Contains("UNIQUE") ? "A product with this SKU already exists." : ex.Message;
            return false;
        }
    }
}
