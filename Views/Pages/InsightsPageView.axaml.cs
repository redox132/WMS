using Avalonia.Controls;
using WMS.ViewModels.Pages;

namespace WMS.Views.Pages;

public partial class InsightsPageView : UserControl
{
    public InsightsPageView()
    {
        InitializeComponent();
        DataContext = new InsightsPageViewModel();
    }
}
