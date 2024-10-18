using System.ComponentModel.DataAnnotations;

namespace MusicPortalWebAPI.Models
{
    public class Song
    {
        public int SongId { get; set; }

        public string Title { get; set; }

        public int GenreId { get; set; }
        public Genre Genre { get; set; }

        public int ArtistsId { get; set; }
        public Artists Artists { get; set; }

        public int AlbumsId { get; set; }
        public Albums Albums { get; set; }

        public int CountriesId { get; set; }
        public Countries Countries { get; set; }

        public string? MusicFile { get; set; }
    }
}
