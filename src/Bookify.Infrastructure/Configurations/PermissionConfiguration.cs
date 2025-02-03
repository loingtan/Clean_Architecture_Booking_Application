using Bookify.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bookify.Infrastructure.Configurations;
public sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.ToTable("permissions");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name)
            .HasMaxLength(200);
        builder.HasData(Permission.UsersRead);
        builder.HasData(Permission.UsersWrite);
        builder.HasData(Permission.RolesRead);
        builder.HasData(Permission.RolesWrite);
    }
}
