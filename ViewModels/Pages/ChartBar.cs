namespace WMS.ViewModels.Pages;

public record ChartBar(string Label, double Value, double MaxValue, string Color)
{
    public double BarWidth   => MaxValue > 0 ? Value / MaxValue * 260 : 0;
    public string ValueLabel => Value.ToString("N0");
}
