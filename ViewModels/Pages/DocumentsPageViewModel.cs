using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using WMS.Models;
using WMS.Services;

namespace WMS.ViewModels.Pages;

public partial class DocumentsPageViewModel : ViewModelBase
{
    public ObservableCollection<WarehouseDocument> Documents { get; }

    [ObservableProperty] private bool _columnPickerOpen;
    public DocumentColumnConfig Columns { get; } = DocumentColumnConfig.Load();

    public DocumentsPageViewModel()
    {
        Documents = new ObservableCollection<WarehouseDocument>(AppServices.Documents.GetAll());
    }
}
