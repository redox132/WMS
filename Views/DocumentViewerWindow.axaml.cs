using Avalonia.Controls;
using WMS.Models;
using WMS.ViewModels;

namespace WMS.Views;

public partial class DocumentViewerWindow : Window
{
    public DocumentViewerWindow() => InitializeComponent();

    public DocumentViewerWindow(WarehouseDocument document)
    {
        InitializeComponent();
        Title = $"{document.TypeDescription} — {document.Number}";
        DataContext = new DocumentViewerViewModel(document);
    }
}
