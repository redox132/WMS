using SQLite;

namespace WMS.Services.Migrations;

public sealed class Migration003_CompanySettings : IMigration
{
    public int    Version     => 3;
    public string Description => "Add CompanySettings table";

    public void Up(SQLiteConnection db)
    {
        db.Execute(@"
            CREATE TABLE IF NOT EXISTS CompanySettings (
                Id              INTEGER PRIMARY KEY,
                CompanyName     TEXT    NOT NULL DEFAULT '',
                LegalForm       TEXT,
                ShortName       TEXT,
                TaxId           TEXT,
                VatNumber       TEXT,
                Regon           TEXT,
                Krs             TEXT,
                Street          TEXT,
                City            TEXT,
                PostalCode      TEXT,
                State           TEXT,
                Country         TEXT    DEFAULT 'PL',
                Phone           TEXT,
                Email           TEXT,
                Website         TEXT,
                BankName        TEXT,
                BankAccount     TEXT,
                BankSwift       TEXT,
                Currency        TEXT    NOT NULL DEFAULT 'PLN',
                PaymentTermDays INTEGER NOT NULL DEFAULT 14,
                InvoiceFooter   TEXT,
                LogoPath        TEXT
            )");

        // Insert default singleton row if not present
        db.Execute(@"INSERT OR IGNORE INTO CompanySettings (Id, CompanyName, Currency, PaymentTermDays)
                     VALUES (1, '', 'PLN', 14)");
    }
}
