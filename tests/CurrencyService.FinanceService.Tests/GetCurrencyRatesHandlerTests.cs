using CurrencyService.FinanceService.Application.Queries.GetCurrencyRatesForUser;
using CurrencyService.Shared.Domain.Entities;
using CurrencyService.Shared.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace CurrencyService.FinanceService.Tests;

/// <summary>
/// Тест выбора курсов избранных валют пользователя
/// </summary>
public class GetCurrencyRatesHandlerTests
{
    /// <summary>
    /// Получить список курсов избранных валют пользователя
    /// </summary>
    [Fact]
    public async Task Handle_ShouldReturnFavoriteRates()
    {
        var user = new User { Id = Guid.NewGuid(), Login = "test_login" };
        var usd = new Currency { Id = Guid.NewGuid(), CharCode = "USD", Rate = 90.5m, Name = "Доллар США" };
        var fav = new UserFavoriteCurrency { UserId = user.Id, CurrencyId = usd.Id, User = user, Currency = usd };

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        using var ctx = new AppDbContext(options);
        await ctx.Users.AddAsync(user);
        await ctx.Currencies.AddAsync(usd);
        await ctx.UserFavoriteCurrencies.AddAsync(fav);
        await ctx.SaveChangesAsync();

        var handler = new GetCurrencyRatesForUserHandler(ctx);
        var query = new GetCurrencyRatesForUserQuery(user.Id);

        var result = await handler.Handle(query, CancellationToken.None);

        Assert.True(result.IsSuccess);
        var rate = Assert.Single(result.Value);
        Assert.Equal("USD", rate.CharCode);
        Assert.Equal(90.5m, rate.Rate);
    }
}