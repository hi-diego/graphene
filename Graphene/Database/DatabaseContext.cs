using System;
using System.Collections.Generic;
using GrapheneCore.Database.Extensions;
using GrapheneCore.Database.Interfaces;
using GrapheneCore.Extensions;
using GrapheneCore.Graph;
using GrapheneCore.Models;
using Microsoft.EntityFrameworkCore;

namespace Graphene.Database
{
    /// <summary>
    /// 
    /// </summary>
    public class DatabaseContext : DbContext, IGrapheneDatabaseContext
    {
        public DbSet<Blog> Blog { get; set; }
        public DbSet<Post> Post { get; set; }

        /// <summary>
        /// This is the Declaration of what is going to be accesible by
        /// the API Interface, all the entities that are declared here are going
        /// to beaccesible through the ApiController.
        /// If the resource is not declared here the ApiController
        /// is going to return a 404 error.
        /// </summary>
        public Dictionary<string, Type> ModelDictionary { get; set; }
            = new Dictionary<string, Type> {
                { "Blog", typeof(Blog) },
                { "Post", typeof(Post) }
            };

        public bool Exists(ref string entityName)
        {
            return GrapheneDatabaseContextExtensions.Exists(this, ref entityName);
        }

        public IQueryable<T> GetSet<T>(string name)
        {
            return GrapheneDatabaseContextExtensions.GetSet<T>(this, name);
        }

        public IQueryable<dynamic> GetSet(string name)
        {
            return GrapheneDatabaseContextExtensions.GetSet(this, name);
        }

        public void SaveAnnotatedGraph(object rootEntity)
        {
            ChangeTracker.TrackGraph(rootEntity, n => {
                GraphModel entity = (GraphModel)n.Entry.Entity;
                n.Entry.State = entity.EntityState;
                if (entity.EntityState == EntityState.Deleted) GraphModel.SoftDelete(entity, n.Entry);
            });
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //#warning To protect potentially sensitive information in your connection string, 
            //            you should move it out of source code.See http://go.microsoft.com/fwlink/?LinkId=723263 
            //     for guidance on storing connection strings.
            //optionsBuilder.UseMySQL("server=localhost;database=graphene;user=root;password=$torage");
            optionsBuilder.UseSqlServer("Server = localhost\\SQLEXPRESS; Database = graphene; Trusted_Connection = True;");
        }

        /// <summary>
        /// All the API Fluent configurations are placed
        /// on the Database\Entities\Configurations Folder.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (Type entity in ModelDictionary.Values) if (entity.BaseType == typeof(GraphModel)) GraphModelConfiguration.Configure(modelBuilder.Entity(entity), entity);
            ModelBuilderToSnakeCase(modelBuilder);
        }

        /// <summary>
        /// Create the custom builder to snakesify all the Database nomenclature automaticly.
        /// </summary>
        /// <param name="builder"></param>
        public void ModelBuilderToSnakeCase(ModelBuilder builder)
        {
            // builder.Entity<Entity>().HasQueryFilter(e => ((Entity)e).DateDeleted != null);
            foreach (var entity in builder.Model.GetEntityTypes())
            {
                // Replace table names
                if (entity.ClrType.BaseType == typeof(GraphModel)) entity.SetTableName(entity.GetTableName().ToSnakeCase().ToPlural());
                // Replace column names            
                foreach (var property in entity.GetProperties()) property.SetColumnName(property.GetColumnBaseName().ToSnakeCase());
                foreach (var key in entity.GetKeys()) key.SetName(key.GetName().ToSnakeCase());
                foreach (var key in entity.GetForeignKeys()) key.SetConstraintName(key.GetConstraintName().ToSnakeCase());
                foreach (var index in entity.GetIndexes()) index.SetName(index.Name.ToSnakeCase());
            }
        }
    }
    public class Blog : GraphModel
    {
        public int BlogId { get; set; }
        public string Url { get; set; }

        public List<Post> Posts { get; } = new();
    }

    public class Post : GraphModel
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int BlogId { get; set; }
        public Blog Blog { get; set; }
    }
}