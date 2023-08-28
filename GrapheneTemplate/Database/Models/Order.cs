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
        public enum StatusCodes {
            Created = 0,
            InReview,
            InProgress,
            InRoute,
            Delivered,
        };
        /// <summary>
        /// 
        /// </summary>
        public StatusCodes Status { get; set; } = StatusCodes.Created;
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
        public virtual int? HandledById { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// 
        public virtual User? HandledBy { get; set; }
        /// <summary>
        /// If you want to hidde auto incremental IDs from JSON API
        /// you can set a Computed Property to fetch the Cache UIDS from each Table see ProductUId
        /// </summary>
        //[JsonIgnore]
        [ValidForeignKey("Company")]
        [ForeignKey(nameof(Company))]
        [JsonConverter(typeof(GuidConverter<Company>))]
        public virtual int? BuyerId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual Company? Buyer { get; set; }
        /// <summary>
        /// If you want to hidde auto incremental IDs from JSON API
        /// you can set a Computed Property to fetch the Cache UIDS from each Table see ProductUId
        /// </summary>
        //[JsonIgnore]
        [ValidForeignKey("Company")]
        [ForeignKey(nameof(Company))]
        [JsonConverter(typeof(GuidConverter<Company>))]
        public virtual int? SellerId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual Company? Seller { get; set; }
    }
}
