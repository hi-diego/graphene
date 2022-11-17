using Graphene.Entities.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphene.Entities.Identity
{
    public class GrapheneIdentityRole : IdentityRole<int>, IEntity
    {
        public Guid Uid { get; set; }
        public string _Entity { get; set; } = nameof(GrapheneIdentityRole);
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public EntityState EntityState { get; set; }
    }
}
