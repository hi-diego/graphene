    using GrapheneCore.Database.Entities;
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
    public class EntityHistory : EntityLog
    {
        /// <summary>
        /// 
        /// </summary>
        [ForeignKey(nameof(ByUserId))]
        public User ActionUser { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int UserId { get; set; }
    }
}
