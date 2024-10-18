using System.ComponentModel.DataAnnotations;

namespace MusicPortalWebAPI.Models
{
    public class Countries
    {
        public int Id { get; set; }

        public string Country { get; set; }

        public List<Song> Songs { get; set; }
    }
}
