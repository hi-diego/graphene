using Graphene.Entities;
using Graphene.Http.Converters;
using Graphene.Http.Validation;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrapheneTemplate.Database.Models
{
    public class Device : InstanceLog
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        public string PublicIp { get; set; } = "127.0.0.1";
        /// <summary>
        /// 
        /// </summary>
        public string LocalIp { get; set; }
        /// <summary>
        /// If you want to hidde auto incremental IDs from JSON API
        /// you can set a Computed Property to fetch the Cache UIDS from each Table see ProductUId
        /// </summary>
        //[JsonIgnore]
        [ValidForeignKey("Account")]
        [ForeignKey(nameof(Account))]
        [JsonConverter(typeof(GuidConverter<Account>))]
        public virtual int? AccountId { get; set; }
    }
}
