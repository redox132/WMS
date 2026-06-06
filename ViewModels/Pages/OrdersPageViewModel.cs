using System;
using System.Collections.ObjectModel;
using WMS.Models;

namespace WMS.ViewModels.Pages;

public partial class OrdersPageViewModel : ViewModelBase
{
    public ObservableCollection<Order> Orders { get; } = new()
    {
        new Order
        {
            Id=1, Number="ZS/2026/042", Type=OrderType.Sales, Status=OrderStatus.Shipped,
            OrderDate=new DateTime(2026,6,6), DueDate=new DateTime(2026,6,10),
            CustomerName="Acme Sp. z o.o.",
            Lines= new() { new OrderLine { ProductName="Śruba M8 x 40", Quantity=120, Unit="opak.", UnitPrice=54.00m } }
        },
        new Order
        {
            Id=2, Number="ZS/2026/041", Type=OrderType.Sales, Status=OrderStatus.Delivered,
            OrderDate=new DateTime(2026,6,5), DueDate=new DateTime(2026,6,9),
            CustomerName="BuildRight Sp. z o.o.",
            Lines= new() { new OrderLine { ProductName="Rura PVC 50mm x 3m", Quantity=30, Unit="szt.", UnitPrice=28.50m } }
        },
        new Order
        {
            Id=3, Number="ZS/2026/040", Type=OrderType.Sales, Status=OrderStatus.InProgress,
            OrderDate=new DateTime(2026,6,4), DueDate=new DateTime(2026,6,8),
            CustomerName="TechMart GmbH",
            Lines= new()
            {
                new OrderLine { ProductName="Kabel YDY 3x1,5mm²", Quantity=20, Unit="rolka", UnitPrice=285.00m },
                new OrderLine { ProductName="Zawór kulowy 1/2\" DN15", Quantity=150, Unit="szt.", UnitPrice=19.80m }
            }
        },
        new Order
        {
            Id=4, Number="ZZ/2026/018", Type=OrderType.Purchase, Status=OrderStatus.Confirmed,
            OrderDate=new DateTime(2026,6,3), DueDate=new DateTime(2026,6,12),
            CustomerName="Metaltech S.A.",
            Lines= new() { new OrderLine { ProductName="Nakrętka M8", Quantity=50, Unit="opak.", UnitPrice=22.00m } }
        },
        new Order
        {
            Id=5, Number="ZS/2026/039", Type=OrderType.Sales, Status=OrderStatus.Delivered,
            OrderDate=new DateTime(2026,6,3),
            CustomerName="GlobalFix Sp. z o.o.",
            Lines= new() { new OrderLine { ProductName="Opaska zaciskowa 200mm", Quantity=20, Unit="opak.", UnitPrice=14.00m } }
        },
    };
}
