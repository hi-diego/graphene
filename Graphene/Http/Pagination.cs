using Graphene.Extensions;
using Graphene.Graph.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace Graphene.Http
{
    /// <summary>
    /// 
    /// </summary>
    public class Pagination
    {
        /// <summary>
        /// 
        /// </summary>
        // [FromQuery(Name = "where[]")]
        public string Where { get; set; } = "true";
        /// <summary>
        /// 
        /// </summary>
        [FromQuery(Name = "load[]")]
        public string[] Load { get; set; } = { };
        /// <summary>
        /// 
        /// </summary>
        [FromQuery(Name = "include[]")]
        public string[] Include { get; set; } = { };
        /// <summary>
        /// 
        /// </summary>
        //[FromQuery(Name = "page")]
        public int Page { get; set; } = 1;
        /// <summary>
        /// 
        /// </summary>
        //[FromQuery(Name = "size")]
        public int Size { get; set; } = 10;
        /// <summary>
        /// 
        /// </summary>
        public int Total { get; set; } = 0;
        /// <summary>
        /// 
        /// </summary>
        public int Pages { get; set; } = 0;
        /// <summary>
        /// 
        /// </summary>
        public object[] Data { get; set; } = { };
        /// <summary>
        /// 
        /// </summary>
        public List<object> Erorrs { get; internal set; } = new List<object>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public async Task<Pagination> Paginate(IQueryable<dynamic> query, object user = null, IGraph graph = null, Type entityType = null)
        {
            return await TryPaginate(this, query, user, graph, entityType);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async Task<Pagination> Paginate(Pagination pagination, IQueryable<dynamic> query, object user = null, IGraph graph = null, Type entityType = null)
        {
            query = query.Where(pagination.Where, user).Includes(pagination.Load).Includes(pagination.Load, graph, entityType).AsNoTracking();
            pagination.Total = query.Count();
            pagination.Pages = pagination.Total / pagination.Size + (pagination.Total % pagination.Size);
            pagination.Data = await query.Skip((pagination.Page - 1) * pagination.Size).Take(pagination.Size).ToArrayAsync();
            return pagination;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static async Task<Pagination> TryPaginate(Pagination pagination, IQueryable<dynamic> query, object user = null, IGraph graph = null, Type entityType = null)
        {
            try { pagination = await Paginate(pagination, query, user, graph, entityType); }
            catch (Exception e) { pagination.Erorrs.Add(e); }
            return pagination;
        }
    }
}