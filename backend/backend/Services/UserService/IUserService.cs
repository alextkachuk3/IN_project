using backend.Models;

namespace backend.Services.UserService
{
    public interface IUserService
    {
        public bool IsUserNameUsed(string username);

        public void AddUser(string username, string password);

        public User? GetUser(string username);
    }
}
