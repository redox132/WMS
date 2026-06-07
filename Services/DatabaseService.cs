using System.IO;
using SQLite;

namespace WMS.Services;

/// <summary>Provides the single shared SQLiteConnection for the application.</summary>
public sealed class DatabaseService
{
    public static DatabaseService Instance { get; } = new();

    public SQLiteConnection Connection { get; }

    private DatabaseService()
    {
        var dbPath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "wms.db");
        Connection = new SQLiteConnection(dbPath);
    }
}
