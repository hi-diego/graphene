using GrapheneCore.Database.Interfaces;
using GrapheneCore.Models.Interfaces;
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

namespace GrapheneCore.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Model : IModel
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public virtual int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public virtual Guid Uid { get; set; } = Guid.NewGuid();

        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        public virtual string Type { get; set; }

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
        public Model()
        {
            Type = GetType().Name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public virtual Model Update(JObject changes)
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
        public static void SoftDelete(Model entity, EntityEntry entry)
        {
            entity.DeletedAt = DateTime.Now;
            entry.State = EntityState.Modified;
        }
    }
}
