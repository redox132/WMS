using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using WMS.Models;
using WMS.ViewModels.Pages;
using WMS.Views.Dialogs;

namespace WMS.Views.Pages;

public partial class ProductsPageView : UserControl
{
    public ProductsPageView()
    {
        InitializeComponent();
    }

    private Window? ParentWindow => TopLevel.GetTopLevel(this) as Window;
    private ProductsPageViewModel? Vm => DataContext as ProductsPageViewModel;

    private void AddProduct_Click(object? sender, RoutedEventArgs e)
    {
        if (ParentWindow is { } w)
            _ = OpenAndRefresh(new AddProductDialog().ShowDialog<bool?>(w));
    }

    private void EditProduct_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button { DataContext: Product p } && ParentWindow is { } w)
            _ = OpenAndRefresh(new AddProductDialog(p).ShowDialog<bool?>(w));
    }

    private void ToggleColumnPicker_Click(object? sender, RoutedEventArgs e)
    {
        if (Vm is { } vm)
            vm.ColumnPickerOpen = !vm.ColumnPickerOpen;
    }

    private void SaveColumns_Click(object? sender, RoutedEventArgs e)
    {
        Vm?.Columns.Save();
        if (Vm is { } vm)
            vm.ColumnPickerOpen = false;
    }

    private async Task OpenAndRefresh(Task<bool?> dialogTask)
    {
        if (await dialogTask == true)
            DataContext = new ProductsPageViewModel();
    }
}
