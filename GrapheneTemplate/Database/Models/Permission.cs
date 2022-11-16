using Graphene.Entities;
using Graphene.Services;
using System.ComponentModel.DataAnnotations.Schema;

namespace GrapheneTemplate.Database.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Permission : Authorizator
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityContext"></param>
        /// <returns></returns>
        public override async Task<bool> IsAuthorized(IEntityContext entityContext)
        {
            return true;
        }
    }
}