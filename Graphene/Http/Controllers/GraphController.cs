using Graphene.Database;
using Graphene.Database.Interfaces;
using Graphene.Graph;
using Graphene.Graph.Interfaces;
using Graphene.Http.Exceptions;
using Graphene.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Graphene.Http.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    //[ApiController]
    //[Route("[controller]")]
    public abstract class GraphController : ControllerBase
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
        //[HttpPost]
        //[Authorize]
        //public ActionResult Graph([FromBody] object request)
        //{
            // User user = ApiController.GetUser(User);
            //Entity entity = request.ToObject<Entity>();
            //GraphType graphType = EntityRepository.Graph.Find(entity._Entity);
            //if (graphType == null) return NotFound();
            //dynamic instance = request.ToObject(graphType.SystemType);
            //if (!TryValidateModel(instance, graphType.PascalName)) return BadRequest(ModelState);
            //try { return Ok(EntityRepository.TrackGraph(instance, null)); }
            //catch (Exception e) { return BadRequest(e.Message); }
        //}
    }
}
