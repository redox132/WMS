using System;
using System.Collections.Generic;
using System.Linq;

namespace WMS.Models;

public enum DocumentType { WZ, PZ, MM, PW, RW }

public enum DocumentStatus { Draft, Confirmed, Cancelled }

public class WarehouseDocument
{
    public int Id { get; set; }
    public string Number { get; set; } = "";
    public DocumentType Type { get; set; }
    public DocumentStatus Status { get; set; } = DocumentStatus.Confirmed;
    public DateTime Date { get; set; }
    public string WarehouseFrom { get; set; } = "";
    public string WarehouseTo { get; set; } = "";
    public string ContractorName { get; set; } = "";
    public List<DocumentLine> Lines { get; set; } = new();

    public decimal TotalValue => Lines.Sum(l => l.TotalPrice);

    public string TypeDescription => Type switch
    {
        DocumentType.WZ => "Wydanie Zewnętrzne",
        DocumentType.PZ => "Przyjęcie Zewnętrzne",
        DocumentType.MM => "Przesunięcie MM",
        DocumentType.PW => "Przychód Wewnętrzny",
        DocumentType.RW => "Rozchód Wewnętrzny",
        _               => ""
    };

    public string StatusLabel => Status switch
    {
        DocumentStatus.Draft     => "Szkic",
        DocumentStatus.Confirmed => "Zatwierdzone",
        DocumentStatus.Cancelled => "Anulowane",
        _                        => ""
    };

    public string StatusColor => Status switch
    {
        DocumentStatus.Draft     => "#616161",
        DocumentStatus.Confirmed => "#2E7D32",
        DocumentStatus.Cancelled => "#B71C1C",
        _                        => "#616161"
    };

    public string TypeColor => Type switch
    {
        DocumentType.WZ => "#1565C0",
        DocumentType.PZ => "#2E7D32",
        DocumentType.MM => "#E65100",
        DocumentType.PW => "#6A1B9A",
        DocumentType.RW => "#B71C1C",
        _               => "#616161"
    };

    public string MovementDescription => Type switch
    {
        DocumentType.WZ => $"{WarehouseFrom} → {ContractorName}",
        DocumentType.PZ => $"{ContractorName} → {WarehouseTo}",
        DocumentType.MM => $"{WarehouseFrom} → {WarehouseTo}",
        DocumentType.PW => $"→ {WarehouseTo}",
        DocumentType.RW => $"{WarehouseFrom} →",
        _               => ""
    };
}
