using Microsoft.AspNetCore.Mvc;
using Graphene.Database.Interfaces;
using Graphene.Entities;
using Graphene.Extensions;
using Microsoft.EntityFrameworkCore;
using GrapheneTemplate.Models;
using GrapheneTemplate.Database;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using System.Linq;
using System.Reflection;
using Graphene.Graph.Interfaces;
using Graphene.Services;

namespace GrapheneTemplate.Http.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("/graphene/api/")]
    public class ApiController : Graphene.Http.Controllers.ApiController
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
