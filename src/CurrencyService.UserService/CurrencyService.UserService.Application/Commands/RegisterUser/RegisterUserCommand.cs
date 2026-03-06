using CurrencyService.Shared.Application.Common;
using MediatR;

namespace CurrencyService.UserService.Application.Commands.RegisterUser;

/// <summary>
/// Команда регистрации пользователя
/// </summary>
/// <param name="Name">Имя пользователя</param>
/// <param name="Login">Логин пользователя</param>
/// <param name="Password">Пароль пользователя</param>
public record RegisterUserCommand(string Name, string Login, string Password) : IRequest<Result<Guid>>;
