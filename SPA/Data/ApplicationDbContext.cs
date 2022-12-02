using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Duende.IdentityServer.EntityFramework.Options;
using SPA.Models;
using Duende.IdentityServer.EntityFramework.Entities;
using Duende.IdentityServer.EntityFramework.Extensions;
using Duende.IdentityServer.EntityFramework.Interfaces;
using System.Reflection.Metadata;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Graphene.Database.Interfaces;
using Graphene.Entities.Identity;
using Graphene.Entities.Interfaces;
using Graphene.Database.Extensions;

namespace SPA.Data
{

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, GrapheneIdentityRole, int, GrapheneIdentityUserClaim, GrapheneIdentityUserRole, GrapheneIdentityUserLogin, GrapheneIdentityRoleClaim, GrapheneIdentityUserToken>, IGrapheneDatabaseContext, IPersistedGrantDbContext
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly IOptions<OperationalStoreOptions> _operationalStoreOptions;

        /// <summary>
        /// 
        /// </summary>
        public override DbSet<ApplicationUser> Users { get; set; }

        /// <summary>
        /// 
        /// </summary>
        //public DbSet<Author> Author { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{PersistedGrant}"/>.
        /// </summary>
        public DbSet<PersistedGrant> PersistedGrants { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{DeviceFlowCodes}"/>.
        /// </summary>
        public DbSet<DeviceFlowCodes> DeviceFlowCodes { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{Key}"/>.
        /// </summary>
        public DbSet<Key> Keys { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<Type, Func<IQueryable<dynamic>>> SetDictionary { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="ApiAuthorizationDbContext{TUser}"/>.
        /// </summary>
        /// <param name="options">The <see cref="DbContextOptions"/>.</param>
        /// <param name="operationalStoreOptions">The <see cref="IOptions{OperationalStoreOptions}"/>.</param>
        public ApplicationDbContext(
            DbContextOptions options,
            IOptions<OperationalStoreOptions> operationalStoreOptions)
            : base(options)
        {
            _operationalStoreOptions = operationalStoreOptions;
            SetDictionary = GetSets();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<int> IPersistedGrantDbContext.SaveChangesAsync() => base.SaveChangesAsync();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Dictionary<Type, Func<IQueryable<dynamic>>> GetSets()
        {
            // Declare the models that you want to expose in the API.
            return new Dictionary<Type, Func<IQueryable<dynamic>>> {
                    { typeof(IAuthenticable), () => Users },
                //{ typeof(IAuthorizator), () => (new List<Permission>() { new Permission() }).AsQueryable() },
                //{ typeof(IAuthorization), () => AuthorPermission },
                //{ typeof(IInstanceLog), () => Log },
                //{ typeof(Blog), () => Blog },
                //{ typeof(Author), () => Author },
                //{ typeof(Post), () => Post }
                };
        }
        /// <summary>
        /// Configures the schema needed for the identity framework.
        /// </summary>
        /// <param name="builder">
        /// The builder being used to construct the model for this context.
        /// </param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ConfigurePersistedGrantContext(_operationalStoreOptions.Value);
            GrapheneDatabaseContextExtensions.OnModelCreating(this, builder);
        }
    }
}