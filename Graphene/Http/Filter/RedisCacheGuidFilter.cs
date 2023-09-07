using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Filters;
using Graphene.Services;
using StackExchange.Redis;
using Microsoft.AspNetCore.Mvc;
using Graphene.Cache;
using Microsoft.Extensions.Options;
using System.Text.Json.Nodes;

namespace Graphene.Http.Filter
{
    public class RedisCacheGuidFilter : IResultFilter
    {
        private IConnectionMultiplexer _multiplexer { get; set; }
        private readonly IOptions<JsonOptions> _jsonOptions;

        public RedisCacheGuidFilter (IConnectionMultiplexer multiplexer, IEntityContext ec, IOptions<JsonOptions> jsonOptions)
        {
            _multiplexer = multiplexer;
            _jsonOptions = jsonOptions;
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            // Do something after the result executes.
            var result = (ObjectResult) context.Result;
            if (result.StatusCode > 300) return;
            string input = JsonSerializer.Serialize(result.Value, _jsonOptions.Value.JsonSerializerOptions);
            string output = new RedisGuidCache(_multiplexer).ReplaceIdsWithGuids(input);
            result.Value = JsonNode.Parse(output); // JsonSerializer.Deserialize(output, result.Value.GetType(), _jsonOptions.Value.JsonSerializerOptions);
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
        }
    }
}