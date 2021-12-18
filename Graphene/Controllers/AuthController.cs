using GrapheneCore.Database.Interfaces;
using GrapheneCore.Graph.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Graphene.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("/auth/")]
    public class AuthController : GrapheneCore.Http.Controllers.AuthController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="configuration"></param>
        /// <param name="graph"></param>
        public AuthController(IGrapheneDatabaseContext dbContext, IConfiguration configuration, IGraph graph) : base(dbContext, configuration, graph)
        {
        }
    }
}
