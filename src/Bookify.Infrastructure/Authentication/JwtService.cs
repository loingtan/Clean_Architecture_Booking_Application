using Bookify.Application.Abstractions.Authentication;
using Bookify.Application.Abstractions.Caching;
using Bookify.Application.Abstractions.Clock;
using Bookify.Domain.Entities.Abstractions;
using Bookify.Infrastructure.Authentication.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace Bookify.Infrastructure.Authentication;
internal sealed class JwtService : IJwtService
{
    private static readonly Error AuthenticationFailed = new("Keycloak.AuthenticationFailed",
        "Failed to acquire access token to do authentication failure");

    private readonly HttpClient _httpClient;
    private readonly KeycloakOptions _keycloakOptions;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICacheService _cacheService;

    public JwtService(HttpClient httpClient, IOptions<KeycloakOptions> keycloakOptions, IHttpContextAccessor contextAccessor, IDateTimeProvider dateTimeProvider, ICacheService cacheService)
    {
        _httpClient = httpClient;
        _keycloakOptions = keycloakOptions.Value;
        _contextAccessor = contextAccessor;
        _dateTimeProvider = dateTimeProvider;
        _cacheService = cacheService;
    }

    public async Task<Result<string>> GetAccessTokenAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        try
        {
            var authRequestParameters = new KeyValuePair<string, string>[]
            {
                new("client_id", _keycloakOptions.AuthClientId),
                new("client_secret", _keycloakOptions.AuthClientSecret),
                new("scope", "openid email"),
                new("grant_type", "password"),
                new("username", email),
                new("password", password)
            };

            var authorizationRequestContent = new FormUrlEncodedContent(authRequestParameters);

            var response = await _httpClient.PostAsync("", authorizationRequestContent, cancellationToken);
            response.EnsureSuccessStatusCode();

            var authorizationToken = await response.Content.ReadFromJsonAsync<AuthorizationToken>(cancellationToken: cancellationToken);
            if (authorizationToken is null) return Result.Failure<string>(AuthenticationFailed);

            return authorizationToken.AccessToken;
        }
        catch (HttpRequestException)
        {
            return Result.Failure<string>(AuthenticationFailed);
        }
    }
    public async Task InvalidateTokenAsync(CancellationToken cancellationToken = default)
    {
        var expiryTime = _dateTimeProvider.Parse(_contextAccessor.HttpContext?.User.GetExpiration()) - _dateTimeProvider.UtcNow;
        await _cacheService.SetAsync($"token-${_contextAccessor.HttpContext?.User.GetTokenId()}", _contextAccessor.HttpContext?.User.GetTokenId(), expiryTime, cancellationToken);


    }
}
