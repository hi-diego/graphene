using Graphene.Entities;
using Graphene.Services;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GrapheneDevelopmentTemplate.Database.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Permission : Authorizator
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
        [NotMapped]
        public int AuthorId { get => AuthorizedId; set => AuthorizedId = value; }
        /// <summary>
        /// 
        /// </summary>
        [Column("author_id")]
        public int AuthorizedId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Action { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool Denied { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Entity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<AuthorPermission> AuthorPermissions { get; set; } = Enumerable.Empty<AuthorPermission>();
    }
}