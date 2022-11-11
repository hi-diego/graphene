using Graphene.Services;
using GrapheneTemplate.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace GrapheneDevelopmentTemplate.Controllers
{
    public class EntityDevelopmentController : EntityController
    {
        public EntityDevelopmentController(IEntityContext entityContext) : base(entityContext)
        {
        }
    }
}