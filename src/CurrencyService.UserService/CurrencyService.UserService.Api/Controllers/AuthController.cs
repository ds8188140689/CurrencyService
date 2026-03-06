using CurrencyService.Shared.Application.Interfaces;
using CurrencyService.UserService.Application.Commands.LoginUser;
using CurrencyService.UserService.Application.Commands.LogoutUser;
using CurrencyService.UserService.Application.Commands.RegisterUser;
using CurrencyService.UserService.Application.Dto;
using CurrencyService.UserService.Application.Mappers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyService.UserService.Api.Controllers;

/// <summary>
/// Контроллер аутентификации пользователей
/// </summary>
/// <param name="mediator">Интерфейс сервиса MediatR</param>
/// <param name="jwt">Интерфейс сервиса JWT</param>
[ApiController]
[Route("api/[controller]")]
public class AuthController(IMediator mediator, IJwtService jwt) : ControllerBase
{
    /// <summary>
    /// Зарегистрировать пользователя
    /// </summary>
    /// <param name="cmd">Команда регистрации пользователя</param>
    /// <param name="ct">Токен отмены операции</param>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand cmd, CancellationToken ct)
    {
        var result = await mediator.Send(cmd, ct);

        return result.IsSuccess 
            ? Created($"/api/auth/{result.Value}", result) 
            : BadRequest(result.Error);
    }

    /// <summary>
    /// Выполнить вход пользователя
    /// </summary>
    /// <param name="cmd">Команда логина пользователя</param>
    /// <param name="ct">Токен отмены операции</param>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserCommand cmd, CancellationToken ct)
    {
        var result = await mediator.Send(cmd, ct);
        if (!result.IsSuccess) return Unauthorized(result.Error);

        var token = jwt.GenerateToken(result.Value!);

        return Ok(new { token, user = UserMapper.ToDto(result.Value!)});
    }

    /// <summary>
    /// Выполнить выход пользователя
    /// </summary>
    /// <param name="ct">Токен отмены операции</param>
    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout(CancellationToken ct)
    {
        LogoutUserCommand cmd = new(Request);
        var result = await mediator.Send(cmd, ct);

        return result.IsSuccess
            ? Ok(new { message = "Вы успешно вышли" })
            : BadRequest(new { error = result.Error });
    }
}
