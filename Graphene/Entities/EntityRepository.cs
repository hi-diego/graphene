using Graphene.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json.Linq;
using System.Linq.Dynamic.Core;
using Graphene.Database.Interfaces;
using Graphene.Entities.Interfaces;
using Graphene.Graph.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Graphene.Http.Converters;

namespace Graphene.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class EntityRepository
    {
        /// <summary>
        /// 
        /// </summary>
        public EntityRepository(IGrapheneDatabaseContext dbContext, IGraph graph, IDistributedCache? cache = null)
        {
            Graph = graph;
            _cache = cache;
            DatabaseContext = dbContext;
        }

        /// <summary>
        /// 
        /// </summary>
        public IGraph Graph { get; }

        /// <summary>
        /// 
        /// </summary>
        public IGrapheneDatabaseContext DatabaseContext { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public IDistributedCache _cache { get; private set; }

        /// <summary>
        /// Create a Entity from the JObject and add the resource into the DbContext
        /// and Persist the changes with SaveChanges if the save flag is provided.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="entityName"></param>
        /// <returns></returns>
        public async Task<object> Create(string entityName, JObject data, bool save = true)
        {
            dynamic instance = data.ToObject(Graph.Find(entityName.DbSetName()).SystemType);
            return save ? await Create(instance) : instance;
        }

        /// <summary>
        /// Create a Entity from the JObject and add the resource into the DbContext
        /// and Persist the changes with SaveChanges if the save flag is provided.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="entityName"></param>
        /// <returns></returns>
        public object Generate(Type type, JObject data)
        {
            dynamic instance = data.ToObject(type);
            return instance;
        }

        /// <summary>
        /// Create a Entity from the JObject and add the resource into the DbContext
        /// and Persist the changes with SaveChanges if the save flag is provided.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="entityName"></param>
        /// <returns></returns>
        public async Task<object> TrackGraph(dynamic instance, object user = null, bool save = true)
        {
            Graphene.Graph.Graph.SaveAnnotatedGraph(DatabaseContext, instance);
            // await AuthorizationService.IsNestedAuthorized(user, DatabaseContext);
            if (save) await Save(instance);
            return instance;
        }

        /// <summary>
        /// Add the instnace into the DbContext
        /// and Persist the changes with SaveChanges if the save flag is provided.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="entityName"></param>
        /// <returns></returns>
        public async Task<Entity> Create(object instance, bool save = true)
        {
            DatabaseContext.Add(instance);
            if (save) return await Save(instance as Entity, false);
            return instance as Entity;
        }

        /// <summary>
        /// Verify if the Resource Exist in the DbContext
        /// and find it by its Id.
        ///
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public IQueryable<dynamic> CreateFindQuery(Type entityType, object id, string[]? load = null, string[]? includes = null, bool tracking = true)
        {
            // Get the Set as var becuse (IQueryable<dynamic>) but the entityType will be calculated at runtime and thats necesary for the 
            // dynamic excecution of "Include" and "IhenInclude" methods, if a (IQueryable<object>, IQueryable<Entity> IQueryable<any>) is given the dynamic excution by reflectrion will crash.
            var set = Graph.GetSet(DatabaseContext, entityType);
            // Generate and Juxtapoze the dynamic includeds by executing the static includeMethod from the EntityFrameworkQueryableExtensions.
            // if (includes != null) set = Graph.SetIncludes(set, entityType, includes);
            if (load != null) set = set.Includes(load);
            // Set the AsNoTracking option value.
            if (!tracking) set = set.AsNoTracking();
            // Find instance by Entity.Id
            if (id is int) return set.Where(i => (i as Entity).Id.Equals(id));
            // check for the incremental Id in redis cache for performance 
            int cacheId = new EntityGuidConverter(entityType, _cache).GetCachedId(id);
            // if found use the chache id
            if (cacheId > 0) return set.Where(i => (i as Entity).Id.Equals(cacheId));
            // if not query with the guid
            return set = set.Where(i => (i as Entity).Uid.Equals(id));
        }

        /// <summary>
        /// Verify if the Resource Exist in the DbContext
        /// and find it by its Id.
        ///
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Entity> Find(string entityName, Guid guid, bool tracking = true, string[] load = null, string[] includes = null)
        {
            // Check if the DbSet and the Key in the EntityDictionary exists in DatabaseContext, return if not
            if (!Graph.Exists(DatabaseContext, ref entityName)) return null;
            // Get the Set as var becuse (IQueryable<dynamic>) but the entityType will be calculated at runtime and thats necesary for the 
            // dynamic excecution of "Include" and "IhenInclude" methods, if a (IQueryable<object>, IQueryable<Entity> IQueryable<any>) is given the dynamic excution by reflectrion will crash.
            var set = Graph.GetSet(DatabaseContext, entityName);
            // Get the entity SystemType and GraphType in order to follow the graph, this is key to know which method (Include, ThenInclude or ThenIncludeMultiple) is necesary to call.
            Type entityType = Graph.Find(entityName.DbSetName()).SystemType;
            // Generate and Juxtapoze the dynamic includeds by executing the static includeMethod from the EntityFrameworkQueryableExtensions.
            set = Graph.SetIncludes(set, entityType, includes);
            // Find instance by Entity.Id
            set = set.Where(i => (i as Entity).Uid.Equals(guid));
            // Set the AsNoTracking option value.
            return tracking
                ? await set.Includes(load).FirstOrDefaultAsync()
                : await set.AsNoTracking().Includes(load).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Verify if the Resource Exist in the DbContext
        /// and find it by its Id.
        ///
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Entity> Find(string entityName, int id, bool tracking = true, string[] load = null, string[] includes = null)
        {
            // Check if the DbSet and the Key in the EntityDictionary exists in DatabaseContext, return if not
            if (!Graph.Exists(DatabaseContext, ref entityName)) return null;
            // Get the Set as var becuse (IQueryable<dynamic>) but the entityType will be calculated at runtime and thats necesary for the 
            // dynamic excecution of "Include" and "IhenInclude" methods, if a (IQueryable<object>, IQueryable<Entity> IQueryable<any>) is given the dynamic excution by reflectrion will crash.
            var set = Graph.GetSet(DatabaseContext, entityName);
            // Get the entity SystemType and GraphType in order to follow the graph, this is key to know which method (Include, ThenInclude or ThenIncludeMultiple) is necesary to call.
            Type entityType = Graph.Find(entityName.DbSetName()).SystemType;
            // Generate and Juxtapoze the dynamic includeds by executing the static includeMethod from the EntityFrameworkQueryableExtensions.
            set = Graph.SetIncludes(set, entityType, includes);
            // Find instance by Entity.Id
            set = set.Where(i => (i as Entity).Id == id);
            // Set the AsNoTracking option value.
            return tracking
                ? await set.Includes(load).FirstOrDefaultAsync()
                : await set.AsNoTracking().Includes(load).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Verify if the Resource Exist in the DbContext
        /// and find it by its Id.
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public Entity FindSync(string entityName, int id, bool tracking = true, string[] load = null)
        {
            if (!Graph.Exists(DatabaseContext, ref entityName)) return null;
            var set = Graph.GetSet<Entity>(DatabaseContext, entityName)
                .Where(i => i.Id == id)
                .Includes(load);
            return tracking
                ? set.FirstOrDefault()
                : set.AsNoTracking().FirstOrDefault();
        }

        /// <summary>
        /// Remove the entity from the Context.
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public async Task<object> Delete(object instance)
        {
            DatabaseContext.Remove(instance);
            await DatabaseContext.SaveChangesAsync();
            return instance;
        }

        /// <summary>
        /// Find the instance and if exists Update it from the JObject updating
        /// the instance resource into the DbContext
        /// and persist the changes with SaveChanges if the save flag is provided.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="request"></param>
        /// <param name="id"></param>
        public async Task<Entity> Update(JObject data, string entityName, int id, bool save = true)
        {
            Entity instance = await Find(entityName, id, false);
            if (instance == null) return instance;
            return await Update(instance, data, save); ;
        }

        /// <summary>
        /// Find the instance and if exists Update it from the JObject updating
        /// the instance resource into the DbContext
        /// and persist the changes with SaveChanges if the save flag is provided.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="request"></param>
        /// <param name="id"></param>
        public async Task<Entity> Edit(Entity instance, JObject data)
        {
            if (instance == null) return instance;
            return await Update(instance, data, true); ;
        }

        /// <summary>
        /// Find the instance and if exists Update it from the JObject updating
        /// the instance resource into the DbContext
        /// and persist the changes with SaveChanges if the save flag is provided.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="request"></param>
        /// <param name="id"></param>
        public async Task<Entity> Update(Entity instance, JObject data, bool save = true)
        {
            Entity changed = instance.Update(data);
            try { DatabaseContext.Update(changed); }
            catch (Exception e) { var f = e; }
            if (save) await Save(changed, false);
            return changed;
        }

        /// <summary>
        /// Persist the instance Data using SaveChanges and  Create 
        /// all the Correspondent Entitylog records for each operation.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="request"></param>
        /// <param name="id"></param>
        public async Task<Entity> Save(Entity instance, bool update = true)
        {
            List<IInstanceLog> logs = BeforeSave(instance);
            if (update) DatabaseContext.Update(instance);
            await DatabaseContext.SaveChangesAsync();
            EntityGuidConverter.CacheUuids(_cache, instance);
            logs.Where(l => l.InstanceEntityState == EntityState.Added).ToList().ForEach(l => {
                l.InstanceId = instance.Id;
            });
            AfterSave(instance, logs);
            return instance;
        }

        /// <summary>
        /// The BeforeCreate logic
        /// consists in creating all the correspondent InstanceLog instances 
        /// for each operation that is recorded to perfom
        /// and adding it to the DbContext and Firing all the EntityEvents.
        /// </summary>
        /// <returns></returns>
        public virtual List<IInstanceLog> BeforeSave(Entity instance)
        {
            List<IInstanceLog> logs = LogChanges();
            FireBeforeEntityEvents(logs);
            return logs;
        }

        /// <summary>
        /// The BeforeCreate logic
        /// consists in creating all the correspondent InstanceLog instances 
        /// for each operation that is recorded to perfom
        /// and adding it to the DbContext and Firing all the EntityEvents.
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<IInstanceLog> AfterSave(Entity instance, IEnumerable<IInstanceLog> logs)
        {
            FireAfterEntityEvents(logs);
            return logs;
        }

        /// <summary>
        /// Excecute the correspondent instance method Event for each Entity log.
        /// </summary>
        /// <param name="logs"></param>
        public void FireBeforeEntityEvents(IEnumerable<IInstanceLog> logs)
        {
            foreach (var log in logs)
            {
                switch (log.Event)
                {
                    case "Added": log.Instance.BeforeAdded(DatabaseContext); break;
                    case "Modified": log.Instance.BeforeModified(DatabaseContext); break;
                    case "Deleted": log.Instance.BeforeDeleted(DatabaseContext); break;
                    case "Detached": log.Instance.BeforeDetached(DatabaseContext); break;
                }
            }
        }


        /// <summary>
        /// Excecute the correspondent instance method Event for each Entity log.
        /// </summary>
        /// <param name="logs"></param>
        public void FireAfterEntityEvents(IEnumerable<IInstanceLog> logs)
        {
            foreach (var log in logs)
            {
                switch (log.Event)
                {
                    case "Added": log.Instance.AfterAdded(DatabaseContext); break;
                    case "Modified": log.Instance.AfterModified(DatabaseContext); break;
                    case "Deleted": log.Instance.AfterDeleted(DatabaseContext); break;
                    case "Detached": log.Instance.AfterDetached(DatabaseContext); break;
                }
            }
        }


        /// <summary>
        /// Check if the Proyect needs to store changes .
        /// </summary>
        public List<IInstanceLog> LogChanges()
        {
            var logGraphType = Graph.Find<IInstanceLog>();
            Type logType = logGraphType?.SystemType ?? typeof(InstanceLog);
            var entries = DatabaseContext.ChangeTracker.Entries().ToList();
            var logInstance = (IInstanceLog) Activator.CreateInstance(logType); 
            var logs = entries.Where(e => e.State != EntityState.Unchanged)
                .Select(entry => logInstance.Init(entry, null))
                .ToList();
            if (logGraphType != null) DatabaseContext.AddRange(logs);
            return logs;
        }
    }

    public static class IQueryableEntityLogExtensions
    {
        public static IQueryable<IInstanceLog> CreateAndAddEntries<T>(this IQueryable<T> set, IEnumerable<EntityEntry> entries, Type logType)
        {
            return entries.Where(e => e.State != EntityState.Unchanged)
                // TODO: Replace null for the current auth user
                .Select(entry => ((IInstanceLog)Activator.CreateInstance(logType)).Init(entry, null)).AsQueryable();
        }
    }
}
