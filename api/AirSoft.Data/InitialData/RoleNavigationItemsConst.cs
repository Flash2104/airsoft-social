using System.Collections.ObjectModel;

namespace AirSoft.Data.InitialData;

public static class RoleNavigationItemsConst
{
    public static readonly ReadOnlyCollection<int> PlayerIds = new ReadOnlyCollection<int>(new[]
    {
        1, 2, 3, 6
    });
    public static readonly ReadOnlyCollection<int> LeaderIds = new ReadOnlyCollection<int>(new[]
    {
        1, 2, 3, 4, 5, 6
    });
}