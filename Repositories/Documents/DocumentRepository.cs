using System.Collections.Generic;
using System.Linq;
using SQLite;
using WMS.Models;
using WMS.Repositories.Base;

namespace WMS.Repositories.Documents;

public sealed class DocumentRepository : IDocumentRepository
{
    private readonly SQLiteConnection _db;

    public DocumentRepository(SQLiteConnection db) => _db = db;

    // ── Document CRUD ─────────────────────────────────────────────────────────

    public WarehouseDocument? GetById(int id)
    {
        var doc = _db.Find<WarehouseDocument>(id);
        if (doc is not null) doc.Lines = GetLines(id);
        return doc;
    }

    public List<WarehouseDocument> GetAll()
    {
        var docs = _db.Table<WarehouseDocument>().ToList();
        foreach (var d in docs) d.Lines = GetLines(d.Id);
        return docs;
    }

    public WarehouseDocument?      GetByNumber(string number)    => _db.Table<WarehouseDocument>().FirstOrDefault(d => d.Number == number);
    public List<WarehouseDocument> GetByType(DocumentType t)     => _db.Table<WarehouseDocument>().Where(d => d.Type == t).ToList();
    public List<WarehouseDocument> GetByStatus(DocumentStatus s) => _db.Table<WarehouseDocument>().Where(d => d.Status == s).ToList();

    public List<WarehouseDocument> GetRecent(int count = 5) =>
        _db.Query<WarehouseDocument>("SELECT * FROM WarehouseDocuments ORDER BY Date DESC LIMIT ?", count);

    public int Insert(WarehouseDocument entity)
    {
        entity.CreatedAt = entity.UpdatedAt = System.DateTime.UtcNow;
        return _db.Insert(entity);
    }

    public int Update(WarehouseDocument entity)
    {
        entity.UpdatedAt = System.DateTime.UtcNow;
        return _db.Update(entity);
    }

    public int Delete(int id)
    {
        _db.Execute("DELETE FROM DocumentLines WHERE DocumentId = ?", id);
        return _db.Delete<WarehouseDocument>(id);
    }

    // ── Document Lines ────────────────────────────────────────────────────────

    public List<DocumentLine> GetLines(int documentId) => _db.Table<DocumentLine>().Where(l => l.DocumentId == documentId).ToList();
    public int InsertLine(DocumentLine line)            => _db.Insert(line);
    public int UpdateLine(DocumentLine line)            => _db.Update(line);
    public int DeleteLine(int lineId)                   => _db.Delete<DocumentLine>(lineId);
}
