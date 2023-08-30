using Graphene.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System.Text.RegularExpressions;
using StackExchange.Redis;
using Newtonsoft.Json.Linq;
using Graphene.Cache;
using Graphene.Http.Exceptions;

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
        private readonly IConnectionMultiplexer _multiplexer;

        public EntityRequestModelBinder(IEntityContext context, IConnectionMultiplexer multiplexer)
        {
            _context = context;
            _multiplexer = multiplexer;
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null) throw new ArgumentNullException(nameof(bindingContext));
            var requestWithGuids = _context.HttpRequestToString(bindingContext.HttpContext).GetAwaiter().GetResult();
            if (requestWithGuids == null) throw new StatusCodeException(new BadRequestObjectResult("Empty request not allowed"));
            var cleanRequest = new RedisGuidCache(_multiplexer).ReplaceGuidsWithIds(requestWithGuids);
            var request = JObject.Parse(cleanRequest);
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
