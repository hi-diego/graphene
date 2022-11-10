using Graphene.Database.Interfaces;
using Graphene.Entities.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphene.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class Entity : IEntity
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public virtual int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        public virtual bool SerializeId { get; set; } = false;

        /// <summary>
        /// 
        /// </summary>
        // [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        // [DefaultValueSql("GETUTCDATE()")]
        public virtual Guid Uid { get; set; } = Guid.NewGuid();

        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        public virtual string _Entity { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual DateTime CreatedAt { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual DateTime? ModifiedAt { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual DateTime? DeletedAt { get; set; } = null;

        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        public EntityState EntityState { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Entity()
        {
            _Entity = GetType().Name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public virtual Entity Update(JObject changes)
        {
            JObject json = JObject.FromObject(this);
            json.Merge(changes, new JsonMergeSettings { MergeArrayHandling = MergeArrayHandling.Union });
            dynamic updated = json.ToObject(GetType());
            return updated;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ToJson()
        {
            DefaultContractResolver contractResolver = new DefaultContractResolver { NamingStrategy = new CamelCaseNamingStrategy() };
            return JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings {
                ContractResolver = contractResolver,
                ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Database"></param>
        public virtual void AfterAdded(IGrapheneDatabaseContext Database)
        {
            // throw new NotImplementedException();
        }

        public virtual void AfterDeleted(IGrapheneDatabaseContext Database)
        {
            // throw new NotImplementedException();
        }

        public virtual void AfterDetached(IGrapheneDatabaseContext Database)
        {
            // throw new NotImplementedException();
        }

        public virtual void AfterModified(IGrapheneDatabaseContext Database)
        {
            // throw new NotImplementedException();
        }

        public virtual void BeforeAdded(IGrapheneDatabaseContext Database)
        {
            CreatedAt = DateTime.Now;
        }

        public virtual void BeforeDeleted(IGrapheneDatabaseContext Database)
        {
            DeletedAt = DateTime.Now;
        }

        public virtual void BeforeDetached(IGrapheneDatabaseContext Database)
        {
            // throw new NotImplementedException();
        }

        public virtual void BeforeModified(IGrapheneDatabaseContext Database)
        {
            ModifiedAt = DateTime.Now;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entry"></param>
        public static void SoftDelete(Entity entity, EntityEntry entry)
        {
            entity.DeletedAt = DateTime.Now;
            entry.State = EntityState.Modified;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IQueryable<T> QueryableOf<T>(T Entity)
        {
            return (new List<T>() { Entity }).AsQueryable();
        }
        /// <summary>
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IQueryable<dynamic> QueryableOf<T>() where T : class
        {
            var Object = Activator.CreateInstance(typeof(T), new object[] { });
            return (new List<T>() { ((T)Object) }).AsQueryable();
        }

        /// <summary>
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static IQueryable<dynamic> DynamicQueryableOf<T>(T instance) where T : class
        {
            return (new List<T>() { instance }).AsQueryable();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual bool ShouldSerializeId()
        {
            return SerializeId;
        }
    }
}
