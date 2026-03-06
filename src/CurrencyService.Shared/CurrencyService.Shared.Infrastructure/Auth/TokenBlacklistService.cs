using CurrencyService.Shared.Application.Interfaces;
using StackExchange.Redis;

namespace CurrencyService.Shared.Infrastructure.Auth;

///<inheritdoc/>
public class TokenBlacklistService(IDatabase redis) : ITokenBlacklistService
{
    ///<inheritdoc/>
    public async Task<bool> BlacklistTokenAsync(string key, TimeSpan remainingTime)
    {
        return await redis.StringSetAsync(
            key,
            "revoked",
            remainingTime,
            When.NotExists
        );
    }

    ///<inheritdoc/>
    public async Task<bool> IsTokenBlacklistedAsync(string key)
    {
        return await redis.KeyExistsAsync(key);
    }
}
