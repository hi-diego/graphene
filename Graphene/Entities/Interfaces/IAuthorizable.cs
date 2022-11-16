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
    public interface IAuthorizable : IEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<IAuthorizator> Authorizations { get; set; }
    }
}
