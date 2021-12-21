using Graphene.Database.Interfaces;
using Graphene.Graph.Interfaces;
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
    public interface IAuthorizator
    {
        /// <summary>
        /// 
        /// </summary>
        public string Action { get; set; }
        /// <summary>
        /// 
        /// </summary>
        string Entity { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Expression { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ICollection<IAuthorization> Authorizations { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityInstance"></param>
        /// <param name="user"></param>
        /// <param name="context"></param>
        /// <param name="graph"></param>
        /// <returns></returns>
        public Task<bool> IsAuthorized(Entity entityInstance, IAuthorizable user, IGrapheneDatabaseContext context);
    }
}
