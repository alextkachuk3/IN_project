namespace backend.Models
{
    public class Cover
    {
        public Cover() { }

        public Cover(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
