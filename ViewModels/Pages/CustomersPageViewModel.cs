using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using WMS.Models;
using WMS.Services;

namespace WMS.ViewModels.Pages;

public partial class CustomersPageViewModel : ViewModelBase
{
    private readonly List<Customer> _all;

    public ObservableCollection<Customer> Customers { get; } = new();

    [ObservableProperty] private string _searchText  = "";
    [ObservableProperty] private string _roleFilter  = "All";

    public string[] RoleFilters { get; } = { "All", "Customer", "Supplier", "Customer / Supplier" };

    public CustomersPageViewModel()
    {
        _all = AppServices.Customers.GetAll();
        ApplyFilter();
    }

    partial void OnSearchTextChanged(string value)  => ApplyFilter();
    partial void OnRoleFilterChanged(string value)  => ApplyFilter();

    private void ApplyFilter()
    {
        var q = SearchText.Trim();
        Customers.Clear();
        foreach (var c in _all.Where(c =>
            (string.IsNullOrEmpty(q) ||
             c.Name.Contains(q, StringComparison.OrdinalIgnoreCase) ||
             (c.TaxId?.Contains(q, StringComparison.OrdinalIgnoreCase) ?? false) ||
             (c.Email?.Contains(q, StringComparison.OrdinalIgnoreCase) ?? false)) &&
            (RoleFilter == "All" || c.TypeLabel == RoleFilter)))
        {
            Customers.Add(c);
        }
    }
}
