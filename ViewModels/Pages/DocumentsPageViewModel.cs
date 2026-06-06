using System;
using System.Collections.ObjectModel;
using WMS.Models;

namespace WMS.ViewModels.Pages;

public partial class DocumentsPageViewModel : ViewModelBase
{
    public ObservableCollection<WarehouseDocument> Documents { get; } = new()
    {
        new WarehouseDocument
        {
            Id=1, Number="WZ/06/2026/001", Type=DocumentType.WZ, Status=DocumentStatus.Confirmed,
            Date=new DateTime(2026,6,6), WarehouseFrom="MAG-1", ContractorName="Acme Sp. z o.o.",
            Lines= new() { new DocumentLine { ProductName="Śruba M8 x 40 (opak. 500 szt.)", SKU="SR-M8-40-500", Quantity=120, Unit="opak.", UnitPrice=54.00m } }
        },
        new WarehouseDocument
        {
            Id=2, Number="PZ/05/2026/001", Type=DocumentType.PZ, Status=DocumentStatus.Confirmed,
            Date=new DateTime(2026,6,5), WarehouseTo="MAG-1", ContractorName="Metaltech S.A.",
            Lines= new() { new DocumentLine { ProductName="Nakrętka M8 (opak. 1000 szt.)", SKU="NK-M8-1000", Quantity=500, Unit="opak.", UnitPrice=22.00m } }
        },
        new WarehouseDocument
        {
            Id=3, Number="MM/06/2026/001", Type=DocumentType.MM, Status=DocumentStatus.Confirmed,
            Date=new DateTime(2026,6,5), WarehouseFrom="MAG-1", WarehouseTo="MAG-2",
            Lines= new() { new DocumentLine { ProductName="Rura PVC 50mm x 3m", SKU="PP-50-3M", Quantity=80, Unit="szt.", UnitPrice=28.50m } }
        },
        new WarehouseDocument
        {
            Id=4, Number="RW/04/2026/001", Type=DocumentType.RW, Status=DocumentStatus.Confirmed,
            Date=new DateTime(2026,6,4), WarehouseFrom="MAG-1",
            Lines= new() { new DocumentLine { ProductName="Opaska zaciskowa 200mm (opak. 100 szt.)", SKU="OZ-200-100", Quantity=10, Unit="opak.", UnitPrice=14.00m } }
        },
        new WarehouseDocument
        {
            Id=5, Number="PW/04/2026/001", Type=DocumentType.PW, Status=DocumentStatus.Confirmed,
            Date=new DateTime(2026,6,3), WarehouseTo="MAG-2",
            Lines= new() { new DocumentLine { ProductName="Uszczelka płaska 50mm (opak. 50 szt.)", SKU="US-50-50", Quantity=5, Unit="opak.", UnitPrice=9.20m } }
        },
        new WarehouseDocument
        {
            Id=6, Number="WZ/03/2026/001", Type=DocumentType.WZ, Status=DocumentStatus.Draft,
            Date=new DateTime(2026,6,3), WarehouseFrom="MAG-1", ContractorName="BuildRight Sp. z o.o.",
            Lines= new() { new DocumentLine { ProductName="Zawór kulowy 1/2\" DN15", SKU="ZK-12-DN15", Quantity=50, Unit="szt.", UnitPrice=19.80m } }
        },
    };
}
