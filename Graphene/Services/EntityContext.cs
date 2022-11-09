using Graphene.Database.Interfaces;
using Graphene.Entities;
using Graphene.Entities.Interfaces;
using Graphene.Extensions;
using Graphene.Graph;
using Graphene.Graph.Interfaces;
using Graphene.Http;
using Graphene.Http.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.Security.Claims;

namespace Graphene.Services
{
    /// <summary>
    /// Read the RequestJson and deconstruct it for future purpuses on the Graphene Pipeline.
    /// </summary>
    public interface IEntityContext
    {

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
        public JObject? Request { get; set; }

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
        public Task<JObject?> HttpRequestToJson(HttpContext? httpContext = null, HttpRequest? httpRequest = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="actionDescriptor"></param>
        /// <param name="graph"></param>
        /// <param name="db"></param>
        /// <returns></returns>
        public IActionResult? Settup(ActionContext actionDescriptor);

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
    }

    /// <summary>
    /// Default Implementation of IEntityContext.
    /// To Read the RequestJson and deconstruct it for future purpuses on the Graphene Pipeline.
    /// </summary>
    public class EntityContext : IEntityContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="db"></param>
        public EntityContext(IGraph graph, IGrapheneDatabaseContext db)
        {
            Graph = graph;
            DbContext = db;
            Repository = new EntityRepository(DbContext, Graph);
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
        public JObject? Request { get; set; }

        /// <summary>
        /// To Read the RequestJson and deconstruct it for future purpuses on the Graphene Pipeline.
        /// </summary>
        public object? RequestInstance { get => GraphType != null ? Request?.ToObject(GraphType.SystemType) : null; }

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
        public IGraph Graph { get; set; }

        /// <summary>
        /// Retrives the Body of the Current HTTP Request
        /// in a JSON format.
        /// </summary>
        /// <returns></returns>
        public async Task<JObject?> HttpRequestToJson(HttpContext? httpContext = null, HttpRequest? httpRequest = null)
        {
            // If the Request was already read before returns it.
            if (Request != null) return Request;
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
            Request = JObject.Parse(input);
            // Return the Stored JSON Request value.
            return Request;
        }

        public IActionResult? Settup(ActionContext actionContext)
        {
            User = Authenticable.Transform((ClaimsIdentity?)actionContext?.HttpContext.User.Identity);
            Repository = new EntityRepository(DbContext, Graph);
            var descriptor = (ControllerActionDescriptor) actionContext.ActionDescriptor;
            var actionName = descriptor.ActionName;
            // var jsonBody = await HttpRequestToJson();
            var entityName = actionContext.RouteData?.Values["entity"]?.ToString()?.DbSetName();
            if (entityName == null) return new NotFoundResult();
            var graphType = Graph.Find(entityName);
            if (graphType == null) return new NotFoundResult();
            bool? idParam = actionContext.RouteData?.Values.ContainsKey("id");
            if (idParam == true)
            {
                string? id = actionContext.RouteData?.Values["id"]?.ToString();
                int resourceId = 0;
                Int32.TryParse(id, out resourceId);
                Guid resourceGuid = new Guid();
                Guid.TryParse(id, out resourceGuid);
                bool hasGuid = !resourceGuid.Equals(new Guid());
                if (resourceId == 0 && !hasGuid) return new BadRequestResult();
                Id = resourceId;
                Guid = resourceGuid;
            }
            GraphType = graphType;
            ActionName = actionName;
            return null;
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
            InstanceQuery = BuildQuery();
            var instance = await InstanceQuery.FirstOrDefaultAsync();
            Instance = instance;
            return instance;
        }

        public Entity? FindInstance(string[]? load = null)
        {
            InstanceQuery = BuildQuery(load);
            var instance = InstanceQuery.FirstOrDefault();
            Instance = instance;
            return instance;
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
