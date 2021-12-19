using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphene.Entities
{
    public interface IInstanceLog
    {
        /// <summary>
        /// 
        /// </summary>
        int InstanceId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        //int? UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        string Entity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        string To { get; set; }

        /// <summary>
        /// 
        /// </summary>
        string From { get; set; }

        /// <summary>
        /// 
        /// </summary>
        string Event { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        //[JsonIgnore]
        EntityState InstanceEntityState { get; set; }

        /// <summary>
        /// 
        /// </summary>
        DateTime CreatedAt { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        Entity Instance { get; set; }

        /// <summary>
        /// 
        /// </summary>
        Guid InstanceUid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entry"></param>
        /// <returns></returns>
        IInstanceLog Init(EntityEntry entry, int? userId = null);
    }
}
