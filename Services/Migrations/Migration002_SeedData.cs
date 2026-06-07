using System;
using SQLite;
using WMS.Models;

namespace WMS.Services.Migrations;

/// <summary>Inserts baseline sample data so the app is usable out of the box.</summary>
public sealed class Migration002_SeedData : IMigration
{
    public int    Version     => 2;
    public string Description => "Seed — warehouses, customers, products, orders, warehouse documents";

    public void Up(SQLiteConnection db)
    {
        // ── Warehouses ────────────────────────────────────────────────────────
        db.Execute(@"INSERT OR IGNORE INTO Warehouses (Code,Name,City,Country,IsDefault,IsActive,CreatedAt)
                     VALUES ('WH-01','Main Warehouse','Warsaw','PL',1,1,?)", Now());
        db.Execute(@"INSERT OR IGNORE INTO Warehouses (Code,Name,City,Country,IsDefault,IsActive,CreatedAt)
                     VALUES ('WH-02','Secondary Warehouse','Kraków','PL',0,1,?)", Now());

        // ── Customers ─────────────────────────────────────────────────────────
        db.Execute(@"INSERT OR IGNORE INTO Customers
                     (Name,ShortName,Type,TaxId,Email,Phone,BillingCity,BillingCountry,
                      PaymentTermDays,PaymentMethod,PriceLevel,DefaultDiscount,CreditLimit,Currency,IsActive,CreatedAt,UpdatedAt)
                     VALUES
                     ('Acme sp. z o.o.','ACME',0,'1234567890','orders@acme.pl','+48 22 123 45 67','Warsaw','PL',
                      14,'Transfer',2,5,50000,'PLN',1,?,?)", Now(), Now());

        db.Execute(@"INSERT OR IGNORE INTO Customers
                     (Name,ShortName,Type,TaxId,Email,Phone,BillingCity,BillingCountry,
                      PaymentTermDays,PaymentMethod,PriceLevel,CreditLimit,Currency,IsActive,CreatedAt,UpdatedAt)
                     VALUES
                     ('BuildRight sp. z o.o.','BUILDRIGHT',0,'9876543210','office@buildright.pl','+48 12 987 65 43','Kraków','PL',
                      21,'Transfer',1,20000,'PLN',1,?,?)", Now(), Now());

        db.Execute(@"INSERT OR IGNORE INTO Customers
                     (Name,ShortName,Type,TaxId,VatNumber,Email,Phone,BillingCity,BillingCountry,
                      PaymentTermDays,PaymentMethod,PriceLevel,DefaultDiscount,Currency,IsActive,CreatedAt,UpdatedAt)
                     VALUES
                     ('TechMart GmbH','TECHMART',0,'DE123456789','DE123456789','orders@techmart.de','+49 30 12345','Berlin','DE',
                      30,'Transfer',3,3,'EUR',1,?,?)", Now(), Now());

        db.Execute(@"INSERT OR IGNORE INTO Customers
                     (Name,ShortName,Type,TaxId,Email,Phone,BillingCity,BillingCountry,
                      PaymentTermDays,PaymentMethod,Currency,IsActive,CreatedAt,UpdatedAt)
                     VALUES
                     ('Metaltech S.A.','METALTECH',1,'1122334455','sales@metaltech.pl','+48 61 555 44 33','Poznań','PL',
                      14,'Transfer','PLN',1,?,?)", Now(), Now());

        db.Execute(@"INSERT OR IGNORE INTO Customers
                     (Name,ShortName,Type,TaxId,Email,Phone,BillingCity,BillingCountry,
                      PaymentTermDays,PaymentMethod,Currency,IsActive,CreatedAt,UpdatedAt)
                     VALUES
                     ('GlobalFix sp. z o.o.','GLOBALFIX',2,'5566778899','info@globalfix.pl','+48 71 333 22 11','Wrocław','PL',
                      7,'Cash','PLN',0,?,?)", Now(), Now());

        db.Execute(@"INSERT OR IGNORE INTO Customers
                     (Name,ShortName,Type,TaxId,Email,Phone,BillingCity,BillingCountry,
                      PaymentTermDays,PaymentMethod,Currency,IsActive,CreatedAt,UpdatedAt)
                     VALUES
                     ('HydroPlast sp. j.','HYDROPLAST',1,'6677889900','contact@hydroplast.pl','+48 32 444 55 66','Katowice','PL',
                      21,'Transfer','PLN',1,?,?)", Now(), Now());

        // ── Products ──────────────────────────────────────────────────────────
        InsertProduct(db, "Steel Bolt M8 x 40 (box/500)",      "SR-M8-40-500", "5901234567890", "box",  "Fasteners", 38.50m, 54.00m, 50.00m, 1200, 100,  5000, 150,  500,  "A1-01");
        InsertProduct(db, "Hex Nut M8 (bag/1000)",             "NK-M8-1000",   "5901234567891", "bag",  "Fasteners", 22.00m, 34.00m, 30.00m, 340,  50,   3000, 80,   200,  "A1-02");
        InsertProduct(db, "PVC Pipe 50mm x 3m",               "PP-50-3M",     "5901234567892", "pcs",  "Pipes",     18.90m, 28.50m, null,   12,   20,   200,  25,   50,   "B3-01");
        InsertProduct(db, "Cable Tie 200mm (bag/100)",         "OZ-200-100",   "5901234567893", "bag",  "Fixings",   8.40m,  14.00m, 12.50m, 5800, 200,  20000,300,  1000, "A2-05");
        InsertProduct(db, "Cable YDY 3x1.5mm² (100m roll)",   "KAB-YDY-315",  "5901234567894", "roll", "Cables",    210.0m, 285.0m, null,   8,    10,   100,  12,   20,   "C1-01");
        InsertProduct(db, "Ball Valve 1/2\" DN15",              "ZK-12-DN15",   "5901234567895", "pcs",  "Fittings",  12.30m, 19.80m, 17.50m, 420,  30,   1000, 50,   100,  "B2-03");
        InsertProduct(db, "Flat Gasket 50mm (pack/50)",        "US-50-50",     "5901234567896", "pack", "Seals",     5.60m,  9.20m,  null,   3,    10,   500,  15,   50,   "A3-02");
        InsertProduct(db, "Mounting Bracket 60x40mm",          "BRK-6040",     "5901234567897", "pcs",  "Brackets",  3.20m,  5.80m,  5.20m,  900,  50,   5000, 80,   200,  "A4-01");

        // ── Orders ────────────────────────────────────────────────────────────
        db.Execute(@"INSERT OR IGNORE INTO Orders
                     (Number,Type,Status,OrderDate,DueDate,CustomerId,CustomerName,
                      PaymentMethod,PaymentStatus,Currency,ExchangeRate,ShippingMethod,ShippingCost,
                      NetAmount,VatAmount,GrossAmount,CreatedAt,UpdatedAt)
                     VALUES ('SO-2026-042',0,3,?,?,1,'Acme sp. z o.o.','Transfer',0,'PLN',1,'DPD',25,5284.55,1215.45,6500,?,?)",
            D(2026,6,6), D(2026,6,10), Now(), Now());

        db.Execute(@"INSERT OR IGNORE INTO Orders
                     (Number,Type,Status,OrderDate,DueDate,CustomerId,CustomerName,
                      PaymentMethod,PaymentStatus,Currency,ExchangeRate,ShippingMethod,
                      NetAmount,VatAmount,GrossAmount,CreatedAt,UpdatedAt)
                     VALUES ('SO-2026-041',0,4,?,?,2,'BuildRight sp. z o.o.','Transfer',2,'PLN',1,'GLS',
                      694.31,159.69,854,?,?)",
            D(2026,6,5), D(2026,6,9), Now(), Now());

        db.Execute(@"INSERT OR IGNORE INTO Orders
                     (Number,Type,Status,OrderDate,DueDate,CustomerId,CustomerName,
                      PaymentMethod,PaymentStatus,Currency,ExchangeRate,ShippingMethod,
                      NetAmount,VatAmount,GrossAmount,CreatedAt,UpdatedAt)
                     VALUES ('SO-2026-040',0,2,?,?,3,'TechMart GmbH','Transfer',0,'EUR',4.28,'FedEx',
                      8862.60,2038.40,10901,?,?)",
            D(2026,6,4), D(2026,6,8), Now(), Now());

        db.Execute(@"INSERT OR IGNORE INTO Orders
                     (Number,Type,Status,OrderDate,DueDate,CustomerId,CustomerName,
                      PaymentMethod,PaymentStatus,Currency,ExchangeRate,
                      NetAmount,VatAmount,GrossAmount,CreatedAt,UpdatedAt)
                     VALUES ('PO-2026-018',1,1,?,?,4,'Metaltech S.A.','Transfer',0,'PLN',1,
                      1100,253,1353,?,?)",
            D(2026,6,3), D(2026,6,12), Now(), Now());

        db.Execute(@"INSERT OR IGNORE INTO Orders
                     (Number,Type,Status,OrderDate,CustomerId,CustomerName,
                      PaymentMethod,PaymentStatus,Currency,ExchangeRate,
                      NetAmount,VatAmount,GrossAmount,CreatedAt,UpdatedAt)
                     VALUES ('SO-2026-039',0,4,?,5,'GlobalFix sp. z o.o.','Cash',2,'PLN',1,
                      227.64,52.36,280,?,?)",
            D(2026,6,3), Now(), Now());

        // ── Order Lines ───────────────────────────────────────────────────────
        db.Execute(@"INSERT OR IGNORE INTO OrderLines (OrderId,ProductId,ProductName,SKU,Quantity,Unit,UnitPrice,VatRate,NetAmount,VatAmount,GrossAmount)
                     VALUES (1,1,'Steel Bolt M8 x 40 (box/500)','SR-M8-40-500',120,'box',54,23,5284.55,1215.45,6500)");
        db.Execute(@"INSERT OR IGNORE INTO OrderLines (OrderId,ProductId,ProductName,SKU,Quantity,Unit,UnitPrice,VatRate,NetAmount,VatAmount,GrossAmount)
                     VALUES (2,3,'PVC Pipe 50mm x 3m','PP-50-3M',30,'pcs',28.5,23,694.31,159.69,854)");
        db.Execute(@"INSERT OR IGNORE INTO OrderLines (OrderId,ProductId,ProductName,SKU,Quantity,Unit,UnitPrice,VatRate,NetAmount,VatAmount,GrossAmount)
                     VALUES (3,5,'Cable YDY 3x1.5mm²','KAB-YDY-315',20,'roll',285,23,4634.15,1065.85,5700)");
        db.Execute(@"INSERT OR IGNORE INTO OrderLines (OrderId,ProductId,ProductName,SKU,Quantity,Unit,UnitPrice,VatRate,NetAmount,VatAmount,GrossAmount)
                     VALUES (3,6,'Ball Valve 1/2"" DN15','ZK-12-DN15',150,'pcs',19.8,23,2414.63,555.37,2970)");
        db.Execute(@"INSERT OR IGNORE INTO OrderLines (OrderId,ProductId,ProductName,SKU,Quantity,Unit,UnitPrice,VatRate,NetAmount,VatAmount,GrossAmount)
                     VALUES (4,2,'Hex Nut M8 (bag/1000)','NK-M8-1000',50,'bag',22,23,894.31,205.69,1100)");
        db.Execute(@"INSERT OR IGNORE INTO OrderLines (OrderId,ProductId,ProductName,SKU,Quantity,Unit,UnitPrice,VatRate,NetAmount,VatAmount,GrossAmount)
                     VALUES (5,4,'Cable Tie 200mm (bag/100)','OZ-200-100',20,'bag',14,23,227.64,52.36,280)");

        // ── Warehouse Documents ────────────────────────────────────────────────
        db.Execute(@"INSERT OR IGNORE INTO WarehouseDocuments
                     (Number,Type,Status,Date,WarehouseFromId,WarehouseFromName,ContractorId,ContractorName,
                      LinkedOrderNumber,TotalQuantity,NetAmount,VatAmount,GrossAmount,CreatedAt,UpdatedAt)
                     VALUES ('WZ/06/2026/001',0,1,?,1,'WH-01 Main',1,'Acme sp. z o.o.','SO-2026-042',120,5284.55,1215.45,6500,?,?)",
            D(2026,6,6), Now(), Now());

        db.Execute(@"INSERT OR IGNORE INTO WarehouseDocuments
                     (Number,Type,Status,Date,WarehouseToId,WarehouseToName,ContractorId,ContractorName,
                      TotalQuantity,NetAmount,VatAmount,GrossAmount,CreatedAt,UpdatedAt)
                     VALUES ('PZ/05/2026/001',1,1,?,1,'WH-01 Main',4,'Metaltech S.A.',500,11000,2530,13530,?,?)",
            D(2026,6,5), Now(), Now());

        db.Execute(@"INSERT OR IGNORE INTO WarehouseDocuments
                     (Number,Type,Status,Date,WarehouseFromId,WarehouseFromName,WarehouseToId,WarehouseToName,
                      TotalQuantity,NetAmount,VatAmount,GrossAmount,CreatedAt,UpdatedAt)
                     VALUES ('MM/06/2026/001',2,1,?,1,'WH-01 Main',2,'WH-02 Secondary',80,1463.41,336.59,1800,?,?)",
            D(2026,6,5), Now(), Now());

        db.Execute(@"INSERT OR IGNORE INTO WarehouseDocuments
                     (Number,Type,Status,Date,WarehouseFromId,WarehouseFromName,
                      TotalQuantity,NetAmount,VatAmount,GrossAmount,CreatedAt,UpdatedAt)
                     VALUES ('RW/04/2026/001',4,1,?,1,'WH-01 Main',10,68.29,15.71,84,?,?)",
            D(2026,6,4), Now(), Now());

        db.Execute(@"INSERT OR IGNORE INTO WarehouseDocuments
                     (Number,Type,Status,Date,WarehouseToId,WarehouseToName,
                      TotalQuantity,NetAmount,VatAmount,GrossAmount,CreatedAt,UpdatedAt)
                     VALUES ('PW/04/2026/001',3,1,?,2,'WH-02 Secondary',5,37.40,8.60,46,?,?)",
            D(2026,6,3), Now(), Now());

        db.Execute(@"INSERT OR IGNORE INTO WarehouseDocuments
                     (Number,Type,Status,Date,WarehouseFromId,WarehouseFromName,ContractorId,ContractorName,
                      TotalQuantity,NetAmount,VatAmount,GrossAmount,CreatedAt,UpdatedAt)
                     VALUES ('WZ/03/2026/001',0,0,?,1,'WH-01 Main',2,'BuildRight sp. z o.o.',50,804.88,185.12,990,?,?)",
            D(2026,6,3), Now(), Now());

        // ── Document Lines ─────────────────────────────────────────────────────
        db.Execute(@"INSERT OR IGNORE INTO DocumentLines (DocumentId,ProductId,ProductName,SKU,Quantity,Unit,UnitPrice,VatRate,NetAmount,VatAmount,GrossAmount,WarehouseLocation)
                     VALUES (1,1,'Steel Bolt M8 x 40 (box/500)','SR-M8-40-500',120,'box',54,23,5284.55,1215.45,6500,'A1-01')");
        db.Execute(@"INSERT OR IGNORE INTO DocumentLines (DocumentId,ProductId,ProductName,SKU,Quantity,Unit,UnitPrice,VatRate,NetAmount,VatAmount,GrossAmount,WarehouseLocation)
                     VALUES (2,2,'Hex Nut M8 (bag/1000)','NK-M8-1000',500,'bag',22,23,11000,2530,13530,'A1-02')");
        db.Execute(@"INSERT OR IGNORE INTO DocumentLines (DocumentId,ProductId,ProductName,SKU,Quantity,Unit,UnitPrice,VatRate,NetAmount,VatAmount,GrossAmount,WarehouseLocation)
                     VALUES (3,3,'PVC Pipe 50mm x 3m','PP-50-3M',80,'pcs',28.5,23,1463.41,336.59,1800,'B3-01')");
        db.Execute(@"INSERT OR IGNORE INTO DocumentLines (DocumentId,ProductId,ProductName,SKU,Quantity,Unit,UnitPrice,VatRate,NetAmount,VatAmount,GrossAmount,WarehouseLocation)
                     VALUES (4,4,'Cable Tie 200mm (bag/100)','OZ-200-100',10,'bag',14,23,68.29,15.71,84,'A2-05')");
        db.Execute(@"INSERT OR IGNORE INTO DocumentLines (DocumentId,ProductId,ProductName,SKU,Quantity,Unit,UnitPrice,VatRate,NetAmount,VatAmount,GrossAmount,WarehouseLocation)
                     VALUES (5,7,'Flat Gasket 50mm (pack/50)','US-50-50',5,'pack',9.2,23,37.40,8.60,46,'A3-02')");
        db.Execute(@"INSERT OR IGNORE INTO DocumentLines (DocumentId,ProductId,ProductName,SKU,Quantity,Unit,UnitPrice,VatRate,NetAmount,VatAmount,GrossAmount,WarehouseLocation)
                     VALUES (6,6,'Ball Valve 1/2"" DN15','ZK-12-DN15',50,'pcs',19.8,23,804.88,185.12,990,'B2-03')");
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    private static string Now() => DateTime.UtcNow.ToString("o");
    private static string D(int y, int m, int d) => new DateTime(y, m, d, 0, 0, 0, DateTimeKind.Utc).ToString("o");

    private static void InsertProduct(SQLiteConnection db,
        string name, string sku, string barcode, string unit, string category,
        decimal buyPrice, decimal sell1, decimal? sell2,
        decimal stock, decimal minStock, decimal maxStock,
        decimal reorder, decimal reorderQty, string location)
    {
        db.Execute(@"
            INSERT OR IGNORE INTO Products
            (Name,SKU,Barcode,Unit,CategoryName,PurchasePrice,SalePrice1,SalePrice2,
             StockLevel,MinStockLevel,MaxStockLevel,ReorderPoint,ReorderQty,
             WarehouseLocation,StockMethod,EcommerceEnabled,IsActive,CreatedAt,UpdatedAt)
            VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,'FIFO',1,1,?,?)",
            name, sku, barcode, unit, category,
            buyPrice, sell1, sell2,
            stock, minStock, maxStock, reorder, reorderQty,
            location, Now(), Now());
    }
}
