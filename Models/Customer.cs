using System;
using SQLite;

namespace WMS.Models;

public enum CustomerType { Customer, Supplier, Both }

[Table("Customers")]
public class Customer
{
    // ── Identity ────────────────────────────────────────────────────────────
    [PrimaryKey, AutoIncrement] public int Id { get; set; }
    [NotNull] public string Name { get; set; } = "";
    public string? ShortName { get; set; }
    public string? LegalForm { get; set; }        // Ltd, GmbH, S.A., sole trader …
    public CustomerType Type { get; set; } = CustomerType.Customer;

    // ── Tax & Registration Numbers ──────────────────────────────────────────
    public string? TaxId { get; set; }            // NIP (PL) / VAT number
    public string? VatNumber { get; set; }        // EU VAT (for intra-community)
    public string? NationalRegNumber { get; set; } // REGON / Companies House number
    public string? CompanyRegNumber { get; set; } // KRS / trade register

    // ── Contact ─────────────────────────────────────────────────────────────
    public string? Email { get; set; }
    public string? Email2 { get; set; }
    public string? Phone { get; set; }
    public string? Phone2 { get; set; }
    public string? Fax { get; set; }
    public string? Website { get; set; }
    public string? ContactPerson { get; set; }

    // ── Billing Address ─────────────────────────────────────────────────────
    public string? BillingStreet { get; set; }
    public string? BillingCity { get; set; }
    public string? BillingPostalCode { get; set; }
    public string? BillingState { get; set; }
    public string? BillingCountry { get; set; } = "PL";

    // ── Shipping / Delivery Address ─────────────────────────────────────────
    public string? ShippingStreet { get; set; }
    public string? ShippingCity { get; set; }
    public string? ShippingPostalCode { get; set; }
    public string? ShippingState { get; set; }
    public string? ShippingCountry { get; set; }

    // ── Correspondence Address ──────────────────────────────────────────────
    public string? CorrespondenceStreet { get; set; }
    public string? CorrespondenceCity { get; set; }
    public string? CorrespondencePostalCode { get; set; }
    public string? CorrespondenceCountry { get; set; }

    // ── Commercial Terms ────────────────────────────────────────────────────
    public int     PaymentTermDays { get; set; } = 14;
    public string  PaymentMethod { get; set; } = "Transfer"; // Cash|Transfer|Card|Credit
    public int     PriceLevel { get; set; } = 1;             // 1-5, maps to SalePrice1-5
    public decimal DefaultDiscount { get; set; }             // %
    public decimal CreditLimit { get; set; }
    public decimal CurrentBalance { get; set; }              // outstanding AR/AP
    public string  Currency { get; set; } = "PLN";

    // ── E-commerce ──────────────────────────────────────────────────────────
    public string? EcommerceCustomerId { get; set; }
    public string? EcommerceUsername { get; set; }

    // ── CRM ─────────────────────────────────────────────────────────────────
    public string? Notes { get; set; }
    public string? Tags { get; set; }
    public string? AssignedTo { get; set; }       // salesperson

    // ── Flags ───────────────────────────────────────────────────────────────
    public bool IsActive { get; set; } = true;
    public bool IsVATExempt { get; set; }
    public bool BlockOnArrears { get; set; }      // block orders when overdue

    // ── Audit ───────────────────────────────────────────────────────────────
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // ── Computed ─────────────────────────────────────────────────────────────
    [Ignore] public string TypeLabel => Type switch
    {
        CustomerType.Customer => "Customer",
        CustomerType.Supplier => "Supplier",
        CustomerType.Both     => "Customer / Supplier",
        _                     => ""
    };

    [Ignore] public string City => BillingCity ?? ShippingCity ?? "";
}
