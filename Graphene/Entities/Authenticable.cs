using Graphene.Database.Interfaces;
using Graphene.Entities.Interfaces;
using Graphene.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Graphene.Entities
{
    /// <summary>
    /// 
    /// </summary>
    public class Authenticable : IdentityUser<int>, IEntity, IAuthenticable
    {
        /// <summary>
        /// 
        /// </summary>
        public virtual string Identifier { get; set; } = "email";
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
        [NotMapped]
        public IEnumerable<IAuthorizator> Authorizations { get; set; }
        public Guid Uid { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string _Entity { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime CreatedAt { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime? ModifiedAt { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime? DeletedAt { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public EntityState EntityState { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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
            return JObject.Parse(claim.Value).ToObject<Authenticable>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="Database"></param>
        //public override void BeforeAdded(IGrapheneDatabaseContext database)
        //{
        //    base.BeforeAdded(database);
        //    if (Id == 0) Password = new SecurePasswordService().Hash(Password);
        //}
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

        public string ToJson()
        {
            throw new NotImplementedException();
        }
    }
}
