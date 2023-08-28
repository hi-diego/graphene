using Graphene.Entities;
using System.ComponentModel.DataAnnotations.Schema; 
using System.ComponentModel.DataAnnotations; 
using Graphene.Extensions;
using Graphene.Database.Interfaces;

namespace GrapheneTemplate.Database.Models
{
    public class Company : Entity
    {
        /// <summary>
        /// 
        /// </summary>
        public enum PricingPlans  {
            Free = 0,
            Basic,
            Premium
        }
        /// <summary>
        /// 
        /// </summary>
        public enum Categories  {
            Restaurant = 0,
            Retail,
            Supplier,
            Wholesale,
        }
        /// <summary>
        /// The name of the company associated to this account, this can be use 
        /// as the subdomain to separate the workspaces.
        /// </summary>
        /// 
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Alias { get; set; } = "";

        /// <summary>
        /// Interal category that help our business to segment this Account Company
        /// is it a local family business? is it a Premium business?
        /// </summary>
        public Categories Category { get; set; } = Categories.Restaurant;
        /// <summary>
        ///
        /// </summary>
        ///
        public PricingPlans? Plan { get; set; } = PricingPlans.Free;
        /// <summary>
        /// 
        /// </summary>
        [InverseProperty(nameof(Product.OfferedBy))]
        public ICollection<Product> Products { get; set; } = new List<Product>();
        public Company () {
            Alias = Name.ToSnakeCase();
        }
        public override void BeforeAdded(IGrapheneDatabaseContext Database)
        {
            base.BeforeAdded(Database);
            Alias = Name.ToSnakeCase();
        }
        public override void BeforeModified(IGrapheneDatabaseContext Database)
        {
            base.BeforeAdded(Database);
            Alias = Name.ToSnakeCase();
        }
    }
}
