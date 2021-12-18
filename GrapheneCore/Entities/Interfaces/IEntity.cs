using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrapheneCore.Entities.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid Uid { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string _Entity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? ModifiedAt { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? DeletedAt { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EntityState EntityState { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ToJson();
    }
}
