using backend.Models;

namespace backend.Services.MusicService
{
    public interface IMusicService
    {
        public Music? GetMusic(Guid id);

        public FileStream GetMusicFileStream(Guid id);

        public void UploadMusic(User user, IFormFile musicFile, string musicName, Cover? cover);

        public void DeleteMusic(Guid id, User user);

        public void DeleteMusic(string filePath);

        public List<Music>? GetUserMusic(User user);

        public List<Music>? GetAllMusic();
    }
}
