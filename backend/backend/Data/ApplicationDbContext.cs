using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Data
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<User>? Users { get; set; }

        public DbSet<Music>? Music { get; set; }

        public DbSet<LikedMusic> LikedMusic { get; set; }

        public DbSet<DislikedMusic> DislikedMusic { get; set; }

        public DbSet<Cover>? Cover { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.LikedMusic)
                .WithOne(l => l.User)
                .HasForeignKey<LikedMusic>(l => l.UserId);

            modelBuilder.Entity<User>()
                .HasOne(u => u.DislikedMusic)
                .WithOne(d => d.User)
                .HasForeignKey<DislikedMusic>(d => d.UserId);

            base.OnModelCreating(modelBuilder);
        }

    }
}
