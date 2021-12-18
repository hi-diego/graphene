using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrapheneCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using GrapheneCore.Database.Interfaces;
using GrapheneCore.Models;
using Graphene.Extensions;
using GrapheneCore.Models.Interfaces;
using System.Reflection;
using GrapheneCore.Graph.Interfaces;

namespace GrapheneCore.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class ModelRepository
    {
        /// <summary>
        /// 
        /// </summary>
        public ModelRepository(IGrapheneDatabaseContext dbContext, IGraph graph)
        {
            Graph = graph;
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
        /// Create a Model from the JObject and add the resource into the DbContext
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
        /// Create a Model from the JObject and add the resource into the DbContext
        /// and Persist the changes with SaveChanges if the save flag is provided.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="entityName"></param>
        /// <returns></returns>
        public async Task<object> TrackGraph(dynamic instance, object user = null, bool save = true)
        {
            GrapheneCore.Graph.Graph.SaveAnnotatedGraph(DatabaseContext, instance);
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
        public async Task<object> Create(object instance, bool save = true)
        {
            DatabaseContext.Add(instance);
            if (save) await Save((Model)instance, false);
            return instance;
        }
        /// <summary>
        /// Verify if the Resource Exist in the DbContext
        /// and find it by its Id.
        ///
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Model> Find(string entityName, int id, bool tracking = true, string[] load = null, string[] includes = null)
        {
            // Check if the DbSet and the Key in the ModelDictionary exists in DatabaseContext, return if not
            if (!Graph.Exists(DatabaseContext, ref entityName)) return null;
            // Get the Set as var becuse (IQueryable<dynamic>) but the ModelType will be calculated at runtime and thats necesary for the 
            // dynamic excecution of "Include" and "IhenInclude" methods, if a (IQueryable<object>, IQueryable<Model> IQueryable<any>) is given the dynamic excution by reflectrion will crash.
            var set = Graph.GetSet(DatabaseContext, entityName);
            // Get the model SystemType and GraphType in order to follow the graph, this is key to know which method (Include, ThenInclude or ThenIncludeMultiple) is necesary to call.
            Type modelType = Graph.Find(entityName.DbSetName()).SystemType;
            // Generate and Juxtapoze the dynamic includeds by executing the static includeMethod from the EntityFrameworkQueryableExtensions.
            set = Graph.SetIncludes(set, modelType, includes);
            // Find instance by Model.Id
            set = set.Where(i => (i as Model).Id == id);
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
        public Model FindSync(string entityName, int id, bool tracking = true, string[] load = null)
        {
            if (!Graph.Exists(DatabaseContext, ref entityName)) return null;
            var set = Graph.GetSet<Model>(DatabaseContext, entityName)
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
        public async Task<Model> Update(JObject data, string entityName, int id, bool save = true)
        {
            Model instance = await Find(entityName, id, false);
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
        public async Task<Model> Update(Model instance, JObject data, bool save = true)
        {
            Model changed = instance.Update(data);
            try { DatabaseContext.Update(changed); }
            catch (Exception e) { var f = e; }
            if (save) await Save(changed, false);
            return changed;
        }

        /// <summary>
        /// Persist the instance Data using SaveChanges and  Create 
        /// all the Correspondent Modellog records for each operation.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="request"></param>
        /// <param name="id"></param>
        public async Task<Model> Save(Model instance, bool update = true)
        {
            IEnumerable<IModelLog> logs = BeforeSave(instance);
            if (update) DatabaseContext.Update(instance);
            await DatabaseContext.SaveChangesAsync();
            AfterSave(instance, logs);
            return instance;
        }

        /// <summary>
        /// The BeforeCreate logic
        /// consists in creating all the correspondent ModelLog instances 
        /// for each operation that is recorded to perfom
        /// and adding it to the DbContext and Firing all the ModelEvents.
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<IModelLog> BeforeSave(Model instance)
        {
            IEnumerable<IModelLog> logs = LogChanges();
            FireBeforeModelEvents(logs);
            return logs;
        }

        /// <summary>
        /// The BeforeCreate logic
        /// consists in creating all the correspondent ModelLog instances 
        /// for each operation that is recorded to perfom
        /// and adding it to the DbContext and Firing all the ModelEvents.
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<IModelLog> AfterSave(Model instance, IEnumerable<IModelLog> logs)
        {
            FireAfterModelEvents(logs);
            return logs;
        }

        /// <summary>
        /// Excecute the correspondent instance method Event for each Model log.
        /// </summary>
        /// <param name="logs"></param>
        public void FireBeforeModelEvents(IEnumerable<IModelLog> logs)
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
        /// Excecute the correspondent instance method Event for each Model log.
        /// </summary>
        /// <param name="logs"></param>
        public void FireAfterModelEvents(IEnumerable<IModelLog> logs)
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
        public IEnumerable<IModelLog> LogChanges()
        {
            Type? logType = Graph.Find<IModelLog>()?.SystemType;
            if (logType == null) return new List<ModelLog>();
            var entries = DatabaseContext.ChangeTracker.Entries();
            var set = GrapheneCore.Graph.Graph.GetSet<IModelLog>(DatabaseContext);
            IEnumerable<IModelLog> logs = set.CreateAndAddEntries(entries, logType);
            DatabaseContext.AddRange(logs);
            // TODO: add a conditional option to store logs
            return logs;
        }
    }
    public static class IQueryableModelLogExtensions
    {
        public static IEnumerable<IModelLog> CreateAndAddEntries<T>(this IQueryable<T> set, IEnumerable<EntityEntry> entries, Type logType)
        {
            return entries.Where(e => e.State != EntityState.Unchanged)
                // TODO: Replace null for the current auth user
                .Select(entry => ((IModelLog)Activator.CreateInstance(logType)).Init(entry, null))
                .ToList();
        }
    }
}
