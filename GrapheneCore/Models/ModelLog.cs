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
        public string Model { get; set; }
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
        public Model Instance { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid ModelUid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IModelLog Init(EntityEntry entry, int? userId = null)
        {
            Model entity = ((Model)entry.Entity);
            var to = entity.ToJson();
            var copy = entry.OriginalValues.Clone().ToObject();
            var from = ((Model)copy).ToJson();
            InstanceEntityState = entry.State;
            //UserId = userId;
            Model = entry.Entity.GetType().Name;
            ModelId = ((Model)entry.Entity).Id;
            To = to;
            From = from;
            Event = entry.State.ToString();
            CreatedAt = DateTime.Now;
            Instance = ((Model)entry.Entity);
            ModelUid = ((Model)entry.Entity).Uid;
            return this;
        }
    }
}
