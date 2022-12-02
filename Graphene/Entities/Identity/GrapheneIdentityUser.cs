using Graphene.Entities.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Graphene.Entities.Identity
{
    public class GrapheneIdentityUser : IdentityUser<int>, IEntity, IAuthenticable
    {
        public Guid Uid { get; set; }
        public string _Entity { get; set; } = nameof(GrapheneIdentityUser);
        public DateTime CreatedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public EntityState EntityState { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Required]
        [NotMapped]
        public string Identifier { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        [NotMapped]
        public string Password { get; set; } // = "secret";
        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        public string? Token { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        public IEnumerable<IAuthorizator> Authorizations { get; set; }
    }
}
