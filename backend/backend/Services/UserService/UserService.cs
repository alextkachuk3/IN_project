using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services.UserService
{
    public class UserService(ApplicationDbContext dbContext) : IUserService
    {
        private readonly ApplicationDbContext _dbContext = dbContext;

        public void AddUser(string username, string password)
        {
            try
            {
                User user = new(username, password)
                {
                    LikedPlaylist = _dbContext.Users.Select(e => e.LikedPlaylist).DefaultIfEmpty().Max() + 1
                };
                _dbContext.Users?.Add(user);
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

        public void DislikeMusic(User user, Guid musicId)
        {
            throw new NotImplementedException();
        }

        public User? GetUser(string username)
        {
            return _dbContext.Users?.FirstOrDefault(u => u.Username!.Equals(username));
        }

        public bool IsUserNameUsed(string username)
        {
            return _dbContext.Users?.Where(u => u.Username!.Equals(username)).Count() > 0;
        }

        public void LikeMusic(int userId, Guid musicId)
        {
            try
            {
                _dbContext.Playlists.Add(new PlaylistItem(_dbContext.Users!.FirstOrDefault(e => e.Id.Equals(userId))!.LikedPlaylist, musicId));
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
    }
}
