using Graphene.Entities;
using Graphene.Graph;
using Graphene.Http.Converters;
using Graphene.Http.Validation;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using static Graphene.Graph.Graph;

namespace GrapheneTemplate.Database.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Order : Entity
    {
        /// <summary>
        /// 
        /// </summary>
        public double Quantity { get; set; }
        /// <summary>
        /// If you want to hidde auto incremental IDs from JSON API
        /// you can set a Computed Property to fetch the Cache UIDS from each Table see ProductUId
        /// </summary>
        //[JsonIgnore]
        [ValidForeignKey("Product")]
        [ForeignKey(nameof(Product))]
        [JsonConverter(typeof(GuidConverter<Product>))]
        public virtual int? ProductId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual Product? Product { get; set; }
    }
}
