using GrapheneCore.Database.Interfaces;
using GrapheneCore.Entities.Interfaces;
using GrapheneCore.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GrapheneCore.Entities
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
        public virtual string Token { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="identity"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static Authenticable Transform(ClaimsIdentity identity)
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
