using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services.CoverService
{
    public interface ICoverService
    {
        Cover AddCover(IFormFile coverFile);

        public FileStream GetCoverFileStream(Guid id);

        public Cover? GetCoverByMusicId(Guid id);
    }
}
