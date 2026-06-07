using SQLite;

namespace WMS.Services.Migrations;

public interface IMigration
{
    int    Version     { get; }
    string Description { get; }
    void   Up(SQLiteConnection db);
}
