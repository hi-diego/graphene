using GrapheneCore.Database.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Graphene.Database.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class Product : Entity
    {
        /// <summary>
        /// 
        /// </summary>
        public enum PRODUCT_TYPES
        {
            //
            PRODUCT,
            // 
            INGREDIENT,
            // Indicates that its a bundle of product like a promotion or package, and that does not havestock , it will disccount the stock accordingly to its INGREDIENT related products.
            BUNDLE,
            // Indicates that will discount the stock of the ingridients if ordered, and shopud be displayed on the menu.
            DISH,
            // Indicates that shoud not dysplay in the menu instead it a asset that its not for sale .(glass for example)
            ASSET,
            // Indicates that shoud not dysplay in the main menu, can be (Extras), these usually are ingredients.
            SUPPLY
        };

        /// <summary>
        /// almost the International System of Units.
        /// </summary>
        public enum UNITS
        {
            UNIT = 0,
            SECOND,
            METRE,
            GRAM,
            LITER,
        };

        /// <summary>
        /// 
        /// </summary>
        public PRODUCT_TYPES Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Category { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double Price { get; set; } = 0;

        /// <summary>
        /// 
        /// </summary>
        public double Stock { get; set; } = 0;

        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        public double CurrentStock { get => Stock; }

        /// <summary>
        /// 
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        public string ImageUrl
        {
            get
            {
                string path = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    "img",
                    _Entity,
                    Uid.ToString() + ".jpg"
                );
                return File.Exists(path) ? Path.Combine("/", "img", _Entity, Uid.ToString() + ".jpg") : null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        public IFormFile ImageFile { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [InverseProperty(nameof(ProductRelation.Parent))]
        public List<ProductRelation> ChilldRelations { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [InverseProperty(nameof(ProductRelation.Chilld))]
        public List<ProductRelation> ParentRelations { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ICollection<Product> Chillds { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ICollection<Product> Parents { get; set; }

        /// <summary>
        /// 
        /// </summary>
        //public IEnumerable<Ingredient> Ingredients { get; set; }
        //get => ParentRelations.Select(pr => pr.Parent.SetPivot<Ingredient>(pr));
        //}

        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        public double? Cost
        {
            get => ChilldRelations
                    ?.Where(pr => pr.Relation == ProductRelation.PRODUCT_RELATIONS.INGREDIENT)
                    .Select(pr => pr.Cost)
                    .Sum();
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateStock(Order order)
        {
            if (Type == PRODUCT_TYPES.DISH)
            {
                SpendSlavesStock(ProductRelation.PRODUCT_RELATIONS.INGREDIENT, order);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        public void SpendSlavesStock(ProductRelation.PRODUCT_RELATIONS type, Order order = null)
        {
            var relations = ChilldRelations.Where(pr => pr.Relation == type).ToList();
            foreach (var relation in relations)
            {
                relation.Chilld.Stock -= relation.Amount * order.Quantity;
            }
        }
    }
}
