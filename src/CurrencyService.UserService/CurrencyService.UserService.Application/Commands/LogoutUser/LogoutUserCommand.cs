using CurrencyService.Shared.Application.Common;
using MediatR;
using Microsoft.AspNetCore.Http;
using Unit = CurrencyService.Shared.Application.Common.Unit;

namespace CurrencyService.UserService.Application.Commands.LogoutUser;

/// <summary>
/// Команда выхода пользователя
/// </summary>
/// <param name="request">Запрос</param>
public record LogoutUserCommand(HttpRequest Request) : IRequest<Result<Unit>>;
