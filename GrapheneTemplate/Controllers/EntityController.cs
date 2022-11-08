using Graphene.Database.Interfaces;
using Graphene.Entities;
using Graphene.Graph.Interfaces;
using Graphene.Http;
using Graphene.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace GrapheneTemplate.Controllers
{
    [Route("/")]
    [ApiController]
    public class EntityController : Graphene.Http.Controllers.EntityController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="db"></param>
        public EntityController (IGraph graph, IGrapheneDatabaseContext db)
        {
            EntityContext = new EntityContext(graph, db);
        }

        /// <summary>
        /// 
        /// </summary>
        public EntityContext EntityContext { get; }

        /// <summary>
        /// 
        /// </summary>
        //[HttpPatch("/{entity}/{id?}")]
        //[HttpDelete("/{entity}/{id?}")]
        //[HttpPost("/{entity}")]
        //public async Task<IActionResult?> EntityEndpoint(string entity, string? id = null)
        //{
        //    var result = await EntityContext.Settup(this.ControllerContext);
        //    if (result != null) return result;
        //    return Ok(new { entity, id });
        //}


        /// <summary>
        /// 
        /// </summary>
        [HttpGet("/{entity}")]
        public async Task<IActionResult?> Index(string entity, [FromQuery] Pagination pagination)
        {
            var result = await EntityContext.Settup(this.ControllerContext);
            if (result != null) return result;
            var set = EntityContext.Graph.GetSet(EntityContext.DbContext, entity);
            return Ok(await pagination.Paginate(set, new { }, EntityContext.EntityRepository.Graph, EntityContext.GraphType.SystemType));
        }

        /// <summary>
        /// 
        /// </summary>
        [HttpGet("/{entity}/{id}")]
        public async Task<IActionResult> Find(string entity, string id, [FromQuery] Pagination pagination)
        {
            var result = await EntityContext.Settup(this.ControllerContext);
            if (result != null) return result;
            var resource = await FindResource(EntityContext.ResourceId, EntityContext.ResourceGuid, pagination);
            if (resource == null) return new NotFoundResult();
            return Ok(resource);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="id"></param>
        /// <param name="pagination"></param>
        /// <returns></returns>
        [NonAction]
        public async Task<Entity> FindResource(int id, Guid uid, Pagination pagination)
        {
            var resource = id == 0
                ? await EntityContext.EntityRepository.Find(EntityContext.GraphType.Name, uid, false, pagination.Load, pagination.Include)
                : await EntityContext.EntityRepository.Find(EntityContext.GraphType.Name, id, false, pagination.Load, pagination.Include);
            return resource;
        }

        /// <summary>
        /// 
        /// </summary>
        [HttpPatch("/{entity}/{id}")]
        public async Task<IActionResult> Update(string entity, string id, [FromQuery] Pagination pagination, [FromBody]JObject request)
        {
            var result = await EntityContext.Settup(this.ControllerContext);
            if (result != null) return result;
            var resource = await FindResource(EntityContext.ResourceId, EntityContext.ResourceGuid, pagination);
            if (resource == null) return new NotFoundResult();
            if (!TryValidateModel(resource, entity)) return BadRequest(ModelState);
            var resourceUpdated = await EntityContext.EntityRepository.Edit(resource, request);
            return Ok(resourceUpdated);
        }

        /// <summary>
        /// 
        /// </summary>
        [HttpPost("/{entity}")]
        public async Task<IActionResult> Add(string entity, [FromQuery] Pagination pagination, [FromBody] JObject request)
        {
            var result = await EntityContext.Settup(this.ControllerContext);
            if (result != null) return result;
            if (!TryValidateModel(request, entity)) return BadRequest(ModelState);
            var resource = EntityContext.EntityRepository.Generate(EntityContext.GraphType.SystemType, request);
            if (!TryValidateModel(resource, entity)) return BadRequest(ModelState);
            return Ok(await EntityContext.EntityRepository.Create(resource, true));
        }

        /// <summary>
        /// 
        /// </summary>
        [HttpDelete("/{entity}/{id}")]
        public async Task<IActionResult> Delete(string entity, string id, [FromQuery] Pagination pagination)
        {
            var result = await EntityContext.Settup(this.ControllerContext);
            if (result != null) return result;
            var resource = await FindResource(EntityContext.ResourceId, EntityContext.ResourceGuid, pagination);
            if (resource == null) return new NotFoundResult();
            return Ok(await EntityContext.EntityRepository.Delete(resource));
        }
    }
}
