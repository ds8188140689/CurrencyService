namespace CurrencyService.Shared.Domain.Entities;

/// <summary>
/// Сущность валюты
/// </summary>
public class Currency
{
    /// <summary>
    /// Идентификатор валюты
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Название валюты
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Код валюты
    /// </summary>
    public string CharCode { get; set; } = string.Empty;

    /// <summary>
    /// Курс к рублю
    /// </summary>
    public decimal Rate { get; set; }

    /// <summary>
    /// Дата последнего обновления курса
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}
