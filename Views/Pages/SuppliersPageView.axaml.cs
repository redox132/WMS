using Avalonia.Controls;
using Avalonia.Interactivity;
using WMS.ViewModels.Pages;

namespace WMS.Views.Pages;

public partial class SuppliersPageView : UserControl
{
    public SuppliersPageView() => InitializeComponent();

    private SuppliersPageViewModel? Vm => DataContext as SuppliersPageViewModel;

    private void ToggleColumnPicker_Click(object? sender, RoutedEventArgs e)
    {
        if (Vm is { } vm) vm.ColumnPickerOpen = !vm.ColumnPickerOpen;
    }

    private void SaveColumns_Click(object? sender, RoutedEventArgs e)
    {
        if (Vm is { } vm) { vm.Columns.Save(); vm.ColumnPickerOpen = false; }
    }
}
