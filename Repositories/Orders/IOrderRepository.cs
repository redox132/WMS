using System.Collections.Generic;
using WMS.Models;
using WMS.Repositories.Base;

namespace WMS.Repositories.Orders;

public interface IOrderRepository : IRepository<Order>
{
    Order?        GetByNumber(string number);
    List<Order>   GetByCustomer(int customerId);
    List<Order>   GetByStatus(OrderStatus status);
    List<Order>   GetOpen();
    List<Order>   GetRecent(int count = 5);
    int           CountOpen();

    List<OrderLine> GetLines(int orderId);
    int             InsertLine(OrderLine line);
    int             UpdateLine(OrderLine line);
    int             DeleteLine(int lineId);
}
