﻿using GrapheneCore.Database.Entities.Configurations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Graphene.Database.Entities.Configurations
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="User"></typeparam>
    public class EntityHistoryConfiguration : EntityTypeConfiguration<EntityHistory>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public override void Configure(EntityTypeBuilder<EntityHistory> builder)
        {
            base.Configure(builder);
            builder.HasOne(h => h.User)
                .WithMany(u => u.History)
                .HasForeignKey(h => h.UserId);
            builder.HasOne(h => h.ActionUser)
                .WithMany(u => u.ActionLog)
                .HasForeignKey(h => h.ByUserId);
        }
    }
}