namespace CurrencyService.Shared.Application.Interfaces;

/// <summary>
/// Интерфейс доступа к текущему пользователю
/// </summary>
public interface ICurrentUserAccessor
{
    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    Guid? UserId { get; }

    /// <summary>
    /// Имя пользователя
    /// </summary>
    string? UserName { get; }

    /// <summary>
    /// Признак аутентификации
    /// </summary>
    bool IsAuthenticated { get; }
}
