using Graphene.Database.Entities;
using Graphene.Database.Entities.Configurations;
using GrapheneCore.Database;
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
        public DbSet<EntityHistory> EntityHistory { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<Permission> Permission { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<User> User { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<UserHasPermission> UserHasPermission { get; set; }

        public DbSet<Product> Product { get; set; }

        public DbSet<Order> Order { get; set; }

        public DbSet<Bill> Bill { get; set; }

        public DbSet<Space> Space { get; set; }

        public DbSet<ProductRelation> ProductRelation { get; set; }

        public DbSet<Dish> Dish { get; set; }

        public DbSet<Bundle> Bundle { get; set; }

        public DbSet<Ingredient> Ingredient { get; set; }

        /// <summary>
        /// All the API Fluent configurations are placed
        /// on the Database\Entities\Configurations Folder.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new EntityHistoryConfiguration());
            modelBuilder.ApplyConfiguration(new UserHasPermissionConfiguration());
            modelBuilder.Entity<Product>().HasDiscriminator(e => e.Type).HasValue(Entities.Product.PRODUCT_TYPES.PRODUCT);
            modelBuilder.Entity<Dish>().HasBaseType<Product>().HasDiscriminator(e => e.Type).HasValue(Entities.Product.PRODUCT_TYPES.DISH);
            modelBuilder.Entity<Ingredient>().HasBaseType<Product>().HasDiscriminator(e => e.Type).HasValue(Entities.Product.PRODUCT_TYPES.INGREDIENT);
            modelBuilder.Entity<Bundle>().HasBaseType<Product>().HasDiscriminator(e => e.Type).HasValue(Entities.Product.PRODUCT_TYPES.BUNDLE);
            //modelBuilder.Entity<DishIngredient>().HasBaseType<ProductRelation>().HasNoDiscriminator().ToTable("ProductRelation");
            modelBuilder.Entity<Product>()
                .HasMany(i => i.Parents)
                .WithMany(p => p.Chillds)
                .UsingEntity<ProductRelation>(
                    pr => pr.HasOne(pr => pr.Parent)
                            .WithMany(p => p.ChilldRelations)
                            .HasForeignKey(pr => pr.ParentId),
                    pr => pr.HasOne(pr => pr.Chilld)
                            .WithMany(p => p.ParentRelations)
                            .HasForeignKey(pr => pr.ChilldId),
                    pr => {
                        pr.Property(pt => pt.Relation); // .HasDefaultValueSql("CURRENT_TIMESTAMP");
                        pr.HasKey(t => t.Id);
                    });
            //modelBuilder.Entity<Dish>()
            //    .HasMany(i => i.Ingredients)
            //    .WithMany(p => p.Dishes)
            //    .UsingEntity<DishIngredient>(
            //        pr => pr.HasOne(pr => pr.Ingredient)
            //                .WithMany(p => p.DishIngredients),
            //        pr => pr.HasOne(pr => pr.Dish)
            //                .WithMany(p => p.DishIngredients),
            //        pr => {
            //            pr.Property(pt => pt.Relation); // .HasDefaultValueSql("CURRENT_TIMESTAMP");
            //            pr.HasKey(t => t.Id);
            //        });
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

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<IPermission> PermissionSet => Permission;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public IQueryable<dynamic> GetSet(string entity) =>
            IDatabaseContextExtensions.GetSet(this, entity);
    }
}
