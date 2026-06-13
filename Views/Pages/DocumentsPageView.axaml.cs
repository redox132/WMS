using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using WMS.Models;
using WMS.Services;
using WMS.ViewModels.Pages;
using WMS.Views.Dialogs;

namespace WMS.Views.Pages;

public partial class DocumentsPageView : UserControl
{
    public DocumentsPageView()
    {
        InitializeComponent();
    }

    private Window? ParentWindow => TopLevel.GetTopLevel(this) as Window;

    private void AddDocument_Click(object? sender, RoutedEventArgs e)
    {
        if (ParentWindow is { } w)
            _ = OpenAndRefresh(new AddDocumentDialog().ShowDialog<bool?>(w));
    }

    private void EditDocument_Click(object? sender, RoutedEventArgs e)
    {
        if (sender is Button { DataContext: WarehouseDocument d } && ParentWindow is { } w)
            _ = OpenAndRefresh(new AddDocumentDialog(d).ShowDialog<bool?>(w));
    }

    private void DocumentRow_DoubleTapped(object? sender, TappedEventArgs e)
    {
        if (sender is Border { DataContext: WarehouseDocument d } && ParentWindow is { } w)
        {
            // Reload with lines from DB to ensure they're populated
            var full = AppServices.Documents.GetById(d.Id) ?? d;
            new DocumentViewerWindow(full).ShowDialog(w);
        }
    }

    private DocumentsPageViewModel? Vm => DataContext as DocumentsPageViewModel;

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
            DataContext = new DocumentsPageViewModel();
    }
}
