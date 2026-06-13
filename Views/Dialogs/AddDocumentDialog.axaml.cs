using Avalonia.Controls;
using Avalonia.Interactivity;
using WMS.Models;
using WMS.ViewModels.Dialogs;

namespace WMS.Views.Dialogs;

public partial class AddDocumentDialog : Window
{
    private readonly AddDocumentDialogViewModel _vm;

    public AddDocumentDialog() : this(null) { }

    public AddDocumentDialog(WarehouseDocument? existing)
    {
        InitializeComponent();
        _vm = existing is null
            ? new AddDocumentDialogViewModel()
            : new AddDocumentDialogViewModel(existing);
        DataContext = _vm;
        Title = _vm.Title;
    }

    private void Save_Click(object? sender, RoutedEventArgs e)
    {
        if (_vm.Save()) Close(true);
    }

    private void Cancel_Click(object? sender, RoutedEventArgs e) => Close(false);
}
