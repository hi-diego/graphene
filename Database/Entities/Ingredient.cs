using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Graphene.Database.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class Ingredient : Product
    {
        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        public IEnumerable<Dish> Dishes { get; set; }
    }
}
