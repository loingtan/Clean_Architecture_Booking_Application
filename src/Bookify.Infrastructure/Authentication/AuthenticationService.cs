using Bookify.Application.Abstractions.Authentication;
using Bookify.Domain.Entities.Users;
using Bookify.Infrastructure.Authentication.Models;
using System.Net.Http.Json;

namespace Bookify.Infrastructure.Authentication;
internal sealed class AuthenticationService(HttpClient httpClient) : IAuthenticationService
{
    private const string PasswordCredentialType = "password";

    public async Task<string> RegisterAsync(User user, string password, CancellationToken cancellationToken = default)
    {
        var userRepresentationModel = UserRepresentationModel.FromUser(user);

        userRepresentationModel.Credentials =
        [
            new CredentialRepresentationModel
            {
                Value = password,
                Temporary = false,
                Type = PasswordCredentialType
            }
        ];
        
        var response = await httpClient.PostAsJsonAsync(
            "users",
            userRepresentationModel,
            cancellationToken);

        return ExtractIdentityIdFromLocationHeader(response);
    }

    private static string ExtractIdentityIdFromLocationHeader(HttpResponseMessage response)
    {
        const string usersSegmentName = "users/";

        var locationHeader = response.Headers.Location?.PathAndQuery;

        if (locationHeader is null) throw new InvalidOperationException("Location header can't be null");

        var userSegmentValueIndex = locationHeader.IndexOf(
            usersSegmentName,
            StringComparison.InvariantCultureIgnoreCase);
        return locationHeader[(userSegmentValueIndex + usersSegmentName.Length)..];        
    }
}
