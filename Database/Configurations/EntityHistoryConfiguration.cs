using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Graphene.Database.Entities.Configurations
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="User"></typeparam>
    public class EntityHistoryConfiguration : IEntityTypeConfiguration<EntityHistory>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public void Configure(EntityTypeBuilder<EntityHistory> builder)
        {
            builder.HasIndex(u => u.Uid).IsUnique();
            //builder.HasIndex(u => u.Uid).IsUnique();
            //   builder.HasIndex(u => u.UserName).IsUnique();
        }
    }
}