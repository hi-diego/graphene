using Graphene.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphene.Http.Filter
{
    public class ResourceFilter: IResourceFilter
    {
        /// <summary>
        /// 
        /// </summary>
        private IEntityContext _entityContext { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityContext"></param>
        public ResourceFilter(IEntityContext entityContext)
        {
            _entityContext = entityContext;
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            // _entityContext.BuildQuery();
        }

        public void OnResourceExecuted(ResourceExecutedContext context) { }
    }
}
