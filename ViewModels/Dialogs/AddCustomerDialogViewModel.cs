using System;
using CommunityToolkit.Mvvm.ComponentModel;
using WMS.Models;
using WMS.Services;

namespace WMS.ViewModels.Dialogs;

public partial class AddCustomerDialogViewModel : ObservableObject
{
    private readonly int? _editId;

    public string Title => _editId.HasValue ? "Edit Customer" : "New Customer";

    [ObservableProperty] private string _name = "";
    [ObservableProperty] private CustomerType _type = CustomerType.Customer;
    [ObservableProperty] private string _taxId = "";
    [ObservableProperty] private string _email = "";
    [ObservableProperty] private string _phone = "";
    [ObservableProperty] private string _contactPerson = "";
    [ObservableProperty] private string _billingStreet = "";
    [ObservableProperty] private string _billingCity = "";
    [ObservableProperty] private string _billingPostalCode = "";
    [ObservableProperty] private string _billingCountry = "PL";
    [ObservableProperty] private int _paymentTermDays = 14;
    [ObservableProperty] private string _currency = "PLN";
    [ObservableProperty] private string _notes = "";
    [ObservableProperty] private string _errorMessage = "";

    public CustomerType[] TypeOptions { get; } = [CustomerType.Customer, CustomerType.Supplier, CustomerType.Both];

    public AddCustomerDialogViewModel() { }

    public AddCustomerDialogViewModel(Customer existing)
    {
        _editId           = existing.Id;
        Name              = existing.Name;
        Type              = existing.Type;
        TaxId             = existing.TaxId ?? "";
        Email             = existing.Email ?? "";
        Phone             = existing.Phone ?? "";
        ContactPerson     = existing.ContactPerson ?? "";
        BillingStreet     = existing.BillingStreet ?? "";
        BillingCity       = existing.BillingCity ?? "";
        BillingPostalCode = existing.BillingPostalCode ?? "";
        BillingCountry    = existing.BillingCountry ?? "PL";
        PaymentTermDays   = existing.PaymentTermDays;
        Currency          = existing.Currency;
        Notes             = existing.Notes ?? "";
    }

    public bool Save()
    {
        ErrorMessage = "";

        if (string.IsNullOrWhiteSpace(Name))           { ErrorMessage = "Name is required.";                        return false; }
        if (PaymentTermDays < 0)                       { ErrorMessage = "Payment term days cannot be negative.";     return false; }
        if (!string.IsNullOrWhiteSpace(Email) && !Email.Contains('@'))
                                                       { ErrorMessage = "Email address is not valid.";               return false; }

        try
        {
            if (_editId.HasValue)
            {
                var existing = AppServices.Customers.GetById(_editId.Value);
                if (existing == null) { ErrorMessage = "Customer not found."; return false; }

                existing.Name              = Name.Trim();
                existing.Type             = Type;
                existing.TaxId            = string.IsNullOrWhiteSpace(TaxId) ? null : TaxId.Trim();
                existing.Email            = string.IsNullOrWhiteSpace(Email) ? null : Email.Trim();
                existing.Phone            = string.IsNullOrWhiteSpace(Phone) ? null : Phone.Trim();
                existing.ContactPerson    = string.IsNullOrWhiteSpace(ContactPerson) ? null : ContactPerson.Trim();
                existing.BillingStreet    = string.IsNullOrWhiteSpace(BillingStreet) ? null : BillingStreet.Trim();
                existing.BillingCity      = string.IsNullOrWhiteSpace(BillingCity) ? null : BillingCity.Trim();
                existing.BillingPostalCode= string.IsNullOrWhiteSpace(BillingPostalCode) ? null : BillingPostalCode.Trim();
                existing.BillingCountry   = string.IsNullOrWhiteSpace(BillingCountry) ? "PL" : BillingCountry.Trim();
                existing.PaymentTermDays  = PaymentTermDays;
                existing.Currency         = string.IsNullOrWhiteSpace(Currency) ? "PLN" : Currency.Trim();
                existing.Notes            = string.IsNullOrWhiteSpace(Notes) ? null : Notes.Trim();
                existing.UpdatedAt        = DateTime.UtcNow;
                AppServices.Customers.Update(existing);
            }
            else
            {
                var customer = new Customer
                {
                    Name              = Name.Trim(),
                    Type              = Type,
                    TaxId             = string.IsNullOrWhiteSpace(TaxId) ? null : TaxId.Trim(),
                    Email             = string.IsNullOrWhiteSpace(Email) ? null : Email.Trim(),
                    Phone             = string.IsNullOrWhiteSpace(Phone) ? null : Phone.Trim(),
                    ContactPerson     = string.IsNullOrWhiteSpace(ContactPerson) ? null : ContactPerson.Trim(),
                    BillingStreet     = string.IsNullOrWhiteSpace(BillingStreet) ? null : BillingStreet.Trim(),
                    BillingCity       = string.IsNullOrWhiteSpace(BillingCity) ? null : BillingCity.Trim(),
                    BillingPostalCode = string.IsNullOrWhiteSpace(BillingPostalCode) ? null : BillingPostalCode.Trim(),
                    BillingCountry    = string.IsNullOrWhiteSpace(BillingCountry) ? "PL" : BillingCountry.Trim(),
                    PaymentTermDays   = PaymentTermDays,
                    Currency          = string.IsNullOrWhiteSpace(Currency) ? "PLN" : Currency.Trim(),
                    Notes             = string.IsNullOrWhiteSpace(Notes) ? null : Notes.Trim(),
                    IsActive          = true,
                };
                AppServices.Customers.Insert(customer);
            }
            return true;
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            return false;
        }
    }
}
