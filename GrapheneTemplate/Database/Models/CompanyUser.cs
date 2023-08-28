using Graphene.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using static Graphene.Graph.Graph;
using Graphene.Http.Converters;
using Graphene.Http.Validation;

namespace GrapheneTemplate.Database.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class CompanyUser : Entity
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
        [ValidForeignKey("Company")]
        [ForeignKey(nameof(Company))]
        [JsonConverter(typeof(GuidConverter<Company>))]
        public virtual int? CompanyId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// 
        /// 
        public virtual Company? Company { get; set; }
        
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
        public virtual User? User { get; set; }
        
        
    }
}