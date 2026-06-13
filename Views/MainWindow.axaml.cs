using Avalonia.Controls;
using WMS.ViewModels;

namespace WMS.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel();
    }

    private void ExitMenu_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Close();
    }
}
