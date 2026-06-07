using System.Collections.Generic;
using WMS.Models;
using WMS.Repositories.Base;

namespace WMS.Repositories.Warehouses;

public interface IWarehouseRepository : IRepository<Warehouse>
{
    Warehouse?     GetDefault();
    Warehouse?     GetByCode(string code);
    List<Warehouse> GetActive();
}
