using Graphene.Entities.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphene.Entities.Identity
{
    public class GrapheneIdentityUserClaim : IdentityUserClaim<int>, IEntity
    {
        [Key]
        public override int Id { get; set; }
        public Guid Uid { get; set; }
        public string _Entity { get; set; } = nameof(GrapheneIdentityUserClaim);
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public EntityState EntityState { get; set; }
    }
}
