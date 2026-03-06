namespace CurrencyService.Shared.Application.Interfaces;

/// <summary>
/// Интерфейс сервиса отозванных токенов
/// </summary>
public interface ITokenBlacklistService
{
    /// <summary>
    /// Поместить токен в список отозванных
    /// </summary>
    /// <param name="key">Ключ (идентификатор) токена</param>
    /// <param name="remainingTime">Оставшееся время жизни токена</param>
    Task<bool> BlacklistTokenAsync(string key, TimeSpan remainingTime);

    /// <summary>
    /// Проверить, что токен в списке отозванных
    /// </summary>
    /// <param name="key">Ключ (идентификатор) токена</param>
    Task<bool> IsTokenBlacklistedAsync(string key);
}
