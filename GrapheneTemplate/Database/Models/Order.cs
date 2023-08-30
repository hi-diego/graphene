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
    public class Order : Entity
    {
        public enum Statuses {
            Requested = 0,
            Comfirmed,
            Cancelled
        }
        /// <summary>
        /// 
        /// </summary>
        public double Quantity { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Comments { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// 
        public Statuses Status { get; set; }
        /// <summary>
        /// If you want to hidde auto incremental IDs from JSON API
        /// you can set a Computed Property to fetch the Cache UIDS from each Table see ProductUId
        /// </summary>
        //[JsonIgnore]
        [ValidForeignKey("User")]
        [ForeignKey(nameof(User))]
        [JsonConverter(typeof(RedisGuidReplaceConverter<User>))]
        public virtual int? CreatedById { get; set; }
        /// <summary>
        /// 
        /// </summary>
        ///
        public virtual User? CreatedBy { get; set; }
        
        /// <summary>
        /// If you want to hidde auto incremental IDs from JSON API
        /// you can set a Computed Property to fetch the Cache UIDS from each Table see ProductUId
        /// </summary>
        //[JsonIgnore]
        [ValidForeignKey("Product")]
        [ForeignKey(nameof(Product))]
        [JsonConverter(typeof(RedisGuidReplaceConverter<Product>))]
        public virtual int? ProductId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        ///
        public virtual Product? Product { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ValidForeignKey("Bill")]
        [ForeignKey(nameof(Bill))]
        [JsonConverter(typeof(RedisGuidReplaceConverter<Bill>))]
        public virtual int? BillId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        ///
        public virtual Bill? Bill { get; set; }
    }
}
