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
    public interface IAuthorization
    {
        /// <summary>
        /// 
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int AuthorizableId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IAuthorizable Authorizable { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int AuthorizatorId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IAuthorizator Authorizator { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool Denied { get; set; }
    }
}
