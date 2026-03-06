using CurrencyService.Shared.Domain.Entities;
using CurrencyService.UserService.Application.Dto;

namespace CurrencyService.UserService.Application.Mappers;

/// <summary>
/// Маппер пользователя в Dto
/// </summary>
public static class UserMapper
{
    public static UserDto ToDto(this User user) => new() 
    { 
        Id = user.Id, 
        Name = user.Name, 
        Login = user.Login
    };
}
