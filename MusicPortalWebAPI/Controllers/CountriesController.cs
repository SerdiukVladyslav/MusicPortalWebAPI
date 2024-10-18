using Microsoft.AspNetCore.Mvc;
using MusicPortalWebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace MusicPortalWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CountriesController : ControllerBase
    {
        private readonly MusicPortalContext _context;

        public CountriesController(MusicPortalContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Countries>>> GetCountries()
        {
            var countries = await _context.Countries
                                          .Select(c => new { id = c.Id, name = c.Country })
                                          .ToListAsync();
            return Ok(countries);
        }
    }
}
