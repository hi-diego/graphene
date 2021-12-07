using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrapheneCore.Models
{
    public interface IModelLog
    {
        /// <summary>
        /// 
        /// </summary>
        int ModelId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        //int? UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        string Model { get; set; }

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
        Model Instance { get; set; }

        /// <summary>
        /// 
        /// </summary>
        Guid ModelUid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entry"></param>
        /// <returns></returns>
        T InitializeGeneric<T>(EntityEntry entry, int? userId = null) where T : IModelLog, new();
    }
}
