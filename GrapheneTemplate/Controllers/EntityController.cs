using Graphene.Entities;
using Graphene.Http;
using Graphene.Http.Filter;
using Graphene.Services;
using GrapheneTemplate.Database.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Text;

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
