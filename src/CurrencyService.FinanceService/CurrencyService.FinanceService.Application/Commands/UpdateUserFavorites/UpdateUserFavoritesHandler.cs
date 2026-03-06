using CurrencyService.Shared.Application.Common;
using CurrencyService.Shared.Application.Interfaces;
using CurrencyService.Shared.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Unit = CurrencyService.Shared.Application.Common.Unit;

namespace CurrencyService.FinanceService.Application.Commands.UpdateUserFavorites;

/// <summary>
/// Обработчик команды изменения набора избранных валют пользователя
/// </summary>
/// <param name="ctx">Контекст БД</param>
public class UpdateUserFavoritesHandler(IAppDbContext ctx) : IRequestHandler<UpdateUserFavoritesCommand, Result<Unit>>
{
    /// <summary>
    /// Выполнить обработку команды изменения набора избранных валют пользователя
    /// </summary>
    /// <param name="cmd">Команда изменения набора избранных валют пользователя</param>
    /// <param name="ct">Токен отмены операции</param>
    public async Task<Result<Unit>> Handle(UpdateUserFavoritesCommand cmd, CancellationToken ct)
    {
        User? user = await ctx.Users.FirstOrDefaultAsync(u => u.Id == cmd.UserId, ct);

        if (user == null)
            return Result.Failure("Пользователь не найден");

        var existingFavorites = await ctx.UserFavoriteCurrencies
            .Where(ufc => ufc.UserId == user.Id)
            .ToListAsync(ct);

        if (existingFavorites.Count > 0)
            ctx.UserFavoriteCurrencies.RemoveRange(existingFavorites);

        var currencyIds = await ctx.Currencies
            .Where(c => cmd.CharCodes.Select(cd => cd.ToUpper()).Contains(c.CharCode.ToUpper()))
            .Select(c => c.Id)
            .ToListAsync(ct);

        if (currencyIds.Count > 0)
        {
            var newFavorites = currencyIds
                .Select(id => new UserFavoriteCurrency
                {
                    UserId = user.Id,
                    CurrencyId = id
                })
                .ToList();

            ctx.UserFavoriteCurrencies.AddRange(newFavorites);
        }

        await ctx.SaveChangesAsync(ct);

        return Result<Unit>.Success(Unit.Value);
    }
}
