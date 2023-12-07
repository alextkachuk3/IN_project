using backend.Models;
using backend.Services.MusicService;
using backend.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MusicController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMusicService _musicService;
        private readonly string musicUploadsFolder;
        private readonly string coverUploadsFolder;

        public MusicController(IUserService userService, IMusicService musicService)
        {
            _userService = userService;
            _musicService = musicService;
            musicUploadsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Music");
            coverUploadsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Cover");

            if (!Directory.Exists(musicUploadsFolder))
            {
                Directory.CreateDirectory(musicUploadsFolder);
            }
            
            if(!Directory.Exists(coverUploadsFolder))
            {
                Directory.CreateDirectory(coverUploadsFolder);
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetMusicFile(string id)
        {
            Music? music = _musicService.GetMusic(Guid.Parse(id));

            if (music == null)
            {
                return NotFound(new { Error = "file_not_found" });
            }

            var filePath = Path.Combine(musicUploadsFolder, music.Id.ToString());

            if (System.IO.File.Exists(filePath))
            {
                var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                return File(fileStream, "audio/mpeg");
            }
            else
            {
                return NotFound(new { Error = "file_not_found" });
            }
        }

        [Authorize]
        [HttpGet]
        public IEnumerable<MusicDto> GetUserMusic()
        {
            User user = _userService.GetUser(User.Identity!.Name!)!;
            List<Music> music = _musicService.GetUserMusic(user)!;
            return music.Select(music => new MusicDto(music.Id, music.Name!)).ToArray();
        }

        [Authorize]
        [HttpPost("Upload")]
        public async Task<IActionResult> Upload(IFormFile file, [FromForm] string? name, IFormFile? cover)
        {
            if (file == null || file.Length <= 0)
            {
                return BadRequest(new { Error = "invalid_file" });
            }

            if (name == null || name.Length < 1 || name.Length > 70)
            {
                return BadRequest(new { Error = "invalid_name" });
            }

            if (cover != null)
            {
                using (Stream stream = cover.OpenReadStream())
                {
                    Guid coverId = Guid.NewGuid();
                    string coverFilePath = Path.Combine(coverUploadsFolder, coverId.ToString());
                    
                    Image image = Image.Load(stream);
                    image.Mutate(x => x.Resize(new ResizeOptions
                    {
                        Size = new Size(500, 500),
                        Mode = ResizeMode.Max
                    }));
                    image.SaveAsPng(coverFilePath);
                    image.Dispose();
                }
            }

            Guid musicId = Guid.NewGuid();
            string musicFilePath = Path.Combine(musicUploadsFolder, musicId.ToString());

            using (var fileStream = new FileStream(musicFilePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            if (!IsFileMP3(musicFilePath))
            {
                _musicService.DeleteMusic(musicFilePath);
                return BadRequest(new { Error = "invalid_file" });
            }

            User user = _userService.GetUser(User.Identity!.Name!)!;
            Music music = new(musicId, name, file.Length, user);

            try
            {
                _musicService.UploadMusic(music);
            }
            catch
            {
                return StatusCode(500, new { Error = "internal_server_error" });
            }

            return Ok(new { Message = "file_uploaded_successfully" });
        }

        private static bool IsFileMP3(string filePath)
        {
            FileStream fs = new(filePath, FileMode.Open, FileAccess.Read);

            if (fs.Length < 4)
            {
                return false;
            }

            byte[] header = new byte[3];
            fs.Read(header, 0, 3);

            return header[0] == 0x49 && header[1] == 0x44 && header[2] == 0x33;
        }
    }
}
