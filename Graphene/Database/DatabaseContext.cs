using System;
using System.Collections.Generic;
using System.Diagnostics;
using Graphene.Models;
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
        /// <summary>
        /// 
        /// </summary>
        public DbSet<Blog> Blog => Set<Blog>();
        /// <summary>
        /// 
        /// </summary>
        public DbSet<Post> Post => Set<Post>();
        /// <summary>
        /// 
        /// </summary>
        public DbSet<Author> Author => Set<Author>();
        /// <summary>
        /// This is the Declaration of what is going to be accesible by
        /// the API Interface, all the entities that are declared here are going
        /// to beaccesible through the ApiController and GraphController.
        /// If the resource is not declared here the ApiController and GraphController
        /// will return a 404 error.
        /// </summary>
        public Dictionary<string, Func<IQueryable<dynamic>>> SetDictionary { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DatabaseContext()
        {
            //
            SetDictionary = new Dictionary<string, Func<IQueryable<dynamic>>> {
                { "PopularBlog", () => Blog.Where(b => b.Posts.Count() > 100) },
                { "Blog", () => Blog },
                { "Author", () => Author },
                { "Post", () => Post }
            };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            GrapheneDatabaseContextExtensions.OnModelCreating(this, modelBuilder);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //#warning To protect potentially sensitive information in your connection string, 
            //            you should move it out of source code.See http://go.microsoft.com/fwlink/?LinkId=723263 
            //     for guidance on storing connection strings.
            // optionsBuilder.UseMySQL("server=localhost;database=graphene;user=root;password=$torage");
            optionsBuilder
                .UseSqlServer("Server = localhost\\SQLEXPRESS; Database = graphene; Trusted_Connection = True;")
                .EnableSensitiveDataLogging()
                .LogTo(message => Debug.WriteLine(message));
        }
    }
}