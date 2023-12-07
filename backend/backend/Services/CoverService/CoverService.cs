using backend.Data;

namespace backend.Services.CoverService
{
    public class CoverService(ApplicationDbContext dbContext) : ICoverService
    {
        private readonly ApplicationDbContext _dbContext = dbContext;

        public void AddCover()
        {
            throw new NotImplementedException();
        }
    }
}
