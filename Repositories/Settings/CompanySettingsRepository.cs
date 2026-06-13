using SQLite;
using WMS.Models;
using WMS.Repositories.Base;

namespace WMS.Repositories.Settings;

public class CompanySettingsRepository : ICompanySettingsRepository
{
    private readonly SQLiteConnection _db;

    public CompanySettingsRepository(SQLiteConnection db) => _db = db;

    public CompanySettings Load()
        => _db.Find<CompanySettings>(1) ?? new CompanySettings();

    public void Save(CompanySettings settings)
    {
        settings.Id = 1;
        _db.InsertOrReplace(settings);
    }
}
