using GrapheneCore.Database.Interfaces;
using GrapheneCore.Extensions;
using GrapheneCore.Models;
using GrapheneCore.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrapheneCore.Database.Extensions
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
                Model entity = (Model)n.Entry.Entity;
                n.Entry.State = entity.EntityState;
                if (entity.EntityState == EntityState.Deleted) Model.SoftDelete(entity, n.Entry);
            });
        }

        /// <summary>
        /// All the API Fluent configurations are placed
        /// on the Database\Entities\Configurations Folder.
        /// </summary>
        /// <param name="modelBuilder"></param>
        public static void OnModelCreating(this IGrapheneDatabaseContext dbContext, ModelBuilder modelBuilder)
        {
            foreach (Type entity in dbContext.SetDictionary.Values.Select(kv => GrapheneCore.Graph.Graph.GetSetType(kv())))
                if (entity.BaseType == typeof(Model)) ModelConfiguration.Configure(modelBuilder.Entity(entity), entity);
            dbContext.ModelBuilderToSnakeCase(modelBuilder);
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
                if (!typeof(Model).IsAssignableFrom(entity.ClrType.BaseType)) continue;
                entity.SetTableName(entity.GetTableName().ToSnakeCase().ToPlural());
                // snakify column names
                foreach (var property in entity.GetProperties()) property.SetColumnName(property.GetColumnBaseName().ToSnakeCase());
                // snakify key names
                foreach (var key in entity.GetKeys()) key.SetName(key.GetName().ToSnakeCase());
                // snakify foreignkeys names
                foreach (var key in entity.GetForeignKeys()) key.SetConstraintName(key.GetConstraintName().ToSnakeCase());
                // snakify index names
                foreach (var index in entity.GetIndexes()) index.SetName(index.Name.ToSnakeCase());
            }
        }
    }
}
