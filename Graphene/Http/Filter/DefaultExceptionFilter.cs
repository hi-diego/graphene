using Graphene.Http.Exceptions;
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
    public class DefaultExceptionFilter : IExceptionFilter
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void OnException(ExceptionContext context)
        {
            // if (context.Exception is Microsoft.EntityFrameworkCore.DbUpdateException)
            //    context.Result = new BadRequestObjectResult(new { Message = "You can not delete the resource because multiples: " + Regex.Match(context.Exception.InnerException.Message, "(dbo.)\\w+").Groups.First().Value.Replace("dbo.", "") +" depend on it." });
            if (context.Exception is StatusCodeException)
                context.Result = ((StatusCodeException)context.Exception).Result;
            context.ExceptionHandled = false;
        }
    }
}
