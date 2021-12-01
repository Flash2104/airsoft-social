
using AirSoft.Service.Common;
using AirSoft.Service.Contracts;
using AirSoft.Service.Contracts.Jwt.Model;
using AirSoft.Service.Exceptions;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AirSoft.Service.Implementation.Jwt;

public class JwtService
{
    private readonly IConfigService _configService;

    public JwtService(
        IConfigService configService
    )
    {
        _configService = configService;
    }
    public Task<JwtResponse> BuildToken(JwtRequest request)
    {
        var jwtSettings = _configService.GetSettings().Jwt;
        if (jwtSettings?.Key == null || jwtSettings.Issuer == null || jwtSettings.ExpiresSeconds == null)
        {
            throw new AirSoftBaseException(ErrorCodes.JwtSettingsIsNull, "Jwt Settings is null.");
        }
        var key = Encoding.ASCII.GetBytes(jwtSettings.Key);

        var expires = DateTime.UtcNow.AddSeconds(jwtSettings.ExpiresSeconds.GetValueOrDefault());
        var expiresStamp = expires.ToString("O");
        var issuedAt = DateTime.UtcNow;
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Expired, expiresStamp),
            }),
            Expires = expires,
            IssuedAt = issuedAt,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var secToken = tokenHandler.CreateToken(tokenDescriptor);
        var token = tokenHandler.WriteToken(secToken);
        return Task.FromResult(new JwtResponse(token, expires));
    }
}
