using Avalonia.Controls;
using WMS.ViewModels;

namespace WMS.Views;

public partial class MainWindow : Window
{
    private readonly ListBox[] _navGroups;

    public MainWindow()
    {
        InitializeComponent();
        DataContext = new MainWindowViewModel();

        _navGroups = new[] { NavInventory, NavOperations, NavAnalytics, NavSystem };
    }

    private void Nav_SelectionChanged(object? sender, SelectionChangedEventArgs e)
    {
        if (DataContext is not MainWindowViewModel vm) return;
        if (sender is not ListBox active) return;

        // Deselect every other group
        foreach (var lb in _navGroups)
            if (!ReferenceEquals(lb, active))
                lb.SelectedIndex = -1;

        if (active.SelectedItem is ListBoxItem item && item.Tag is string tag)
            vm.Navigate(tag);
    }
}
