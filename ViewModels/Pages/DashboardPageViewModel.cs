using System.Collections.Generic;
using WMS.Models;

namespace WMS.ViewModels.Pages;

public partial class DashboardPageViewModel : ViewModelBase
{
    public int TotalProducts { get; } = 148;
    public int OpenOrders { get; } = 23;
    public int TotalCustomers { get; } = 57;
    public int LowStockItems { get; } = 6;

    public List<ActivityItem> RecentActivity { get; } = new()
    {
        new() { Text="WZ/06/2026 – wydano 120 szt. Śruba M8 dla Acme Sp. z o.o.", Time="2 min temu" },
        new() { Text="PZ/05/2026 – przyjęto 500 szt. Nakrętka M8 od Metaltech S.A.", Time="18 min temu" },
        new() { Text="Nowe zamówienie ZS/2026/042 – BuildRight Sp. z o.o. (2 340,00 zł)", Time="1 godz. temu" },
        new() { Text="MM/06/2026 – przesunięto 80 szt. Rura PVC 50mm z MAG-1 → MAG-2", Time="2 godz. temu" },
        new() { Text="RW/04/2026 – rozchód wewnętrzny 10 szt. Opaska zaciskowa 200mm", Time="3 godz. temu" },
    };
}
