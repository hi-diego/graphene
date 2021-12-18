using GrapheneCore.Database.Interfaces;
using GrapheneCore.Graph.Interfaces;
using GrapheneCore.Models;
using GrapheneCore.Models.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GrapheneCore.Services
{
    public class AuthenticationService
    {
        /// <summary>
        /// 
        /// </summary>
        public AuthenticationService(IGrapheneDatabaseContext database, IConfiguration configuration, IGraph graph)
        {
            Configuration = configuration;
            Database = database;
            Graph = graph;
        }
        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }
        /// <summary>
        /// 
        /// </summary>
        public IGrapheneDatabaseContext Database { get; }
        /// <summary>
        /// 
        /// </summary>
        public IGraph Graph { get; }
        /// <summary>
        /// 
        /// </summary>
        public static string[] Includes { get; set; } = new string[] { };
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="includes"></param>
        /// <returns></returns>
        public async Task<IAuthenticable?> Auth(string email, string password, string[]? includes = null)
        {
            includes ??= Includes;
            var hasher = new SecurePasswordService();
            IAuthenticable? user = await Graph.GetIAuthenticable(Database, email, includes);
            if (user == null || !hasher.Check(user.Password, password).Verified) return null;
            var secretKey = Configuration.GetSection("JWT").GetValue<string>("Key");
            var key = Encoding.ASCII.GetBytes(secretKey);
            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Identifier),
                new Claim(ClaimTypes.UserData, user.ToJson())
            };
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var createdToken = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(createdToken);
            return user;
        }
    }
}
