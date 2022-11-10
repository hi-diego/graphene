using Graphene.Entities.Interfaces;
using Graphene.Http.Exceptions;
using Graphene.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphene.Http.Binders
{
    public class FromEntityContext : ModelBinderAttribute
    {
        public FromEntityContext() : base(typeof(EntityModelBinder))
        {
            //
        }
    }
    public class EntityModelBinder : IModelBinder
    {
        private readonly IEntityContext _context;

        public EntityModelBinder(IEntityContext context)
        {
            _context = context;
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null) throw new ArgumentNullException(nameof(bindingContext));
            var load = bindingContext.ValueProvider.GetValue("load[]").Values.ToArray();
            IEntity? instance = null;
            try { instance = _context.FindInstance(load); }
            catch (System.InvalidOperationException e) { throw new StatusCodeException(new BadRequestObjectResult(new { error = e.Message })); }
            if (instance == null) throw new StatusCodeException(new NotFoundResult());
            bindingContext.Result = ModelBindingResult.Success(instance);
            return Task.CompletedTask;
        }
    }
}