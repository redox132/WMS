using SQLite;

namespace WMS.Services.Migrations;

/// <summary>Creates all application tables.</summary>
public sealed class Migration001_InitialSchema : IMigration
{
    public int    Version     => 1;
    public string Description => "Initial schema — Warehouses, Products, Customers, Orders, OrderLines, WarehouseDocuments, DocumentLines";

    public void Up(SQLiteConnection db)
    {
        db.Execute(@"
            CREATE TABLE IF NOT EXISTS Warehouses (
                Id          INTEGER PRIMARY KEY AUTOINCREMENT,
                Code        TEXT    NOT NULL UNIQUE,
                Name        TEXT    NOT NULL,
                Description TEXT,
                Street      TEXT,
                City        TEXT,
                PostalCode  TEXT,
                Country     TEXT    DEFAULT 'PL',
                ManagerName TEXT,
                Phone       TEXT,
                Email       TEXT,
                IsDefault   INTEGER NOT NULL DEFAULT 0,
                IsActive    INTEGER NOT NULL DEFAULT 1,
                CreatedAt   TEXT    NOT NULL
            )");

        db.Execute(@"
            CREATE TABLE IF NOT EXISTS Products (
                Id                   INTEGER PRIMARY KEY AUTOINCREMENT,
                Name                 TEXT    NOT NULL,
                ShortDescription     TEXT,
                Description          TEXT,
                SKU                  TEXT    NOT NULL UNIQUE,
                Barcode              TEXT,
                Barcode2             TEXT,
                BarcodeType          TEXT,
                FiscalName           TEXT,
                PLUCode              TEXT,
                SupplierSKU          TEXT,
                CategoryId           INTEGER,
                CategoryName         TEXT,
                Tags                 TEXT,
                CountryOfOrigin      TEXT,
                HSCode               TEXT,
                Unit                 TEXT    NOT NULL DEFAULT 'pcs',
                Unit2                TEXT,
                UnitConversionFactor REAL,
                PurchasePrice        REAL    NOT NULL DEFAULT 0,
                VatRatePurchase      REAL    NOT NULL DEFAULT 23,
                SalePrice1           REAL    NOT NULL DEFAULT 0,
                SalePrice2           REAL,
                SalePrice3           REAL,
                SalePrice4           REAL,
                SalePrice5           REAL,
                VatRateSale          REAL    NOT NULL DEFAULT 23,
                MinSalePrice         REAL,
                StockLevel           REAL    NOT NULL DEFAULT 0,
                MinStockLevel        REAL    NOT NULL DEFAULT 0,
                MaxStockLevel        REAL    NOT NULL DEFAULT 0,
                ReorderPoint         REAL    NOT NULL DEFAULT 0,
                ReorderQty           REAL    NOT NULL DEFAULT 0,
                StockMethod          TEXT    NOT NULL DEFAULT 'FIFO',
                TrackExpiry          INTEGER NOT NULL DEFAULT 0,
                TrackSerialNumbers   INTEGER NOT NULL DEFAULT 0,
                TrackBatches         INTEGER NOT NULL DEFAULT 0,
                WarehouseLocation    TEXT,
                Weight               REAL,
                WeightUnit           TEXT    DEFAULT 'kg',
                Volume               REAL,
                DimensionLength      REAL,
                DimensionWidth       REAL,
                DimensionHeight      REAL,
                DefaultSupplierId    INTEGER,
                LeadTimeDays         INTEGER,
                EcommerceEnabled     INTEGER NOT NULL DEFAULT 0,
                EcommerceId          TEXT,
                EcommerceSlug        TEXT,
                ImageUrl             TEXT,
                ImageUrl2            TEXT,
                ImageUrl3            TEXT,
                IsActive             INTEGER NOT NULL DEFAULT 1,
                IsService            INTEGER NOT NULL DEFAULT 0,
                IsBundle             INTEGER NOT NULL DEFAULT 0,
                IsVATExempt          INTEGER NOT NULL DEFAULT 0,
                CreatedAt            TEXT    NOT NULL,
                UpdatedAt            TEXT    NOT NULL
            )");

        db.Execute(@"
            CREATE TABLE IF NOT EXISTS Customers (
                Id                       INTEGER PRIMARY KEY AUTOINCREMENT,
                Name                     TEXT    NOT NULL,
                ShortName                TEXT,
                LegalForm                TEXT,
                Type                     INTEGER NOT NULL DEFAULT 0,
                TaxId                    TEXT,
                VatNumber                TEXT,
                NationalRegNumber        TEXT,
                CompanyRegNumber         TEXT,
                Email                    TEXT,
                Email2                   TEXT,
                Phone                    TEXT,
                Phone2                   TEXT,
                Fax                      TEXT,
                Website                  TEXT,
                ContactPerson            TEXT,
                BillingStreet            TEXT,
                BillingCity              TEXT,
                BillingPostalCode        TEXT,
                BillingState             TEXT,
                BillingCountry           TEXT    DEFAULT 'PL',
                ShippingStreet           TEXT,
                ShippingCity             TEXT,
                ShippingPostalCode       TEXT,
                ShippingState            TEXT,
                ShippingCountry          TEXT,
                CorrespondenceStreet     TEXT,
                CorrespondenceCity       TEXT,
                CorrespondencePostalCode TEXT,
                CorrespondenceCountry    TEXT,
                PaymentTermDays          INTEGER NOT NULL DEFAULT 14,
                PaymentMethod            TEXT    NOT NULL DEFAULT 'Transfer',
                PriceLevel               INTEGER NOT NULL DEFAULT 1,
                DefaultDiscount          REAL    NOT NULL DEFAULT 0,
                CreditLimit              REAL    NOT NULL DEFAULT 0,
                CurrentBalance           REAL    NOT NULL DEFAULT 0,
                Currency                 TEXT    NOT NULL DEFAULT 'PLN',
                EcommerceCustomerId      TEXT,
                EcommerceUsername        TEXT,
                Notes                    TEXT,
                Tags                     TEXT,
                AssignedTo               TEXT,
                IsActive                 INTEGER NOT NULL DEFAULT 1,
                IsVATExempt              INTEGER NOT NULL DEFAULT 0,
                BlockOnArrears           INTEGER NOT NULL DEFAULT 0,
                CreatedAt                TEXT    NOT NULL,
                UpdatedAt                TEXT    NOT NULL
            )");

        db.Execute(@"
            CREATE TABLE IF NOT EXISTS Orders (
                Id              INTEGER PRIMARY KEY AUTOINCREMENT,
                Number          TEXT    NOT NULL UNIQUE,
                Type            INTEGER NOT NULL DEFAULT 0,
                Status          INTEGER NOT NULL DEFAULT 0,
                OrderDate       TEXT    NOT NULL,
                ConfirmedAt     TEXT,
                DueDate         TEXT,
                ShippingDate    TEXT,
                DeliveryDate    TEXT,
                CustomerId      INTEGER NOT NULL,
                CustomerName    TEXT    NOT NULL,
                BillingName     TEXT,
                BillingStreet   TEXT,
                BillingCity     TEXT,
                BillingPostal   TEXT,
                BillingCountry  TEXT,
                ShippingName    TEXT,
                ShippingStreet  TEXT,
                ShippingCity    TEXT,
                ShippingPostal  TEXT,
                ShippingCountry TEXT,
                PaymentMethod   TEXT    NOT NULL DEFAULT 'Transfer',
                PaymentStatus   INTEGER NOT NULL DEFAULT 0,
                PaymentDate     TEXT,
                Currency        TEXT    NOT NULL DEFAULT 'PLN',
                ExchangeRate    REAL    NOT NULL DEFAULT 1,
                ShippingMethod  TEXT,
                ShippingCarrier TEXT,
                TrackingNumber  TEXT,
                ShippingCost    REAL    NOT NULL DEFAULT 0,
                DiscountPercent REAL    NOT NULL DEFAULT 0,
                DiscountAmount  REAL    NOT NULL DEFAULT 0,
                NetAmount       REAL    NOT NULL DEFAULT 0,
                VatAmount       REAL    NOT NULL DEFAULT 0,
                GrossAmount     REAL    NOT NULL DEFAULT 0,
                WarehouseId     INTEGER,
                InvoiceNumber   TEXT,
                ExternalRef     TEXT,
                EcommerceOrderId TEXT,
                EcommerceOrderNo TEXT,
                Notes           TEXT,
                InternalNotes   TEXT,
                CreatedBy       TEXT,
                CreatedAt       TEXT    NOT NULL,
                UpdatedAt       TEXT    NOT NULL,
                FOREIGN KEY (CustomerId) REFERENCES Customers(Id)
            )");

        db.Execute("CREATE INDEX IF NOT EXISTS idx_orders_customerid ON Orders(CustomerId)");
        db.Execute("CREATE INDEX IF NOT EXISTS idx_orders_status     ON Orders(Status)");

        db.Execute(@"
            CREATE TABLE IF NOT EXISTS OrderLines (
                Id                INTEGER PRIMARY KEY AUTOINCREMENT,
                OrderId           INTEGER NOT NULL,
                ProductId         INTEGER NOT NULL,
                ProductName       TEXT    NOT NULL,
                SKU               TEXT    NOT NULL,
                Barcode           TEXT,
                Quantity          REAL    NOT NULL DEFAULT 0,
                Unit              TEXT    NOT NULL DEFAULT 'pcs',
                UnitPrice         REAL    NOT NULL DEFAULT 0,
                DiscountPercent   REAL    NOT NULL DEFAULT 0,
                VatRate           REAL    NOT NULL DEFAULT 23,
                NetAmount         REAL    NOT NULL DEFAULT 0,
                VatAmount         REAL    NOT NULL DEFAULT 0,
                GrossAmount       REAL    NOT NULL DEFAULT 0,
                WarehouseLocation TEXT,
                BatchNumber       TEXT,
                SerialNumber      TEXT,
                ExpiryDate        TEXT,
                EcommerceLineId   TEXT,
                SortOrder         INTEGER,
                FOREIGN KEY (OrderId) REFERENCES Orders(Id) ON DELETE CASCADE
            )");

        db.Execute("CREATE INDEX IF NOT EXISTS idx_orderlines_orderid ON OrderLines(OrderId)");

        db.Execute(@"
            CREATE TABLE IF NOT EXISTS WarehouseDocuments (
                Id                  INTEGER PRIMARY KEY AUTOINCREMENT,
                Number              TEXT    NOT NULL UNIQUE,
                Type                INTEGER NOT NULL,
                Status              INTEGER NOT NULL DEFAULT 1,
                Date                TEXT    NOT NULL,
                ConfirmedAt         TEXT,
                ConfirmedBy         TEXT,
                WarehouseFromId     INTEGER,
                WarehouseFromName   TEXT,
                WarehouseToId       INTEGER,
                WarehouseToName     TEXT,
                ContractorId        INTEGER,
                ContractorName      TEXT,
                LinkedOrderId       INTEGER,
                LinkedOrderNumber   TEXT,
                LinkedInvoiceNumber TEXT,
                ExternalRef         TEXT,
                Carrier             TEXT,
                TrackingNumber      TEXT,
                VehiclePlate        TEXT,
                DriverName          TEXT,
                TotalQuantity       REAL    NOT NULL DEFAULT 0,
                NetAmount           REAL    NOT NULL DEFAULT 0,
                VatAmount           REAL    NOT NULL DEFAULT 0,
                GrossAmount         REAL    NOT NULL DEFAULT 0,
                Notes               TEXT,
                InternalNotes       TEXT,
                CreatedBy           TEXT,
                CreatedAt           TEXT    NOT NULL,
                UpdatedAt           TEXT    NOT NULL
            )");

        db.Execute("CREATE INDEX IF NOT EXISTS idx_whdocs_type   ON WarehouseDocuments(Type)");
        db.Execute("CREATE INDEX IF NOT EXISTS idx_whdocs_status ON WarehouseDocuments(Status)");

        db.Execute(@"
            CREATE TABLE IF NOT EXISTS DocumentLines (
                Id                INTEGER PRIMARY KEY AUTOINCREMENT,
                DocumentId        INTEGER NOT NULL,
                ProductId         INTEGER NOT NULL,
                ProductName       TEXT    NOT NULL,
                SKU               TEXT    NOT NULL,
                Barcode           TEXT,
                Quantity          REAL    NOT NULL DEFAULT 0,
                Unit              TEXT    NOT NULL DEFAULT 'pcs',
                UnitPrice         REAL    NOT NULL DEFAULT 0,
                VatRate           REAL    NOT NULL DEFAULT 23,
                NetAmount         REAL    NOT NULL DEFAULT 0,
                VatAmount         REAL    NOT NULL DEFAULT 0,
                GrossAmount       REAL    NOT NULL DEFAULT 0,
                WarehouseLocation TEXT,
                BatchNumber       TEXT,
                SerialNumber      TEXT,
                ExpiryDate        TEXT,
                SortOrder         INTEGER,
                FOREIGN KEY (DocumentId) REFERENCES WarehouseDocuments(Id) ON DELETE CASCADE
            )");

        db.Execute("CREATE INDEX IF NOT EXISTS idx_doclines_documentid ON DocumentLines(DocumentId)");
    }
}
