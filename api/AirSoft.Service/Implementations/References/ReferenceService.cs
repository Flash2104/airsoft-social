using AirSoft.Service.Contracts;
using AirSoft.Service.Contracts.References;
using AirSoft.Service.Contracts.References.Cities;
using Microsoft.Extensions.Logging;

namespace AirSoft.Service.Implementations.References;

public class ReferenceService : IReferenceService
{
    private readonly ILogger<ReferenceService> _logger;
    private readonly ICorrelationService _correlationService;
    private readonly IDataService _dataService;

    public ReferenceService(ILogger<ReferenceService> logger, ICorrelationService correlationService, IDataService dataService)
    {
        _logger = logger;
        _correlationService = correlationService;
        _dataService = dataService;
    }

    public async Task<GetCityReferencesResponse> GetCities(GetCityReferencesRequest request)
    {
        var dbCities = await _dataService.Cities.ListAsync(x => x.CountryIsoCode == request.CountryIsoCode);
        return new GetCityReferencesResponse(dbCities.Select(x => new CityReferenceData(
                x.Id,
                x.CityAddress,
                x.FederalDistrict,
                x.RegionType,
                x.Region,
                x.City
            )).ToList());
    }
}