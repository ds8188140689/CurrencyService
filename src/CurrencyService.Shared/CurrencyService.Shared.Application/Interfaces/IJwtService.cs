using CurrencyService.Shared.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace CurrencyService.Shared.Application.Interfaces;

/// <summary>
/// Интерфейс сервиса Jwt
/// </summary>
public interface IJwtService
{
    /// <summary>
    /// Сгенерировать токен JWT
    /// </summary>
    /// <param name="user">Пользователь</param>
    string GenerateToken(User user);

    /// <summary>
    /// Получить токен из запроса
    /// </summary>
    /// <param name="request">Запрос</param>
    string? GetTokenFromRequest(HttpRequest request);

    /// <summary>
    /// Валидировать токен
    /// </summary>
    /// <param name="token">Токен</param>
    /// <param name="validationParameters">Параметры валидации</param>
    Task<TokenValidationResult> ValidateTokenAsync(string token, TokenValidationParameters validationParameters);
}
