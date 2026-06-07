using System.Collections.Generic;
using System.Linq;
using SQLite;
using WMS.Models;
using WMS.Repositories.Base;

namespace WMS.Repositories.Orders;

public sealed class OrderRepository : IOrderRepository
{
    private readonly SQLiteConnection _db;

    public OrderRepository(SQLiteConnection db) => _db = db;

    // ── Order CRUD ────────────────────────────────────────────────────────────

    public Order? GetById(int id)
    {
        var order = _db.Find<Order>(id);
        if (order is not null) order.Lines = GetLines(id);
        return order;
    }

    public List<Order> GetAll()
    {
        var orders = _db.Table<Order>().ToList();
        foreach (var o in orders) o.Lines = GetLines(o.Id);
        return orders;
    }

    public Order?      GetByNumber(string number)      => _db.Table<Order>().FirstOrDefault(o => o.Number == number);
    public List<Order> GetByCustomer(int customerId)   => _db.Table<Order>().Where(o => o.CustomerId == customerId).ToList();
    public List<Order> GetByStatus(OrderStatus status) => _db.Table<Order>().Where(o => o.Status == status).ToList();

    public List<Order> GetOpen() =>
        _db.Table<Order>()
           .Where(o => o.Status != OrderStatus.Delivered && o.Status != OrderStatus.Cancelled)
           .ToList();

    public List<Order> GetRecent(int count = 5) =>
        _db.Query<Order>("SELECT * FROM Orders ORDER BY OrderDate DESC LIMIT ?", count);

    public int CountOpen() =>
        _db.Table<Order>()
           .Where(o => o.Status != OrderStatus.Delivered && o.Status != OrderStatus.Cancelled)
           .Count();

    public int Insert(Order entity)
    {
        entity.CreatedAt = entity.UpdatedAt = System.DateTime.UtcNow;
        return _db.Insert(entity);
    }

    public int Update(Order entity)
    {
        entity.UpdatedAt = System.DateTime.UtcNow;
        return _db.Update(entity);
    }

    public int Delete(int id)
    {
        _db.Execute("DELETE FROM OrderLines WHERE OrderId = ?", id);
        return _db.Delete<Order>(id);
    }

    // ── Order Lines ───────────────────────────────────────────────────────────

    public List<OrderLine> GetLines(int orderId)   => _db.Table<OrderLine>().Where(l => l.OrderId == orderId).ToList();
    public int InsertLine(OrderLine line)           => _db.Insert(line);
    public int UpdateLine(OrderLine line)           => _db.Update(line);
    public int DeleteLine(int lineId)               => _db.Delete<OrderLine>(lineId);
}
