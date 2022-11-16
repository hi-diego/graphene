using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Graphene;
using Graphene.Services;

namespace Tests.Authorization
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthorizationServiceTests
    {
        /// <summary>
        /// 
        /// </summary>
        [Fact]
        public void CanCreateQueryableToAuthorizeReadAtion()
        {
            //AuthorizationService service = new AuthorizationService();
            //service.GetAuthorizeQueryable();
            Assert.False(result, "1 should not be prime");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="permission"></param>
        /// <returns></returns>
        //public async Task<bool> IsAuthorized(Entity entityInstance, IAuthorizable? user, IGrapheneDatabaseContext context)
        //{
        //    if (Expression == null) return true;
        //    var query = Graphene.Graph.Graph.GetSet<IAuthorizable>(context).Where(Expression, entityInstance);
        //    if (user != null) query = query.Where($"user => user.Id == {user.Id}");
        //    return await query.AsNoTracking().CountAsync() > 0;
        //}
    }
}
