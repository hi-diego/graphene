using GrapheneCore.Attributes;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Graphene.Database.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class Permission : GrapheneCore.Database.Entities.Permission
    {
        /// <summary>
        /// 
        /// </summary>
        [InverseForeignKey(nameof(UserHasPermission.PermissionId))]
        [InverseProperty(nameof(UserHasPermission.Permission))]
        public new IEnumerable<UserHasPermission> UserPermissions { get; set; } = new List<UserHasPermission>();
    }
}
