using Graphene.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphene.Http.Binders
{
    public class EntityRequest : ModelBinderAttribute
    {
        public EntityRequest() : base (typeof(EntityRequestModelBinder))
        {
            //
        }
    }

    public class EntityRequestModelBinder : Attribute, IModelBinder
    {
        private readonly IEntityContext _context;

        public EntityRequestModelBinder(IEntityContext context)
        {
            _context = context;
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null) throw new ArgumentNullException(nameof(bindingContext));
            var request = _context.HttpRequestToJson(bindingContext.HttpContext).GetAwaiter().GetResult();
            try
            {
                var result = request.ToObject(_context.GraphType.SystemType);
                bindingContext.Result = ModelBindingResult.Success(result);
            }
            catch (JsonSerializationException e)
            {
                bindingContext.ModelState.TryAddModelError(e.Path.Replace("Id", "Uid"), e.InnerException.Message);
            }
            catch (JsonReaderException e)
            {
                bindingContext.ModelState.TryAddModelError(e.Path.Replace("Id", "Uid"), e.Message);
            }
            catch (Exception e)
            {
                bindingContext.ModelState.TryAddModelError("", e.Message);
            }
            return Task.CompletedTask;
        }
    }
}
