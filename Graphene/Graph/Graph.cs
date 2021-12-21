using Graphene.Extensions;
using Graphene.Database.Extensions;
using Graphene.Database.Interfaces;
using Graphene.Extensions;
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

namespace Graphene.Graph
{
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
        /// <summary>
        /// All the types including the fake ones;
        /// </summary>
        public IEnumerable<GraphType> Types { get; set; } = new List<GraphType>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public Graph(IGrapheneDatabaseContext context)
        {
            Init(context);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void Init(IGrapheneDatabaseContext databaseContext)
        {
            Types = GetGraph(databaseContext);
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
            GraphType rootGraphType = Types.Single(t => t.SystemType == root);
            List<IncludeExpression> includeExpressions = new List<IncludeExpression>(); 
            IncludeExpression prevIncludeExpression = null;
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
        public static void RegisterServices<T>(WebApplicationBuilder builder) where T : class, IGrapheneDatabaseContext, IDisposable, new()
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
        private static void RegisterAuthorizationServices<T>(WebApplicationBuilder builder) where T : class, IGrapheneDatabaseContext, IDisposable, new()
        {
            builder.Services.AddScoped<AuthorizationService>();
            builder.Services.AddScoped<AuthorizeActionFilter>();
            // TODO USE AuthorizationService or IAuthorizationHandler
            // builder.Services.AddSingleton<IAuthorizationHandler, MyHandler1>();
            //builder.Services.AddScoped<IAuthorizationService, AuthorizationService<T>>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        public static void RegisterGraphServices<T>(WebApplicationBuilder builder) where T : class, IGrapheneDatabaseContext, new()
        {
            builder.Services.AddScoped<IGrapheneDatabaseContext>((IServiceProvider provider) => provider.GetService<T>());
            builder.Services.AddSingleton<IGraph>((IServiceProvider provider) => new Graph(new T()));
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
            builder.Services.AddMvc(options => {
                options.Filters.Add(typeof(DefaultExceptionFilter));
            }).AddNewtonsoftJson(opt => {
                opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                opt.SerializerSettings.DateFormatString = "yyyy-MM-ddTHH:mm:ss.fffffffK";
            });
        }
    }
}