using Graphene.Entities;
using Graphene.Graph;
using Graphene.Http.Validation;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrapheneDevelopmentTemplate.Database.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Blog : DevEntity
    {

        /// <summary>
        /// 
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// If you want to hidde auto incremental IDs from JSON API
        /// you can set a Computed Property to fetch the Cache UIDS from each Table see AuthorUId
        /// </summary>
        [JsonIgnore]
        [NotMapped]
        public virtual int AuthorId { get; set; }

        /// <summary>
        /// This computed property takes ands sets the correspondet UID and autoincrementable Ids
        /// to avoid Brute force attacks on AutoIncrementable Ids.
        /// </summary>
        public virtual Guid AuthorUId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual Author? Author { get; set; }
    }
}
