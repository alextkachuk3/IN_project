namespace backend.Models
{
    public class MusicInfoDto(Guid id, string name, string uploaderName, bool isLiked)
    {
        public Guid Id { get; set; } = id;

        public string Name { get; set; } = name;

        public string UploaderName { get; set; } = uploaderName;

        public bool IsLiked { get; set; } = isLiked;
    }
}
