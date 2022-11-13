using Graphene.Entities;
using Graphene.Graph;
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
    public class Blog : Entity
    {
        /// <summary>
        /// 
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// If you want to hidde auto incremental IDs from JSON API
        /// you can set a Computed Property to fetch the Cache UIDS from each Table see AuthorUId
        /// </summary>
        [ValidForeignKey("Author")]
        // [JsonProperty("AuthorUid")]
        [ForeignKey(nameof(Author))]
        [JsonConverter(typeof(KeyConverter<Author>))]
        public virtual int AuthorId { get; set; }

        /// <summary>
        /// This computed property takes ands sets the correspondet UID and autoincrementable Ids
        /// to avoid Brute force attacks on AutoIncrementable Ids.
        /// </summary>
        //[NotMapped]
        //[JsonIgnore]
        //public virtual Guid AuthorUId
        //{
        //    // Get from Graph.UIDS GUID static cache dictionary.
        //    get => Graph.UIDS["Author"].GetGuid(AuthorId);
        //    // Set from Graph.UIDS ID static cache dictionary.
        //    set => AuthorId = Graph.UIDS["Author"].GetId(value);
        //}

        /// <summary>
        /// 
        /// </summary>
        public virtual Author? Author { get; set; }
    }
}
