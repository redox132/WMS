using CommunityToolkit.Mvvm.ComponentModel;
using WMS.ViewModels.Pages;

namespace WMS.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private ViewModelBase _currentPage;

    public MainWindowViewModel()
    {
        _currentPage = new DashboardPageViewModel();
    }

    public void Navigate(string page)
    {
        CurrentPage = page switch
        {
            "Dashboard" => new DashboardPageViewModel(),
            "Products"  => new ProductsPageViewModel(),
            "Orders"    => new OrdersPageViewModel(),
            "Suppliers" => new SuppliersPageViewModel(),
            "Customers" => new CustomersPageViewModel(),
            "Documents" => new DocumentsPageViewModel(),
            "Analytics" => new AnalyticsPageViewModel(),
            "Reports"   => new ReportsPageViewModel(),
            "Settings"  => new SettingsPageViewModel(),
            _           => CurrentPage
        };
    }
}
