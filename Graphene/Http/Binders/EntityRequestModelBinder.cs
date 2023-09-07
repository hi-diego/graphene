using Graphene.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json;
using StackExchange.Redis;
using Graphene.Cache;
using Graphene.Http.Exceptions;
using Microsoft.Extensions.Options;

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
        private readonly IOptions<JsonOptions> _jsonOptions;

        public EntityRequestModelBinder(IEntityContext context, IConnectionMultiplexer multiplexer, IOptions<JsonOptions> jsonOptions)
        {
            _context = context;
            _multiplexer = multiplexer;
            _jsonOptions = jsonOptions;
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null) throw new ArgumentNullException(nameof(bindingContext));
            var requestWithGuids = _context.HttpRequestToString(bindingContext.HttpContext).GetAwaiter().GetResult();
            if (requestWithGuids == null) throw new StatusCodeException(new BadRequestObjectResult("Empty request not allowed"));
            var cleanRequest = new RedisGuidCache(_multiplexer).ReplaceGuidsWithIds(requestWithGuids);
            try
            {
                var result = JsonSerializer.Deserialize(cleanRequest, _context.GraphType.SystemType, _jsonOptions.Value.JsonSerializerOptions);
                bindingContext.Result = ModelBindingResult.Success(result);
            }
            catch (Exception e)
            {
                bindingContext.ModelState.TryAddModelError("", e.Message);
            }
            return Task.CompletedTask;
        }
    }
}
