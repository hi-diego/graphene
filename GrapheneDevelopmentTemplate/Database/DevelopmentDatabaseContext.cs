using GrapheneDevelopmentTemplate.Database.Models;
using Graphene.Database.Extensions;
using Graphene.Database.Interfaces;
using Graphene.Entities.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GrapheneDevelopmentTemplate.Database
{
    /// <summary>
    /// 
    /// </summary>
    public class DevelopmentDatabaseContext : DbContext, IGrapheneDatabaseContext
    {
        /// <summary>
        /// 
        /// </summary>
        public DbSet<Blog> Blog { get; set; }
        /// <summary>
        /// 
        /// </summary>
        //public DbSet<Post> Post { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DbSet<Author> Author { get; set; }
        /// <summary>
        /// 
        /// </summary>
        //public DbSet<Models.Log> Log { get; set; }
        /// <summary>
        /// 
        /// </summary>
        //public DbSet<Permission> Permission { get; set; }
        /// <summary>
        /// 
        /// </summary>
        //public DbSet<AuthorPermission> AuthorPermission { get; set; }
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
        //public DatabaseContext()
        //{
        //    // Declare the models that you want to expose in the API.
        //    SetDictionary = GetSets();
        //}
        /// <summary>
        /// 
        /// </summary>
        public DevelopmentDatabaseContext(DbContextOptions<DevelopmentDatabaseContext> options) : base(options)
        {
            // Declare the models that you want to expose in the API.
            SetDictionary = GetSets();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Dictionary<Type, Func<IQueryable<dynamic>>> GetSets ()
        {
            // Declare the models that you want to expose in the API.
            return new Dictionary<Type, Func<IQueryable<dynamic>>> {
                { typeof(IAuthenticable), () => Author },
                //{ typeof(IAuthorizator), () => Permission },
                //{ typeof(IAuthorization), () => AuthorPermission },
                //{ typeof(IInstanceLog), () => Log },
                { typeof(Blog), () => Blog },
                { typeof(Author), () => Author },
                //{ typeof(Post), () => Post }
            };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            GrapheneDatabaseContextExtensions.OnModelCreating(this, modelBuilder);
            foreach (Type entity in SetDictionary.Values.Select(kv => Graphene.Graph.Graph.GetSetType(kv())))
            {
                modelBuilder.Entity(entity).HasBaseType((Type) null);
            }
        }

    }
}