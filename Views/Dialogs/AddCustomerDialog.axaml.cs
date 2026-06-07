using Avalonia.Controls;
using Avalonia.Interactivity;
using WMS.ViewModels.Dialogs;

namespace WMS.Views.Dialogs;

public partial class AddCustomerDialog : Window
{
    private readonly AddCustomerDialogViewModel _vm;

    public AddCustomerDialog()
    {
        InitializeComponent();
        _vm = new AddCustomerDialogViewModel();
        DataContext = _vm;
    }

    private void Save_Click(object? sender, RoutedEventArgs e)
    {
        if (_vm.Save())
            Close(true);
    }

    private void Cancel_Click(object? sender, RoutedEventArgs e) => Close(false);
}
