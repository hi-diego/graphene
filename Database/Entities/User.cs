using GrapheneCore.Database.Entities;
using GrapheneCore.Database.Entities.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Graphene.Database.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class User : GrapheneCore.Database.Entities.User
    {
        //
        public IEnumerable<EntityHistory> ActionLog { get; set; }
    }
}
