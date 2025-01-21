using Microsoft.AspNetCore.Authorization;

namespace Bookify.Infrastructure.Authorization;
public class HasPermissionAttribute(string permission) : AuthorizeAttribute(permission);
