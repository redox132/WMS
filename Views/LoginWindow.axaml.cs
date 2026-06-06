using Avalonia.Controls;
using WMS.ViewModels;
using WMS.ViewModels.Auth;

namespace WMS.Views;

public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();
        DataContext = new LoginWindowViewModel();
    }
}