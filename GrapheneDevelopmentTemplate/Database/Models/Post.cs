using Graphene.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GrapheneDevelopmentTemplate.Database.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Post : Entity
    {
        /// <summary>
        /// 
        /// </summary>
        public override int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        public override bool SerializeId { get; set; } = true;

        /// <summary>
        /// 
        /// </summary>
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override Guid Uid { get; set; } = Guid.NewGuid();
        /// <summary>
        /// 
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int BlogId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Blog? Blog { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int AuthorId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Author? Author { get; set; }
    }
}
