using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieDataScraper.Data;

namespace MovieDataScraper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly MovieDbContext _dbContext;

        public MovieController(MovieDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: api/movies
        [HttpGet]
        public async Task<IActionResult> GetMovies()
        {
            var movies = await _dbContext.Movies.ToListAsync();
            return Ok(movies);
        }
    }
}
