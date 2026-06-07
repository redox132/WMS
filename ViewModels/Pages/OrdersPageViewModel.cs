using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using WMS.Models;
using WMS.Services;

namespace WMS.ViewModels.Pages;

public partial class OrdersPageViewModel : ViewModelBase
{
    private readonly List<Order> _all;

    public ObservableCollection<Order> Orders { get; } = new();

    [ObservableProperty] private string _searchText    = "";
    [ObservableProperty] private string _statusFilter  = "All";
    [ObservableProperty] private string _typeFilter    = "All";

    public string[] StatusFilters { get; } = { "All", "New", "Confirmed", "In Progress", "Shipped", "Delivered", "Cancelled" };
    public string[] TypeFilters   { get; } = { "All", "Sales Order", "Purchase Order" };

    public OrdersPageViewModel()
    {
        _all = AppServices.Orders.GetAll();
        ApplyFilter();
    }

    partial void OnSearchTextChanged(string value)   => ApplyFilter();
    partial void OnStatusFilterChanged(string value) => ApplyFilter();
    partial void OnTypeFilterChanged(string value)   => ApplyFilter();

    private void ApplyFilter()
    {
        var q = SearchText.Trim();
        Orders.Clear();
        foreach (var o in _all.Where(o =>
            (string.IsNullOrEmpty(q) ||
             o.Number.Contains(q, StringComparison.OrdinalIgnoreCase) ||
             o.CustomerName.Contains(q, StringComparison.OrdinalIgnoreCase)) &&
            (StatusFilter == "All" || o.StatusLabel == StatusFilter) &&
            (TypeFilter == "All" ||
             (TypeFilter == "Sales Order"    && o.Type == OrderType.Sales) ||
             (TypeFilter == "Purchase Order" && o.Type == OrderType.Purchase))))
        {
            Orders.Add(o);
        }
    }
}
