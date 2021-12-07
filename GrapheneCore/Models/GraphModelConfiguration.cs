using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace GrapheneCore.Models
{
    /// <summary>
    /// Generic Model Configuration
    /// </summary>
    public class ModelConfiguration
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public static void Configure(EntityTypeBuilder builder, Type type)
        {
            builder.HasIndex("Uid").IsUnique();
            LambdaExpression e = DynamicExpressionParser.ParseLambda(
                type, typeof(bool),
                "e => e.DeletedAt == null"
            );
            builder.HasQueryFilter(e);
        }
    }
}
