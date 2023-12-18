using backend.Models;

namespace backend.Services.UserService
{
    public interface IUserService
    {
        public bool IsUserNameUsed(string username);

        public void AddUser(string username, string password);

        public void LikeMusic(int userId, Guid musicId);

        public void DislikeMusic(User user, Guid musicId);

        public User? GetUser(string username);
    }
}
