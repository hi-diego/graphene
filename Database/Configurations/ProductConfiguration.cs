using Graphene.Database.Entities;
using GrapheneCore.Database.Entities.Configurations;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Graphene.Database.Configurations
{
    public class ProductConfiguration : EntityTypeConfiguration<Product>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public override void Configure(EntityTypeBuilder<Product> builder)
        {
            base.Configure(builder);
            //builder.HasBaseType()
        }
    }
}
