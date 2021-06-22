using Graphene.Database.Entities;
using GrapheneCore.Database;
using GrapheneCore.Graph;
using GrapheneCore.Http.Controllers;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Graphene.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class ApiController : ApiBaseController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="configuration"></param>
        /// <param name="schema"></param>
        public ApiController(IEntityRepository entityRepository) : base (entityRepository)
        {
            Database.Context dbContext = (Database.Context) entityRepository.OriginalContext;
            if (dbContext.User.Where(u => u.Email == "crysramyrez@live.com").Count() == 0)
            {
                User adminUser = new User
                {
                    Name = "Christian Meza",
                    // UserName = "DiegoMezaExt",
                    PaternalLastName = "Meza",
                    MaternalLastName = "Ramirez",
                    Email = "crysramyrez@live.com",
                    Password = "123456789",
                    Identifier = "MERC930128HCMZMH09"
                };
                dbContext.Add(adminUser);
                dbContext.SaveChanges();
            }
            //new EntityRepository(dbContext, schema).Create(adminUser);
        }
    }
}
