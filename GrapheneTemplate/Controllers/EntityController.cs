using Graphene.Entities;
using Graphene.Http;
using Graphene.Http.Filter;
using Graphene.Services;
using GrapheneTemplate.Database.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.Security.Cryptography;
using System.Text;

namespace GrapheneTemplate.Controllers
{
    [Route("/entity")]
    //[Authorize]
    [ApiController]
    //[ServiceFilter(typeof(AuthorizationFilter))]
    //[ServiceFilter(typeof(ResourceFilter))]
    public class EntityController : Graphene.Http.Controllers.EntityController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityContext"></param>
        public EntityController(IEntityContext entityContext) : base(entityContext)
        {
            //
        }

        [HttpPost("/a-million")]
        public async  Task<IActionResult> CreateMillionBlogs ()
        {
            var authors = new List<Author>();
            for (int i = 0; i < 1000; i++)
            {
                // string? password = "QWDEWTVEW#C#@cEw1432#^NB(<M87cf$#@s2x";
                //string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                //    password: password!,
                //    salt: RandomNumberGenerator.GetBytes(128 / 8),
                //    prf: KeyDerivationPrf.HMACSHA256,
                //    iterationCount: 100000,
                //    numBytesRequested: 256 / 8)
                //);

                var blogs = new List<Blog>(1000);
                blogs.AddRange(Enumerable.Repeat(new Blog() {
                    Url = RandomString(20)
                }, 1000));
                blogs = blogs.Select(b => (new Blog()
                {
                    Url = RandomString(20)
                })).ToList();
                var author = new Author() {
                    Name = RandomString(20),
                    Email = RandomString(20),
                    Bolgs = blogs
                };
                authors.Add(author);
            }
            EC.Repository.DatabaseContext.AddRange(authors);
            await EC.Repository.DatabaseContext.SaveChangesAsync();
            return Ok(new { count = $"Creating {authors.Count()} Authors" });
        }

        // Generates a random string with a given size.    
        [NonAction]
        public string RandomString(int size, bool lowerCase = false)
        {
            var builder = new StringBuilder(size);
            char offset = lowerCase ? 'a' : 'A';
            const int lettersOffset = 26; // A...Z or a..z: length=26  
            var random = new Random();
            for (var i = 0; i < size; i++)
            {
                var @char = (char)random.Next(offset, offset + lettersOffset);
                builder.Append(@char);
            }
            return lowerCase ? builder.ToString().ToLower() : builder.ToString();
        }
    }
}
