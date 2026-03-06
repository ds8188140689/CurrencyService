using CurrencyService.Shared.Application.Interfaces;
using CurrencyService.Shared.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CurrencyService.Shared.Infrastructure.Auth;

///<inheritdoc/>
/// <summary>
/// Конструктор
/// </summary>
/// <param name="settings">Настройки JWT</param>
/// <param name="tokenBlacklistService">Сервис отозванных токенов</param>
public class JwtService(
    IOptions<JwtSettings> settings, 
    ITokenBlacklistService tokenBlacklistService) : IJwtService
{
    private readonly JwtSettings _settings = settings.Value;

    ///<inheritdoc/>
    public string GenerateToken(User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var creds = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.SecretKey)), SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(
            issuer: _settings.Issuer,
            audience: _settings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(_settings.TokenLifetimeHours),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    ///<inheritdoc/>
    public string? GetTokenFromRequest(HttpRequest request)
    {
        var authorization = request.Headers.Authorization.FirstOrDefault();

        if (string.IsNullOrEmpty(authorization) || !authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
            return null;

        return authorization["Bearer ".Length..].Trim();
    }

    ///<inheritdoc/>
    public async Task<TokenValidationResult> ValidateTokenAsync(string token, TokenValidationParameters validationParameters)
    {
        TokenValidationResult tokenValidationResult = await new JwtSecurityTokenHandler().ValidateTokenAsync(token, validationParameters);
        if (!tokenValidationResult.IsValid || !tokenValidationResult.ClaimsIdentity.IsAuthenticated)
            return tokenValidationResult;

        var isBlacklisted = await tokenBlacklistService.IsTokenBlacklistedAsync($"blacklist:{tokenValidationResult.SecurityToken.Id}");

        if (isBlacklisted)
            tokenValidationResult.IsValid = false;

        return tokenValidationResult;
    }
}
