using AirSoft.Service.Contracts.Models;

namespace AirSoft.Service.Contracts.Navigation;

public class UserNavigationDataResponse
{
    public List<RolesNavigationData> Data { get; }

    public UserNavigationDataResponse(List<RolesNavigationData>? data)
    {
        Data = data ?? new List<RolesNavigationData>();
    }
}

public class RolesNavigationData
{
    public ReferenceData<int>? Role { get; }
    public List<NavigationItem>? NavItems { get; }

    public RolesNavigationData( ReferenceData<int>? role, List<NavigationItem>? navItems)
    {
        Role = role;
        NavItems = navItems;
    }
}

public class NavigationItem
{
    public int Id { get; }
    public string Path { get; }
    public string Title { get; }
    public string? Icon { get; }

    public int Order { get; }
    public List<NavigationItem> Children { get; }

    public NavigationItem(int id, string path, string title, string? icon, int order, List<NavigationItem> children)
    {
        Id = id;
        Path = path;
        Title = title;
        Icon = icon;
        Children = children;
        Order = order;
    }
}