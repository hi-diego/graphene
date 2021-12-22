using Graphene.Database.Interfaces;
using Graphene.Http.Exceptions;
using Graphene.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphene.Http.Filter
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthorizeActionFilter : Attribute, IActionFilter
    {
        /// <summary>
        /// 
        /// </summary>
        public AuthorizationService AuthorizationService { get; }
        /// <summary>
        /// 
        /// </summary>
        public AuthorizeActionFilter(AuthorizationService authorizationService)
        {
            AuthorizationService = authorizationService;
        }
        /// <summary>
        /// Authorize every action before excuting.
        /// </summary>
        /// <param name="context"></param>
        public virtual void OnActionExecuting(ActionExecutingContext context)
        {
            if (!(AuthorizationService.IsAuthorized(context)).GetAwaiter().GetResult()) throw new StatusCodeException(new UnauthorizedResult());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public virtual void OnActionExecuted(ActionExecutedContext context)
        {
            //context.Result
            // Do something after the action executes.
        }
    }
}
