namespace backend.Models
{
    public class DislikedMusic
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public User? User { get; set; }

        public ICollection<Music>? Music { get; } = new List<Music>();
    }
}
