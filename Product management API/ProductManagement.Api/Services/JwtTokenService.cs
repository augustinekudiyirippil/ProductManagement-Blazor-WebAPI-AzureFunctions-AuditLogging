using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using ProductManagement.Api.Configuration;
using ProductManagement.Api.Models;

namespace ProductManagement.Api.Services;

public class JwtTokenService : IJwtTokenService
{
    private readonly JwtSettings _settings;

    public JwtTokenService(
        IOptions<JwtSettings> options)
    {
        _settings = options.Value;
    }

    public string GenerateToken(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(
                ClaimTypes.Name,
                user.Username),

            new Claim(
                ClaimTypes.Email,
                user.Email),

            new Claim(
                "DisplayName",
                user.Name)
        };

        var key =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _settings.Key));

        var credentials =
            new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

        var token =
            new JwtSecurityToken(
                issuer: _settings.Issuer,
                audience: _settings.Audience,
                claims: claims,
                expires:
                    DateTime.UtcNow.AddMinutes(
                        _settings.ExpiryMinutes),
                signingCredentials:
                    credentials);

        return new JwtSecurityTokenHandler()
            .WriteToken(token);
    }
}