using System;
using System.Collections.Generic;
using System.Diagnostics;
using GrapheneTemplate.Models;
using Graphene.Database.Extensions;
using Graphene.Database.Interfaces;
using Graphene.Extensions;
using Graphene.Graph;
using Graphene.Entities;
using Graphene.Entities.Interfaces;
using Microsoft.EntityFrameworkCore;
using Graphene.Services;

namespace GrapheneTemplate.Database
{
    /// <summary>
    /// 
    /// </summary>
    public class DatabaseContext : DbContext, IGrapheneDatabaseContext
    {
        /// <summary>
        /// 
        /// </summary>
        public DbSet<Blog> Blog { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DbSet<Post> Post { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DbSet<Author> Author { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DbSet<Models.Log> Log { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DbSet<Permission> Permission { get; set; }
        /// <summary>
        /// This is the Declaration of what is going to be accesible by
        /// the API Interface, all the entities that are declared here are going
        /// to beaccesible through the ApiController and GraphController.
        /// If the resource is not declared here the ApiController and GraphController
        /// will return a 404 error.
        /// </summary>
        public Dictionary<Type, Func<IQueryable<dynamic>>> SetDictionary { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DatabaseContext()
        {
            // Declare the models that you want to expose in the API.
            SetDictionary = new Dictionary<Type, Func<IQueryable<dynamic>>> {
                { typeof(IAuthenticable), () => Author },
                { typeof(IAuthorizator), () => Permission },
                { typeof(IInstanceLog), () => Log },
                { typeof(Blog), () => Blog },
                { typeof(Author), () => Author },
                { typeof(Post), () => Post }
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