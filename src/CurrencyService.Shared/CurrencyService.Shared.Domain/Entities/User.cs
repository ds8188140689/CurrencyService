namespace CurrencyService.Shared.Domain.Entities;

/// <summary>
/// Сущность пользователя
/// </summary>
public class User
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Имя пользователя
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Логин
    /// </summary>
    public string Login { get; set; } = string.Empty;

    /// <summary>
    /// Пароль
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Список избранных валют
    /// </summary>
    public List<UserFavoriteCurrency> FavoriteCurrencies { get; set; } = [];
}
