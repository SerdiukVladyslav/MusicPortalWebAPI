using Microsoft.AspNetCore.Mvc;
using MusicPortalWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MusicPortalWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenresController : ControllerBase
    {
        private readonly MusicPortalContext _context;

        public GenresController(MusicPortalContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Genre>>> GetGenres()
        {
            return await _context.Genres.ToListAsync();
        }
    }
}
