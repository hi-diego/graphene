using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.IO.Pipelines;
using Microsoft.AspNetCore.Routing;
using Graphene.Database;
using GrapheneCore.Extensions;
using GrapheneCore.Database;
using GrapheneCore.Http;
using GrapheneCore.Models;
using GrapheneCore.Database.Extensions;

namespace Backend.Http.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    [Route("/[controller]/")]
    //[ServiceFilter(typeof(AuthorizeActionFilter))]
    //[Authorize]
    public class ApiController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="databaseContext"></param>
        public ApiController(DatabaseContext dbContext, IConfiguration configuration)
        {
            Configuration = configuration;
            DatabaseContext = dbContext;
            ModelRepository = new ModelRepository(dbContext);
        }

        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public DatabaseContext DatabaseContext { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public ModelRepository ModelRepository { get; private set; }


        [HttpPost("files/{model}/{uid}")]
        public async Task<IActionResult> OnPostUploadAsync(IFormFile formFile, string model, string uid)
        {
            if (formFile.Length > 0)
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", model.DbSetName(), uid + ".jpg");
                System.IO.FileInfo file = new System.IO.FileInfo(path);
                file.Directory.Create();
                using (var stream = System.IO.File.Create(path))
                {
                    await formFile.CopyToAsync(stream);
                }
            }
            // Process uploaded files
            // Don't rely on or trust the FileName property without validation.
            return Ok(formFile);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        [NonAction]
        private static string GetSubDomain(HttpContext httpContext)
        {
            var subDomain = string.Empty;
            var host = httpContext.Request.Host.Host;
            if (!string.IsNullOrWhiteSpace(host)) subDomain = host.Split('.')[0];
            return subDomain.Trim().ToLower();
        }

        /// <summary>
        /// Retrives the Body of the Current HTTP Request
        /// in a JSON format.
        /// </summary>
        /// <returns></returns>
        [NonAction]
        public JObject GetJsonRequest()
        {
            return ToJson(Request);
        }

        /// <summary>
        /// Retrives the Body of the Current HTTP Request
        /// in a JSON format.
        /// </summary>
        /// <returns></returns>
        [NonAction]
        public static JObject ToJson(HttpRequest request)
        {
            if (request == null) return null;
            request.EnableBuffering();
            if (request.Body.CanSeek) request.Body.Position = 0;
            string input = (new StreamReader(request.Body)).ReadToEndAsync().GetAwaiter().GetResult();
            if (input == "" || input == null) return null;
            if (request.Body.CanSeek) request.Body.Position = 0;
            return JObject.Parse(input);
        }

        /// <summary>
        /// Retrives the Body of the Current HTTP Request
        /// in a JSON format.
        /// </summary>
        /// <returns></returns>
        [NonAction]
        public static async Task<JObject> ToJsonAsync(HttpRequest request)
        {
            if (request == null) return null;
            request.EnableBuffering();
            if (request.Body.CanSeek) request.Body.Position = 0;
            string input = await (new StreamReader(request.Body)).ReadToEndAsync();
            if (input == "" || input == null) return null;
            if (request.Body.CanSeek) request.Body.Position = 0;
            return JObject.Parse(input);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{model}/{id}")]
        public async Task<ActionResult> Find(string model, int id, [FromQuery] Pagination pagination)
        {
            object instance = await ModelRepository.Find(model, id, false, pagination.Load);
            if (instance == null)
                return NotFound();
            return Ok(instance);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{model}/{id}")]
        public async Task<ActionResult> Delete(string model, int id)
        {
            object instance = await ModelRepository.Find(model, id);
            if (instance == null)
                return NotFound();
            return Ok(ModelRepository.Delete(instance));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("{model}")]
        public async Task<ActionResult> Add(string model, [FromBody] JObject request)
        {
            if (!DatabaseContext.Exists(ref model)) return NotFound();
            object instance = await ModelRepository.Create(model, request, false);
            if (!TryValidateModel(instance, model))
                return BadRequest(ModelState);
            return Ok(await ModelRepository.Create(instance, true));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPatch("{model}/{id}")]
        public async Task<ActionResult> Edit(string model, int id, [FromBody] JObject request)
        {
            //DatabaseContext.AuthUser = GetUser();
            GraphModel instance = await ModelRepository.Update(request, model, id, false);
            if (instance == null)
                return NotFound();
            if (!TryValidateModel(instance, model))
                return BadRequest(ModelState);
            return Ok(await ModelRepository.Save(instance));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pagination"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet("{model}")]
        public async Task<ActionResult> Index([FromQuery] Pagination pagination, string model)
        {
            if (!DatabaseContext.Exists(ref model)) return NotFound();
            //pagination.Where += GetPermissionsWhere(model, "Index");
            return Ok(await pagination.Paginate(DatabaseContext.GetSet(model), new { }));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //public User GetUser() =>
        //    ApiController.GetUser(User);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //public static User GetUser(ClaimsPrincipal claims) =>
        //    Backend.Database.Entities.User.Transform((ClaimsIdentity)claims.Identity);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        //private string GetPermissionsWhere(string model, string actionName)
        //{
        //    string[] wheres = DatabaseContext.Permission
        //        .Where(p => p.Entity == model && actionName == p.Name)
        //        .Select(p => p.Expression)
        //        .ToArray();
        //    string where = wheres.Length > 0
        //        ? " && " + string.Join(" && ", wheres)
        //        : "";
        //    return where;
        //}
    }
}
