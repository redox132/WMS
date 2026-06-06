using System.Collections.ObjectModel;
using WMS.Models;

namespace WMS.ViewModels.Pages;

public partial class CustomersPageViewModel : ViewModelBase
{
    public ObservableCollection<Customer> Customers { get; } = new()
    {
        new Customer { Id=1, Name="Acme Sp. z o.o.", ShortName="ACME", NIP="1234567890", Email="zamowienia@acme.pl", Phone="22 123 45 67", City="Warszawa", Type=CustomerType.Customer, PaymentTermDays=14, DefaultDiscount=5 },
        new Customer { Id=2, Name="BuildRight Sp. z o.o.", ShortName="BUILDRIGHT", NIP="9876543210", Email="biuro@buildright.pl", Phone="12 987 65 43", City="Kraków", Type=CustomerType.Customer, PaymentTermDays=21 },
        new Customer { Id=3, Name="TechMart GmbH", ShortName="TECHMART", NIP="DE123456789", Email="orders@techmart.de", Phone="+49 30 123456", City="Berlin", Type=CustomerType.Customer, PaymentTermDays=30, DefaultDiscount=3 },
        new Customer { Id=4, Name="Metaltech S.A.", ShortName="METALTECH", NIP="1122334455", Email="sprzedaz@metaltech.pl", Phone="61 555 44 33", City="Poznań", Type=CustomerType.Supplier, PaymentTermDays=14 },
        new Customer { Id=5, Name="GlobalFix Sp. z o.o.", ShortName="GLOBALFIX", NIP="5566778899", Email="info@globalfix.pl", Phone="71 333 22 11", City="Wrocław", Type=CustomerType.Both, PaymentTermDays=7, IsActive=false },
        new Customer { Id=6, Name="HydroPlast Sp. j.", ShortName="HYDROPLAST", NIP="6677889900", Email="kontakt@hydroplast.pl", Phone="32 444 55 66", City="Katowice", Type=CustomerType.Supplier, PaymentTermDays=21 },
    };
}
