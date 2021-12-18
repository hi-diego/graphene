using Microsoft.AspNetCore.Mvc;
using GrapheneCore.Database.Interfaces;
using GrapheneCore.Entities;
using GrapheneCore.Extensions;
using Graphene.Extensions;
using Microsoft.EntityFrameworkCore;
using Graphene.Models;
using Graphene.Database;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using System.Linq;
using System.Reflection;
using GrapheneCore.Graph.Interfaces;
using GrapheneCore.Services;

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
        public DatabaseContext Context { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseContext"></param>
        public ApiController(IGrapheneDatabaseContext context, IConfiguration configuration, IGraph graph)
            : base(context, configuration, graph)
        {
            Context = (DatabaseContext)context;
        }
    }
}
