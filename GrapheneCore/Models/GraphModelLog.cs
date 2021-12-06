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
    public class GraphModelLog: GraphModel, IGraphModelLog
    {
        public int ModelId { get; set; }
        public int? UserId { get; set; }

        //[ForeignKey(nameof(UserId))]
        //public User User { get; set; }

        public string Model { get; set; }
        public string To { get; set; }
        public string From { get; set; }
        public string Event { get; set; }
        public EntityState InstanceEntityState { get; set; }
        public DateTime CreatedAt { get; set; }

        [NotMapped]
        public GraphModel Instance { get; set; }
        public Guid ModelUid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entry"></param>
        /// <returns></returns>
        public virtual T InitializeGeneric<T>(EntityEntry entry, int? userId = null) where T : IGraphModelLog, new()
        {
            T log = new T();
            GraphModel entity = ((GraphModelLog)entry.Entity);
            var to = entity.ToJson();
            var copy = entry.OriginalValues.Clone().ToObject();
            var from = ((GraphModel)copy).ToJson();
            log.InstanceEntityState = entry.State;
            //log.UserId = userId;
            log.Model = entry.Entity.GetType().Name;
            log.ModelId = ((GraphModel)entry.Entity).Id;
            log.To = to;
            log.From = from;
            log.Event = entry.State.ToString();
            log.CreatedAt = DateTime.Now;
            log.Instance = ((GraphModel)entry.Entity);
            log.ModelUid = ((GraphModel)entry.Entity).Uid;
            return log;
        }
    }
}
