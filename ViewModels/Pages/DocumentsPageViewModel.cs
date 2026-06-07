using System.Collections.ObjectModel;
using WMS.Models;
using WMS.Services;

namespace WMS.ViewModels.Pages;

public partial class DocumentsPageViewModel : ViewModelBase
{
    public ObservableCollection<WarehouseDocument> Documents { get; }

    public DocumentsPageViewModel()
    {
        Documents = new ObservableCollection<WarehouseDocument>(AppServices.Documents.GetAll());
    }
}
