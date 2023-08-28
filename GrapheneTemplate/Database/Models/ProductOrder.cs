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
    public class OrderProduct : Entity
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
        public string Unit { get; set; }
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
        [JsonConverter(typeof(GuidConverter<User>))]
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
        [ValidForeignKey("User")]
        [ForeignKey(nameof(User))]
        [JsonConverter(typeof(GuidConverter<User>))]
        public virtual int? ConfirmedById { get; set; }
        /// <summary>
        /// 
        /// </summary>
        ///
        public virtual User? ConfirmedBy { get; set; }


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
        ///
        public virtual Product? Product { get; set; }
        /// <summary>
        /// If you want to hidde auto incremental IDs from JSON API
        /// you can set a Computed Property to fetch the Cache UIDS from each Table see ProductUId
        /// </summary>
        //[JsonIgnore]
        [ValidForeignKey("Order")]
        [ForeignKey(nameof(Order))]
        [JsonConverter(typeof(GuidConverter<Order>))]
        public virtual int? OrderId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual Order? Order { get; set; }
        
    }
}
