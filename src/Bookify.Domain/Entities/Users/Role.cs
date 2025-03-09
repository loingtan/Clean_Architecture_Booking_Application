using System.Collections.Generic;
using Bookify.Domain.Entities.Abstractions;

namespace Bookify.Domain.Entities.Users
{
    public sealed class Role : Enumeration<Role>
    {
        public static readonly Role Registered = new(1, "Registered");
        public static readonly Role Admin = new(2, "Admin");
        public static readonly Role Moderator = new(3, "Moderator");
        public ICollection<User> Users { get; init; } = new List<User>();
        public ICollection<Permission> Permissions { get; set; } = new List<Permission>();

        public Role(int id, string name) : base(id, name)
        {
        }
        public bool HasPermission(Permission permission)
        {
            return Permissions.Contains(permission);
        }

        public void AddPermission(Permission permission)
        {
            if (!Permissions.Contains(permission))
            {
                Permissions.Add(permission);
            }
        }
    }
}