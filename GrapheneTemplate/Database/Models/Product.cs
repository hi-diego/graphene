using Graphene.Entities;
using Graphene.Http.Converters;
using Graphene.Http.Validation;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrapheneTemplate.Database.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Product : Entity
    {
        
        
        public string? Subcategory { get; set;} = "default";
        public string? Category { get; set;} = "default";

        /// <summary>
        /// 
        /// </summary>
        public enum Unit { 
            Bottle = 0,
        }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double Price { get; set; } = 0;
        /// <summary>
        /// 
        /// </summary>
        public string? Description { get; set; } = "default";
        /// <summary>
        /// 
        /// </summary>
        public string Packaging { get; set; } = "default";
        /// <summary>
        /// 
        /// </summary>
        public bool Available { get; set; } = true;
        /// <summary>
        /// If you want to hidde auto incremental IDs from JSON API
        /// you can set a Computed Property to fetch the Cache UIDS from each Table see AuthorUId
        /// </summary>
        //[JsonIgnore]
        [ValidForeignKey("User")]
        [ForeignKey(nameof(User))]
        [JsonConverter(typeof(RedisGuidReplaceConverter<User>))]
        public virtual int? CreatedById { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual User? CreatedBy { get; set; }    
        /// <summary>
        /// 
        /// </summary>
        public virtual IEnumerable<Order>? Orders { get; set; }
    }
}
