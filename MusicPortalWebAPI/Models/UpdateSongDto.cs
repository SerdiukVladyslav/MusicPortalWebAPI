namespace MusicPortalWebAPI.Models
{
    public class UpdateSongDto
    {
        public string Title { get; set; }
        public int GenreId { get; set; }
        public int ArtistId { get; set; }
        public int AlbumId { get; set; }
        public int CountryId { get; set; }
    }
}
