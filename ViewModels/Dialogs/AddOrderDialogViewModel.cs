using System;
using System.Collections.Generic;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using WMS.Models;
using WMS.Services;

namespace WMS.ViewModels.Dialogs;

public partial class AddOrderDialogViewModel : ObservableObject
{
    [ObservableProperty] private OrderType _orderType = OrderType.Sales;
    [ObservableProperty] private Customer? _selectedCustomer;
    [ObservableProperty] private string _currency = "PLN";
    [ObservableProperty] private string _paymentMethod = "Transfer";
    [ObservableProperty] private DateTime? _dueDate;
    [ObservableProperty] private string _notes = "";
    [ObservableProperty] private string _errorMessage = "";

    public List<Customer> Customers { get; }
    public OrderType[] OrderTypeOptions { get; } = [OrderType.Sales, OrderType.Purchase];
    public string[] PaymentMethods { get; } = ["Transfer", "Cash", "Card", "Credit"];
    public string[] Currencies { get; } = ["PLN", "EUR", "USD", "GBP"];

    public AddOrderDialogViewModel()
    {
        Customers = AppServices.Customers.GetAll().Where(c => c.IsActive).OrderBy(c => c.Name).ToList();
        SelectedCustomer = Customers.FirstOrDefault();
    }

    public bool Save()
    {
        ErrorMessage = "";

        if (SelectedCustomer is null) { ErrorMessage = "Please select a customer."; return false; }

        var now    = DateTime.UtcNow;
        var prefix = OrderType == OrderType.Sales ? "SO" : "PO";
        var number = $"{prefix}/{now:yyyy}/{now:MM}/{now:dd}/{now.Ticks % 10000:D4}";

        var order = new Order
        {
            Number        = number,
            Type          = OrderType,
            Status        = OrderStatus.New,
            PaymentStatus = PaymentStatus.Unpaid,
            CustomerId    = SelectedCustomer.Id,
            CustomerName  = SelectedCustomer.Name,
            Currency      = string.IsNullOrWhiteSpace(Currency) ? "PLN" : Currency,
            PaymentMethod = PaymentMethod,
            DueDate       = DueDate,
            Notes         = string.IsNullOrWhiteSpace(Notes) ? null : Notes.Trim(),
            ExchangeRate  = 1,
            OrderDate     = DateTime.UtcNow,
        };

        try
        {
            AppServices.Orders.Insert(order);
            return true;
        }
        catch (Exception ex)
        {
            ErrorMessage = ex.Message;
            return false;
        }
    }
}
