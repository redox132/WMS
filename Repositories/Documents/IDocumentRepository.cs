using System.Collections.Generic;
using WMS.Models;
using WMS.Repositories.Base;

namespace WMS.Repositories.Documents;

public interface IDocumentRepository : IRepository<WarehouseDocument>
{
    WarehouseDocument?       GetByNumber(string number);
    List<WarehouseDocument>  GetByType(DocumentType type);
    List<WarehouseDocument>  GetByStatus(DocumentStatus status);
    List<WarehouseDocument>  GetRecent(int count = 5);

    List<DocumentLine> GetLines(int documentId);
    int                InsertLine(DocumentLine line);
    int                UpdateLine(DocumentLine line);
    int                DeleteLine(int lineId);
}
