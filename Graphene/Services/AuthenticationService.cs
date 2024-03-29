﻿using Graphene.Database.Interfaces;
using Graphene.Graph.Interfaces;
using Graphene.Entities.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Graphene.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthenticationService
    {
        /// <summary>
        /// 
        /// </summary>
        public AuthenticationService(IGrapheneDatabaseContext database, IConfiguration configuration, IGraph graph, IOptions<JsonOptions> jsonOptions)
        {
            Configuration = configuration;
            Database = database;
            Graph = graph;
            _jsonOptions = jsonOptions;
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

        private IOptions<JsonOptions> _jsonOptions;

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
                new Claim(ClaimTypes.UserData, user.ToJson(_jsonOptions.Value.JsonSerializerOptions))
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
