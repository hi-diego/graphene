using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphene.Entities.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public class Authorization : Entity, IAuthorization
    {
        public virtual int AuthorizableId { get; set; }
        public virtual IAuthorizable Authorizable { get; set; }
        public virtual int AuthorizatorId { get; set; }
        public virtual IAuthorizator Authorizator { get; set; }
        public virtual bool Denied { get; set; }
    }
}
