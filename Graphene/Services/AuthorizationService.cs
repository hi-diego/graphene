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
using Newtonsoft.Json.Linq;
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
        public AuthorizationService(IGrapheneDatabaseContext databaseContext, IGraph graph)
        {
            Graph = graph;
            DatabaseContext = databaseContext;
            Repository = new EntityRepository(databaseContext, graph);
        }

        /// <summary>
        /// 
        /// </summary>
        public IGraph Graph { get; }

        /// <summary>
        /// 
        /// </summary>
        public IGrapheneDatabaseContext DatabaseContext { get; }

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
        //public static async Task<bool> IsAuthorizedByExpression(dynamic instance, IAuthenticable user, Permission permission, IGrapheneDatabaseContext databaseContext)
        //{
        //    if (permission != null && permission.NeedNestedAuthorization) return await IsNestedAuthorized(instance, permission.Name, user, databaseContext);
        //    return await Permission.IsAuthorized(permission, instance, user, databaseContext);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="user"></param>
        /// <param name="resource"></param>
        /// <param name="policyName"></param>
        /// <returns></returns>
        //public static async Task<bool> IsAuthorizedByExpression(IEntityRequest r, IGrapheneDatabaseContext context)
        //{
        //    return await IsAuthorizedByExpression(r.Instance, r.IAuthorizable, r.Permission, context);
        //}

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
            var ActionContext = context;
            var Action = ((ControllerActionDescriptor)context.ActionDescriptor).ActionName;
            var EntityName = ((string) context.ActionArguments["entity"]).DbSetName();
            var Entity = Graph.Find(EntityName);
            IAuthorizable User = Authenticable.Transform(((ClaimsIdentity) context.HttpContext.User.Identity));
            // Return true if the user is Authorized explicitly (has the IAuthorizablePermission)
            return await HasIAuthorizator(User, Action, EntityName);
            //if (await HasIAuthorizablePermision(ERequest.IAuthorizable, ERequest.Action, ERequest.EntityName)) return true;
            //return await AuthorizeByPermissionExpression();
        }
        /// <summary>
        ///  Authorize the Action by excecuting the Database Permission Expression.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="action"></param>
        /// <param name="entityName"></param>
        /// <returns></returns>
        //private async Task<bool> AuthorizeByPermissionExpression()
        //{
        //    return await AuthorizationService.IsAuthorizedByExpression(ERequest, DatabaseContext);
        //}
        /// <summary>
        /// Check in the database if the user has the IAuthorizablePermission.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="action"></param>
        /// <param name="entityName"></param>
        /// <returns></returns>
        private async Task<bool> HasIAuthorizator(IAuthorizable user, string action, string entityName)
            => await Graphene.Graph.Graph.GetSet<IAuthorizator>(DatabaseContext)
                .Where(up =>
                    up.AuthorizedId == user.Id &&
                    up.Action == action &&
                    up.Entity == entityName &&
                    !up.Denied
                ).AsNoTracking()
                .CountAsync() > 0;
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
    /// <summary>
    /// 
    /// </summary>
    public interface IAuthorizator
    {
        /// <summary>
        /// 
        /// </summary>
        public int AuthorizedId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Action { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool Denied { get; set; }
        /// <summary>
        /// 
        /// </summary>
        string Entity { get; set; }
    }
}
