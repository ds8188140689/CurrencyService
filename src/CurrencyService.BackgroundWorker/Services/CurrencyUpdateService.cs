using CurrencyService.Shared.Application.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace CurrencyService.BackgroundWorker.Services;

/// <summary>
/// Сервис обновления курсов валют по данным ЦБ
/// </summary>
/// <param name="services">Список сервисов</param>
/// <param name="logger">Логгер</param>
/// <param name="parser">Парсер курсов валют ЦБ</param>
public class CurrencyUpdateService(IServiceProvider services, ILogger<CurrencyUpdateService> logger, CbrXmlParser parser) : BackgroundService
{
    /// <summary>
    /// Интервал обновленния курсов (6 часов)
    /// </summary>
    private readonly TimeSpan _interval = TimeSpan.FromHours(6);

    /// <summary>
    /// Запустить выполнение обновления
    /// </summary>
    /// <param name="ct">Токен отмены операции</param>
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        await DoWork(ct);
        while (!ct.IsCancellationRequested)
        {
            await Task.Delay(_interval, ct);
            await DoWork(ct);
        }
    }

    /// <summary>
    /// Выполнить обновление
    /// </summary>
    /// <param name="ct">Токен отмены операции</param>
    private async Task DoWork(CancellationToken ct)
    {
        try
        {
            using var scope = services.CreateScope();
            var ctx = scope.ServiceProvider.GetRequiredService<IAppDbContext>();
            var currencies = await parser.ParseCurrenciesAsync(ct);

            foreach (var c in currencies)
            {
                var existing = await ctx.Currencies.FirstOrDefaultAsync(x => x.CharCode == c.CharCode, ct);
                if (existing != null)
                {
                    existing.Rate = c.Rate;
                    existing.UpdatedAt = c.UpdatedAt;
                }
                else
                {
                    await ctx.Currencies.AddAsync(c, ct);
                }
            }

            await ctx.SaveChangesAsync(ct);
            logger.LogInformation("Курсы валют обновлены: {Count}", currencies.Count);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Ошибка обновления курсов валют");
        }
    }
}
