using System.Collections.Generic;
using WMS.Models;
using WMS.Repositories.Base;

namespace WMS.Repositories.Customers;

public interface ICustomerRepository : IRepository<Customer>
{
    Customer?     GetByTaxId(string taxId);
    List<Customer> GetByType(CustomerType type);
    List<Customer> GetActive();
    int            CountActive();
}
