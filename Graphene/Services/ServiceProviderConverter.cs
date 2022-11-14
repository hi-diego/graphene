using Graphene.Database.Interfaces;
using Newtonsoft.Json;

namespace Graphene.Services
{
    /// <summary>
    /// This isn't a real converter. It only exists as a hack to expose
    /// IServiceProvider on the JsonSerializerOptions.
    /// </summary>
    public class ServiceProviderConverter :
        JsonConverter,
        IServiceProvider
    {
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContextAccessor;
        //private readonly IGrapheneDatabaseContext _db;
        private readonly IServiceProvider _serviceProvider;

        public ServiceProviderConverter(
            Microsoft.AspNetCore.Http.IHttpContextAccessor httpContextAccessor,
            //IGrapheneDatabaseContext db,
            IServiceProvider serviceProvider)
        {
            _httpContextAccessor = httpContextAccessor;
            //_db = db;
            _serviceProvider = serviceProvider;
        }

        public object? GetService(Type serviceType)
        {
            // Use the request services, if available, to be able to resolve
            // scoped services.
            // If there isn't a current HttpContext, just use the root service
            // provider.
            var services = _httpContextAccessor.HttpContext?.RequestServices
                ?? _serviceProvider;
            return services.GetService(serviceType);
        }
        public override bool CanConvert(Type typeToConvert) => false;
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            throw new NotSupportedException();
        }
        public override object ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            throw new NotSupportedException();
        }
    }
}
