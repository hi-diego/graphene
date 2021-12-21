using Graphene.Entities.Interfaces;

namespace GrapheneTemplate.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthorPermission : Authorization
    {
        /// <summary>
        /// 
        /// </summary>
        public override IAuthorizator Authorizator { get => Permission; set => Permission = (Permission) value; }
        /// <summary>
        /// 
        /// </summary>
        public override int AuthorizatorId { get => PermissionId; set => PermissionId = value; }
        /// <summary>
        /// 
        /// </summary>
        public override IAuthorizable Authorizable { get => Author; set => Author = (Author) value; }
        /// <summary>
        /// 
        /// </summary>
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
