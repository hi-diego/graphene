﻿using Graphene.Extensions;
using Graphene.Database.Extensions;
using Graphene.Database.Interfaces;
using Graphene.Graph.Interfaces;
using Graphene.Http.Filter;
using Graphene.Entities;
using Graphene.Entities.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;
using Graphene.Services;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace Graphene.Graph
{
    /// <summary>
    /// 
    /// </summary>
    public class UID
    {
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<int, Guid> Guid { get; set; } = new Dictionary<int, Guid>();

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<Guid, int> Id { get; set; } = new Dictionary<Guid, int>();

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<int, bool> IdTest { get; set; } = new Dictionary<int, bool>();

        /// <summary>
        /// 
        /// </summary>
        public int[] ArrayTest { get; set; } = new int[1];

        /// <summary>
        /// 
        /// </summary>
        //public Guid[] ArrayGuidTest { get; set; } = new Guid[1000000];

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Guid GetGuid(int id)
        {
            Guid.TryGetValue(id, out var guid);
            return guid;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int GetId(Guid guid)
        {
            if (Id.TryGetValue(guid, out var id)) return id;
            return 0;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class Graph : IGraph
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public async Task<IAuthenticable?> GetIAuthenticable(IGrapheneDatabaseContext dbContext, string email, string[] includes)
            =>  await GetSet<IAuthenticable>(dbContext)
                .Where(a => a.Identifier == email)
                .Includes(includes)
                .FirstOrDefaultAsync();
        /// <summary>
        /// 
        /// </summary>
        public static readonly MethodInfo ThenIncludeMethodInfo =
            typeof(EntityFrameworkQueryableExtensions)
                .GetTypeInfo()
                .GetDeclaredMethods("ThenInclude")
                .Last();
        /// <summary>
        /// 
        /// </summary>
        public static readonly MethodInfo ThenIncludeMethodInfoMultiple =
            typeof(EntityFrameworkQueryableExtensions)
                .GetTypeInfo()
                .GetDeclaredMethods("ThenInclude")
                .First();
        /// <summary>
        /// 
        /// </summary>
        public static readonly MethodInfo IncludeMethodInfo =
            typeof(EntityFrameworkQueryableExtensions)
                .GetTypeInfo()
                .GetDeclaredMethods("Include")
                .Single((MethodInfo mi) => mi.GetGenericArguments().Count() == 2
                    && mi.GetParameters().Any((ParameterInfo pi) => pi.Name == "navigationPropertyPath"
                        && pi.ParameterType != typeof(string)
                    )
                );
        /// <summary>
        /// 
        /// </summary>
        public static readonly MethodInfo DynamicExpressionMethodInfo =
            typeof(DynamicExpressionParser)
                .GetTypeInfo()
                .GetDeclaredMethods("ParseLambda")
                .Single((MethodInfo mi) =>
                    mi.GetParameters().Count() == 4 &&
                    mi.GetGenericArguments().Count() == 2 &&
                    mi.GetParameters().Any((ParameterInfo pi) =>
                        pi.Name == "parsingConfig" &&
                        pi.ParameterType == typeof(ParsingConfig)
                    )
                );
        private IDistributedCache _cahce;

        /// <summary>
        /// All the types including the fake ones;
        /// </summary>
        public IEnumerable<GraphType> Types { get; set; } = new List<GraphType>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public Graph(IGrapheneDatabaseContext context, IDistributedCache cache)
        {
            _cahce = cache;
            Init(context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void Init(IGrapheneDatabaseContext databaseContext)
        {
            Types = GetGraph(databaseContext);
            //databaseContext.SetDictionary.ToList().ForEach(kv =>
            //{
            //    var intances = kv.Value().AsNoTracking().ToList();
            //    intances.ForEach(e => {
            //        dynamic instance = e;
            //        string idKey = instance._Entity + "-" + instance.Id.ToString();
            //        string guidKey = instance._Entity + "-" + instance.Uid.ToString();
            //        string uid = (string)instance.Uid.ToString();
            //        string id = (string)instance.Id.ToString();
            //        _cahce.SetString(idKey, uid);
            //        _cahce.SetString(guidKey, id);
            //    });
            //});
        }

        /// <summary>
        /// Converts every Entity from the project namespace (classes that implements IEntity)
        /// to IEntityDescriptor and concatenates the Custom Entities descriptors from the database
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<GraphType> GetGraph(IGrapheneDatabaseContext context)
            => context.SetDictionary.ToList()
                    //.Where(m => !m.Value.IsAbstract && m.Value.IsSubclassOf(typeof(Entity)))
                    .Where(m => Graph.GetSetType(m.Value()).IsSubclassOf(typeof(Entity)))
                    .Select(m => new GraphType(Graph.GetSetType(m.Value()))) // until Rules are implemented, context.Rule.Where(r => r.Entity == m.Key).ToList()))
                    .ToList();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public GraphType? Find(string name)
        {
            return Types.FirstOrDefault(t => t.PascalName == name.DbSetName());
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public GraphType? Find<T>() => Types.FirstOrDefault(t => typeof(T).IsAssignableFrom(t.SystemType));
        /// <summary>
        /// Returns the _Entity of the relation that corresponds to the given path in the given type
        /// </summary>
        /// <param name="include"></param>
        /// <returns></returns>
        public Type GetRelationType(Type root, string path)
        {
            return GetRelationGraphType(root, path);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Type GetRelationGraphType(Type root, string path)
        {
            GraphType rootGraphType = Types.Single(t => t.SystemType == root);
            GraphType graphType = rootGraphType;
            GraphType prevGraphType = rootGraphType;
            foreach (var piece in path.Split("."))
            {
                prevGraphType = graphType;
                graphType = graphType.Fields.Single(f => f.PascalName == piece.UcFirst());
            }
            return graphType.SystemType;
        }

        /// <summary>
        /// Returns the _Entity of the relation that corresponds to the given path in the given type
        /// </summary>
        /// <param name="include"></param>
        /// <returns></returns>
        public IEnumerable<IncludeExpression> GetIncludeExpressions(Type root, string[] includes)
        {
            GraphType? rootGraphType = Types.FirstOrDefault(t => t.SystemType == root);
            List<IncludeExpression> includeExpressions = new List<IncludeExpression>();
            if (rootGraphType == null) return includeExpressions;
            IncludeExpression prevIncludeExpression = null;
            if (includes == null) return includeExpressions;
            foreach (string i in includes) {
                IncludeExpression includeExpression = new IncludeExpression(rootGraphType, i, this, prevIncludeExpression);
                prevIncludeExpression = includeExpression;
                includeExpressions.Add(includeExpression);
            }
            return includeExpressions;
        }
        /// <summary>
        // Set The dynamic includes to the given
        /// </summary>
        /// <param name="set"></param>
        /// <param name="entityType"></param>
        /// <param name="load"></param>
        /// <returns></returns>
        public IQueryable<dynamic> SetIncludes(IQueryable<dynamic> set, Type rootentityType, string[] load)
        {
            // Deconstruct the request query params from "&load[]=blog=>blog.Post.Take(10)" to the
            // correspondent IncludeExpression structure using the Graph.
            IEnumerable<IncludeExpression> iExpressions = GetIncludeExpressions(rootentityType, load);
            // Add each Include to the IQueryable<dynamic> DbSet
            foreach (IncludeExpression iExpression in iExpressions)
            {
                set = SetInclude(set, iExpression);
            }
            return set;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="set"></param>
        /// <param name="iExpression"></param>
        /// <returns></returns>
        public IQueryable<dynamic> SetInclude(IQueryable<dynamic> set, IncludeExpression iExpression)
        {
            // Get the correspondent SystemTypes for the "Include/ThenInclude" reflection/dynamic call from the IncludeExpression
            // thetypes are called TEntity, TPreviousProperty, TProperty to follow the EntityFrameworkQueryableExtensions declare the Generic arguments for the method Include and ThenInclude.
            // EntityFrameworkQueryableExtensions.IIncludableQueryable<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(this IIncludableQueryable<TEntity, IEnumerable<TPreviousProperty>> source, Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath) where TEntity : class
            Type TEntity = iExpression.Root.SystemType;
            // If the prev property is a IEnumerable<> we take the Generic argument
            // for example for .Include(blog => blog.Posts.Take(10)).ThenInclude(posts => posts.Author), the type of blog.posts is IEnumerable<Post> so we take only the Post type.
            Type TPreviousProperty = iExpression.IsPrevMultiple
                ? iExpression?.PreviousInclude?.Relation?.SystemType.GetGenericArguments().First()
                : iExpression?.PreviousInclude?.Relation?.SystemType;
            Type TProperty = iExpression?.Relation?.SystemType;
            // Create the Lambda Expression dynamicly from "&load[]=blog=>blog.Post.Take(10)" include string
            // The Expression Generic arguments Arguments differ from Include and thenInclude, thats whay the ternary is there
            var expression = iExpression.PreviousInclude == null
                // Generic arguments for Include:
                ? DynamicExpressionMethodInfo.MakeGenericMethod(TEntity, TProperty).Invoke(null, new object[] { new ParsingConfig() { }, true, iExpression.IncludeString, new object[] { } })
                // Generic arguments for ThenInclude:
                : iExpression.IsPrevMultiple
                    ? DynamicExpressionMethodInfo.MakeGenericMethod(TPreviousProperty, TProperty).Invoke(null, new object[] { new ParsingConfig() { }, true, iExpression.IncludeString, new object[] { } })
                    : DynamicExpressionMethodInfo.MakeGenericMethod(TPreviousProperty, TProperty).Invoke(null, new object[] { new ParsingConfig() { }, true, iExpression.IncludeString, new object[] { } });
            // Select the correspondent IncludeMethod/ThenIncludeMethod/ThenIncludeMethodMultiple depending on the iExpression.IsPrevMultiple
            MethodInfo includeMethod = iExpression.IsThenInclude
                ? iExpression.IsPrevMultiple
                    ? ThenIncludeMethodInfoMultiple.MakeGenericMethod(TEntity, TPreviousProperty, TProperty)
                    : ThenIncludeMethodInfo.MakeGenericMethod(TEntity, TPreviousProperty, TProperty)
                : IncludeMethodInfo.MakeGenericMethod(TEntity, TProperty);
            // Juxtapoze the query with the new included query, executing the static includeMethod from the EntityFrameworkQueryableExtensions.
            return (IQueryable<dynamic>) includeMethod.Invoke(null, new object[] { set, expression });
        }

        /// <summary>
        /// Verify if the resource Exist in the dictionary.
        /// </summary>
        /// <param name="entityName"></param>
        /// <returns></returns>
        public bool Exists(IGrapheneDatabaseContext dbContext, ref string entityName)
        {
            entityName = entityName.DbSetName();
            return Find(entityName) != null;
        }

        /// <summary>
        /// Verify if the resource Exist in the dictionary.
        /// </summary>
        /// <param name="entityName"></param>
        /// <returns></returns>
        public bool Exists<T>(IGrapheneDatabaseContext dbContext)
        {
            return Find<T>() != null;
        }

        /// <summary>
        /// Verify if the resource Exist in the dictionary.
        /// </summary>
        /// <param name="entityName"></param>
        /// <returns></returns>
        public bool ItExists(IGrapheneDatabaseContext dbContext, string entityName)
            => Find(entityName) != null;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public static IQueryable<T> GetSet<T>(IGrapheneDatabaseContext dbContext) 
            => (IQueryable<T>) dbContext.SetDictionary[typeof(T)]();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public IQueryable<dynamic> GetSet(IGrapheneDatabaseContext dbContext, GraphType graphType)
            => GetSet(dbContext, graphType.SystemType);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public IQueryable<dynamic> GetSet(IGrapheneDatabaseContext dbContext, Type type)
            => dbContext.SetDictionary[type]();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public  IQueryable<T> GetSet<T>(IGrapheneDatabaseContext dbContext, string name)
            => (IQueryable<T>) dbContext.SetDictionary[Find(name).SystemType]();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public IQueryable<dynamic> GetSet(IGrapheneDatabaseContext dbContext, string name)
            => dbContext.SetDictionary[Find(name).SystemType]();

        /// <summary>
        /// Verify if the resource Exist in the dictionary.
        /// </summary>
        /// <param name="entityName"></param>
        /// <returns></returns>
        public static Type GetSetType(IQueryable<dynamic> set) => set.GetType().GetGenericArguments().First();

        /// <summary>
        /// Verify if the resource Exist in the dictionary.
        /// </summary>
        /// <param name="entityName"></param>
        /// <returns></returns>
        public static string GetSetTypeName(IQueryable<dynamic> set) => GetSetType(set).Name;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rootEntity"></param>
        public static void SaveAnnotatedGraph(IGrapheneDatabaseContext dbContext, object rootEntity) => GrapheneDatabaseContextExtensions.SaveAnnotatedGraph(dbContext, rootEntity);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="services"></param>
        public static void RegisterServices<T>(WebApplicationBuilder builder) where T : class, IGrapheneDatabaseContext, IDisposable
        {
            RegisterGraphServices<T>(builder);
            RegisterMCVServices(builder);
            RegisterAuthenticationServices(builder);
            RegisterAuthorizationServices<T>(builder);
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
            builder.Services.AddScoped<ResourceFilter>();
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
            builder.Services.AddScoped<IGrapheneDatabaseContext>((IServiceProvider provider) => provider.GetService<T>());
            builder.Services.AddSingleton<IGraph>((IServiceProvider provider) => {
                using (var scope = provider.CreateScope())
                {
                    var service = scope.ServiceProvider.GetService<T>();
                    var cache = scope.ServiceProvider.GetService<IDistributedCache>();
                    return new Graph(service, cache);
                }
            });
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
            builder.Services.AddStackExchangeRedisCache(options => {
                options.InstanceName = "GrapheneCache";
                options.Configuration = builder.Configuration.GetConnectionString("redis");
            });
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddMvc(options =>
            {
                options.Filters.Add(typeof(DefaultExceptionFilter));
            }).AddNewtonsoftJson();
            builder.Services.ConfigureOptions<ConfigureJsonOptions>();
        }

    }
    public class KeyConverter<E> : JsonConverter
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
            string key = typeof(E).Name + "-" + (string?) value.ToString();
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
        private readonly IGrapheneDatabaseContext _db;
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
            JsonConvert.DefaultSettings = () =>
            {
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


    /// <summary>
    /// This isn't a real converter. It only exists as a hack to expose
    /// IServiceProvider on the JsonSerializerOptions.
    /// </summary>
    public class ServiceProviderConverter :
        JsonConverter,
        IServiceProvider
    {
        private readonly Microsoft.AspNetCore.Http.IHttpContextAccessor _httpContextAccessor;
        private readonly IGrapheneDatabaseContext _db;
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
    public static class Extensions
    {
        public static IServiceProvider GetServiceProvider(this JsonSerializer serializer)
        {
            return serializer.Converters.OfType<IServiceProvider>().FirstOrDefault()
                ?? throw new InvalidOperationException(
                    "No service provider found in JSON converters");
        }
    }
}