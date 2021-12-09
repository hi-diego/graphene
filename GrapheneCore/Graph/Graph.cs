﻿using GrapheneCore.Database.Interfaces;
using GrapheneCore.Extensions;
using GrapheneCore.Graph.Interfaces;
using GrapheneCore.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Reflection;

namespace GrapheneCore.Graph
{
    /// <summary>
    /// 
    /// </summary>
    public class Graph : IGraph
    {
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
        /// Converts every Model from the project namespace (classes that implements IEntity)
        /// to IEntityDescriptor and concatenates the Custom Entities descriptors from the database
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<GraphType> GetGraph(IGrapheneDatabaseContext context)
            => context.ModelDictionary.ToList()
                    .Where(m => !m.Value.IsAbstract && m.Value.IsSubclassOf(typeof(Model)))
                    .Select(m => new GraphType(m.Value)) // until Rules are implemented, context.Rule.Where(r => r.Entity == m.Key).ToList()))
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
        /// Returns the Type of the relation that corresponds to the given path in the given type
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
            //return prevGraphType.Multiple
            //    ? typeof(IEnumerable<>).MakeGenericType(graphType.SystemType)
            //    : graphType.SystemType;
        }

        /// <summary>
        /// Returns the Type of the relation that corresponds to the given path in the given type
        /// </summary>
        /// <param name="include"></param>
        /// <returns></returns>
        public IEnumerable<IncludeExpression> GetIncludeExpressions(Type root, string[] includes)
        {
            GraphType rootGraphType = Types.Single(t => t.SystemType == root);
            List<IncludeExpression> includeExpressions = new List<IncludeExpression>(); //  = includes.Select(i => new IncludeExpression(rootGraphType, i, this));
            IncludeExpression prevIncludeExpression = null;
            foreach (string i in includes) {
                IncludeExpression includeExpression = new IncludeExpression(rootGraphType, i, this, prevIncludeExpression);
                prevIncludeExpression = includeExpression;
                includeExpressions.Add(includeExpression);
            }
            //IEnumerable<IncludeExpression> includeExpressions = includes.Select(i => new IncludeExpression(rootGraphType, i, this));
            return includeExpressions;
        }
        /// <summary>
        // Set The dynamic includes to the given
        /// </summary>
        /// <param name="set"></param>
        /// <param name="modelType"></param>
        /// <param name="load"></param>
        /// <returns></returns>
        public IQueryable<dynamic> SetIncludes(IQueryable<dynamic> set, Type rootModelType, string[] load)
        {
            // Deconstruct the request query params from "&load[]=blog=>blog.Post.Take(10)" to the
            // correspondent IncludeExpression structure using the Graph.
            IEnumerable<IncludeExpression> iExpressions = GetIncludeExpressions(rootModelType, load);
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
    }
}