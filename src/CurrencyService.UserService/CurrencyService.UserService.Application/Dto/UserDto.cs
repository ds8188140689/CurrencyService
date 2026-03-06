namespace CurrencyService.UserService.Application.Dto;

/// <summary>
/// Dto пользователя
/// </summary>
public class UserDto
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
}
