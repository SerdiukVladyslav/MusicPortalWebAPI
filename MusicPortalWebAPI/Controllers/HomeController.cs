using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MusicPortalWebAPI.Models;

namespace MusicPortalWebAPI.Controllers
{
    [ApiController]
    [Route("api/Home")]
    public class HomeController : ControllerBase
    {
        private readonly MusicPortalContext _context;

        public HomeController(MusicPortalContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SongDto>>> GetSongs()
        {
            var songs = _context.Songs
                                .Include(s => s.Albums)
                                .Include(s => s.Artists)
                                .Include(s => s.Genre)
                                .Include(s => s.Countries);

            var result = await songs.Select(s => new SongDto
            {
                Id = s.SongId,
                Title = s.Title,
                AlbumCover = s.Albums.AlbumCover,
                Album = s.Albums.Album,
                Year = s.Albums.Year,
                Artist = s.Artists.Artist,
                Genre = s.Genre.Name,
                Country = s.Countries.Country,
                MusicFile = s.MusicFile
            }).ToListAsync();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetSongById(int id)
        {
            var song = _context.Songs.Find(id);
            if (song == null)
            {
                return NotFound();
            }
            return Ok(song);
        }

        [HttpPost]
        public async Task<IActionResult> AddSong([FromForm] CreateSongDto songDto, [FromForm] IFormFile musicFile)
        {
            try
            {
                if (musicFile == null || musicFile.Length == 0)
                {
                    return BadRequest("The MusicFile field is required.");
                }

                string wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                string songsDirectoryPath = Path.Combine(wwwRootPath, "Songs");

                if (!Directory.Exists(songsDirectoryPath))
                {
                    Directory.CreateDirectory(songsDirectoryPath);
                }

                string fileName = Path.GetFileName(musicFile.FileName);
                string filePath = Path.Combine(songsDirectoryPath, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await musicFile.CopyToAsync(stream);
                }

                string relativePath = $"/Songs/{fileName}";

                var countryExists = await _context.Countries.AnyAsync(c => c.Id == songDto.CountryId);
                if (!countryExists)
                {
                    return BadRequest("Country does not exist.");
                }

                if (ModelState.IsValid)
                {
                    var song = new Song
                    {
                        Title = songDto.Title,
                        GenreId = songDto.GenreId,
                        ArtistsId = songDto.ArtistId,
                        CountriesId = songDto.CountryId,
                        AlbumsId = songDto.AlbumId,
                        MusicFile = relativePath
                    };

                    _context.Songs.Add(song);
                    await _context.SaveChangesAsync();
                }

                return Ok("Song added successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSong(int id, [FromForm] UpdateSongDto songDto, [FromForm] IFormFile newMusicFile)
        {
            try
            {
                var existingSong = await _context.Songs.FindAsync(id);
                if (existingSong == null)
                {
                    return NotFound("Song not found.");
                }

                existingSong.Title = songDto.Title;
                existingSong.GenreId = songDto.GenreId;
                existingSong.ArtistsId = songDto.ArtistId;
                existingSong.AlbumsId = songDto.AlbumId;
                existingSong.CountriesId = songDto.CountryId;

                if (newMusicFile != null && newMusicFile.Length > 0)
                {
                    var oldFilePath = existingSong.MusicFile;
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }

                    string wwwRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                    string songsDirectoryPath = Path.Combine(wwwRootPath, "Songs");

                    if (!Directory.Exists(songsDirectoryPath))
                    {
                        Directory.CreateDirectory(songsDirectoryPath);
                    }

                    string newFilePath = Path.Combine(songsDirectoryPath, newMusicFile.FileName);

                    using (var stream = new FileStream(newFilePath, FileMode.Create))
                    {
                        await newMusicFile.CopyToAsync(stream);
                    }

                    existingSong.MusicFile = $"/Songs/{newMusicFile.FileName}";
                }

                await _context.SaveChangesAsync();

                return Ok("Song updated successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteSong(int id)
        {
            var song = _context.Songs.Find(id);
            if (song == null)
            {
                return NotFound();
            }

            _context.Songs.Remove(song);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
