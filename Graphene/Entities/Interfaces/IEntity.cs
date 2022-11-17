using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphene.Entities.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IEntity : GrapheneEntity<int>
    {
    }
    /// <summary>
    /// 
    /// </summary>
    public interface GrapheneEntity<KeyType>
    {
        /// <summary>
        /// 
        /// </summary>
        public KeyType Id { get; set; }

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
    }
}
