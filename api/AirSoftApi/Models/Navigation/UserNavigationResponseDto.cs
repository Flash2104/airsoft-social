using AirSoft.Service.Contracts.Navigation;

namespace AirSoftApi.Models.Navigation;

public class UserNavigationResponseDto
{
    public List<RolesNavigationData> Data { get; }

    public UserNavigationResponseDto(List<RolesNavigationData>? data)
    {
        Data = data ?? new List<RolesNavigationData>();
    }
}