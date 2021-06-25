using GrapheneCore.Database.Entities.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Graphene.Database.Entities.Configurations
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="User"></typeparam>
    public class UserHasPermissionConfiguration : EntityTypeConfiguration<UserHasPermission>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public override void Configure(EntityTypeBuilder<UserHasPermission> builder)
        {
            base.Configure(builder);
            builder.HasOne(usp => usp.User).WithMany(u => u.UserHasPermissions);
            builder.HasOne(usp => usp.Permission).WithMany(p => p.UserPermissions);
        }
    }
}