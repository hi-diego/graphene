using Graphene.Database.Extensions;
using Graphene.Database.Interfaces;
using Graphene.Extensions;
using Graphene.Graph.Interfaces;
using Graphene.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;
using Microsoft.Extensions.Options;

namespace Graphene.Http.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    //[Authorize]
    public abstract class ApiController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="configuration"></param>
        public ApiController(IGrapheneDatabaseContext dbContext, IConfiguration configuration, IGraph graph, IOptions<JsonOptions> jsonOptions)
        {
            Configuration = configuration;
            DatabaseContext = dbContext;
            EntityRepository = new EntityRepository(dbContext, graph, jsonOptions);
            Graph = graph;
        }

        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public IGrapheneDatabaseContext DatabaseContext { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public EntityRepository EntityRepository { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public IGraph Graph { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="formFile"></param>
        /// <param name="entity"></param>
        /// <param name="uid"></param>
        /// <returns></returns>
        [HttpPost("files/{entity}/{uid}")]
        public async Task<IActionResult> OnPostUploadAsync(IFormFile formFile, string entity, string uid)
        {
            if (formFile.Length > 0)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", entity.DbSetName(), uid + ".jpg");
                System.IO.FileInfo file = new System.IO.FileInfo(path);
                file.Directory.Create();
                using (var stream = System.IO.File.Create(path))
                {
                    await formFile.CopyToAsync(stream);
                }
            }
            // Process uploaded files
            // Don't rely on or trust the FileName property without validation.
            return Ok(formFile);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        [NonAction]
        private static string GetSubDomain(HttpContext httpContext)
        {
            var subDomain = string.Empty;
            var host = httpContext.Request.Host.Host;
            if (!string.IsNullOrWhiteSpace(host)) subDomain = host.Split('.')[0];
            return subDomain.Trim().ToLower();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{entity}/{id}")]
        public async Task<ActionResult> Find(string entity, int id, [FromQuery] Pagination pagination)
        {
            object instance = await EntityRepository.Find(entity, id, false, pagination.Load, pagination.Include);
            if (instance == null) return NotFound();
            return Ok(instance);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{entity}/{id}")]
        public async Task<ActionResult> Delete(string entity, int id)
        {
            object instance = await EntityRepository.Find(entity, id);
            if (instance == null) return NotFound();
            return Ok(EntityRepository.Delete(instance));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("{entity}")]
        public async Task<ActionResult> Add(string entity, [FromBody] object request)
        {
            if (!Graph.Exists(DatabaseContext, ref entity)) return NotFound();
            object instance = await EntityRepository.Create(entity, request, false);
            if (!TryValidateModel(instance, entity))
                return BadRequest(ModelState);
            return Ok(await EntityRepository.Create(instance, true));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPatch("{entity}/{id}")]
        public async Task<ActionResult> Edit(string entity, int id, [FromBody] object request)
        {
            // DatabaseContext.AuthUser = GetUser();
            Entity instance = await EntityRepository.Update(request, entity, id, false);
            if (instance == null)
                return NotFound();
            if (!TryValidateModel(instance, entity))
                return BadRequest(ModelState);
            return Ok(await EntityRepository.Save(instance));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpGet("{entity}")]
        public async Task<ActionResult> Index([FromQuery] Pagination pagination, string entity)
        {
            if (!Graph.Exists(DatabaseContext, ref entity)) return NotFound();
            var set = Graph.GetSet(DatabaseContext, entity);
            Type entityType = Graphene.Graph.Graph.GetSetType(set);
            // pagination.Where += GetPermissionsWhere(entity, "Index");
            // User Permission not implemented jet so a new {} is given
            return Ok(await pagination.Paginate(set, new { }, EntityRepository.Graph, entityType));
        }
    }
}
