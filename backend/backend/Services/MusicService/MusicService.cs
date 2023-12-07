using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services.MusicService
{
    public class MusicService(ApplicationDbContext dbContext) : IMusicService
    {
        private readonly ApplicationDbContext _dbContext = dbContext;

        public void DeleteMusic(Guid id, User user)
        {
            Music? file = _dbContext.Music!.Where(i => i.Id.Equals(id)).Include(i => i.User).FirstOrDefault();
            if (file is not null)
            {
                if (file.User!.Equals(user))
                {
                    try
                    {
                        _dbContext.Music!.Remove(file);
                    }
                    catch
                    {
                        throw;
                    }
                    finally
                    {
                        _dbContext.SaveChanges();
                        DeleteMusic(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Music", id.ToString()));
                    }
                }
                else
                {
                    throw new InvalidOperationException("user_doesnt_own_file");
                }
            }
            else
            {
                throw new InvalidOperationException("file_not_exists");
            }
        }

        public void DeleteMusic(string filePath)
        {
            File.Delete(filePath);
        }

        public List<Music>? GetAllMusic()
        {
            return _dbContext.Music?.Include(i => i.User!.Username).ToList();
        }

        public Music? GetMusic(Guid id)
        {
            return _dbContext.Music?.Where(i => i.Id.Equals(id)).FirstOrDefault();
        }

        public List<Music>? GetUserMusic(User user)
        {
            return _dbContext.Music?.Where(i => i.User!.Equals(user)).ToList();
        }

        public void UploadMusic(Music music)
        {
            try
            {
                _dbContext.Music?.Add(music);
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
