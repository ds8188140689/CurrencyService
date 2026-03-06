using CurrencyService.Shared.Application.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace CurrencyService.Shared.Infrastructure.Auth;

///<inheritdoc/>
public class JwtAuthenticationHandler(
    IOptionsMonitor<JwtBearerOptions> options,
    IJwtService jwtService,
    ILoggerFactory logger) : AuthenticationHandler<JwtBearerOptions>(options, logger, UrlEncoder.Default)
{
    /// <inheritdoc/>
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        try
        {
            var token = jwtService.GetTokenFromRequest(Request);
            if (string.IsNullOrEmpty(token))
                return AuthenticateResult.NoResult();

            var validationParameters = Options.TokenValidationParameters.Clone();
            var tokenValidationResult = await jwtService.ValidateTokenAsync(token, validationParameters);

            if (!tokenValidationResult.IsValid)
                return AuthenticateResult.Fail("Токен невалиден");

            var ticket = new AuthenticationTicket(new ClaimsPrincipal(tokenValidationResult.ClaimsIdentity), Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
        catch (Exception ex)
        {
            return AuthenticateResult.Fail(ex);
        }
    }
}
