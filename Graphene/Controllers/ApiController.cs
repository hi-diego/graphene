using Microsoft.AspNetCore.Mvc;
using GrapheneCore.Database.Interfaces;

namespace Graphene.Http.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("/graphene/api/")]
    public class ApiController : GrapheneCore.Http.Controllers.ApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseContext"></param>
        public ApiController(IGrapheneDatabaseContext dbContext, IConfiguration configuration) : base(dbContext, configuration)
        {
            //
        }
    }
}
