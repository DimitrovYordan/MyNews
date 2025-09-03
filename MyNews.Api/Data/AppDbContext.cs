using Microsoft.EntityFrameworkCore;
using MyNews.Api.Enums;
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
        public DbSet<Source> Sources { get; set; }

        // Seed initial data
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Explicitly map NewsItem to table NewsItems
            modelBuilder.Entity<NewsItem>().ToTable("NewsItems");
            modelBuilder.Entity<Source>().ToTable("Sources");

            // Relationships
            modelBuilder.Entity<NewsItem>()
                .Property(n => n.Section)
                .HasConversion<int>();

            // Static seed data
            modelBuilder.Entity<NewsItem>().HasData(
                new NewsItem { Id = 1, Title = "First News", Content = "Example news content", Section = SectionType.Sports, SourceId = 1, PublishedAt = new DateTime(2025, 8, 20, 12, 10, 0) },
                new NewsItem { Id = 2, Title = "First News 2", Content = "Example news content 2", Section = SectionType.Movies, SourceId = 2, PublishedAt = new DateTime(2025, 8, 10, 12, 10, 0) }
            );

            modelBuilder.Entity<Source>().HasData(
                new Source { Id = 1, Name = "Sportal", Url = "sportal.bg" },
                new Source { Id = 2, Name = "IMDB", Url = "imdb.com"}
            );
        }

    }
}
