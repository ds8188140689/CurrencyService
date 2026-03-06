using CurrencyService.Shared.Application.Common;
using CurrencyService.Shared.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Unit = CurrencyService.Shared.Application.Common.Unit;

namespace CurrencyService.UserService.Application.Commands.LogoutUser;

/// <summary>
/// Обработчик выхода пользователя
/// </summary>
/// <param name="jwtService">Интерфейс сервиса JWT</param>
/// <param name="tokenBlacklistService">Интерфейс сервиса отозванных токенов</param>
/// <param name="jwtOptionsMonitor">Монитор состояния опций Jwt Bearer</param>
public class LogoutHandler(
    IJwtService jwtService, 
    ITokenBlacklistService tokenBlacklistService, 
    IOptionsMonitor<JwtBearerOptions> jwtOptionsMonitor) : IRequestHandler<LogoutUserCommand, Result<Unit>>
{
    /// <summary>
    /// Обработать выхода пользователя
    /// </summary>
    /// <param name="cmd">Команда выхода пользователя</param>
    /// <param name="ct">Токен отмены операции</param>
    public async Task<Result<Unit>> Handle(LogoutUserCommand cmd, CancellationToken ct)
    {
        var token = jwtService.GetTokenFromRequest(cmd.Request);
        if (string.IsNullOrEmpty(token))
            return Result<Unit>.Failure("Токен не найден");

        var jwtOptions = jwtOptionsMonitor.Get(JwtBearerDefaults.AuthenticationScheme);
        var validationParameters = jwtOptions.TokenValidationParameters.Clone();
        validationParameters.ValidateLifetime = false;

        var tokenValidationResult = await jwtService.ValidateTokenAsync(token, validationParameters);
        if (!tokenValidationResult.IsValid)
            return Result<Unit>.Failure("Токен невалиден");

        var tokenId = tokenValidationResult.SecurityToken.Id;
        var expiration = tokenValidationResult.SecurityToken.ValidTo;
        var now = DateTime.UtcNow;

        if (expiration > now)
        {
            var ttl = expiration - now;
            await tokenBlacklistService.BlacklistTokenAsync($"blacklist:{tokenId}", ttl);
        }

        return Result<Unit>.Success(Unit.Value);
    }
}
