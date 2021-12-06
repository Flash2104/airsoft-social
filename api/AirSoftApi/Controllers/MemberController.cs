using AirSoft.Service.Contracts;
using AirSoft.Service.Contracts.Member;
using AirSoftApi.Models;
using AirSoftApi.Models.Member;
using AirSoftApi.Models.Member.GetCurrent;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirSoftApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MemberController : RootController
    {
        private readonly ILogger<MemberController> _logger;
        private readonly IMemberService _memberService;
        private readonly ICorrelationService _correlationService;

        public MemberController(ILogger<MemberController> logger, IMemberService memberService, ICorrelationService correlationService) : base(logger)
        {
            _logger = logger;
            _memberService = memberService;
            _correlationService = correlationService;
        }

        [HttpGet("get-current")]
        [Authorize]
        public async Task<ServerResponseDto<GetCurrentMemberResponseDto>> GetCurrent()
        {
            var logPath = $"{_correlationService.GetUserId()}.{nameof(MemberController)} {nameof(GetCurrent)} | ";
            return await HandleRequest(
                _memberService.GetCurrentMember,
                res => new GetCurrentMemberResponseDto(
                     new MemberDataDto(
                         res.MemberData.Id,
                         res.MemberData.Name,
                         res.MemberData.Surname,
                         res.MemberData.Email,
                         res.MemberData.Phone,
                         res.MemberData.Avatar?.ToArray(),
                         res.MemberData.TeamId,
                         res.MemberData.Roles
                         )
                    ),
                logPath
            );
        }
    }
}
