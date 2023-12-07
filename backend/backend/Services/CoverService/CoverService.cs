using backend.Data;
using backend.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace backend.Services.CoverService
{
    public class CoverService : ICoverService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly string _coverUploadsFolder;

        public CoverService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
            _coverUploadsFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Cover");

            if (!Directory.Exists(_coverUploadsFolder))
            {
                Directory.CreateDirectory(_coverUploadsFolder);
            }
        }

        public Cover AddCover(IFormFile coverFile)
        {            
            Stream stream = coverFile.OpenReadStream();
            Image image;

            try
            {
                image = Image.Load(stream);
            }
            catch (Exception)
            {
                throw new InvalidDataException("wrong_cover_file_format");
            }

            image.Mutate(x => x.Resize(new ResizeOptions
            {
                Size = new Size(500, 500),
                Mode = ResizeMode.Stretch
            }));

            Guid coverId = Guid.NewGuid();
            string coverFilePath = Path.Combine(_coverUploadsFolder, coverId.ToString());

            try
            {
                image.SaveAsPng(coverFilePath);
            }
            catch (Exception)
            {
                throw new Exception("internal_server_error");
            }

            image.Dispose();

            Cover cover = new(coverId);

            _dbContext.Cover!.Add(cover);

            return cover;
        }
    }
}
