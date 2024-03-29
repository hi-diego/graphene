﻿using Graphene.Database.Interfaces;
using Graphene.Entities;
using Graphene.Entities.Interfaces;
using Graphene.Extensions;
using Graphene.Graph;
using Graphene.Graph.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json.Nodes;
using Graphene.Cache;
using StackExchange.Redis;
using System.Text.Json;
using Microsoft.Extensions.Options;

namespace Graphene.Services
{
    /// <summary>
    /// Read the RequestJson and deconstruct it for future purpuses on the Graphene Pipeline.
    /// </summary>
    public interface IEntityContext
    {
        public Dictionary<string, string> RedisKeys { get; set; }

        /// <summary>
        /// To Read the RequestJson and deconstruct it for future purpuses on the Graphene Pipeline.
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid Guid { get; set; }

        /// <summary>
        /// The equivalent Json object of the request.
        /// </summary>
        public JsonNode? Request { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public GraphType GraphType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public object? RequestInstance { get; }

        /// <summary>
        /// 
        /// </summary>
        public object? Resource { get; }

        /// <summary>
        /// To Read the RequestJson and deconstruct it for future purpuses on the Graphene Pipeline.
        /// </summary>
        public string? ActionName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IAuthorizable? User { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IGrapheneDatabaseContext DbContext { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IGraph Graph { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Entity? Instance { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EntityRepository Repository { get; set; }

        /// <summary>
        /// Retrives the Body of the Current HTTP Request
        /// in a JSON format.
        /// </summary>
        /// <returns></returns>
        public Task<JsonNode?> HttpRequestToJson(HttpContext? httpContext = null, HttpRequest? httpRequest = null);

        public Task<string?> HttpRequestToString(HttpContext? httpContext = null, HttpRequest? httpRequest = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionDescriptor"></param>
        /// <param name="graph"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public IActionResult? DeconstructAction(ActionContext actionDescriptor);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Task<Entity?> FindInstanceAsync(string[]? load = null);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Entity? FindInstance(string[]? load = null);

        /// <summary>
        /// 
        /// </summary>
        public IQueryable<dynamic> InstanceQuery { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IQueryable<dynamic> BuildQuery(string[]? load = null);

        /// <summary>
        /// 
        /// </summary>
        public IConnectionMultiplexer Multiplexer { get; set; }
    }

    /// <summary>
    /// Default Implementation of IEntityContext.
    /// To Read the RequestJson and deconstruct it for future purpuses on the Graphene Pipeline.
    /// </summary>
    public class EntityContext : IEntityContext
    {
        public Dictionary<string, string> RedisKeys { get; set; } = new Dictionary<string, string>();

        /// <summary>
        ///  
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="db"></param>
        public EntityContext(IGraph graph, IGrapheneDatabaseContext db, IConnectionMultiplexer multiplexer, IOptions<JsonOptions> jsonOptions)
        {
            Multiplexer = multiplexer;
            Graph = graph;
            DbContext = db;
            Repository = new EntityRepository(DbContext, Graph, jsonOptions, Multiplexer);
        }

        /// <summary>
        /// 
        /// </summary>
        public Entity? Instance { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IQueryable<dynamic> InstanceQuery { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IAuthorizable? User { get; set; }
        /// <summary>
        /// To Read the RequestJson and deconstruct it for future purpuses on the Graphene Pipeline.
        /// </summary>
        public JsonNode? Request { get; set; }

        /// <summary>
        /// To Read the RequestJson and deconstruct it for future purpuses on the Graphene Pipeline.
        /// </summary>
        public object? RequestInstance { get => null; }

        /// <summary>
        /// To Read the RequestJson and deconstruct it for future purpuses on the Graphene Pipeline.
        /// </summary>
        public object? Resource { get; set; }

        /// <summary>
        /// To Read the RequestJson and deconstruct it for future purpuses on the Graphene Pipeline.
        /// </summary>
        public int Id { get; set; } = 0;
        /// <summary>
        /// 
        /// </summary>
        public Guid Guid { get; set; }

        /// <summary>
        /// To Read the RequestJson and deconstruct it for future purpuses on the Graphene Pipeline.
        /// </summary>
        public GraphType? GraphType { get; set; }

        /// <summary>
        /// To Read the RequestJson and deconstruct it for future purpuses on the Graphene Pipeline.
        /// </summary>
        public string? ActionName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public EntityRepository Repository { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public IGrapheneDatabaseContext DbContext { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IConnectionMultiplexer Multiplexer { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public IGraph Graph { get; set; }

        /// <summary>
        /// Retrives the Body of the Current HTTP Request
        /// in a JSON format.
        /// </summary>
        /// <returns></returns>
        public async Task<JsonNode?> HttpRequestToJson(HttpContext? httpContext = null, HttpRequest? httpRequest = null)
        {
            // If the Request was already read before returns it.
            if (Request != null) return Request;
            string? input = await HttpRequestToString(httpContext, httpRequest);
            if (input == null) return null;
            Request = JsonNode.Parse(input);
            // Return the Stored JSON Request value.
            return Request;
        }

        public async Task<string?> HttpRequestToString(HttpContext? httpContext = null, HttpRequest? httpRequest = null)
        {

            // Get the request for the given params.
            HttpRequest? request = httpRequest ?? httpContext?.Request;
            // If request is null return.
            if (request == null) return null;
            // EnableBuffering to read the request.
            request.EnableBuffering();
            // Read the request stream as string.
            string input = await (new StreamReader(request.Body)).ReadToEndAsync();
            // If cast ends up as null return.
            if (input == "" || input == null) return null;
            // Return the request stream cursor to 0 .
            request.Body.Position = 0;
            // Parse and store the value as JSON object.
            return input;
        }

        /// <summary>
        /// Initialize the Graphene Pipeline by deconstructing the Request params.
        /// It will return a NotFoundResult if the requested Entity url param 
        /// https://api.foo.bar/{entity} does not correspond to any Model in the DatabaseContext or Graphene.Graph.Types cache;
        /// It also returns a NotFoundResult if the request does not provide a valid GUID on the id url parameter.
        /// </summary>
        /// <param name="actionContext"></param>
        /// <returns></returns>
        public IActionResult? DeconstructAction(ActionContext actionContext)
        {
            User = Authenticable.Transform((ClaimsIdentity?)actionContext?.HttpContext.User.Identity);
            var descriptor = (ControllerActionDescriptor) actionContext.ActionDescriptor;
            var actionName = descriptor.ActionName;
            // var jsonBody = await HttpRequestToJson();
            var entityName = actionContext.RouteData?.Values["entity"]?.ToString()?.DbSetName();
            string? id = actionContext.RouteData?.Values["id"]?.ToString();
            int cachedId = 0;
            Guid resourceGuid = Guid.Empty;
            if (id != null)  (cachedId, entityName, resourceGuid) = FetchId(id);
            Id = cachedId;
            Guid = resourceGuid;
            var graphType = Graph.Find(entityName);
            if (graphType == null) return new NotFoundResult();
            GraphType = graphType;
            ActionName = actionName;
            return null;
        }

        public Tuple<int, string, Guid> FetchId (string id)
        {
            Guid resourceGuid;
            if (!Guid.TryParse(id, out resourceGuid)) resourceGuid = id.FromBase64();
            var redisGuidCache = new RedisGuidCache(Multiplexer);
            var cached = redisGuidCache.GetCached(resourceGuid);
            return cached == null
                ? new Tuple<int, string, Guid>(0, resourceGuid.ToString(), resourceGuid)
                : new Tuple<int, string, Guid>(cached.Item1, cached.Item2, resourceGuid);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="id"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public async Task<Entity?> FindInstanceAsync(string[]? load = null)
        {
            // If it was already fetched return it.
            if (Instance != null) return Instance;
            InstanceQuery = BuildQuery(load);
            Instance = await InstanceQuery.AsNoTracking().FirstOrDefaultAsync();
            return Instance;
        }

        public Entity? FindInstance(string[]? load = null)
        {
            // If it was already fetched return it.
            if (Instance != null) return Instance;
            InstanceQuery = BuildQuery(load);
            Instance = InstanceQuery.AsNoTracking().FirstOrDefault();
            return Instance;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="id"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        public IQueryable<dynamic> BuildQuery(string[]? load = null)
        {
            object id = Id == 0 ? Guid : Id;
            InstanceQuery = Repository.CreateFindQuery(GraphType.SystemType, id);
            if (load != null) InstanceQuery = InstanceQuery.Includes(load);
            return InstanceQuery;
        }
    }
}
