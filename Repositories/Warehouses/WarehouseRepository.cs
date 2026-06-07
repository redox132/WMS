using System.Collections.Generic;
using System.Linq;
using SQLite;
using WMS.Models;
using WMS.Repositories.Base;

namespace WMS.Repositories.Warehouses;

public sealed class WarehouseRepository : IWarehouseRepository
{
    private readonly SQLiteConnection _db;

    public WarehouseRepository(SQLiteConnection db) => _db = db;

    public Warehouse?      GetById(int id)          => _db.Find<Warehouse>(id);
    public List<Warehouse> GetAll()                 => _db.Table<Warehouse>().ToList();
    public Warehouse?      GetDefault()             => _db.Table<Warehouse>().FirstOrDefault(w => w.IsDefault);
    public Warehouse?      GetByCode(string code)   => _db.Table<Warehouse>().FirstOrDefault(w => w.Code == code);
    public List<Warehouse> GetActive()              => _db.Table<Warehouse>().Where(w => w.IsActive).ToList();

    public int Insert(Warehouse entity)
    {
        entity.CreatedAt = System.DateTime.UtcNow;
        return _db.Insert(entity);
    }

    public int Update(Warehouse entity) => _db.Update(entity);
    public int Delete(int id)           => _db.Delete<Warehouse>(id);
}
