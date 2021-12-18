using GrapheneCore.Models.Interfaces;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GrapheneCore.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Authenticable : Model, IAuthenticable
    {
        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        public string Identifier { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 
        /// </summary>
        [NotMapped]
        public string Token { get; set; }
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
    }
}
