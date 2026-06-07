using System;
using CommunityToolkit.Mvvm.ComponentModel;
using WMS.Models;
using WMS.Services;

namespace WMS.ViewModels.Dialogs;

public partial class AddCustomerDialogViewModel : ObservableObject
{
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

    public bool Save()
    {
        ErrorMessage = "";

        if (string.IsNullOrWhiteSpace(Name)) { ErrorMessage = "Name is required."; return false; }

        var customer = new Customer
        {
            Name             = Name.Trim(),
            Type             = Type,
            TaxId            = string.IsNullOrWhiteSpace(TaxId) ? null : TaxId.Trim(),
            Email            = string.IsNullOrWhiteSpace(Email) ? null : Email.Trim(),
            Phone            = string.IsNullOrWhiteSpace(Phone) ? null : Phone.Trim(),
            ContactPerson    = string.IsNullOrWhiteSpace(ContactPerson) ? null : ContactPerson.Trim(),
            BillingStreet    = string.IsNullOrWhiteSpace(BillingStreet) ? null : BillingStreet.Trim(),
            BillingCity      = string.IsNullOrWhiteSpace(BillingCity) ? null : BillingCity.Trim(),
            BillingPostalCode= string.IsNullOrWhiteSpace(BillingPostalCode) ? null : BillingPostalCode.Trim(),
            BillingCountry   = string.IsNullOrWhiteSpace(BillingCountry) ? "PL" : BillingCountry.Trim(),
            PaymentTermDays  = PaymentTermDays,
            Currency         = string.IsNullOrWhiteSpace(Currency) ? "PLN" : Currency.Trim(),
            Notes            = string.IsNullOrWhiteSpace(Notes) ? null : Notes.Trim(),
            IsActive         = true,
        };

        try
        {
            AppServices.Customers.Insert(customer);
            return true;
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            return false;
        }
    }
}
