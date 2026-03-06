using CurrencyService.Shared.Application.Common;
using CurrencyService.Shared.Application.Interfaces;
using CurrencyService.Shared.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CurrencyService.UserService.Application.Commands.LoginUser;

/// <summary>
/// Обработчик входа пользователя
/// </summary>
/// <param name="ctx">Контекст БД</param>
/// <param name="passwordHasher">Интерфейс сервиса хэширования пароля</param>
public class LoginUserHandler(IAppDbContext ctx, IPasswordHasher<User> passwordHasher) : IRequestHandler<LoginUserCommand, Result<User>>
{
    /// <summary>
    /// Обработать вход пользователя
    /// </summary>
    /// <param name="cmd">Команда входа пользователя</param>
    /// <param name="ct">Токен отмены операции</param>
    public async Task<Result<User>> Handle(LoginUserCommand cmd, CancellationToken ct)
    {
        User? user = await ctx.Users.FirstOrDefaultAsync(u => u.Login == cmd.Login, ct);

        if (user == null)
            return Result<User>.Failure("Неверный логин");

        if (passwordHasher.VerifyHashedPassword(user, user.Password, cmd.Password) == PasswordVerificationResult.Failed)
            return Result<User>.Failure("Неверный пароль");

        return Result<User>.Success(user);
    }
}
