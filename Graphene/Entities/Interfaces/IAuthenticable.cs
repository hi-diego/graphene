using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Graphene.Entities.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAuthenticable : IEntity, IAuthorizable
    {
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
    }
}
