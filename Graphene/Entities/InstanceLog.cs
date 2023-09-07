using Graphene.Entities.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Graphene.Entities
{
    public class InstanceLog: Entity, IInstanceLog
    {
        /// <summary>
        /// 
        /// </summary>
        public int InstanceId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? UserId { get; set; }

        //[ForeignKey(nameof(UserId))]
        //public User User { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Entity { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string To { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string From { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Event { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public EntityState InstanceEntityState { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime CreatedAt { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        public Entity Instance { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid InstanceUid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IInstanceLog Init(EntityEntry entry, int? userId = null, JsonSerializerOptions serializeOptions = null)
        {
            Entity entity = ((Entity)entry.Entity);
            var to = entity.ToJson(serializeOptions);
            var copy = entry.OriginalValues.Clone().ToObject();
            var from = ((Entity)copy).ToJson(serializeOptions);
            InstanceEntityState = entry.State;
            //UserId = userId;
            Entity = entry.Entity.GetType().Name;
            InstanceId = ((Entity)entry.Entity).Id;
            To = to;
            From = from;
            Event = entry.State.ToString();
            CreatedAt = DateTime.Now;
            Instance = ((Entity)entry.Entity);
            InstanceUid = ((Entity)entry.Entity).Uuid;
            return this;
        }
    }
}
