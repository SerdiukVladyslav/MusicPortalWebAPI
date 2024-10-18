using Microsoft.AspNetCore.Mvc;
using MusicPortalWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MusicPortalWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArtistsController : ControllerBase
    {
        private readonly MusicPortalContext _context;

        public ArtistsController(MusicPortalContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Artists>>> GetArtists()
        {
            var artists = await _context.Artists
                                        .Select(a => new { id = a.Id, name = a.Artist })
                                        .ToListAsync();
            return Ok(artists);
        }
    }
}
