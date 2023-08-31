using Graphene.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Graphene.Cache;

namespace Graphene.Http.Converters
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="E"></typeparam>
    public class RedisGuidReplaceConverter<E> : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? v, JsonSerializer serializer)
        {
            int id = 0;
            Int32.TryParse((string) reader.Value, out id);
            return id;
        }

        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            writer.WriteValue(RedisGuidCache.FormatId(typeof(E).Name, value));
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
