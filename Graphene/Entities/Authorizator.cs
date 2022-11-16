using Graphene.Database.Interfaces;
using Graphene.Entities.Interfaces;
using Graphene.Graph.Interfaces;
using Graphene.Services;
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
    /// Responsable of Authorize an Action for a given User given an DinamiqLinQ expression
    /// </summary>
    public class Authorizator : Entity, IAuthorizator
    {
        /// <summary>
        /// The name of this Autorizator
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The Action to Authorize, this correspond to and endpoint call (create/delete/update/or read).
        /// </summary>
        public string Action { get; set; }
        /// <summary>
        /// The entity that will be affected: this corresponds to a DbSet in the Databasecontext
        /// </summary>
        public string Entity { get; set; }
        /// <summary>
        /// The Dinamic LinQ expression to excecute to authorize the User
        /// </summary>
        public string Expression { get; set; }
        /// <summary>
        /// In case that it needs to query the database to authorize 
        /// this holds the Table in which starts the query.
        /// </summary>
        public string From { get; set; }
        public ICollection<IAuthorization> Authorizations { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        /// <summary>
        /// 
        /// </summary>
        // public bool  { get; set; }
        /// <summary>
        /// TODO: EXPLICIT AUTHORIZATION without linq
        /// </summary>
        //[NotMapped]
        //public ICollection<IAuthorization> Authorizations { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public virtual  bool Evaluate(IAuthorizable user, dynamic resource, dynamic request)
           => Expression != null && user != null && resource != null
                ? Graphene.Entities.Entity.QueryableOf(resource).Where(Expression, user, request).Count() > 0
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityContext"></param>
        /// <param name="graph"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public virtual async Task<bool> AuthorizeFromDatabase(IEntityContext entityContext, IGraph graph, IGrapheneDatabaseContext db)
        {
            var query = graph.GetSet(db, From ?? Entity).Where(Expression, entityContext.User, entityContext.RequestInstance, entityContext.Resource);
            return (await query.AsNoTracking().CountAsync()) > 0;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityContext"></param>
        /// <param name="graph"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public virtual async Task<bool> Authorize(IAuthenticable user, object request, IEntity resource)
        {
            return Evaluate(user, resource, request);
        }
    }
}
