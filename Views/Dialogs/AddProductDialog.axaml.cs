using Avalonia.Controls;
using Avalonia.Interactivity;
using WMS.ViewModels.Dialogs;

namespace WMS.Views.Dialogs;

public partial class AddProductDialog : Window
{
    private readonly AddProductDialogViewModel _vm;

    public AddProductDialog()
    {
        InitializeComponent();
        _vm = new AddProductDialogViewModel();
        DataContext = _vm;
    }

    private void Save_Click(object? sender, RoutedEventArgs e)
    {
        if (_vm.Save())
            Close(true);
    }

    private void Cancel_Click(object? sender, RoutedEventArgs e) => Close(false);
}
