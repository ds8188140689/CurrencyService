using CurrencyService.Shared.Application.Common;
using CurrencyService.Shared.Domain.Entities;
using MediatR;

namespace CurrencyService.UserService.Application.Commands.LoginUser;

/// <summary>
/// Команда входа пользователя
/// </summary>
/// <param name="Login">Логин пользователя</param>
/// <param name="Password">Пароль пользователя</param>
public record LoginUserCommand(string Login, string Password) : IRequest<Result<User>>;
