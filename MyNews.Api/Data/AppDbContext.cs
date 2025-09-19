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
        public DbSet<User> Users { get; set; }

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
                new NewsItem { Id = new Guid("11111111-1111-1111-1111-111111111111"), Title = "First News", Content = "Example news content", Section = SectionType.Sports, SourceId = 1, PublishedAt = new DateTime(2025, 8, 20, 12, 10, 0) },
                new NewsItem { Id = new Guid("22222222-2222-2222-2222-222222222222"), Title = "First News 2", Content = "Example news content 2", Section = SectionType.Movies, SourceId = 2, PublishedAt = new DateTime(2025, 8, 10, 12, 10, 0) }
            );

            modelBuilder.Entity<Source>().HasData(
                new Source { Id = 1, Name = "Nova", Url = "https://nova.bg/", Section = SectionType.Local_News },
                new Source { Id = 2, Name = "BTV", Url = "https://btvnovinite.bg/", Section = SectionType.Local_News },
                new Source { Id = 3, Name = "Dnevnik", Url = "https://www.dnevnik.bg/", Section = SectionType.Local_News },
                new Source { Id = 4, Name = "Offnews", Url = "https://offnews.bg/", Section = SectionType.Local_News },
                new Source { Id = 5, Name = "Sportal", Url = "https://www.sportal.bg/", Section = SectionType.Sports },
                new Source { Id = 6, Name = "Gong", Url = "https://gong.bg/", Section = SectionType.Sports },
                new Source { Id = 7, Name = "24Chasa", Url = "https://www.24chasa.bg/", Section = SectionType.Local_News },
                new Source { Id = 8, Name = "Dir.bg", Url = "https://www.dir.bg/", Section = SectionType.Local_News },
                new Source { Id = 9, Name = "Investor", Url = "https://www.investor.bg/", Section = SectionType.Business },
                new Source { Id = 10, Name = "Vesti.bg", Url = "https://www.vesti.bg/", Section = SectionType.Local_News }
            );
        }
    }
}
