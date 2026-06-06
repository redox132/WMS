namespace WMS.Models;

public enum CustomerType { Customer, Supplier, Both }

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string ShortName { get; set; } = "";
    public string NIP { get; set; } = "";
    public string Email { get; set; } = "";
    public string Phone { get; set; } = "";
    public string City { get; set; } = "";
    public string Address { get; set; } = "";
    public CustomerType Type { get; set; } = CustomerType.Customer;
    public int PaymentTermDays { get; set; } = 14;
    public decimal DefaultDiscount { get; set; }
    public bool IsActive { get; set; } = true;

    public string TypeLabel => Type switch
    {
        CustomerType.Customer => "Odbiorca",
        CustomerType.Supplier => "Dostawca",
        CustomerType.Both     => "Odbiorca/Dostawca",
        _                     => ""
    };
}
