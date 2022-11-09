using Graphene.Database.Interfaces;
using Graphene.Entities;
using Graphene.Extensions;
using Graphene.Graph.Interfaces;
using Graphene.Http;
using Graphene.Http.Filter;
using Graphene.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.DynamicLinq;
using Newtonsoft.Json.Linq;

namespace GrapheneTemplate.Controllers
{
    [Route("/")]
    // [Authorize]
    [ApiController]
    [ServiceFilter(typeof(AuthorizationFilter))]
    [ServiceFilter(typeof(ResourceFilter))]
    public class EntityController : Graphene.Http.Controllers.EntityController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="db"></param>
        public EntityController(IEntityContext entityContext)
        {
            EC = entityContext;
        }

        /// <summary>
        /// 
        /// </summary>
        public IEntityContext EC { get; }


        /// <summary>
        /// 
        /// </summary>
        [HttpGet("/{entity}")]
        public async Task<IActionResult?> Index(string entity, [FromQuery] Pagination pagination)
        {
            var set = EC.Graph.GetSet(EC.DbContext, entity);
            return Ok(await pagination.Paginate(set, new { }, EC.Repository.Graph, EC.GraphType.SystemType));
        }

        /// <summary>
        /// 
        /// </summary>
        [HttpGet("/{entity}/{id}")]
        public async Task<IActionResult> Find([FromQuery(Name = "load[]")] string[]? load = null)
        {
            var instance = await EC.FindInstanceAsync(load);
            if (instance == null) return new NotFoundResult();
            return Ok(instance);
        }

        ///// <summary>
        ///// 
        ///// </summary>
        //[HttpPatch("/{entity}/{id}")]
        //public async Task<IActionResult> Update(string entity, string id, [FromQuery] Pagination pagination, [FromBody]JObject request)
        //{
        //    var resource = await FindInstance(EC.Id, EC.Guid, pagination);
        //    if (resource == null) return new NotFoundResult();
        //    if (!TryValidateModel(resource, entity)) return BadRequest(ModelState);
        //    var resourceUpdated = await EC.Repository.Edit(resource, request);
        //    return Ok(resourceUpdated);
        //}

        /// <summary>
        /// 
        /// </summary>
        [HttpPost("/{entity}")]
        public async Task<IActionResult> Add(string entity, [FromQuery] Pagination pagination, [FromBody] JObject request)
        {
            if (!TryValidateModel(request, entity)) return BadRequest(ModelState);
            var resource = EC.Repository.Generate(EC.GraphType.SystemType, request);
            if (!TryValidateModel(resource, entity)) return BadRequest(ModelState);
            return Ok(await EC.Repository.Create(resource, true));
        }

        ///// <summary>
        ///// 
        ///// </summary>
        //[HttpDelete("/{entity}/{id}")]
        //public async Task<IActionResult> Delete(string entity, string id, [FromQuery] Pagination pagination)
        //{
        //    var resource = await FindInstance(EC.Id, EC.Guid, pagination);
        //    if (resource == null) return new NotFoundResult();
        //    return Ok(await EC.Repository.Delete(resource));
        //}
    }
}
