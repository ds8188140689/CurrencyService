using CurrencyService.FinanceService.Application.Dto;
using CurrencyService.Shared.Application.Common;
using MediatR;

namespace CurrencyService.FinanceService.Application.Queries.GetCurrencyRatesForUser;

/// <summary>
/// Запрос получения списка курсов избранных валют пользователя
/// </summary>
/// <param name="UserId">Идентификатор пользователя</param>
public record GetCurrencyRatesForUserQuery(Guid UserId) : IRequest<Result<List<CurrencyRateDto>>>;
