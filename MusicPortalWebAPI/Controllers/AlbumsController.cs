using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicPortalWebAPI.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MusicPortalWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AlbumsController : ControllerBase
    {
        private readonly MusicPortalContext _context;
        private readonly IWebHostEnvironment _appEnvironment;

        public AlbumsController(MusicPortalContext context, IWebHostEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Albums>>> GetAlbums()
        {
            var albums = await _context.Albums
                                       .Select(a => new { id = a.Id, name = a.Album })
                                       .ToListAsync();
            return Ok(albums);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Albums>> GetAlbum(int id)
        {
            var album = await _context.Albums.FindAsync(id);

            if (album == null)
            {
                return NotFound();
            }

            return album;
        }

        [HttpPost]
        public async Task<ActionResult<Albums>> CreateAlbum([FromForm] Albums album, [FromForm] IFormFile cover)
        {
            if (cover != null)
            {
                string path = "/Images/" + cover.FileName;
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await cover.CopyToAsync(fileStream);
                }

                album.AlbumCover = path;
            }

            _context.Albums.Add(album);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAlbum), new { id = album.Id }, album);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAlbum(int id, [FromForm] Albums album, [FromForm] IFormFile cover)
        {
            if (id != album.Id)
            {
                return BadRequest();
            }

            if (cover != null)
            {
                string path = "/Images/" + cover.FileName;
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await cover.CopyToAsync(fileStream);
                }

                album.AlbumCover = path;
            }

            _context.Entry(album).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlbumExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAlbum(int id)
        {
            var album = await _context.Albums.FindAsync(id);
            if (album == null)
            {
                return NotFound();
            }

            _context.Albums.Remove(album);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AlbumExists(int id)
        {
            return _context.Albums.Any(e => e.Id == id);
        }
    }
}
