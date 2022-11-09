using Graphene.Graph;
using Graphene.Http.Validation;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

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
        [JsonIgnore]
        [ValidForeignKeyAttribute("Author")]
        public int AuthorId { get; set; }

        /// <summary>
        /// This computed property takes ands sets the correspondet UID and autoincrementable Ids
        /// to avoid Brute force attacks on AutoIncrementable Ids.
        /// </summary>
        [NotMapped]
        public Guid AuthorUId
        {
            // Get from Graph.UIDS GUID static cache dictionary.
            get => Graph.UIDS["Author"].GetGuid(AuthorId);
            // Set from Graph.UIDS ID static cache dictionary.
            set => AuthorId = Graph.UIDS["Author"].GetId(value);
        }

        /// <summary>
        /// 
        /// </summary>
        public Author? Author { get; set; }

        /// <summary>
        /// 
        /// </summary>
        // public IEnumerable<Post> Posts { get; set; }
    }
}
