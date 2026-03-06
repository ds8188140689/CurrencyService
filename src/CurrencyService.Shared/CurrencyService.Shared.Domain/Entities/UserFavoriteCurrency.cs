namespace CurrencyService.Shared.Domain.Entities;

/// <summary>
/// Сущность избранной валюты
/// </summary>
public class UserFavoriteCurrency
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Идентификатор валюты
    /// </summary>
    public Guid CurrencyId { get; set; }

    /// <summary>
    /// Пользователь
    /// </summary>
    public User User { get; set; } = null!;

    /// <summary>
    /// Валюта
    /// </summary>
    public Currency Currency { get; set; } = null!;
}
