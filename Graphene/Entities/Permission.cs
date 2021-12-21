//using Graphene.Database.Interfaces;
//using Graphene.Entities.Interfaces;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.ChangeTracking;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Linq;
//using System.Text;
//using System.Text.Json.Serialization;
//using System.Threading.Tasks;

//namespace Graphene.Entities
//{
//    /// <summary>
//    /// 
//    /// </summary>
//    public class Permission : Entity
//    {
//        /// <summary>
//        /// 
//        /// </summary>
//        [NotMapped]
//        [JsonIgnore]
//        public bool NeedNestedAuthorization { get => Name == "Add" || Name == "Edit"; }

//        /// <summary>
//        /// 
//        /// </summary>
//        public string Name { get; set; }

//        /// <summary>
//        /// 
//        /// </summary>
//        [NotMapped]
//        public string Action { get => Name; }

//        /// <summary>
//        /// 
//        /// </summary>
//        public string Entity { get; set; }

//        /// <summary>
//        /// 
//        /// </summary>
//        public string Expression { get; set; }

//        /// <summary>
//        /// 
//        /// </summary>
//        public ICollection<UserPermission> UserPermissions { get; set; } = new List<UserPermission>();

//        /// <summary>
//        /// 
//        /// </summary>
//        public static async Task<Permission> GetPermission(IGrapheneDatabaseContext context, string action = null, string entity = null, EntityState? state = null)
//        {
//            action = state != null ? IGrapheneDatabaseContext.ActionNameDictionary[(EntityState)state] : action;
//            return await context.Permission
//                .Where(p => p.Name == action && p.Entity == entity)
//                .AsNoTracking()
//                .FirstOrDefaultAsync();
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="user"></param>
//        /// <param name="res"></param>
//        /// <returns></returns>
//        public bool Evaluate(User user, Entity res)
//        {
//            return Expression != null && user != null && res != null
//                ? Backend.Database.Entities.Entity.QueryableOf(res).Where(Expression, user).Count() > 0
//                : false;
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="permission"></param>
//        /// <returns></returns>
//        public static async Task<bool> IsAuthorized(Permission permission, EntityEntry entry, IAuthorizable user, IGrapheneDatabaseContext context)
//        {
//            Entity instance = (Entity)entry.Entity;
//            instance.EntityState = entry.State;
//            return await Permission.IsAuthorized(permission, instance, user, context);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="permission"></param>
//        /// <returns></returns>
//        public static async Task<bool> IsAuthorized(Permission permission, Entity instance, User user, IGrapheneDatabaseContext context)
//        {
//            if (permission == null) return true;
//            return await permission.IsAuthorized(instance, user, context);
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="permission"></param>
//        /// <returns></returns>
//        public async Task<bool> IsAuthorized(Entity entityInstance, User user, IGrapheneDatabaseContext context)
//        {
//            if (Expression == null) return true;
//            var query = context.GetSet(Entity).Where(Expression, user);
//            if (entityInstance.EntityState == EntityState.Modified) query = query.Where($"e => e.Id == {entityInstance.Id}");
//            return await query.AsNoTracking().CountAsync() > 0;
//        }
//    }
//}