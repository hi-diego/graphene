using Graphene.Database.Interfaces;
using Graphene.Entities;
using Graphene.Entities.Interfaces;
using Graphene.Extensions;
using Graphene.Graph.Interfaces;
using Graphene.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Graphene.Http.Filter
{
    public class AuthorizationFilter : IAuthorizationFilter
    {
        /// <summary>
        /// 
        /// </summary>
        private IEntityContext _entityContext { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityContext"></param>
        public AuthorizationFilter(IEntityContext entityContext)
        {
            _entityContext = entityContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            context.Result = _entityContext.DeconstructAction(context);
            if (context.Result != null) return;
            IAuthorizator? authorizator = _entityContext.GraphType.Authorizator ?? Authorizator.GetFromContext(_entityContext);
            bool isAuthorized = authorizator.IsAuthorized(_entityContext).GetAwaiter().GetResult();
            if (!isAuthorized) context.Result = new UnauthorizedResult();
        }
    }
}
