using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Graphene.Entities
{
    /// <summary>
    /// Generic Entity Configuration
    /// </summary>
    public class EntityConfiguration
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public static void Configure(EntityTypeBuilder builder, Type type)
        {
            // Mysql
            // builder.Property(nameof(Entity.Uuid))
            //     .HasColumnType("binary(16)")
            //     .HasDefaultValueSql("(unhex(replace(uuid(),'-','')))");
            // builder.HasIndex(nameof(Entity.Uuid)).IsUnique();

            // Sqlite
            builder.Property(nameof(Entity.Uuid))
                .HasColumnType("BLOB")
                .HasDefaultValueSql("randomblob(16)");
            builder.HasIndex(nameof(Entity.Uuid)).IsUnique();

            builder.HasIndex("Id").IsUnique();
            LambdaExpression e = DynamicExpressionParser.ParseLambda(
                type, typeof(bool),
                "e => e.DeletedAt == null"
            );
            builder.HasQueryFilter(e);
        }
    }
}
