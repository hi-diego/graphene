using GrapheneTemplate.Database.Models;
using Graphene.Database.Extensions;
using Graphene.Database.Interfaces;
using Graphene.Entities.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GrapheneTemplate.Database
{
    /// <summary>
    /// 
    /// </summary>
    public class GrapheneCache : DbContext, IGrapheneDatabaseContext
    {   
        /// <summary>
        /// 
        /// </summary>
        public DbSet<Order> Order { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public DbSet<Staff> Staff { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public DbSet<Product> Product { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<Bill> Bill { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public DbSet<Space> Space { get; set; }

        /// <summary>
        /// 
        /// </summary>
        // public DbSet<PayDesk> PayDesk { get; set; }

        /// <summary>
        /// 
        /// </summary>
        // public DbSet<Location> Location { get; set; }

        /// <summary>
        /// 
        /// </summary>
        // public DbSet<DinnerTable> DinnerTable { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public DbSet<User> User { get; set; }
        /// <summary>
        /// 
        /// </summary>
        // public DbSet<Log> Log { get; set; }
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
        public GrapheneCache(DbContextOptions<GrapheneCache> options) : base(options)
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
                { typeof(IAuthenticable), () => User },
                //{ typeof(IAuthorizator), () => Permission },
                //{ typeof(IAuthorization), () => AuthorPermission },
                // { typeof(IInstanceLog), () => Log },
                { typeof(User), () => User },
                { typeof(Bill), () => Bill },
                { typeof(Product), () => Product },
                { typeof(Space), () => Space },
                { typeof(Order), () => Order },
                { typeof(Staff), () => Staff },
            };
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            GrapheneDatabaseContextExtensions.OnModelCreating(this, modelBuilder);
            // modelBuilder
            //     .Entity<Space>()
            //     .HasDiscriminator<Models.Space.SpaceTypes>("Type")
            //     .HasValue<Location>(Models.Space.SpaceTypes.Location)
            //     .HasValue<DinnerTable>(Models.Space.SpaceTypes.DinnerTable)
            //     .HasValue<PayDesk>(Models.Space.SpaceTypes.PayDesk);
        }
    }
}