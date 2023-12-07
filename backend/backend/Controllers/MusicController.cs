﻿using backend.Models;
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

        private readonly string coverUploadsFolder;

        public MusicController(IUserService userService, IMusicService musicService)
        {
            _userService = userService;
            _musicService = musicService;

            coverUploadsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Cover");

            if (!Directory.Exists(coverUploadsFolder))
            {
                Directory.CreateDirectory(coverUploadsFolder);
            }
        }

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
                using (Stream stream = coverFile.OpenReadStream())
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
