using Graphene.Entities;
using Graphene.Http;
using Graphene.Http.Filter;
using Graphene.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace GrapheneTemplate.Controllers
{
    // [Authorize]
    public class EntityController : Graphene.Http.Controllers.EntityController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityContext"></param>
        public EntityController(IEntityContext entityContext) : base(entityContext)
        {
            //
        }
    }
}
