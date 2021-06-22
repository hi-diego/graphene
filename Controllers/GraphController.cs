using GrapheneCore.Database;
using GrapheneCore.Graph;
using GrapheneCore.Http.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Graphene.Controllers
{
    /// <summary>
    /// 
    /// </summary
    public class GraphController : GraphBaseController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseContext"></param>
        public GraphController(IEntityRepository entityRepository) : base(entityRepository)
        {
            //
        }
    }
}
