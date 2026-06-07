using System;
using SQLite;

namespace WMS.Models;

[Table("Warehouses")]
public class Warehouse
{
    [PrimaryKey, AutoIncrement] public int Id { get; set; }
    [NotNull] public string Code { get; set; } = "";
    [NotNull] public string Name { get; set; } = "";
    public string? Description { get; set; }
    public string? Street      { get; set; }
    public string? City        { get; set; }
    public string? PostalCode  { get; set; }
    public string? Country     { get; set; } = "PL";
    public string? ManagerName { get; set; }
    public string? Phone       { get; set; }
    public string? Email       { get; set; }
    public bool    IsDefault   { get; set; }
    public bool    IsActive    { get; set; } = true;
    public DateTime CreatedAt  { get; set; } = DateTime.UtcNow;
}
