using Microsoft.EntityFrameworkCore;

namespace backend.Models
{
    [Index(nameof(PlaylistId))]
    public class PlaylistItem
    {
        public PlaylistItem() { }

        public PlaylistItem(int id, Guid musicId)
        {
            PlaylistId = id;
            MusicId = musicId;
        }

        public int Id { get; set; }

        public int PlaylistId { get; set; }

        public Guid MusicId { get; set; }
    }
}
