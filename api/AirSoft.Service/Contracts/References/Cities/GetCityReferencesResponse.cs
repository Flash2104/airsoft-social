namespace AirSoft.Service.Contracts.References.Cities;

public class GetCityReferencesResponse
{
    public List<CityReferenceData> Cities { get; }

    public GetCityReferencesResponse(List<CityReferenceData> cities)
    {
        Cities = cities;
    }
}

public class CityReferenceData
{
    public int Id { get; }

    public string CityAddress { get; }

    public string? FederalDistrict { get; }

    public string? RegionType { get; set; }

    public string? Region { get; set; }

    public string City { get; }

    public CityReferenceData(int id, string cityAddress, string? federalDistrict, string? regionType, string? region, string city)
    {
        CityAddress = cityAddress;
        FederalDistrict = federalDistrict;
        RegionType = regionType;
        Region = region;
        City = city;
        Id = id;
    }
}