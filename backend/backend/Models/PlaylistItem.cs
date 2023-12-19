using Microsoft.EntityFrameworkCore;

namespace backend.Models
{
    [Index(nameof(PlaylistId))]
    public class PlaylistItem
    {
        public PlaylistItem() { }

        public PlaylistItem(int playlistId, Guid musicId)
        {
            PlaylistId = playlistId;
            MusicId = musicId;
        }

        public int Id { get; set; }

        public int PlaylistId { get; set; }

        public Guid MusicId { get; set; }
    }
}
