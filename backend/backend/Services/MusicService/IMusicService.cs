using backend.Models;

namespace backend.Services.MusicService
{
    public interface IMusicService
    {
        public Music? GetMusic(Guid id);

        public void UploadMusic(Music music);

        public void DeleteMusic(Guid id, User user);

        public void DeleteMusic(string filePath);

        List<Music>? GetUserMusic(User user);

        List<Music>? GetAllMusic();
    }
}
