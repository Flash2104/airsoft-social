using AirSoft.Service.Contracts.References.Cities;

namespace AirSoftApi.Models.References.Cities;

public class GetCityReferencesResponseDto
{
    public List<CityReferenceData> Cities { get; }

    public GetCityReferencesResponseDto(List<CityReferenceData> cities)
    {
        Cities = cities;
    }
}