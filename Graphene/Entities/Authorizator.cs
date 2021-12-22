using Graphene.Database.Interfaces;
using Graphene.Entities.Interfaces;
using Graphene.Graph.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
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
        // public bool  { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        public ICollection<IAuthorization> Authorizations { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public virtual  bool Evaluate(IAuthorizable user, Entity res)
           => Expression != null && user != null && res != null
                ? Graphene.Entities.Entity.QueryableOf(res).Where(Expression, user).Count() > 0
                : false;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityInstance"></param>
        /// <param name="user"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual async Task<bool> IsAuthorized(Entity instance, IAuthorizable user, IGrapheneDatabaseContext context, IGraph graph)
            => (await CreateAuthorizedQueryable(instance, user, context, graph)
                .AsNoTracking()
                .CountAsync()) > 0;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityInstance"></param>
        /// <param name="user"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public virtual IQueryable<dynamic> CreateAuthorizedQueryable(Entity instance, IAuthorizable user, IGrapheneDatabaseContext context, IGraph graph)
            => graph.GetSet(context, instance._Entity)
                .Where(GetInstanceIdentityExpression(instance))
                .Where(Expression, user);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public virtual string GetInstanceIdentityExpression(Entity instance)
            => $"e => e.Id == {instance.Id}";
    }
}
