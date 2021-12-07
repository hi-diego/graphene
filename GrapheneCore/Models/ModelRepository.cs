﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using GrapheneCore.Database.Models;
//using GrapheneCore.Database.Entities.Abstractions;
using GrapheneCore.Extensions;
//using GrapheneCore.Http.Exceptions;
//using GrapheneCore.Services;
//using Microsoft.AspNetCore.Mvc;
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
        public ModelRepository(IGrapheneDatabaseContext dbContext)
        {
            DatabaseContext = dbContext;
        }

        /// <summary>
        /// 
        /// </summary>
        public IGrapheneDatabaseContext DatabaseContext { get; private set; }

        /// <summary>
        /// Create a GraphModel from the JObject and add the resource into the DbContext
        /// and Persist the changes with SaveChanges if the save flag is provided.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="entityName"></param>
        /// <returns></returns>
        public async Task<object> Create(string entityName, JObject data, bool save = true)
        {
            dynamic instance = data.ToObject(DatabaseContext.ModelDictionary[entityName.DbSetName()]);
            return save ? await Create(instance) : instance;
        }

        /// <summary>
        /// Create a GraphModel from the JObject and add the resource into the DbContext
        /// and Persist the changes with SaveChanges if the save flag is provided.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="entityName"></param>
        /// <returns></returns>
        public async Task<object> TrackGraph(dynamic instance, object user = null, bool save = true)
        {
            DatabaseContext.SaveAnnotatedGraph(instance);
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
            if (save) await Save((GraphModel)instance, false);
            return instance;
        }

        /// <summary>
        /// Verify if the Resource Exist in the DbContext
        /// and find it by its Id.
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<GraphModel> Find(string entityName, int id, bool tracking = true, string[] load = null)
        {
            if (!DatabaseContext.Exists(ref entityName)) return null;
            var set = DatabaseContext.GetSet<GraphModel>(entityName)
                .Where(i => i.Id == id);
            return tracking
                ? await set.Includes(load).FirstOrDefaultAsync()
                : await set.AsNoTracking().Includes(load).AsNoTracking().FirstOrDefaultAsync();
        }

        /// <summary>
        /// Verify if the Resource Exist in the DbContext
        /// and find it by its Id.
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public GraphModel FindSync(string entityName, int id, bool tracking = true, string[] load = null)
        {
            if (!DatabaseContext.Exists(ref entityName)) return null;
            var set = DatabaseContext.GetSet<GraphModel>(entityName)
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
        public async Task<GraphModel> Update(JObject data, string entityName, int id, bool save = true)
        {
            GraphModel instance = await Find(entityName, id, false);
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
        public async Task<GraphModel> Update(GraphModel instance, JObject data, bool save = true)
        {
            GraphModel changed = instance.Update(data);
            try { DatabaseContext.Update(changed); }
            catch (Exception e) { var f = e; }
            if (save) await Save(changed, false);
            return changed;
        }

        /// <summary>
        /// Persist the instance Data using SaveChanges and  Create 
        /// all the Correspondent GraphModellog records for each operation.
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="request"></param>
        /// <param name="id"></param>
        public async Task<GraphModel> Save(GraphModel instance, bool update = true)
        {
            IEnumerable<IGraphModelLog> logs = BeforeSave(instance);
            if (update) DatabaseContext.Update(instance);
            await DatabaseContext.SaveChangesAsync();
            AfterSave(instance, logs);
            return instance;
        }

        /// <summary>
        /// The BeforeCreate logic
        /// consists in creating all the correspondent GraphModelLog instances 
        /// for each operation that is recorded to perfom
        /// and adding it to the DbContext and Firing all the ModelEvents.
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<IGraphModelLog> BeforeSave(GraphModel instance)
        {
            IEnumerable<IGraphModelLog> logs = LogChanges();
            FireBeforeGraphModelEvents(logs);
            return logs;
        }

        /// <summary>
        /// The BeforeCreate logic
        /// consists in creating all the correspondent GraphModelLog instances 
        /// for each operation that is recorded to perfom
        /// and adding it to the DbContext and Firing all the ModelEvents.
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<IGraphModelLog> AfterSave(GraphModel instance, IEnumerable<IGraphModelLog> logs)
        {
            FireAfterGraphModelEvents(logs);
            return logs;
        }

        /// <summary>
        /// Excecute the correspondent instance method Event for each GraphModel log.
        /// </summary>
        /// <param name="logs"></param>
        public void FireBeforeGraphModelEvents(IEnumerable<IGraphModelLog> logs)
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
        /// Excecute the correspondent instance method Event for each GraphModel log.
        /// </summary>
        /// <param name="logs"></param>
        public void FireAfterGraphModelEvents(IEnumerable<IGraphModelLog> logs)
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
        public IEnumerable<IGraphModelLog> LogChanges()
        {
            var entries = DatabaseContext.ChangeTracker.Entries();
            var logs = ModelTracker.GenerateGraphModelLogs(entries, new GraphModelLog());
            // TODO: add a conditional option to store logs
            // DatabaseContext.AddRange(logs);
            return logs;
        }
    }
}