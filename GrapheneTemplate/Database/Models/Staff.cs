using Graphene.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Graphene.Http.Converters;
using Graphene.Http.Validation;

namespace GrapheneTemplate.Database.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Staff : Entity
    {
        public enum Roles {
            Staff = 0,
            Delivery,
            Auditor,
            Admin,
            Owner,
        }

        /// <summary>
        /// 
        /// </summary>
        public Roles Role { get; set; }

        /// <summary>
        /// If you want to hidde auto incremental IDs from JSON API
        /// you can set a Computed Property to fetch the Cache UIDS from each Table see ProductUId
        /// </summary>
        //[JsonIgnore]
        [ValidForeignKey("Space")]
        [ForeignKey(nameof(Space))]
        [JsonConverter(typeof(RedisGuidReplaceConverter<Space>))]
        public virtual int? SpaceId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// 
        /// 
        public virtual Space? Space { get; set; }
        
        /// <summary>
        /// If you want to hidde auto incremental IDs from JSON API
        /// you can set a Computed Property to fetch the Cache UIDS from each Table see ProductUId
        /// </summary>
        //[JsonIgnore]
        [ValidForeignKey("User")]
        [ForeignKey(nameof(User))]
        [JsonConverter(typeof(RedisGuidReplaceConverter<User>))]
        public virtual int? UserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual User? User { get; set; }
        
        
    }
}