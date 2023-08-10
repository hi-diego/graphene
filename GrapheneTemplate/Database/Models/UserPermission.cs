using Graphene.Entities.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrapheneTemplate.Database.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class UserPermission : Authorization
    {
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
        public override IAuthorizable Authorizable { get => User; set => User = (User) value; }
        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        public override int AuthorizableId { get => UserId; set => UserId = value; }
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
        public int UserId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public User User { get; set; }
    }
}
