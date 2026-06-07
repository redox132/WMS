using System.Collections.Generic;
using System.Linq;
using SQLite;
using WMS.Models;
using WMS.Repositories.Base;

namespace WMS.Repositories.Customers;

public sealed class CustomerRepository : ICustomerRepository
{
    private readonly SQLiteConnection _db;

    public CustomerRepository(SQLiteConnection db) => _db = db;

    public Customer?       GetById(int id)            => _db.Find<Customer>(id);
    public List<Customer>  GetAll()                   => _db.Table<Customer>().ToList();
    public Customer?       GetByTaxId(string taxId)   => _db.Table<Customer>().FirstOrDefault(c => c.TaxId == taxId);
    public List<Customer>  GetByType(CustomerType t)  => _db.Table<Customer>().Where(c => c.Type == t).ToList();
    public List<Customer>  GetActive()                => _db.Table<Customer>().Where(c => c.IsActive).ToList();
    public int             CountActive()              => _db.Table<Customer>().Where(c => c.IsActive).Count();

    public int Insert(Customer entity)
    {
        entity.CreatedAt = entity.UpdatedAt = System.DateTime.UtcNow;
        return _db.Insert(entity);
    }

    public int Update(Customer entity)
    {
        entity.UpdatedAt = System.DateTime.UtcNow;
        return _db.Update(entity);
    }

    public int Delete(int id) => _db.Delete<Customer>(id);
}
