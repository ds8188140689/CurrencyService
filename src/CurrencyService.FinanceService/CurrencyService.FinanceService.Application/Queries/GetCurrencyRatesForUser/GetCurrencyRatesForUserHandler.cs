using CurrencyService.FinanceService.Application.Dto;
using CurrencyService.Shared.Application.Common;
using CurrencyService.Shared.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CurrencyService.FinanceService.Application.Queries.GetCurrencyRatesForUser;

/// <summary>
/// Обработчик запроса получения списка курсов избранных валют пользователя
/// </summary>
/// <param name="ctx">Контекст БД</param>
public class GetCurrencyRatesForUserHandler(IAppDbContext ctx) : IRequestHandler<GetCurrencyRatesForUserQuery, Result<List<CurrencyRateDto>>>
{
    /// <summary>
    /// Обработать запрос получения списка курсов избранных валют пользователя
    /// </summary>
    /// <param name="query">Запрос получения списка курсов избранных валют пользователя</param>
    /// <param name="ct">Токен отмены операции</param>
    public async Task<Result<List<CurrencyRateDto>>> Handle(GetCurrencyRatesForUserQuery query, CancellationToken ct)
    {
        var favorites = await ctx.UserFavoriteCurrencies
            .Where(x => x.UserId == query.UserId)
            .Include(x => x.Currency)
            .Select(x => x.Currency)
            .ToListAsync(ct);

        if (favorites.Count == 0)
            return Result<List<CurrencyRateDto>>.Success([]);

        List<CurrencyRateDto> rates = [.. favorites.Select(c => new CurrencyRateDto()
        {
            CharCode = c.CharCode,
            Name = c.Name,
            Rate = c.Rate,
            UpdatedAt = c.UpdatedAt
        })];

        return Result<List<CurrencyRateDto>>.Success(rates);
    }
}
