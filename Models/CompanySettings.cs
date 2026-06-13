using SQLite;

namespace WMS.Models;

[Table("CompanySettings")]
public class CompanySettings
{
    [PrimaryKey] public int Id { get; set; } = 1; 

    public string  CompanyName    { get; set; } = "";
    public string? LegalForm      { get; set; }  
    public string? ShortName      { get; set; }

    public string? TaxId          { get; set; }  
    public string? VatNumber      { get; set; }  
    public string? Regon          { get; set; }  
    public string? Krs            { get; set; }  


    public string? Street         { get; set; }
    public string? City           { get; set; }
    public string? PostalCode     { get; set; }
    public string? State          { get; set; }
    public string? Country        { get; set; } = "PL";

    public string? Phone          { get; set; }
    public string? Email          { get; set; }
    public string? Website        { get; set; }

    public string? BankName       { get; set; }
    public string? BankAccount    { get; set; }  
    public string? BankSwift      { get; set; }


    public string  Currency       { get; set; } = "PLN";
    public int     PaymentTermDays{ get; set; } = 14;
    public string? InvoiceFooter  { get; set; }  
    public string? LogoPath       { get; set; }  
}
