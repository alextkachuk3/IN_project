using backend.Models;

namespace backend.Services.CoverService
{
    public interface ICoverService
    {
        Cover AddCover(IFormFile coverFile);
    }
}
