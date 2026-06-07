using WMS.Repositories.Customers;
using WMS.Repositories.Documents;
using WMS.Repositories.Orders;
using WMS.Repositories.Products;
using WMS.Repositories.Warehouses;
using WMS.Services.Migrations;

namespace WMS.Services;

/// <summary>
/// Lightweight service locator. Initialises the database (runs pending migrations)
/// and exposes typed repositories. Call <see cref="Initialise"/> once at startup.
/// </summary>
public static class AppServices
{
    public static IProductRepository   Products   { get; private set; } = null!;
    public static ICustomerRepository  Customers  { get; private set; } = null!;
    public static IOrderRepository     Orders     { get; private set; } = null!;
    public static IDocumentRepository  Documents  { get; private set; } = null!;
    public static IWarehouseRepository Warehouses { get; private set; } = null!;

    public static void Initialise()
    {
        var db = DatabaseService.Instance.Connection;

        new MigrationRunner(db, new IMigration[]
        {
            new Migration001_InitialSchema(),
            new Migration002_SeedData(),
        }).RunPending();

        Products   = new ProductRepository(db);
        Customers  = new CustomerRepository(db);
        Orders     = new OrderRepository(db);
        Documents  = new DocumentRepository(db);
        Warehouses = new WarehouseRepository(db);
    }
}
