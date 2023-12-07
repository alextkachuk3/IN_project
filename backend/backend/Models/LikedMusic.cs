namespace backend.Models
{
    public class LikedMusic
    {
        public LikedMusic() { }

        public int Id { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }

        public List<Music>? Music { get; set; }
    }
}
