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
    //public class Pivot<P, E> : List<E>
    //{
    //    public Pivot(IEnumerable<P> childs)
    //    {
    //    }
    //}

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
        [NotMapped]
        public IEnumerable<User> Editors {
            get => History?.Select(h => h.User?.SetPivot<User>(h));
        }
    }
}
