using System;
using System.Collections.Generic;
using GrapheneCore.Database.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Graphene.Database
{
    public class DatabaseContext : DbContext, IGrapheneDatabaseContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Post> Posts { get; set; }
        public string DbPath { get; }

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

        /// <summary>
        /// 
        /// </summary>
        //public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        //{
        //    ///
        //}

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //#warning To protect potentially sensitive information in your connection string, 
            //            you should move it out of source code.See http://go.microsoft.com/fwlink/?LinkId=723263 
            //     for guidance on storing connection strings.
            optionsBuilder.UseMySQL("server=localhost;database=graphene;user=root;password=$torage");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        //protected override void OnConfiguring(DbContextOptionsBuilder options)
        //    => options.UseSqlite($"Data Source={DbPath}");
    }
    public class Blog
    {
        public int BlogId { get; set; }
        public string Url { get; set; }

        public List<Post> Posts { get; } = new();
    }

    public class Post
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int BlogId { get; set; }
        public Blog Blog { get; set; }
    }
}