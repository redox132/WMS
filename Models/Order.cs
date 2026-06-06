using System;
using System.Collections.Generic;
using System.Linq;

namespace WMS.Models;

public enum OrderType { Sales, Purchase }

public enum OrderStatus { New, Confirmed, InProgress, Shipped, Delivered, Cancelled }

public class Order
{
    public int Id { get; set; }
    public string Number { get; set; } = "";
    public OrderType Type { get; set; }
    public OrderStatus Status { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime? DueDate { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = "";
    public List<OrderLine> Lines { get; set; } = new();

    public decimal TotalValue => Lines.Sum(l => l.TotalPrice);

    public string TypeLabel => Type == OrderType.Sales ? "ZS" : "ZZ";

    public string StatusLabel => Status switch
    {
        OrderStatus.New        => "Nowe",
        OrderStatus.Confirmed  => "Potwierdzone",
        OrderStatus.InProgress => "W realizacji",
        OrderStatus.Shipped    => "Wysłane",
        OrderStatus.Delivered  => "Dostarczone",
        OrderStatus.Cancelled  => "Anulowane",
        _                      => ""
    };

    public string StatusColor => Status switch
    {
        OrderStatus.New        => "#616161",
        OrderStatus.Confirmed  => "#1565C0",
        OrderStatus.InProgress => "#E65100",
        OrderStatus.Shipped    => "#1565C0",
        OrderStatus.Delivered  => "#2E7D32",
        OrderStatus.Cancelled  => "#B71C1C",
        _                      => "#616161"
    };

    public string TypeColor => Type == OrderType.Sales ? "#1565C0" : "#6A1B9A";
}
