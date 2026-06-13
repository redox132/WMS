using Avalonia.Controls;
using Avalonia.Interactivity;
using WMS.Models;
using WMS.ViewModels.Dialogs;

namespace WMS.Views.Dialogs;

public partial class AddCustomerDialog : Window
{
    private readonly AddCustomerDialogViewModel _vm;

    public AddCustomerDialog() : this(null) { }

    public AddCustomerDialog(Customer? existing)
    {
        InitializeComponent();
        _vm = existing is null
            ? new AddCustomerDialogViewModel()
            : new AddCustomerDialogViewModel(existing);
        DataContext = _vm;
        Title = _vm.Title;
    }

    private void Save_Click(object? sender, RoutedEventArgs e)
    {
        if (_vm.Save()) Close(true);
    }

    private void Cancel_Click(object? sender, RoutedEventArgs e) => Close(false);
}
