using Graphene.Database.Interfaces;
using Graphene.Graph.Interfaces;
using Graphene.Http.Converters;
using Graphene.Http.Filter;
using Graphene.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using StackExchange.Redis;

namespace Graphene.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static void AddGraphene<Db>(this IServiceCollection services, WebApplicationBuilder builder) where Db : class, IGrapheneDatabaseContext, IInfrastructure<IServiceProvider>, IDbContextDependencies, IDbSetCache, IDbContextPoolable, IResettableService, IDisposable, IAsyncDisposable
        {
            RegisterGraphServices<Db>(builder);
            RegisterMCVServices(builder);
            RegisterAuthenticationServices(builder);
            RegisterAuthorizationServices<Db>(builder);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        private static void RegisterAuthorizationServices<T>(WebApplicationBuilder builder) where T : class, IGrapheneDatabaseContext, IDisposable
        {
            builder.Services.AddScoped<AuthorizationService>();
            builder.Services.AddScoped<AuthorizationFilter>();
            builder.Services.AddScoped<AuthorizeActionFilter>();
            // builder.Services.AddScoped<ResourceFilter>();
            // TODO USE AuthorizationService or IAuthorizationHandler
            // builder.Services.AddSingleton<IAuthorizationHandler, MyHandler1>();
            //builder.Services.AddScoped<IAuthorizationService, AuthorizationService<T>>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public static void RegisterGraphServices<T>(WebApplicationBuilder builder) where T : class, IGrapheneDatabaseContext
        {
            // Register redis as the main IdistributedCache strategy
            builder.Services.AddStackExchangeRedisCache(options => {
                // Name the instance this will put a prexif on the REdis Keys
                // 1) "GrapheneCacheBlog-820020"
                options.Configuration = builder.Configuration.GetConnectionString("redis");
                var name = typeof(T).Name;
                // 2) "GrapheneCacheBlog-f311addd-39e8-40d5-aadd-5ba127620020"
                // Get the redis connection string from the correspondent appsettins.json
                // options.InstanceName = name + ":";
            });
            builder.Services.AddSingleton<IConnectionMultiplexer>(
                sp => ConnectionMultiplexer.Connect(ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("redis")))
            );
            // Add IGrapheneDatabaseContext and explicitly resolve it to the provided DbContext type.
            builder.Services.AddScoped<IGrapheneDatabaseContext>((IServiceProvider provider) => provider.GetService<T>());
            // Resolve IGrapheneDatabaseContext explicitly and Create the Graph Singleton to cache the Types to resolve the Api 404 Error quickly.
            builder.Services.AddSingleton<IGraph>((IServiceProvider provider) => {
                // Create an Instance of the Database Context to Acces the Dictionary and cache the System.Types
                // so we can acces them by string search instead of using reflaction all the time.
                using (var scope = provider.CreateScope()) {
                    // Get an instance of the DbContext
                    var service = scope.ServiceProvider.GetService<T>();
                    // Get an instance of the Rediscontext to cache all the GUID-ID dictionaries
                    var cache = scope.ServiceProvider.GetService<IDistributedCache>();
                    // creates and Initialize the Graph instance.
                    return new Graph.Graph(service, cache);
                }
            });
            // Add the Entity Context that will deconstruct the Request for all the CRUD operations.
            builder.Services.AddScoped<IEntityContext, EntityContext>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public static void RegisterAuthenticationServices(WebApplicationBuilder builder)
        {
            var key = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("JWT").GetValue<string>("Key"));
            builder.Services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public static void RegisterMCVServices(WebApplicationBuilder builder)
        {
            // We will need the Http contrext accesort inmultples parts of the app
            builder.Services.AddHttpContextAccessor();
            // Use MVC logic
            builder.Services.AddMvc(options =>
            {
                // Use the default DefaultExceptionFilter so we can throw StatusCodeException handly in any part of the app
                // this will handle it and return the correspondet result
                options.Filters.Add(typeof(DefaultExceptionFilter));
                options.Filters.Add(typeof(RedisCacheGuidFilter));
            });
            // TODO: add an option to use NewtonsoftJson conditionally .
            // .AddNewtonsoftJson();
            // Configure the ConfigureJsonOptions Servise so we can have the Pipeline
            // services on the Converter Json Serialization.
            builder.Services.ConfigureOptions<ConfigureJsonOptions>();
            // CORS: allow everything if we are in local Development enviroment
            builder.Services.AddCors(options => options.AddPolicy(name: "development", policy => {
                    policy.AllowAnyOrigin();
                    policy.AllowAnyHeader();
                    policy.AllowAnyMethod();
                })
            );
        }
    }
}
