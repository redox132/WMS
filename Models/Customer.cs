using System;
using SQLite;

namespace WMS.Models;

public enum CustomerType { Customer, Supplier, Both }

[Table("Customers")]
public class Customer
{
    [PrimaryKey, AutoIncrement] public int Id { get; set; }
    [NotNull] public string Name { get; set; } = "";
    public string? ShortName { get; set; }
    public string? LegalForm { get; set; }        
    public CustomerType Type { get; set; } = CustomerType.Customer;

    public string? TaxId { get; set; }           
    public string? VatNumber { get; set; }       
    public string? NationalRegNumber { get; set; } 
    public string? CompanyRegNumber { get; set; }

    public string? Email { get; set; }
    public string? Email2 { get; set; }
    public string? Phone { get; set; }
    public string? Phone2 { get; set; }
    public string? Fax { get; set; }
    public string? Website { get; set; }
    public string? ContactPerson { get; set; }

    public string? BillingStreet { get; set; }
    public string? BillingCity { get; set; }
    public string? BillingPostalCode { get; set; }
    public string? BillingState { get; set; }
    public string? BillingCountry { get; set; } = "PL";

    public string? ShippingStreet { get; set; }
    public string? ShippingCity { get; set; }
    public string? ShippingPostalCode { get; set; }
    public string? ShippingState { get; set; }
    public string? ShippingCountry { get; set; }

    public string? CorrespondenceStreet { get; set; }
    public string? CorrespondenceCity { get; set; }
    public string? CorrespondencePostalCode { get; set; }
    public string? CorrespondenceCountry { get; set; }

    public int     PaymentTermDays { get; set; } = 14;
    public string  PaymentMethod { get; set; } = "Transfer"; 
    public int     PriceLevel { get; set; } = 1;             
    public decimal DefaultDiscount { get; set; }             
    public decimal CreditLimit { get; set; }
    public decimal CurrentBalance { get; set; }              
    public string  Currency { get; set; } = "PLN";

    public string? EcommerceCustomerId { get; set; }
    public string? EcommerceUsername { get; set; }

    public string? Notes { get; set; }
    public string? Tags { get; set; }
    public string? AssignedTo { get; set; }       

    public bool IsActive { get; set; } = true;
    public bool IsVATExempt { get; set; }
    public bool BlockOnArrears { get; set; }      

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [Ignore] public string TypeLabel => Type switch
    {
        CustomerType.Customer => "Customer",
        CustomerType.Supplier => "Supplier",
        CustomerType.Both     => "Customer / Supplier",
        _                     => ""
    };

    [Ignore] public string City => BillingCity ?? ShippingCity ?? "";
}
