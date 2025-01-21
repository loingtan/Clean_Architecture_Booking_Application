using Microsoft.AspNetCore.Authorization;

namespace Bookify.Infrastructure.Authorization;

internal sealed class PermissionRequirement(string permissions) : IAuthorizationRequirement
{
    public string Permissions { get; } = permissions;
}
