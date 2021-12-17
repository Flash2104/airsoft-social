using AirSoft.Service.Contracts;
using AirSoft.Service.Contracts.References;
using AirSoft.Service.Contracts.References.Cities;
using AirSoftApi.Models;
using AirSoftApi.Models.References.Cities;
using AirSoftApi.Models.Team.GetCurrent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirSoftApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReferencesController : RootController
    {
        private readonly ILogger<ReferencesController> _logger;
        private readonly IReferenceService _referenceService;
        private readonly ICorrelationService _correlationService;

        public ReferencesController(ILogger<ReferencesController> logger, IReferenceService referenceService, ICorrelationService correlationService) : base(logger)
        {
            _logger = logger;
            _referenceService = referenceService;
            _correlationService = correlationService;
        }

        [HttpPost("cities")]
        public async Task<ServerResponseDto<GetCityReferencesResponseDto>> GetCities(GetCityReferencesRequestDto request)
        {
            var logPath = $"{_correlationService.GetUserId()}.{nameof(ReferencesController)} {nameof(GetCities)} | ";
            return await HandleRequest(
                _referenceService.GetCities,
                request,
                (dto) => new GetCityReferencesRequest("RUS"),
                res => new GetCityReferencesResponseDto(res.Cities),
                logPath
            );
        }
    }
}
