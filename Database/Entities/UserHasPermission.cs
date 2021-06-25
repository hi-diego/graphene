using System.ComponentModel.DataAnnotations.Schema;

namespace Graphene.Database.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class UserHasPermission : GrapheneCore.Database.Entities.UserPermission
    {
        /// <summary>
        /// 
        /// </summary>
        [ForeignKey(nameof(UserId))]
        public new User User { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [ForeignKey(nameof(PermissionId))]
        public new Permission Permission { get; set; }
    }
}
