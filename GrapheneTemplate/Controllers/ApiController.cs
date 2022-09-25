using Microsoft.AspNetCore.Mvc;
using Graphene.Database.Interfaces;
using Graphene.Entities;
using Graphene.Extensions;
using Microsoft.EntityFrameworkCore;
using GrapheneTemplate.Database.Models;
using GrapheneTemplate.Database;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using System.Linq;
using System.Reflection;
using Graphene.Graph.Interfaces;
using Graphene.Services;
using Microsoft.AspNetCore.Authorization;
using Graphene.Http.Filter;
using Microsoft.AspNetCore.Mvc.Filters;

namespace GrapheneTemplate.Http.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    //[ApiController]
    //[Authorize]
    //[ServiceFilter(typeof(AuthorizeActionFilter))]
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
