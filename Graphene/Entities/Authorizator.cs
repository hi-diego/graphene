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
        /// 
        /// </summary>
        /// <param name="entityContext"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static IAuthorizator GetFromContext(IEntityContext entityContext)
        {
            // Wee dont need to retrive it from Db each time is already cached in the Graph
            // return entityContext.User.Authorizations.Where(a => a.Action == entityContext.ActionName).FirstOrDefault();
            IAuthorizator authorizator = Graph.Graph.GetSet<IAuthorizator>(entityContext.DbContext)?.FirstOrDefault() ?? (IAuthorizator) new Authorization();
            authorizator.Action = entityContext.ActionName;
            authorizator.Entity = entityContext.GraphType.Name;
            authorizator.Expression = entityContext.ActionName == "Find" || entityContext.ActionName == "Index" ? "true" : "false";
            return authorizator;
        }

        /// <summary>
        /// The entity that will be affected: this corresponds to a DbSet in the Databasecontext
        /// </summary>
        public string Entity { get; set; }
        /// <summary>
        /// The Dinamic LinQ expression to excecute to authorize the User
        /// </summary>
        public string Expression { get; set; } = "false";
        /// <summary>
        /// In case that it needs to query the database to authorize 
        /// this holds the Table in which starts the query.
        /// </summary>
        public string From { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="res"></param>
        /// <returns></returns>
        public virtual bool Evaluate(dynamic user, dynamic? resource, dynamic? request)
           => Graphene.Entities.Entity.QueryableOf(user).Where(FormatExpression(), resource, request).Count() > 0;
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private object FormatExpression()
        {
            return Expression
                .Replace("$user", "@0")
                .Replace("$resource", "@1")
                .Replace("$request", "@2");
        }

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
        public virtual async Task<bool> EvaluateFromDatabase(IEntityContext entityContext, IGraph graph, IGrapheneDatabaseContext db)
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
        public virtual async Task<bool> EvaluateFromDatabase(IAuthenticable user, object? request, object? resource, IGraph graph, IGrapheneDatabaseContext db)
        {
            var query = graph.GetSet(db, From ?? Entity).Where(Expression, user, request, resource);
            return (await query.AsNoTracking().CountAsync()) > 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityContext"></param>
        /// <param name="graph"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public virtual async Task<bool> Authorize(IAuthenticable user, object? request, object? resource, IGraph? graph = null, IGrapheneDatabaseContext? db = null)
        {
            return graph == null || db == null
                ? Evaluate(user, resource, request)
                : await EvaluateFromDatabase(user, resource, request, graph, db);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityContext"></param>
        /// <returns></returns>
        public virtual async Task<bool> IsAuthorized(IEntityContext entityContext)
        {
            return entityContext.User == null
                ? false
                : await Authorize(entityContext.User, entityContext.Request, entityContext.Resource);
        }
    }
}
