using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GrapheneCore.Entities.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAuthenticable : IEntity
    {
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Identifier { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public string Password { get; set; } // = "secret";
        /// <summary>
        /// 
        /// </summary>
        string? Token { get; set; }
    }
}
