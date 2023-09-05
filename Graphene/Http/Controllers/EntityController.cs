using Graphene.Entities;
using Graphene.Http.Binders;
using Graphene.Http.Filter;
using Graphene.Services;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;

namespace Graphene.Http.Controllers
{
    [Route("/")]
    // [Authorize]
    [ApiController]
    [ServiceFilter(typeof(AuthorizationFilter))]
    public abstract class EntityController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="graph"></param>
        /// <param name="db"></param>
        public EntityController(IEntityContext entityContext, IOptions<JsonOptions> options = null)
        {
            _options = options;
            EC = entityContext;
        }

        private IOptions<JsonOptions> _options;

        /// <summary>
        /// 
        /// </summary>
        public IEntityContext EC { get; }

        /// <summary>
        /// 
        /// </summary>
        [HttpGet("/x/{id}")]
        public IActionResult FindResource([FindEntity] Entity instance)
        {
            return Ok(instance);
        }


        /// <summary>
        /// 
        /// </summary>
        [HttpGet("/{entity}")]
        public async Task<IActionResult?> Index(string entity, [FromQuery] Pagination pagination)
        {
            var set = EC.Graph.GetSet(EC.DbContext, entity);
            var result = await pagination.Paginate(set, new { }, EC.Repository.Graph, EC.GraphType.SystemType);
            return Ok(result);
        }

        /// <summary>
        /// 
        /// </summary>
        [HttpGet("/{entity}/{id}")]
        public IActionResult Find([FindEntity] Entity instance)
        {
            return Ok(instance);
        }

        /// <summary>
        /// 
        /// </summary>
        [HttpPatch("/{entity}/{id}")]
        public async Task<IActionResult> Update([FindEntity] Entity resource, [FromBody] JObject request)
        {
            if (!TryValidateModel(request)) return BadRequest(ModelState);
            var resourceUpdated = await EC.Repository.Edit(resource, request);
            return Ok(resourceUpdated);
        }

        /// <summary>
        /// Validates and Store the requested instance
        /// It will trigger the entire Graphene Entity Pipeline.
        /// </summary>
        /// <param name="request">The Entity instance created by the model binder<m</param>
        /// <returns></returns>
        [HttpPost("/{entity}")]
        public async Task<IActionResult> Add([EntityRequest] Entity request)
        {
            // Validate the given model against the DataAnotation validation Attributes.
            // return the error bag in fvalidation fails.
            if (!TryValidateModel(request)) return BadRequest(ModelState);
            // Persist the given request instance
            var instance = (Entity) await EC.Repository.Create(request);
            // return the new created Instance with 201 status code and the URL link on where to find the new created resource.
            return Created($"{HttpContext.Request.GetEncodedUrl()}/{instance.Uuid}", instance);
        }

        /// <summary>
        /// 
        /// </summary>
        [HttpDelete("/{entity}/{id}")]
        public async Task<IActionResult> Delete([FindEntity] Entity instance)
        {
           return Ok(await EC.Repository.Delete(instance));
        }
    }
}
