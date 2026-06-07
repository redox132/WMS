using System.Collections.Generic;
using System.Linq;
using SQLite;
using WMS.Models;
using WMS.Repositories.Base;

namespace WMS.Repositories.Products;

public sealed class ProductRepository : IProductRepository
{
    private readonly SQLiteConnection _db;

    public ProductRepository(SQLiteConnection db) => _db = db;

    public Product?       GetById(int id)              => _db.Find<Product>(id);
    public List<Product>  GetAll()                     => _db.Table<Product>().ToList();
    public Product?       GetBySKU(string sku)         => _db.Table<Product>().FirstOrDefault(p => p.SKU == sku);
    public Product?       GetByBarcode(string barcode) => _db.Table<Product>().FirstOrDefault(p => p.Barcode == barcode || p.Barcode2 == barcode);
    public List<Product>  GetLowStock()                => _db.Table<Product>().Where(p => p.MinStockLevel > 0 && p.StockLevel <= p.MinStockLevel).ToList();
    public List<Product>  GetByCategory(string cat)    => _db.Table<Product>().Where(p => p.CategoryName == cat).ToList();
    public int            CountAll()                   => _db.Table<Product>().Count();
    public int            CountLowStock()              => _db.Table<Product>().Where(p => p.MinStockLevel > 0 && p.StockLevel <= p.MinStockLevel).Count();

    public int Insert(Product entity)
    {
        entity.CreatedAt = entity.UpdatedAt = System.DateTime.UtcNow;
        return _db.Insert(entity);
    }

    public int Update(Product entity)
    {
        entity.UpdatedAt = System.DateTime.UtcNow;
        return _db.Update(entity);
    }

    public int Delete(int id) => _db.Delete<Product>(id);
}
