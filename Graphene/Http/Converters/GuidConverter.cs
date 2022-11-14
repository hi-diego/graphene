using Graphene.Extensions;
using Graphene.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphene.Http.Converters
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="E"></typeparam>
    public class GuidConverter<E> : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? id, JsonSerializer serializer)
        {
            var redis = serializer.GetServiceProvider().GetRequiredService<IDistributedCache>();
            if (redis == null) return 0;
            string key = typeof(E).Name + "-" + (string?)reader.Value;
            int cacheId = Int32.Parse(redis.GetString(key ?? "") ?? "0");
            //var db = serializer.GetServiceProvider().GetRequiredService<IGrapheneDatabaseContext>();
            //Guid guid = new Guid((string?) reader.Value ?? Guid.Empty.ToString());
            //var instance = Graph.GetSet<Entity>(db).FirstOrDefault(i => i.Uid == guid);
            return cacheId;
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            var redis = serializer.GetServiceProvider().GetRequiredService<IDistributedCache>();
            if (redis == null) return;
            string key = typeof(E).Name + "-" + (string?)value.ToString();
            string? guid = redis.GetString(key);
            Guid cacheGuid = new Guid(guid ?? Guid.Empty.ToString());
            //var db = serializer.GetServiceProvider().GetRequiredService<IGrapheneDatabaseContext>();
            //int id = (int?)value ?? 0;
            //var instance = Graph.GetSet<Entity>(db).FirstOrDefault(i => i.Id == id);
            writer.WriteValue(cacheGuid.ToString());
        }
    }

    class ConfigureJsonOptions : IConfigureOptions<MvcNewtonsoftJsonOptions>
    {
        //private readonly IGrapheneDatabaseContext _db;
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContextAccessor;
        private readonly IServiceProvider _serviceProvider;

        public ConfigureJsonOptions(
            Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor,
            //IGrapheneDatabaseContext db,
            IServiceProvider serviceProvider)
        {
            _httpContextAccessor = httpContextAccessor;
            //_db = db;
            _serviceProvider = serviceProvider;
        }

        public void Configure(MvcNewtonsoftJsonOptions options)
        {
            var sp = new ServiceProviderConverter(_httpContextAccessor, _serviceProvider);
            JsonConvert.DefaultSettings = () => {
                var settings = new JsonSerializerSettings();
                settings.Converters.Add(sp);
                settings.DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffffffK";
                settings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                return settings;
            };
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            options.SerializerSettings.DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffffffK";
            options.SerializerSettings.Converters.Add(sp);
        }
    }
}
