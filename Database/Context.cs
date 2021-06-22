using Graphene.Database.Entities;
using Graphene.Database.Entities.Configurations;
using GrapheneCore.Database;
using GrapheneCore.Database.Entities;
using GrapheneCore.Database.Entities.Abstractions;
using GrapheneCore.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Graphene.Database
{
    /// <summary>
    /// 
    /// </summary>
    public class Context: DbContext, IDatabaseContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public Context(DbContextOptions<Context> options) : base(options)
        {
            //
        }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<Database.Entities.User> User { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<EntityHistory> EntityHistory { get; set; }

        /// <summary>
        /// All the API Fluent configurations are placed
        /// on the Database\Entities\Configurations Folder.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new EntityHistoryConfiguration());
        }

        /// <summary>
        /// 
        /// </summary>
        public GrapheneCore.Database.Entities.User AuthUser { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, Type> EntityDictionary { get; set; } = new Dictionary<string, Type>() {
            { "User",  typeof(Entities.User) }
        };

        public IEnumerable<IPermission> Permission => throw new NotImplementedException();

        public IQueryable<dynamic> GetSet(string entity) =>
            IDatabaseContextExtensions.GetSet(this, entity);
    }
}
