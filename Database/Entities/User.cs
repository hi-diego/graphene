using GrapheneCore.Attributes;
using GrapheneCore.Database.Entities;
using GrapheneCore.Database.Entities.Abstractions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Graphene.Database.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class User : GrapheneCore.Database.Entities.User
    {
        /// <summary>
        /// 
        /// </summary>
        [InverseProperty(nameof(EntityHistory.ActionUser))]
        [InverseForeignKey(nameof(EntityHistory.ByUserId))]
        public IEnumerable<EntityHistory> ActionLog { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [InverseForeignKey(nameof(EntityHistory.UserId))]
        [InverseProperty(nameof(EntityHistory.User))]
        public IEnumerable<EntityHistory> History { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [InverseForeignKey(nameof(UserHasPermission.UserId))]
        [InverseProperty(nameof(UserHasPermission.User))]
        public IEnumerable<UserHasPermission> UserHasPermissions { get; set; } = new List<UserHasPermission>();

        /// <summary>
        /// 
        /// </summary>
        [InverseForeignKey(nameof(Bill.UserId))]
        [InverseProperty(nameof(Bill.User))]
        public IEnumerable<Bill> Bills { get; set; } = new List<Bill>();

        /// <summary>
        /// 
        /// </summary>
        [InverseForeignKey(nameof(Order.CreatedById))]
        [InverseProperty(nameof(Order.CreatedBy))]
        public IEnumerable<Order> Orders { get; set; } = new List<Order>();

        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        public IEnumerable<User> Editors {
            get => History?.Select(h => h.User?.SetPivot<User>(h));
        }
    }
}
