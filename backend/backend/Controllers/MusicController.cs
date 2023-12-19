using backend.Models;
using backend.Services.CoverService;
using backend.Services.MusicService;
using backend.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp;

namespace backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MusicController(IUserService userService, IMusicService musicService, ICoverService coverService) : Controller
    {
        private readonly IUserService _userService = userService;
        private readonly IMusicService _musicService = musicService;
        private readonly ICoverService _coverService = coverService;

        [HttpGet("{id}")]
        public IActionResult GetMusicFile(string id)
        {
            try
            {
                return File(_musicService.GetMusicFileStream(Guid.Parse(id)), "audio/mpeg");
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(new { Error = ex.Message });
            }
        }

        [HttpGet("Cover/{id}")]
        public IActionResult GetMusicCover(string id)
        {
            try
            {
                return File(_coverService.GetCoverFileStream(Guid.Parse(id)), "image/jpeg");
            }
            catch (FileNotFoundException ex)
            {
                return NotFound(new { Error = ex.Message });
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
        [HttpPost("Like")]
        public IActionResult LikeMusic(string id)
        {
            User user = _userService.GetUser(User.Identity!.Name!)!;
            _userService.LikeMusic(user.Id, Guid.Parse(id));
            return Ok();
        }

        [Authorize]
        [HttpPost("Upload")]
        public IActionResult Upload(IFormFile musicFile, [FromForm] string musicName, IFormFile? coverFile)
        {
            if (musicFile == null || musicFile.Length <= 0)
            {
                return BadRequest(new { Error = "invalid_file" });
            }

            if (musicName == null || musicName.Length < 1 || musicName.Length > 70)
            {
                return BadRequest(new { Error = "invalid_name" });
            }

            Cover? cover = null;

            if (coverFile != null)
            {
                try
                {
                    cover = _coverService.AddCover(coverFile);
                }
                catch (Exception ex)
                {
                    return BadRequest(new { Error = ex.Message });
                }
            }

            User user = _userService.GetUser(User.Identity!.Name!)!;

            try
            {
                _musicService.UploadMusic(user, musicFile, musicName, cover);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }

            return Ok(new { Message = "file_uploaded_successfully" });
        }
    }
}
