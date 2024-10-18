namespace MusicPortalWebAPI.Models
{
    public class SongDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string AlbumCover { get; set; }
        public string Album { get; set; }
        public int Year { get; set; }
        public string Artist { get; set; }
        public string Genre { get; set; }
        public string Country { get; set; }
        public string MusicFile { get; set; }
    }
}
