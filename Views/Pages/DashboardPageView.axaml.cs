using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using WMS.ViewModels.Pages;
using WMS.Views.Dialogs;

namespace WMS.Views.Pages;

public partial class DashboardPageView : UserControl
{
    public DashboardPageView()
    {
        InitializeComponent();
    }

    private Window? ParentWindow => TopLevel.GetTopLevel(this) as Window;

    private async Task RefreshAfter(Task<bool?> dialogTask)
    {
        var result = await dialogTask;
        if (result == true)
            DataContext = new DashboardPageViewModel();
    }

    private void AddProduct_Click(object? sender, RoutedEventArgs e)
    {
        if (ParentWindow is { } w)
            _ = RefreshAfter(new AddProductDialog().ShowDialog<bool?>(w));
    }

    private void AddOrder_Click(object? sender, RoutedEventArgs e)
    {
        if (ParentWindow is { } w)
            _ = RefreshAfter(new AddOrderDialog().ShowDialog<bool?>(w));
    }

    private void AddCustomer_Click(object? sender, RoutedEventArgs e)
    {
        if (ParentWindow is { } w)
            _ = RefreshAfter(new AddCustomerDialog().ShowDialog<bool?>(w));
    }

    private void AddDocument_Click(object? sender, RoutedEventArgs e)
    {
        if (ParentWindow is { } w)
            _ = RefreshAfter(new AddDocumentDialog().ShowDialog<bool?>(w));
    }
}
