using CurrencyService.Shared.Application.Common;
using CurrencyService.Shared.Application.Interfaces;
using CurrencyService.Shared.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CurrencyService.UserService.Application.Commands.RegisterUser;

/// <summary>
/// Обработчик регистрации пользователя
/// </summary>
/// <param name="ctx">Контекст БД</param>
/// <param name="passwordHasher">Интерфейс сервиса отозванных токенов</param>
public class RegisterUserHandler(IAppDbContext ctx, IPasswordHasher<User> passwordHasher) : IRequestHandler<RegisterUserCommand, Result<Guid>>
{
    /// <summary>
    /// Обработать регистрацию пользователя
    /// </summary>
    /// <param name="cmd">Команда регистрации пользователя</param>
    /// <param name="ct">Токен отмены операции</param>
    public async Task<Result<Guid>> Handle(RegisterUserCommand cmd, CancellationToken ct)
    {
        if (await ctx.Users.AnyAsync(u => u.Login == cmd.Login, ct))
            return Result<Guid>.Failure("Пользователь с таким логином уже существует");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Name = cmd.Name,
            Login = cmd.Login
        };

        user.Password = passwordHasher.HashPassword(user, cmd.Password);

        try
        {
            await ctx.Users.AddAsync(user, ct);
            await ctx.SaveChangesAsync(ct);
        }
        catch (DbUpdateException)
        {
            return Result<Guid>.Failure("Произошла ошибка при регистрации");
        }

        return Result<Guid>.Success(user.Id);
    }
}
