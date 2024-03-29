﻿using Graphene.Database.Interfaces;
using Graphene.Entities.Interfaces;
using Graphene.Services;

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Graphene.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class Authenticable : Entity, IAuthenticable
    {
        /// <summary>
        /// 
        /// </summary>
        public virtual string Identifier { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public virtual string Password { get; set; } = "secret";
        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        public virtual string? Token { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static Authenticable Transform(ClaimsIdentity? identity)
        {
            if (identity == null) return null;
            Claim claim = identity.Claims.Where(c => c.Type == ClaimTypes.UserData).FirstOrDefault();
            if (claim == null) return null;
            return JsonSerializer.Deserialize<Authenticable>(claim.Value);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Database"></param>
        public override void BeforeAdded(IGrapheneDatabaseContext database)
        {
            base.BeforeAdded(database);
            if (Id == 0) Password = new SecurePasswordService().Hash(Password);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializePassword() => false;
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool ShouldSerializeIdentifier() => false;
    }
}
