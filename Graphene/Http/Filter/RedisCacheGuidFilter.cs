using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Mvc.Filters;
using Graphene.Services;
using StackExchange.Redis;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Graphene.Cache;
using Microsoft.Extensions.Options;

namespace Graphene.Http.Filter
{
    public class RedisCacheGuidFilter : IResultFilter
    {
        private IConnectionMultiplexer _multiplexer { get; set; }
        
        private readonly JsonSerializerSettings _jsonSerializerSettings;

        public RedisCacheGuidFilter (IConnectionMultiplexer multiplexer, IEntityContext ec, IOptions<MvcNewtonsoftJsonOptions> jsonOptions)
        {
            _multiplexer = multiplexer;
            _jsonSerializerSettings = jsonOptions.Value.SerializerSettings;
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            // Do something after the result executes.
            var result = (ObjectResult) context.Result;
            if (result.StatusCode > 300) return;
            string input = JsonConvert.SerializeObject(result.Value, Formatting.Indented, _jsonSerializerSettings);
            string output = new RedisGuidCache(_multiplexer).ReplaceIdsWithGuids(input);
            result.Value = JObject.Parse(output);
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
        }
    }
}