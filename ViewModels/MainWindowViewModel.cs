using CommunityToolkit.Mvvm.ComponentModel;
using WMS.ViewModels.Pages;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace WMS.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private ViewModelBase _currentPage;

    public MainWindowViewModel()
    {
        _currentPage = new DashboardPageViewModel();
    }

    // Command for menu navigation
    public ICommand NavigateToCommand => new RelayCommand<string?>(p => {
        if (p is not null)
            Navigate(p);
    });

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
            "Insights"  => new InsightsPageViewModel(),
            "Reports"   => new ReportsPageViewModel(),
            "Settings"  => new SettingsPageViewModel(),
            _           => CurrentPage
        };
    }
}
