﻿using Graphene.Database.Interfaces;
using Graphene.Graph.Interfaces;
using Graphene.Entities;
using Graphene.Entities.Interfaces;
using Graphene.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.Extensions.Options;

namespace Graphene.Http.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    //[Route("/auth")]
    public abstract class AuthController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseContext"></param>
        public AuthController(IGrapheneDatabaseContext dbContext, IConfiguration configuration, IGraph graph, IOptions<JsonOptions> jsonOptions)
        {
            Configuration = configuration;
            DatabaseContext = dbContext;
            Graph = graph;
            _jsonOptions = jsonOptions;
        }
        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public IGrapheneDatabaseContext DatabaseContext { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public IGraph Graph { get; }

        private IOptions<JsonOptions> _jsonOptions;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!TryValidateModel(request)) return BadRequest(ModelState);
            IAuthenticable? user = await (new AuthenticationService(DatabaseContext, Configuration, Graph, _jsonOptions))
                .Auth(request.Email, request.Password, request.Load);
            if (user == null) return Unauthorized();
            return Ok(user);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("me")]
        [Authorize]
        public IActionResult Get([FromQuery] Pagination pagination)
        {
            ClaimsIdentity? identity = (ClaimsIdentity?) User.Identity;
            Claim? claim = identity?.Claims.Where(c => c.Type == ClaimTypes.UserData).FirstOrDefault();
            Authenticable user = (Authenticable) JsonSerializer.Deserialize(claim?.Value ?? "{}", typeof(Authenticable));
            return Ok(Graph.GetIAuthenticable(DatabaseContext, user.Identifier, pagination.Load));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [NonAction]
        public static IAuthenticable CurrentUser(ControllerBase controller)
            => Authenticable.Transform((ClaimsIdentity) controller.User.Identity);
        /// <summary>
        /// 
        /// </summary>
        public class LoginRequest
        {
            /// <summary>
            /// 
            /// </summary>
            [Required]
            public string Email { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [Required]
            public string Password { get; set; }
            /// <summary>
            /// 
            /// </summary>
            [FromQuery(Name = "load[]")]
            public string[] Load { get; set; } = { };
        }
    }
}
