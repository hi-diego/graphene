using Graphene.Http;
using GrapheneTemplate.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GrapheneTemplate.Controllers
{
    [Route("/test")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private DatabaseContext _dbContext;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entityContext"></param>
        public TestController(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Test([FromQuery]Pagination pagination)
        {
            //return Ok(_dbContext.Blog.Take(100).ToList());
            //return Ok(await pagination.Paginate(_dbContext.Blog));
            var query = _dbContext.Blog.AsNoTracking();
            pagination.Total = 1000009; // query.Count();
            pagination.Pages = pagination.Total / pagination.Size + (pagination.Total % pagination.Size);
            pagination.Data = await query.Skip((pagination.Page - 1) * pagination.Size).Take(pagination.Size).ToArrayAsync();
            return Ok(pagination);
        }
    }
}
