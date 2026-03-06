namespace CurrencyService.FinanceService.Application.Dto;

/// <summary>
/// DTO результата выбора курсов избранных валют пользователя
/// </summary>
public class CurrencyRateDto
{
    /// <summary>
    /// Название валюты
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Код валюты
    /// </summary>
    public string CharCode { get; set; } = string.Empty;

    /// <summary>
    /// Курс валюты к рублю
    /// </summary>
    public decimal Rate { get; set; }

    /// <summary>
    /// Дата последнего обновления курса по данным ЦБ
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}
