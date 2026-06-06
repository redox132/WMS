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

    private void NavList_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (DataContext is not MainWindowViewModel vm) return;
        if (sender is not ListBox lb) return;
        if (lb.SelectedItem is not ListBoxItem item) return;

        var tag = item.Tag?.ToString();
        if (tag is not null)
            vm.Navigate(tag);
    }
}
