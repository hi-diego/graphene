# Graphene Graph

In order to have some of the benefits of GraphQL in an API-REST Graphene Framework starts your application by transformimg all your models declared in your DatabaseContext
into a Graph-like Schema and uses a Singleton cache of your Database Models meta-data for the lifetime of your application (so the Graph generation is only computed once).
The Graphene Graph contains multiple key parts, if you want to dive deep you must check:

- Graph Singleton (https://graphene.software.freedom.icu/docs/graph#graph-class).
- GraphType (https://graphene.software.freedom.icu/docs/graph#graph-type).
- IGrapheneDatabaseContext (https://graphene.software.freedom.icu/docs/graph#igraphenedatabasecontext).
- ModelDictionary (https://graphene.software.freedom.icu/docs/graph#model-dictionary).

## Graph Implementation

### Steps

1. Inherit your models from Model.cs.
2. Implement IGrapheneDatabaseContext on YourDatabaseContext.cs.
3. Expose your models on the ModelDictionary on YourDatabaseContext.cs.
4. Register the necesary Graph services in your App.

### Example

In order to use the Graph Functionality you must do all 3 steps, here is an example with the classic Blog and Post Schema:

```c#
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
    public class Blog : Model
    {
        public int BlogId { get; set; }
        public string Url { get; set; }

        public List<Post> Posts { get; } = new();
    }
    /// <summary>
    /// 
    /// </summary>
    public class Post : Model
    {
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        public int BlogId { get; set; }
        public Blog Blog { get; set; }
    }
}
```

And then register the necesary services in your app:

```c#
using Graphene.Database;
using GrapheneCore.Database.Interfaces;
using GrapheneCore.Graph;
using GrapheneCore.Graph.Interfaces;
using Newtonsoft.Json;

// Declare youre YourDatabaseContext
builder.Services.AddDbContext<YourDatabaseContext>();
builder.Services.AddScoped<IGrapheneDatabaseContext>((IServiceProvider provider) => provider.GetService<YourDatabaseContext>());
builder.Services.AddSingleton<IGraph>((IServiceProvider provider) => new Graph(new YourDatabaseContext()));
// you will also need newtonsoft installed and configured
builder.Services.AddMvc().AddNewtonsoftJson(opt => {
    opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    opt.SerializerSettings.DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffffffK";
});
```

## Verify

You can verify the functionality through the auto generated API: [see](https://graphene.software.freedom.icu/docs/rest-api).

##Graph Singleton

The Graph singleton class is locaded in GrapheneCore.Graph.Graph

##GraphType

The GraphType class is locaded in GrapheneCore.Graph.GraphType

##IGrapheneDatabaseContext and IGrapheneDatabaseContextExtensions

The IGrapheneDatabaseContext interface is locaded in GrapheneCore.Database.IGrapheneDatabaseContext and The IGrapheneDatabaseContextExtensions are locaded in GrapheneCore.Database.Extensions.IGrapheneDatabaseContextExtensions

##ModelDictionary

The ModelDictionary is class is locaded in GrapheneCore.Graph.GraphType
