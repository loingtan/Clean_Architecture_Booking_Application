using Bookify.Domain.Entities.Abstractions;

namespace Bookify.Domain.Entities.Users;
public sealed class Permission : Enumeration<Permission>
{
    public static readonly Permission UsersRead = new(1, "users:read");
    public static readonly Permission UsersWrite = new(2, "users:write");
    public static readonly Permission RolesRead = new(3, "roles:read");
    public static readonly Permission RolesWrite = new(4, "roles:write");
    private Permission(int id, string name) : base(id, name) { }
}