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
    public class Product : Entity
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double Price { get; set; }
        /// <summary>
        /// If you want to hidde auto incremental IDs from JSON API
        /// you can set a Computed Property to fetch the Cache UIDS from each Table see AuthorUId
        /// </summary>
        //[JsonIgnore]
        [ValidForeignKey("User")]
        [ForeignKey(nameof(User))]
        [JsonConverter(typeof(GuidConverter<User>))]
        public virtual int? UserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual User? User { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public virtual IEnumerable<Order>? Orders { get; set; }
    }
}
