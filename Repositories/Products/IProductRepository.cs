using System.Collections.Generic;
using WMS.Models;
using WMS.Repositories.Base;

namespace WMS.Repositories.Products;

public interface IProductRepository : IRepository<Product>
{
    Product?    GetBySKU(string sku);
    Product?    GetByBarcode(string barcode);
    List<Product> GetLowStock();
    List<Product> GetByCategory(string categoryName);
    int         CountAll();
    int         CountLowStock();
}
