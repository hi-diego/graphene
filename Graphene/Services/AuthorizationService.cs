using Graphene.Database.Interfaces;
using Graphene.Entities;
using Graphene.Entities.Interfaces;
using Graphene.Extensions;
using Graphene.Graph.Interfaces;
using Graphene.Http.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using Microsoft.Extensions.Options;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Graphene.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Database"></param>
        public AuthorizationService(IGrapheneDatabaseContext databaseContext, IGraph graph, IOptions<JsonOptions> jsonOptions)
        {
            Graph = graph;
            DatabaseContext = databaseContext;
            Repository = new EntityRepository(databaseContext, graph, jsonOptions);
        }

        /// <summary>
        /// 
        /// </summary>
        public IGraph Graph { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IGrapheneDatabaseContext DatabaseContext { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EntityRepository Repository { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="resource"></param>
        /// <param name="policyName"></param>
        /// <returns></returns>
        public async Task<bool> IsAuthorizedByExpression(dynamic instance, IAuthorizable user, string action, string entityName)
        {
            IAuthorizator authorizator = (IAuthorizator) Graphene.Graph.Graph
                .GetSet<IAuthorizator>(DatabaseContext)
                .First(a => a.Action == action && a.Entity == entityName);
            // if (authorizator != null && authorizator.NeedNestedAuthorization) return await IsNestedAuthorized(instance, authorizator.Name, user, databaseContext);
            return await authorizator.IsAuthorized(instance, user, DatabaseContext, Graph);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="action"></param>
        /// <param name="user"></param>
        /// <param name="databaseContext"></param>
        /// <returns></returns>
        //public static async Task<bool> IsNestedAuthorized(dynamic instance, string action, IAuthenticable user, IGrapheneDatabaseContext databaseContext)
        //{
        //    // Copy the context to avoid multiple Tracking  on the same instance.
        //    using (var context = new Context())
        //    {
        //        if (action == "Add") context.Add(instance);
        //        else if (action == "Edit") context.Update(instance);
        //        var entries = context.ChangeTracker.Entries();
        //        foreach (EntityEntry entry in entries)
        //        {
        //            if (entry.State == EntityState.Unchanged) continue;
        //            // Cache of permissions in the future.
        //            Permission permission = await Permission.GetPermission(databaseContext, action, ((Entity)entry.Entity)._Entity, entry.State);
        //            if (!(await Permission.IsAuthorized(permission, entry, user, context)))
        //                throw new StatusCodeException(new UnauthorizedResult());
        //        }
        //    }
        //    return true;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="action"></param>
        /// <param name="user"></param>
        /// <param name="databaseContext"></param>
        /// <returns></returns>
        //public static async Task<bool> IsNestedAuthorized(IAuthenticable user, IGrapheneDatabaseContext databaseContext)
        //{
        //    // Copy the context to avoid multiple Tracking  on the same instance.
        //    using (var context = new Context())
        //    {
        //        var entries = context.ChangeTracker.Entries();
        //        foreach (EntityEntry entry in entries)
        //        {
        //            if (entry.State == EntityState.Unchanged) continue;
        //            // Cache of permissions in the future.
        //            Permission permission = await Permission.GetPermission(databaseContext, null, ((Entity)entry.Entity)._Entity, entry.State);
        //            if (!(await Permission.IsAuthorized(permission, entry, user, context)))
        //                throw new StatusCodeException(new UnauthorizedResult());
        //        }
        //    }
        //    return true;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="Repository"></param>
        /// <returns></returns>
        public async Task<bool> IsAuthorized(ActionExecutingContext context)
        {
            var action = ((ControllerActionDescriptor)context.ActionDescriptor).ActionName;
            var entityName = ((string) context.ActionArguments["entity"]).DbSetName();
            var instance = Graph.Find(entityName);
            IAuthorizable user = Authenticable.Transform((ClaimsIdentity) context.HttpContext.User.Identity);
            // Return true if the user is Authorized explicitly (has the IAuthorizablePermission)
            // return await HasIAuthorizator(User, Action, EntityName);
            return (await HasIAuthorizator(user, action, entityName))
                || (await IsAuthorizedByExpression(instance, user, action, entityName));
        }
        /// <summary>
        /// Check in the database if the user has the IAuthorizablePermission.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="action"></param>
        /// <param name="entityName"></param>
        /// <returns></returns>
        public async Task<bool> HasIAuthorizator(IAuthorizable user, string action, string entityName)
            => await GetAuthorizeQueryable(user, action, entityName).AsNoTracking().CountAsync() > 0;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="action"></param>
        /// <param name="entityName"></param>
        /// <returns></returns>
        public IQueryable<IAuthorization> GetAuthorizeQueryable(IAuthorizable user, string action, string entityName)
            => Graphene.Graph.Graph.GetSet<IAuthorization>(DatabaseContext).Where(a =>
                a.AuthorizableId == user.Id &&
                a.Authorizator.Action == action &&
                a.Authorizator.Entity == entityName &&
                !a.Denied
            );
        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="resource"></param>
        /// <param name="requirements"></param>
        /// <returns></returns>
        public Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object resource, IEnumerable<IAuthorizationRequirement> requirements)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="resource"></param>
        /// <param name="policyName"></param>
        /// <returns></returns>
        public Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object resource, string policyName)
        {
            throw new NotImplementedException();
        }
    }
}
