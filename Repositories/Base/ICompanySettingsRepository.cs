using WMS.Models;

namespace WMS.Repositories.Base;

public interface ICompanySettingsRepository
{
    CompanySettings Load();
    void Save(CompanySettings settings);
}
