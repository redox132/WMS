using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using WMS.Models;
using WMS.Services;

namespace WMS.ViewModels.Pages;

public partial class SuppliersPageViewModel : ViewModelBase
{
    private readonly List<Customer> _all;

    public ObservableCollection<Customer> Suppliers { get; } = new();

    [ObservableProperty] private string _searchText = "";

    public SuppliersPageViewModel()
    {
        _all = AppServices.Customers
            .GetAll()
            .Where(c => c.Type == CustomerType.Supplier || c.Type == CustomerType.Both)
            .ToList();
        ApplyFilter();
    }

    partial void OnSearchTextChanged(string value) => ApplyFilter();

    private void ApplyFilter()
    {
        var q = SearchText.Trim();
        Suppliers.Clear();
        foreach (var c in _all.Where(c =>
            string.IsNullOrEmpty(q) ||
            c.Name.Contains(q, StringComparison.OrdinalIgnoreCase) ||
            (c.TaxId?.Contains(q, StringComparison.OrdinalIgnoreCase) ?? false) ||
            (c.Email?.Contains(q, StringComparison.OrdinalIgnoreCase) ?? false)))
        {
            Suppliers.Add(c);
        }
    }
}
