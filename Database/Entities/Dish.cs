using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Graphene.Database.Entities
{
    //[Table("ProductRelation")]
    //[NotMapped]
    //public class DishIngredient : ProductRelation
    //{
    //    //[ForeignKey(nameof(ParentId))]
    //    public Dish Dish { get; set; }

    //    //[ForeignKey(nameof(ChilldId))]
    //    public Ingredient Ingredient { get; set; }
    //}
    public class Dish : Product
    {
        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        public IEnumerable<Ingredient> Ingredients { get; set; }

        /// <summary>
        /// 
        /// </summary>
        //public IEnumerable<DishIngredient> DishIngredients { get; set; }
    }
}
