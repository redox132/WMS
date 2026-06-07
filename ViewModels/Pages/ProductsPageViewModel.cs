using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using WMS.Models;
using WMS.Services;

namespace WMS.ViewModels.Pages;

public partial class ProductsPageViewModel : ViewModelBase
{
    private readonly List<Product> _all;

    public ObservableCollection<Product> Products { get; } = new();

    [ObservableProperty] private string _searchText = "";
    [ObservableProperty] private string _categoryFilter = "All";
    [ObservableProperty] private bool   _lowStockOnly;

    public string[] Categories { get; }

    public ProductsPageViewModel()
    {
        _all = AppServices.Products.GetAll();

        var cats = _all
            .Select(p => p.CategoryName ?? "Uncategorised")
            .Distinct()
            .OrderBy(c => c)
            .ToList();
        cats.Insert(0, "All");
        Categories = cats.ToArray();

        ApplyFilter();
    }

    partial void OnSearchTextChanged(string value)     => ApplyFilter();
    partial void OnCategoryFilterChanged(string value) => ApplyFilter();
    partial void OnLowStockOnlyChanged(bool value)     => ApplyFilter();

    private void ApplyFilter()
    {
        var q = SearchText.Trim();
        Products.Clear();
        foreach (var p in _all.Where(p =>
            (string.IsNullOrEmpty(q) ||
             p.Name.Contains(q, StringComparison.OrdinalIgnoreCase) ||
             p.SKU.Contains(q, StringComparison.OrdinalIgnoreCase) ||
             (p.Barcode?.Contains(q, StringComparison.OrdinalIgnoreCase) ?? false)) &&
            (CategoryFilter == "All" || p.CategoryName == CategoryFilter) &&
            (!LowStockOnly || p.IsLowStock)))
        {
            Products.Add(p);
        }
    }
}
