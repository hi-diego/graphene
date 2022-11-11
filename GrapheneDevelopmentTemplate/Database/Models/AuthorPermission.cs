using Graphene.Entities.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace GrapheneDevelopmentTemplate.Database.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthorPermission : Authorization
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
        public override IAuthorizator Authorizator { get => Permission; set => Permission = (Permission) value; }
        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        public override int AuthorizatorId { get => PermissionId; set => PermissionId = value; }
        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        public override IAuthorizable Authorizable { get => Author; set => Author = (Author) value; }
        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        public override int AuthorizableId { get => AuthorId; set => AuthorId = value; }
        /// <summary>
        /// 
        /// </summary>
        public Permission Permission { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int PermissionId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int AuthorId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Author Author { get; set; }
    }
}
