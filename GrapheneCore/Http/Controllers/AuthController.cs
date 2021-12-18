using GrapheneCore.Database.Interfaces;
using GrapheneCore.Graph.Interfaces;
using GrapheneCore.Models;
using GrapheneCore.Models.Interfaces;
using GrapheneCore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace GrapheneCore.Http.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseContext"></param>
        public AuthController(IGrapheneDatabaseContext dbContext, IConfiguration configuration, IGraph graph)
        {
            Configuration = configuration;
            DatabaseContext = dbContext;
            Graph = graph;
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            if (!TryValidateModel(request)) return BadRequest(ModelState);
            IAuthenticable? user = await (new AuthenticationService(DatabaseContext, Configuration, Graph))
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
            Authenticable? user = JObject.Parse(claim?.Value ?? "{}")?.ToObject<Authenticable>();
            return Ok(Graph.GetIAuthenticable(DatabaseContext, user.Identifier, pagination.Include));
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
            public string[]? Load { get; set; } = { };
        }
    }
}
