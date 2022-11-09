using Graphene.Entities;
using Graphene.Entities.Interfaces;
using Graphene.Http;
using Graphene.Http.Filter;
using Graphene.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace Graphene.Http.Controllers
{
    [Route("/")]
    // [Authorize]
    [ApiController]
    [ServiceFilter(typeof(AuthorizationFilter))]
    [ServiceFilter(typeof(ResourceFilter))]
    public abstract class EntityController : ControllerBase
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
            instance.SerializeId = true;
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
        public async Task<IActionResult> Add(string entity, [FromQuery] Pagination pagination, [ModelBinder(typeof(EntityRequestModelBinder))] Entity request)
        {
            // if (!TryValidateModel(request, entity)) return BadRequest(ModelState);
            //try { var res = request.ToObject(EC.GraphType.SystemType);  }
            //catch (Exception e) { System.Console.WriteLine(e); }
             // var resource = EC.Repository.Generate(EC.GraphType.SystemType, request);
            if (!TryValidateModel(request)) return BadRequest(ModelState);
            Entity instance = (Entity)await EC.Repository.Create(request, true);
            return Ok(instance);
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
