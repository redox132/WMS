namespace WMS.Models;

public class OrderLine
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = "";
    public string SKU { get; set; } = "";
    public decimal Quantity { get; set; }
    public string Unit { get; set; } = "szt.";
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice => Quantity * UnitPrice;
}
