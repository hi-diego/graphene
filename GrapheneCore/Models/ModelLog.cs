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
    /// <summary>
    /// 
    /// </summary>
    public class ModelLog: Model, IModelLog
    {
        /// <summary>
        /// 
        /// </summary>
        public int ModelId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? UserId { get; set; }
        //[ForeignKey(nameof(UserId))]
        //public User User { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Model { get; set; } = String.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string To { get; set; } = String.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string From { get; set; } = String.Empty;
        /// <summary>
        /// 
        /// </summary>
        public string Event { get; set; } = EntityState.Unchanged.ToString();
        /// <summary>
        /// 
        /// </summary>
        public EntityState InstanceEntityState { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        public Model? Instance { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid ModelUid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entry"></param>
        /// <returns></returns>
        public virtual T InitializeGeneric<T>(EntityEntry entry, int? userId = null) where T : IModelLog, new()
        {
            T log = new T();
            Model entity = ((Model)entry.Entity);
            var to = entity.ToJson();
            var copy = entry.OriginalValues.Clone().ToObject();
            var from = ((Model)copy).ToJson();
            log.InstanceEntityState = entry.State;
            //log.UserId = userId;
            log.Model = entry.Entity.GetType().Name;
            log.ModelId = ((Model)entry.Entity).Id;
            log.To = to;
            log.From = from;
            log.Event = entry.State.ToString();
            log.CreatedAt = DateTime.Now;
            log.Instance = ((Model)entry.Entity);
            log.ModelUid = ((Model)entry.Entity).Uid;
            return log;
        }
    }
}
