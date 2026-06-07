using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using WMS.ViewModels.Pages;
using WMS.Views.Dialogs;

namespace WMS.Views.Pages;

public partial class OrdersPageView : UserControl
{
    public OrdersPageView()
    {
        InitializeComponent();
    }

    private Window? ParentWindow => TopLevel.GetTopLevel(this) as Window;

    private void AddOrder_Click(object? sender, RoutedEventArgs e)
    {
        if (ParentWindow is { } w)
            _ = OpenAndRefresh(new AddOrderDialog().ShowDialog<bool?>(w));
    }

    private async Task OpenAndRefresh(Task<bool?> dialogTask)
    {
        if (await dialogTask == true)
            DataContext = new OrdersPageViewModel();
    }
}
