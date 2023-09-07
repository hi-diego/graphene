using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Filters;
using Graphene.Services;
using StackExchange.Redis;
using Microsoft.AspNetCore.Mvc;
using Graphene.Cache;
using Microsoft.Extensions.Options;

namespace Graphene.Http.Filter
{
    public class RedisCacheGuidFilter : IResultFilter
    {
        private IConnectionMultiplexer _multiplexer { get; set; }

        public RedisCacheGuidFilter (IConnectionMultiplexer multiplexer, IEntityContext ec)
        {
            _multiplexer = multiplexer;
        }

        public void OnResultExecuting(ResultExecutingContext context)
        {
            // Do something after the result executes.
            var result = (ObjectResult) context.Result;
            if (result.StatusCode > 300) return;
            //string input = JsonConvert.SerializeObject(result.Value, Formatting.Indented, _jsonSerializerSettings);
            //string output = new RedisGuidCache(_multiplexer).ReplaceIdsWithGuids(input);
            // result.Value = JsonNode.Parse(output);
        }

        public void OnResultExecuted(ResultExecutedContext context)
        {
        }
    }
}