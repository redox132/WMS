using System;
using System.Collections.Generic;
using System.Linq;
using SQLite;
using WMS.Services.Migrations;

namespace WMS.Services;

public class MigrationRunner
{
    private readonly SQLiteConnection   _db;
    private readonly List<IMigration>  _migrations;

    public MigrationRunner(SQLiteConnection db, IEnumerable<IMigration> migrations)
    {
        _db         = db;
        _migrations = migrations.OrderBy(m => m.Version).ToList();
    }

    public void RunPending()
    {
        _db.Execute(@"
            CREATE TABLE IF NOT EXISTS __Migrations (
                Version     INTEGER PRIMARY KEY,
                Description TEXT    NOT NULL,
                AppliedAt   TEXT    NOT NULL
            )");

        var current = _db.ExecuteScalar<int>("SELECT COALESCE(MAX(Version), 0) FROM __Migrations");

        foreach (var m in _migrations.Where(m => m.Version > current))
        {
            m.Up(_db);
            _db.Execute(
                "INSERT INTO __Migrations (Version, Description, AppliedAt) VALUES (?, ?, ?)",
                m.Version, m.Description, DateTime.UtcNow.ToString("o"));
        }
    }
}
