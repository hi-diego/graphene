﻿using System;
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
        /// <summary>
        /// 
        /// </summary>
        public DbSet<Blog> Blog { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DbSet<Post> Post { get; set; }
        /// <summary>
        /// This is the Declaration of what is going to be accesible by
        /// the API Interface, all the entities that are declared here are going
        /// to beaccesible through the ApiController and GraphController.
        /// If the resource is not declared here the ApiController and GraphController
        /// will return a 404 error.
        /// </summary>
        public Dictionary<string, Type> ModelDictionary { get; set; }
            = new Dictionary<string, Type> {
                { "Blog", typeof(Blog) },
                { "Post", typeof(Post) }
            };
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityName"></param>
        /// <returns></returns>
        public bool Exists(ref string entityName) => GrapheneDatabaseContextExtensions.Exists(this, ref entityName);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public IQueryable<T> GetSet<T>(string name) => GrapheneDatabaseContextExtensions.GetSet<T>(this, name);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IQueryable<dynamic> GetSet(string name) => GrapheneDatabaseContextExtensions.GetSet(this, name);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rootEntity"></param>
        public void SaveAnnotatedGraph(object rootEntity) => GrapheneDatabaseContextExtensions.SaveAnnotatedGraph(this, rootEntity);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder) => GrapheneDatabaseContextExtensions.OnModelCreating(this, modelBuilder);
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
            optionsBuilder.UseSqlServer("Server = localhost\\SQLEXPRESS; Database = graphene; Trusted_Connection = True;");
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class Blog : GraphModel
    {
        public int BlogId { get; set; }
        public string Url { get; set; }

        public List<Post> Posts { get; } = new();
    }
    /// <summary>
    /// 
    /// </summary>
    public class Post : GraphModel
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int BlogId { get; set; }
        public Blog Blog { get; set; }
    }
}