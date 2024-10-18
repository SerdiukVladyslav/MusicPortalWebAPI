using System.ComponentModel.DataAnnotations;

namespace MusicPortalWebAPI.Models
{
    public class Artists
    {
        public int Id { get; set; }

        public string Artist { get; set; }

        public List<Song> Songs { get; set; }
    }
}
