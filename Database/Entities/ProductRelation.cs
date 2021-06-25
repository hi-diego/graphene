using GrapheneCore.Database.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Graphene.Database.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class ProductRelation : Entity
    {
        /// <summary>
        /// 
        /// </summary>
        public enum PRODUCT_RELATIONS
        {
            VARIATION, // Indicates that have the same price it just a variation of some ingredients.
            OPTIONAL_INGREDIENT, // Indicates that can be and ingredient.
            PACKAGE, // Indicates that will not discount the stock if ordered, its just packaging.
            INGREDIENT // Indicates that will discount the stock if ordered.
        };

        /// <summary>
        /// 
        /// </summary>
        public PRODUCT_RELATIONS Relation { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double Amount { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int ParentId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ForeignKey(nameof(ParentId))]
        public Product Parent { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int ChilldId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ForeignKey(nameof(ChilldId))]
        public Product Chilld { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        public double Cost { get => Chilld != null ? (Amount * Chilld.Price) : 0; }
    }
}
