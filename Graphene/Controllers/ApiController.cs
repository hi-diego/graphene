using Microsoft.AspNetCore.Mvc;
using GrapheneCore.Database.Interfaces;
using GrapheneCore.Models;
using GrapheneCore.Extensions;
using Graphene.Extensions;
using Microsoft.EntityFrameworkCore;
using Graphene.Models;
using Graphene.Database;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using System.Linq;
using System.Reflection;
using GrapheneCore.Graph.Interfaces;

namespace Graphene.Http.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("/graphene/api/")]
    public class ApiController : GrapheneCore.Http.Controllers.ApiController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseContext"></param>
        public ApiController(IGrapheneDatabaseContext dbContext, IConfiguration configuration, IGraph graph, DatabaseContext context) : base(dbContext, configuration, graph)
        {
            //
            this.Context = context;
        }

        public DatabaseContext Context { get; set; }

        public static readonly MethodInfo IncludeMethodInfo =
            typeof(EntityFrameworkQueryableExtensions)
                .GetTypeInfo()
                .GetDeclaredMethods("Include")
                .Single((MethodInfo mi) => mi.GetGenericArguments().Count() == 2 && mi.GetParameters().Any((ParameterInfo pi) => pi.Name == "navigationPropertyPath" && pi.ParameterType != typeof(string)));

        [HttpGet("/test")]
        public object testInclude([FromQuery] string include)
        {
            //string entityName = "blogs";
            //var set = DatabaseContext.GetSet<dynamic>(entityName).Where("i => i.Id == 1").Include(blog => ((Blog) blog).Posts);
            //Type modelType = DatabaseContext.ModelDictionary[entityName.DbSetName()];
            //return this.DatabaseContext. set.Include(b => b.Posts.Take(10)).FirstOrDefaultAsync();
            // : await set.AsNoTracking().Includes(load, modelType).AsNoTracking().FirstOrDefaultAsync();
            //ParameterExpression arg = Expression.Parameter(typeof(object), "b");
            //query = query.Include(Expression.Lambda<Func<dynamic, dynamic>>(e, arg));
            //query = query.Provider.CreateQuery<dynamic>(Expression.Call(null, StringIncludeMethodInfo.MakeGenericMethod(typeof(object), typeof(object)), query.Expression, Expression.Constant(include)));

            //var ee = DynamicExpressionParser.ParseLambda(
            //     Blog, Enumerable<Post>,
            //     "blog => blog.Posts.Take(10)"
            // );
            //Expression<Func<Blog, IEnumerable<Post>>> e = DynamicExpressionParser.ParseLambda(
            //     typeof(Blog), null,
            //     "blog => blog.Posts.Take(10)"
            // );

            // var include = "x => x.Posts";
            Type blogType = typeof(Blog);
            Type postType = typeof(IEnumerable<Post>);
            var methodInfo = typeof(DynamicExpressionParser)
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
            var expression = methodInfo.MakeGenericMethod(blogType, postType).Invoke(null, new object[] { new ParsingConfig() { }, true, include, new object[] { } });
            var e2 = DynamicExpressionParser.ParseLambda<Blog, IEnumerable<Post>>(new ParsingConfig() { }, true, include);
            //var de = DynamicExpressionParser.ParseLambda(typeof(IEnumerable<Model>), null, include);
            // var p = Expression.Parameter(typeof(Blog), "x");
            // Expression<Func<Blog, dynamic>> e = Expression.Lambda<Func<Blog, dynamic>>(e2, p);
            // Expression<Func<Blog, IEnumerable<Post>>> e = x => "blog.Posts.Take(10)";

            // Try to make a method in each model class that retutn an  Expression<Func<dynamic, dynamic>> 
            // Expression<Func<dynamic, dynamic>> e = x => (x as Blog).Posts.Take(10);

            // Trying to call the Extension Method mmanually
            var q = Context.GetSet("Blog");
            var qq = IncludeMethodInfo.MakeGenericMethod(typeof(Blog), typeof(IEnumerable<Post>)).Invoke(null, new object[] { q, expression });
            return qq;
            // return Context.GetSet<Blog>("Blog").Include(e2);


            // q = q.Provider.CreateQuery<Blog>(Expression.Call(IncludeMethodInfo.MakeGenericMethod(typeof(Blog), typeof(Post))));
        }
    }
}
