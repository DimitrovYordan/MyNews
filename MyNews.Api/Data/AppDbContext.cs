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
        public DbSet<Section> Sections { get; set; }
        public DbSet<Source> Sources { get; set; }

        // Seed initial data
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Explicitly map NewsItem to table NewsItems
            modelBuilder.Entity<NewsItem>().ToTable("NewsItems");
            modelBuilder.Entity<Section>().ToTable("Sections");
            modelBuilder.Entity<Source>().ToTable("Sources");

            // Relationships
            modelBuilder.Entity<NewsItem>()
                .HasOne(n => n.Section)
                .WithMany(s => s.NewsItems)
                .HasForeignKey(n => n.SectionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<NewsItem>()
                .HasOne(n => n.Source)
                .WithMany(s => s.NewsItems)
                .HasForeignKey(n => n.SourceId)
                .OnDelete(DeleteBehavior.Cascade);

            // Static seed data
            modelBuilder.Entity<NewsItem>().HasData(
                new NewsItem { Id = 1, Title = "First News", Content = "Example news content", SectionId = 1, SourceId = 1, PublishedAt = new DateTime(2025, 8, 20, 12, 10, 0) },
                new NewsItem { Id = 2, Title = "First News 2", Content = "Example news content 2", SectionId = 2, SourceId = 2, PublishedAt = new DateTime(2025, 8, 10, 12, 10, 0) }
            );

            modelBuilder.Entity<Section>().HasData(
                new Section { Id = 1, Name = "Sports" },
                new Section { Id = 2, Name = "Movies" }
            );

            modelBuilder.Entity<Source>().HasData(
                new Source { Id = 1, Name = "Sportal", Url = "sportal.bg" },
                new Source { Id = 2, Name = "IMDB", Url = "imdb.com"}
            );
        }

    }
}
