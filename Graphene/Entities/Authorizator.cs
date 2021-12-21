using Graphene.Database.Interfaces;
using Graphene.Entities.Interfaces;
using Graphene.Graph.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace Graphene.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class Authorizator : Entity, IAuthorizator
    {
        /// <summary>
        /// 
        /// </summary>
        public string Action { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Entity { get; set; }
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
        /// <param name="user"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public bool Evaluate(IAuthorizable user, Entity res)
        {
            return Expression != null && user != null && res != null
                ? Graphene.Entities.Entity.QueryableOf(res).Where(Expression, user).Count() > 0
                : false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="permission"></param>
        /// <returns></returns>
        public async Task<bool> IsAuthorized(Entity entityInstance, IAuthorizable user, IGrapheneDatabaseContext context)
        {
            if (Expression == null) return true;
            var query = Graphene.Graph.Graph.GetSet<IAuthorizable>(context).Where(Expression, user, entityInstance);
            if (entityInstance.EntityState == EntityState.Modified) query = query.Where($"e => e.Id == {entityInstance.Id}");
            return await query.AsNoTracking().CountAsync() > 0;
        }
    }
}
