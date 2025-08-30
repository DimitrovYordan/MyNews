using Microsoft.EntityFrameworkCore;

using MyNews.Api.Models;

namespace MyNews.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<NewsItem> NewsItems { get; set; }

        // Seed initial data
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Explicitly map NewsItem to table NewsItems
            modelBuilder.Entity<NewsItem>().ToTable("NewsItems");

            // Static seed data
            modelBuilder.Entity<NewsItem>().HasData(
                new NewsItem { Id = 1, Title = "Welcome to MyNews!", Source = "System", Date = new DateTime(2025, 8, 10, 12, 0, 0) },
                new NewsItem { Id = 2, Title = "Test News 1", Source = "Demo", Date = new DateTime(2025, 8, 30, 12, 5, 0) },
                new NewsItem { Id = 3, Title = "Test News 2", Source = "Demo", Date = new DateTime(2025, 8, 20, 12, 10, 0) }
            );
        }

    }
}
