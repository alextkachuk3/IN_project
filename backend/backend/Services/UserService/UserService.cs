using backend.Data;
using backend.Models;

namespace backend.Services.UserService
{
    public class UserService(ApplicationDbContext dbContext) : IUserService
    {
        private readonly ApplicationDbContext _dbContext = dbContext;

        public void AddUser(string username, string password)
        {
            try
            {
                _dbContext.Users?.Add(new User(username, password));
            }
            catch
            {
                throw;
            }
            finally
            {
                _dbContext.SaveChanges();
            }
        }

        public User? GetUser(string username)
        {
            return _dbContext.Users?.FirstOrDefault(u => u.Username!.Equals(username));
        }

        public bool IsUserNameUsed(string username)
        {
            return _dbContext.Users?.Where(u => u.Username!.Equals(username)).Count() > 0;
        }
    }
}
