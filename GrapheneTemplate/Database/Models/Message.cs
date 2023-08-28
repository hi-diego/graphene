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
    public class OrderMessage : Entity
    {
        public string Content { get; set; }
        /// <summary>
        /// If you want to hidde auto incremental IDs from JSON API
        /// you can set a Computed Property to fetch the Cache UIDS from each Table see ProductUId
        /// </summary>
        //[JsonIgnore]
        [ValidForeignKey("User")]
        [ForeignKey(nameof(User))]
        [JsonConverter(typeof(GuidConverter<User>))]
        public virtual int? UserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// 
        public virtual User? User { get; set; }
        
        /// <summary>
        /// If you want to hidde auto incremental IDs from JSON API
        /// you can set a Computed Property to fetch the Cache UIDS from each Table see ProductUId
        /// </summary>
        //[JsonIgnore]
        [ValidForeignKey("Order")]
        [ForeignKey(nameof(Order))]
        [JsonConverter(typeof(GuidConverter<Order>))]
        public virtual int OrderId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// 
        public virtual Order Order { get; set; }
    }
}
