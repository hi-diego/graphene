﻿using Graphene.Database.Interfaces;
using Graphene.Extensions;
using Graphene.Entities;
using Graphene.Entities.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphene.Database.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class GrapheneDatabaseContextExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rootEntity"></param>
        public static void SaveAnnotatedGraph(this IGrapheneDatabaseContext dbContext, object rootEntity)
        {
            dbContext.ChangeTracker.TrackGraph(rootEntity, n => {
                Entity instance = (Entity)n.Entry.Entity;
                n.Entry.State = instance.EntityState;
                if (instance.EntityState == EntityState.Deleted) Entity.SoftDelete(instance, n.Entry);
            });
        }

        /// <summary>
        /// All the API Fluent configurations are placed
        /// on the Database\Entities\Configurations Folder.
        /// </summary>
        /// <param name="entityBuilder"></param>
        public static void OnModelCreating(this IGrapheneDatabaseContext dbContext, ModelBuilder entityBuilder)
        {
            // Iterate the Dictionary to get all the Models.
            foreach (Type entity in dbContext.SetDictionary.Values.Select(kv => Graphene.Graph.Graph.GetSetType(kv())))
                // If is child of Entity lets configure it.
                if (typeof(Entity).IsAssignableFrom(entity.BaseType)) EntityConfiguration.Configure(entityBuilder.Entity(entity), entity);
            // Use snakecase simplicity
            dbContext.ModelBuilderToSnakeCase(entityBuilder);
        }

        /// <summary>
        /// Create the custom builder to snakify all the Database nomenclature automaticly.
        /// </summary>
        /// <param name="builder"></param>
        public static void ModelBuilderToSnakeCase(this IGrapheneDatabaseContext dbContext, ModelBuilder builder)
        {
            // For softDelete
            // builder.Entity<Entity>().HasQueryFilter(e => ((Entity)e).DateDeleted != null);
            foreach (IMutableEntityType entity in builder.Model.GetEntityTypes())
            {
                // snakify table names
                if (!typeof(Entity).IsAssignableFrom(entity.ClrType.BaseType)) continue;
                entity.SetTableName(entity.GetTableName().ToSnakeCase().ToPlural());
                // snakify column names
                foreach (var property in entity.GetProperties()) property.SetColumnName(property.GetColumnBaseName().ToSnakeCase());
                // snakify key names
                foreach (IMutableKey key in entity.GetKeys()) key.SetName(key.GetName().ToSnakeCase());
                // snakify foreignkeys names
                foreach (var key in entity.GetForeignKeys()) key.SetConstraintName(key.GetConstraintName().ToSnakeCase());
                // snakify index names
                foreach (IMutableIndex index in entity.GetIndexes()) index.SetDatabaseName(index.Name.ToSnakeCase());
            }
        }
    }
}
