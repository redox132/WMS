using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using WMS.Models;
using WMS.ViewModels.Pages;
using WMS.Views.Dialogs;

namespace WMS.Views.Pages;

public partial class CustomersPageView : UserControl
{
    public CustomersPageView()
    {
        InitializeComponent();
    }

    private Window? ParentWindow => TopLevel.GetTopLevel(this) as Window;

    private void AddCustomer_Click(object? sender, RoutedEventArgs e)
    {
        if (ParentWindow is { } w)
            _ = OpenAndRefresh(new AddCustomerDialog().ShowDialog<bool?>(w));
    }

    private void EditCustomer_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button { DataContext: Customer c } && ParentWindow is { } w)
            _ = OpenAndRefresh(new AddCustomerDialog(c).ShowDialog<bool?>(w));
    }

    private CustomersPageViewModel? Vm => DataContext as CustomersPageViewModel;

    private void ToggleColumnPicker_Click(object? sender, RoutedEventArgs e)
    {
        if (Vm is { } vm) vm.ColumnPickerOpen = !vm.ColumnPickerOpen;
    }

    private void SaveColumns_Click(object? sender, RoutedEventArgs e)
    {
        if (Vm is { } vm) { vm.Columns.Save(); vm.ColumnPickerOpen = false; }
    }

    private async Task OpenAndRefresh(Task<bool?> dialogTask)
    {
        if (await dialogTask == true)
            DataContext = new CustomersPageViewModel();
    }
}
