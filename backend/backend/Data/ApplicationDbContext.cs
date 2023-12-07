using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<User>? Users { get; set; }

        public DbSet<Music>? Music { get; set; }

        public DbSet<Cover>? Cover { get; set; }

    }
}
