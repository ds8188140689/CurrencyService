using CurrencyService.Shared.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CurrencyService.Shared.Application.Interfaces
{
    /// <summary>
    /// Интерфейс контекста БД
    /// </summary>
    public interface IAppDbContext
    {
        /// <summary>
        /// Список валют
        /// </summary>
        DbSet<Currency> Currencies { get; }

        /// <summary>
        /// Список пользователей
        /// </summary>
        public DbSet<User> Users { get; }

        /// <summary>
        /// Список избранных валют пользователей
        /// </summary>
        public DbSet<UserFavoriteCurrency> UserFavoriteCurrencies { get; }

        /// <summary>
        /// Сохранить изменения
        /// </summary>
        /// <param name="cancellationToken">Токен отмены операции</param>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
