using CurrencyService.Shared.Application.Interfaces;
using CurrencyService.Shared.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CurrencyService.Shared.Infrastructure.Persistence;

///<inheritdoc/>
public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IAppDbContext
{
    ///<inheritdoc/>
    public DbSet<Currency> Currencies => Set<Currency>();

    ///<inheritdoc/>
    public DbSet<User> Users => Set<User>();

    ///<inheritdoc/>
    public DbSet<UserFavoriteCurrency> UserFavoriteCurrencies => Set<UserFavoriteCurrency>();

    ///<inheritdoc/>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Currency>(e =>
        {
            e.HasKey(x => x.Id);
        });

        modelBuilder.Entity<User>(e =>
        {
            e.HasKey(x => x.Id);
            e.Property(x => x.Login).IsRequired();
            e.HasIndex(x => x.Login).IsUnique();
        });

        modelBuilder.Entity<UserFavoriteCurrency>(e =>
        {
            e.HasKey(x => new { x.UserId, x.CurrencyId });
            e.HasOne(x => x.User).WithMany(u => u.FavoriteCurrencies).HasForeignKey(x => x.UserId);
            e.HasOne(x => x.Currency).WithMany().HasForeignKey(x => x.CurrencyId);
        });
    }
}
