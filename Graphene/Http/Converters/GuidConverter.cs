using Graphene.Entities;
using Graphene.Extensions;
using Graphene.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Text;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using Microsoft.Extensions.Configuration;

namespace Graphene.Http.Converters
{
    
    
    public class EntityGuidConverter<E> : EntityGuidConverter {
        public EntityGuidConverter (IDistributedCache redis) : base(typeof(E), redis)
        {
            //
        }
        public EntityGuidConverter (IDistributedCache redis, E instance) : base(typeof(E), redis)
        {
            //
        }
    }

    public class EntityGuidConverter {
        public Type EntityType { get; set; }
        public IDistributedCache Redis { get; set; }
        public EntityGuidConverter(Type type, IDistributedCache redis) {
            this.EntityType = type;
            this.Redis = redis;
        }
        public EntityGuidConverter(Entity instance, IDistributedCache redis) {
            this.EntityType = instance.GetType();
            this.Redis = redis;
        }

        public int GetCachedId (object? value) {
            string key = GetIdKey(value);
            string? stringId = Redis.GetString(key);
            // if (stringId == null) throw new StatusCodeException(new BadRequestObjectResult($"id {value} does not exist"));
            int cacheId = Int32.Parse(stringId ?? "0");
            return cacheId;
        }

        public Guid GetCachedGuid (object? value) {
            string key = GetIdKey(value);
            string? guidString = Redis.GetString(key);
            // if (guidString == null) throw new StatusCodeException(new BadRequestObjectResult($"uuid {value} does not exist"));
            Guid cacheGuid = new Guid(guidString ?? Guid.Empty.ToString());
            return cacheGuid;
        }

        
        public string GetIdKey (object? value) {
            return EntityType.Name + "-" + (string?) (value?.ToString());
        }

        public void CacheUuids (Entity instance) {
            string uid = (string)instance.Uid.ToString();
            string id = (string)instance.Id.ToString();
            Redis.SetString(GetIdKey(instance.Id), uid);
            Redis.SetString(GetIdKey(instance.Uid), id);
            var r = (Microsoft.Extensions.Caching.StackExchangeRedis.RedisCache) Redis;
            var s = new RedisKey[] { "A", "B" };
        }

        public static void CacheUuids (IDistributedCache cache, Entity instance) {
            var converter = new EntityGuidConverter(instance.GetType(), cache);
            converter.CacheUuids(instance);
        }
    }

    public class FetchCacheGuidConverter<E> : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? v, JsonSerializer serializer)
        {
            var value = reader.Value ?? v;
            var redis = serializer.GetServiceProvider().GetRequiredService<IDistributedCache>();
            if (redis == null || value == null) return 0;
            return new EntityGuidConverter<E>(redis).GetCachedId(value);
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            var ec = serializer.GetServiceProvider().GetRequiredService<IEntityContext>();
            var key = $"{typeof(E).Name}-{value}";
            if (!ec.RedisKeys.ContainsKey(key)) ec.RedisKeys.Add(key, null);
            writer.WriteValue(value);
        }

    }

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

        public override object? ReadJson(JsonReader reader, Type objectType, object? v, JsonSerializer serializer)
        {
            var value = reader.Value ?? v;
            var redis = serializer.GetServiceProvider().GetRequiredService<IDistributedCache>();
            if (redis == null || value == null) return 0;
            return new EntityGuidConverter<E>(redis).GetCachedId(value);
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            var redis = serializer.GetServiceProvider().GetRequiredService<IDistributedCache>() as Microsoft.Extensions.Caching.StackExchangeRedis.RedisCache;
            var _configuration = serializer.GetServiceProvider().GetRequiredService<IConfiguration>();
            var ec = serializer.GetServiceProvider().GetRequiredService<IEntityContext>();
            var key = $"{typeof(E).Name}-{value}";
            var rediskeys = ec.RedisKeys;
            var guid = rediskeys[key];
            if (guid == null) {
                ConfigurationOptions options = ConfigurationOptions.Parse(_configuration.GetConnectionString("redis"));
                ConnectionMultiplexer connection = ConnectionMultiplexer.Connect(options);
                IDatabase db = connection.GetDatabase();
                // ec.RedisKeys.Keys.ToList().ForEach(k => {
                    var r = db.HashGetAll(new RedisKey($"GrapheneCache{key}"))?.LastOrDefault().Value.ToString();
                    ec.RedisKeys.Remove(key);
                    ec.RedisKeys.Add(key, r ?? Guid.Empty.ToString());
                // });
                guid = ec.RedisKeys[key];
            }
            if (redis == null || value == null) return;
            Guid cacheGuid = guid == null ? Guid.Empty : Guid.Parse(guid);// new EntityGuidConverter<E>(redis).GetCachedGuid(value);
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
