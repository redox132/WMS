using System.Collections.ObjectModel;
using WMS.Models;

namespace WMS.ViewModels.Pages;

public partial class ProductsPageViewModel : ViewModelBase
{
    public ObservableCollection<Product> Products { get; } = new()
    {
        new Product { Id=1, Name="Śruba M8 x 40 (opak. 500 szt.)", SKU="SR-M8-40-500", Barcode="5901234567890", Unit="opak.", Category="Złączniki", PurchasePrice=38.50m, SalePrice=54.00m, StockLevel=1200, MinStockLevel=100 },
        new Product { Id=2, Name="Nakrętka M8 (opak. 1000 szt.)", SKU="NK-M8-1000", Barcode="5901234567891", Unit="opak.", Category="Złączniki", PurchasePrice=22.00m, SalePrice=34.00m, StockLevel=340, MinStockLevel=50 },
        new Product { Id=3, Name="Rura PVC 50mm x 3m", SKU="PP-50-3M", Barcode="5901234567892", Unit="szt.", Category="Rury", PurchasePrice=18.90m, SalePrice=28.50m, StockLevel=12, MinStockLevel=20 },
        new Product { Id=4, Name="Opaska zaciskowa 200mm (opak. 100 szt.)", SKU="OZ-200-100", Barcode="5901234567893", Unit="opak.", Category="Opaski", PurchasePrice=8.40m, SalePrice=14.00m, StockLevel=5800, MinStockLevel=200 },
        new Product { Id=5, Name="Kabel YDY 3x1,5mm² (rolka 100m)", SKU="KAB-YDY-315", Barcode="5901234567894", Unit="rolka", Category="Kable", PurchasePrice=210.00m, SalePrice=285.00m, StockLevel=8, MinStockLevel=10 },
        new Product { Id=6, Name="Zawór kulowy 1/2\" DN15", SKU="ZK-12-DN15", Barcode="5901234567895", Unit="szt.", Category="Armatura", PurchasePrice=12.30m, SalePrice=19.80m, StockLevel=420, MinStockLevel=30 },
        new Product { Id=7, Name="Uszczelka płaska 50mm (opak. 50 szt.)", SKU="US-50-50", Barcode="5901234567896", Unit="opak.", Category="Uszczelki", PurchasePrice=5.60m, SalePrice=9.20m, StockLevel=3, MinStockLevel=10 },
    };
}
