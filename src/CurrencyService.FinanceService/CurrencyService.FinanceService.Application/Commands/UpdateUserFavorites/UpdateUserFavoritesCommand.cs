using CurrencyService.Shared.Application.Common;
using MediatR;

namespace CurrencyService.FinanceService.Application.Commands.UpdateUserFavorites;

/// <summary>
/// Команда изменения набора избранных валют пользователя
/// </summary>
/// <param name="UserId">Идентификатор пользователя</param>
/// <param name="CharCodes">Список кодов избранных валют</param>
public record UpdateUserFavoritesCommand(Guid UserId, List<string> CharCodes) : IRequest<Result<Shared.Application.Common.Unit>>;
