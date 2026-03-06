using CurrencyService.FinanceService.Application.Commands.UpdateUserFavorites;
using CurrencyService.FinanceService.Application.Queries.GetCurrencyRatesForUser;
using CurrencyService.Shared.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CurrencyService.FinanceService.Api.Controllers;

/// <summary>
/// Контроллер для работы с финансами
/// </summary>
/// <param name="mediator">IMediator сервис</param>
/// <param name="currentUser">Текущий пользователь</param>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CurrencyController(IMediator mediator, ICurrentUserAccessor currentUser) : ControllerBase
{
    /// <summary>
    /// Получить список курсов избранных валют
    /// </summary>
    /// <param name="ct">Токен отмены операции</param>
    [HttpGet("rates")]
    public async Task<IActionResult> GetRates(CancellationToken ct)
    {
        var userId = currentUser.UserId;

        if (userId == null)
            return Unauthorized();

        var query = new GetCurrencyRatesForUserQuery(userId.Value);
        var result = await mediator.Send(query, ct);

        return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
    }

    /// <summary>
    /// Изменить набор избранных валют пользователя
    /// </summary>
    /// <param name="charCodes">Список кодов избранных валют</param>
    /// <param name="ct">Токен отмены операции</param>
    [HttpPut("favorites")]
    public async Task<IActionResult> UpdateFavorites([FromBody] List<string> charCodes, CancellationToken ct)
    {
        var userId = currentUser.UserId;

        if (userId == null)
            return Unauthorized();

        var command = new UpdateUserFavoritesCommand(userId.Value, charCodes);
        var result = await mediator.Send(command, ct);

        return result.IsSuccess ? NoContent() : BadRequest(result.Error);
    }
}