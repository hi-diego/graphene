using GrapheneCore.Database;
using GrapheneCore.Database.Interfaces;
using GrapheneCore.Graph;
using GrapheneCore.Graph.Interfaces;
using GrapheneCore.Http.Exceptions;
using GrapheneCore.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrapheneCore.Http.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class GraphController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        public IGrapheneDatabaseContext DatabaseContext { get; }

        /// <summary>
        /// 
        /// </summary>
        public EntityRepository EntityRepository { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseContext"></param>
        public GraphController(IGrapheneDatabaseContext databaseContext, IGraph graph)
        {
            DatabaseContext = databaseContext;
            EntityRepository = new EntityRepository(databaseContext, graph);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("schema")]
        public ActionResult Schema() => Ok(EntityRepository.Graph.Types);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public ActionResult Graph([FromBody] JObject request)
        {
            // User user = ApiController.GetUser(User);
            Entity entity = request.ToObject<Entity>();
            GraphType graphType = EntityRepository.Graph.Find(entity._Entity);
            if (graphType == null) return NotFound();
            dynamic instance = request.ToObject(graphType.SystemType);
            if (!TryValidateModel(instance, graphType.PascalName)) return BadRequest(ModelState);
            try { return Ok(EntityRepository.TrackGraph(instance, null)); }
            catch (Exception e) { return BadRequest(e.Message); }
        }
    }
}
