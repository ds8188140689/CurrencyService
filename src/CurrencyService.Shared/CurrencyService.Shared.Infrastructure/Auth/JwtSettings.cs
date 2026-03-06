namespace CurrencyService.Shared.Infrastructure.Auth;

/// <summary>
/// Настройки JWT
/// </summary>
public class JwtSettings
{
    /// <summary>
    /// Секретный ключ
    /// </summary>
    public string SecretKey { get; set; } = string.Empty;

    /// <summary>
    /// Издатель
    /// </summary>
    public string Issuer { get; set; } = string.Empty;

    /// <summary>
    /// Получатель
    /// </summary>
    public string Audience { get; set; } = string.Empty;

    /// <summary>
    /// Время жизни токена в часах
    /// </summary>
    public int TokenLifetimeHours { get; set; }
}
