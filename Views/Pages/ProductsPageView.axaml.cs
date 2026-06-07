using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
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

    private void AddProduct_Click(object? sender, RoutedEventArgs e)
    {
        if (ParentWindow is { } w)
            _ = OpenAndRefresh(new AddProductDialog().ShowDialog<bool?>(w));
    }

    private async Task OpenAndRefresh(Task<bool?> dialogTask)
    {
        if (await dialogTask == true)
            DataContext = new ProductsPageViewModel();
    }
}
