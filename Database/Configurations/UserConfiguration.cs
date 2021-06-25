using GrapheneCore.Database.Entities.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Graphene.Database.Entities.Configurations
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="User"></typeparam>
    public class UserConfiguration : EntityTypeConfiguration<User>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);
            builder.HasIndex(u => u.Email).IsUnique();
            builder.HasMany(u => u.ActionLog).WithOne(el => el.ActionUser);
            //builder.HasMany(u => u.History).WithOne(el => el.Instance);
        }
    }
}