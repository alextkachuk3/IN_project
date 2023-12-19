using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services.MusicService
{
    public class MusicService : IMusicService
    {
        private readonly ApplicationDbContext _dbContext;

        private readonly string _musicUploadsFolder;

        public MusicService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _musicUploadsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Music");

            if (!Directory.Exists(_musicUploadsFolder))
            {
                Directory.CreateDirectory(_musicUploadsFolder);
            }
        }

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
                        DeleteMusic(Path.Combine(_musicUploadsFolder, id.ToString()));
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
            return [.. _dbContext.Music.Include(i => i.User!.Username)];
        }

        public Music? GetMusic(Guid id)
        {
            return _dbContext.Music.Where(i => i.Id.Equals(id)).FirstOrDefault() ?? throw new FileNotFoundException("file_not_found");
        }

        public List<Music>? GetUserMusic(User user)
        {
            return [.. _dbContext.Music.Where(i => i.User!.Equals(user))];
        }

        public FileStream GetMusicFileStream(Guid id)
        {
            Music? music = GetMusic(id) ?? throw new FileNotFoundException();

            var filePath = Path.Combine(_musicUploadsFolder, music.Id.ToString());

            if (File.Exists(filePath))
            {
                var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                return fileStream;
            }
            else
            {
                throw new FileNotFoundException();
            }
        }

        public void UploadMusic(User user, IFormFile musicFile, string musicName, Cover? cover)
        {
            Guid musicId = Guid.NewGuid();
            string musicFilePath = Path.Combine(_musicUploadsFolder, musicId.ToString());

            using (var fileStream = new FileStream(musicFilePath, FileMode.Create))
            {
                if (!IsFileMP3(musicFile.OpenReadStream()))
                {
                    throw new InvalidDataException("invalid_file_format");
                }
                musicFile.CopyTo(fileStream);
            }

            Music music = new(musicId, musicName, musicFile.Length, user, cover);

            try
            {
                _dbContext.Music?.Add(music);
            }
            catch
            {
                throw new Exception("internal_server_error");
            }
            finally
            {
                _dbContext.SaveChanges();
            }
        }

        private static bool IsFileMP3(Stream fileStream)
        {
            if (fileStream.Length < 4)
            {
                return false;
            }

            byte[] header = new byte[3];
            fileStream.Read(header, 0, 3);

            return header[0] == 0x49 && header[1] == 0x44 && header[2] == 0x33;
        }

        public bool CheckIfLiked(Guid id, User user)
        {
            return _dbContext.Playlists.Where(i => i.PlaylistId.Equals(user.LikedPlaylist)).Where(i => i.MusicId.Equals(id)).FirstOrDefault() != null;
        }

        public List<MusicInfoDto> GetLikedUserMusicInfo(User user)
        {
            var likedMusicIds = _dbContext.Playlists
                .Where(ulm => ulm.PlaylistId == user.LikedPlaylist)
                .Select(ulm => ulm.MusicId)
                .ToList();

            var likedMusicDetails = _dbContext.Music
                .Where(m => likedMusicIds.Contains(m.Id))
                .Select(m => new MusicInfoDto(m.Id, m.Name!, user.Username!, true))
                .ToList();

            return likedMusicDetails;
        }
    }
}
