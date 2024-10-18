namespace MusicPortalWebAPI.Models
{
    public class CreateSongDto
    {
        public string Title { get; set; }
        public int GenreId { get; set; }
        public int ArtistId { get; set; }
        public int AlbumId { get; set; }
        public int CountryId { get; set; }
        public string? MusicFile { get; set; }
    }
}
