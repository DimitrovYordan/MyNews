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
        public DbSet<Source> Sources { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserNewsRead> UserNewsReads { get; set; }
        public DbSet<UserPreferences> UserPreferences { get; set; }
        public DbSet<NewsTranslation> NewsTranslations { get; set; }

        // Seed initial data
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasQueryFilter(u => !u.IsDeleted);
            modelBuilder.Entity<Source>().ToTable("Sources");
            modelBuilder.Entity<NewsItem>().ToTable("NewsItems");

            // Relationships
            modelBuilder.Entity<UserPreferences>()
                .HasKey(p => new { p.UserId, p.SectionType });

            modelBuilder.Entity<UserPreferences>()
                .HasIndex(p => new { p.UserId, p.SourceId })
                .IsUnique()
                .HasFilter("[SourceId] IS NOT NULL");

            modelBuilder.Entity<UserPreferences>()
                .HasOne(p => p.User)
                .WithMany(u => u.SectionPreferences)
                .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<UserPreferences>()
                .Property(p => p.SectionType)
                .HasConversion<int>();

            modelBuilder.Entity<UserNewsRead>()
                .HasKey(unr => new { unr.UserId, unr.NewsItemId });

            modelBuilder.Entity<UserNewsRead>()
                .HasOne(unr => unr.User)
                .WithMany(u => u.NewsReads)
                .HasForeignKey(unr => unr.UserId);

            modelBuilder.Entity<UserNewsRead>()
                .HasOne(unr => unr.NewsItem)
                .WithMany(n => n.UserReads)
                .HasForeignKey(unr => unr.NewsItemId);

            modelBuilder.Entity<NewsItem>()
                .HasOne(n => n.Source)
                .WithMany(s => s.NewsItems)
                .HasForeignKey(n => n.SourceId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<NewsItem>()
                .HasIndex(n => new { n.Title, n.SourceId })
                .IsUnique();

            modelBuilder.Entity<NewsItem>()
                .Property(n => n.Section)
                .HasConversion<int>();

            modelBuilder.Entity<NewsTranslation>()
                .HasOne(nt => nt.NewsItem)
                .WithMany(n => n.Translations)
                .HasForeignKey(nt => nt.NewsItemId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<NewsTranslation>()
                .HasIndex(nt => new { nt.NewsItemId, nt.LanguageCode })
                .IsUnique();

            modelBuilder.Entity<NewsItem>()
                .HasIndex(n => new { n.Title, n.SourceId })
                .IsUnique();

            modelBuilder.Entity<Source>().HasData(
                // ========================
                // Bulgaria
                // ========================
                new Source { Id = 1, Name = "Dnevnik", Url = "https://www.dnevnik.bg/rss/" },
                new Source { Id = 2, Name = "Novinite", Url = "https://www.novinite.com/rss/news" },
                new Source { Id = 3, Name = "Capital", Url = "https://www.capital.bg/rss/" },
                new Source { Id = 4, Name = "Mediapool", Url = "https://www.mediapool.bg/rss" },
                new Source { Id = 5, Name = "Actualno", Url = "https://www.actualno.com/rss" },
                new Source { Id = 6, Name = "24 Chasa", Url = "https://www.24chasa.bg/rss_category/2/novini.html" },
                new Source { Id = 7, Name = "BTA", Url = "https://www.bta.bg/bg/rss/free" },
                new Source { Id = 9, Name = "SofiaGlobe", Url = "http://feeds.feedburner.com/TheSofiaGlobe" },
                new Source { Id = 10, Name = "Pogled", Url = "https://pogled.info/rss" },
                new Source { Id = 11, Name = "BurgasNews", Url = "https://www.burgasnews.com/feed" },
                new Source { Id = 12, Name = "Gong", Url = "https://gong.bg/rss" },
                new Source { Id = 13, Name = "Investor", Url = "https://www.investor.bg/rss/news" },
                new Source { Id = 14, Name = "Vesti", Url = "https://www.vesti.bg/rss" },
                new Source { Id = 15, Name = "Dnes", Url = "https://www.dnes.bg/rss/news" },
                new Source { Id = 16, Name = "Blitz", Url = "https://www.blitz.bg/rss" },
                new Source { Id = 17, Name = "Standart", Url = "https://www.standartnews.com/rss" },
                new Source { Id = 18, Name = "Banker", Url = "https://banker.bg/feed/" },

                new Source { Id = 1001, Name = "Offnews - Политика", Url = "https://feed.offnews.bg/rss/%D0%9F%D0%BE%D0%BB%D0%B8%D1%82%D0%B8%D0%BA%D0%B0_8" },
                new Source { Id = 1002, Name = "Offnews - Общество", Url = "https://feed.offnews.bg/rss/%D0%9E%D0%B1%D1%89%D0%B5%D1%81%D1%82%D0%B2%D0%BE_4" },
                new Source { Id = 1003, Name = "Offnews - Икономика", Url = "https://feed.offnews.bg/rss/%D0%98%D0%BA%D0%BE%D0%BD%D0%BE%D0%BC%D0%B8%D0%BA%D0%B0_59" },
                new Source { Id = 1004, Name = "Offnews - Темида", Url = "https://feed.offnews.bg/rss/%D0%A2%D0%B5%D0%BC%D0%B8%D0%B4%D0%B0_18762" },
                new Source { Id = 1005, Name = "Offnews - Инциденти", Url = "https://feed.offnews.bg/rss/%D0%98%D0%BD%D1%86%D0%B8%D0%B4%D0%B5%D0%BD%D1%82%D0%B8_6" },
                new Source { Id = 1006, Name = "Offnews - Медии", Url = "https://feed.offnews.bg/rss/%D0%9C%D0%B5%D0%B4%D0%B8%D0%B8_73" },
                new Source { Id = 1007, Name = "Offnews - Свят", Url = "https://feed.offnews.bg/rss/%D0%A1%D0%B2%D1%8F%D1%82%20_12" },
                new Source { Id = 1008, Name = "Offnews - Туризъм", Url = "https://feed.offnews.bg/rss/%D0%A2%D1%83%D1%80%D0%B8%D0%B7%D1%8A%D0%BC_75" },
                new Source { Id = 1009, Name = "Offnews - Здраве", Url = "https://feed.offnews.bg/rss/%D0%97%D0%B4%D1%80%D0%B0%D0%B2%D0%B5_18753" },

                // ========================
                // Global / English
                // ========================
                new Source { Id = 25, Name = "BBC News", Url = "http://feeds.bbci.co.uk/news/rss.xml" },
                new Source { Id = 26, Name = "CNN", Url = "http://rss.cnn.com/rss/cnn_topstories.rss" },
                new Source { Id = 27, Name = "Reuters", Url = "http://feeds.reuters.com/reuters/topNews" },
                new Source { Id = 28, Name = "The New York Times", Url = "http://rss.nytimes.com/services/xml/rss/nyt/HomePage.xml" },
                new Source { Id = 29, Name = "The Guardian", Url = "https://www.theguardian.com/world/rss" },
                new Source { Id = 30, Name = "Associated Press", Url = "https://apnews.com/rss" },
                new Source { Id = 31, Name = "NBC News", Url = "http://feeds.nbcnews.com/nbcnews/public/news" },
                new Source { Id = 32, Name = "USA Today", Url = "http://rssfeeds.usatoday.com/usatoday-NewsTopStories" },
                new Source { Id = 33, Name = "Fox News", Url = "http://feeds.foxnews.com/foxnews/latest" },
                new Source { Id = 34, Name = "HuffPost", Url = "https://www.huffpost.com/section/front-page/feed" },
                new Source { Id = 35, Name = "Bloomberg", Url = "https://www.bloomberg.com/feed/podcast/etf-report.xml" },
                new Source { Id = 36, Name = "Yahoo News", Url = "https://news.yahoo.com/rss/" },
                new Source { Id = 37, Name = "Politico", Url = "https://www.politico.com/rss/politicopicks.xml" },
                new Source { Id = 38, Name = "The Verge", Url = "https://www.theverge.com/rss/index.xml" },
                new Source { Id = 39, Name = "TechCrunch", Url = "http://feeds.feedburner.com/TechCrunch/" },
                new Source { Id = 40, Name = "Wired", Url = "https://www.wired.com/feed/rss" },
                new Source { Id = 41, Name = "Mashable", Url = "http://feeds.mashable.com/Mashable" },
                new Source { Id = 42, Name = "Business Insider", Url = "https://www.businessinsider.com/rss" },
                new Source { Id = 43, Name = "Bloomberg Technology", Url = "https://www.bloomberg.com/feed/podcast/technology.xml" },
                
                // ========================
                // Japan
                // ========================
                new Source { Id = 44, Name = "The Japan Times", Url = "https://www.japantimes.co.jp/feed/" },
                new Source { Id = 45, Name = "NHK World", Url = "https://www3.nhk.or.jp/rss/news/cat0.xml" },
                new Source { Id = 46, Name = "Asahi Shimbun", Url = "http://www.asahi.com/rss/asahi/newsheadlines.rdf" },
                new Source { Id = 47, Name = "Mainichi Shimbun", Url = "https://mainichi.jp/rss/etc/flashnews.xml" },
                new Source { Id = 48, Name = "Kyodo News", Url = "https://english.kyodonews.net/rss/news.xml" },
                
                // ========================
                // China
                // ========================
                new Source { Id = 49, Name = "China Daily", Url = "http://www.chinadaily.com.cn/rss/china_rss.xml" },
                new Source { Id = 50, Name = "Xinhua News", Url = "http://www.xinhuanet.com/english/rss/worldrss.xml" },
                new Source { Id = 51, Name = "Global Times", Url = "https://www.globaltimes.cn/rss/china.xml" },
                new Source { Id = 52, Name = "Caixin Global", Url = "https://www.caixinglobal.com/feed/" },
                
                // ========================
                // South Korea
                // ========================
                new Source { Id = 53, Name = "Korea Herald", Url = "http://www.koreaherald.com/rss/018015000000/rssfeed.xml" },
                
                // ========================
                // India
                // ========================
                new Source { Id = 54, Name = "The Times of India", Url = "https://timesofindia.indiatimes.com/rssfeeds/-2128936835.cms" },
                new Source { Id = 55, Name = "Hindustan Times", Url = "https://www.hindustantimes.com/rss/topnews/rssfeed.xml" },
                
                // ========================
                // Germany
                // ========================
                new Source { Id = 56, Name = "Der Spiegel", Url = "https://www.spiegel.de/international/index.rss" },
                new Source { Id = 57, Name = "Die Zeit", Url = "https://www.zeit.de/index.rss" },
                new Source { Id = 58, Name = "Frankfurter Allgemeine", Url = "https://www.faz.net/rss/aktuell/" },
                new Source { Id = 59, Name = "Süddeutsche Zeitung", Url = "https://www.sueddeutsche.de/rss" },
                
                // ========================
                // France
                // ========================
                new Source { Id = 60, Name = "Le Monde", Url = "https://www.lemonde.fr/rss/une.xml" },
                new Source { Id = 61, Name = "Le Figaro", Url = "http://www.lefigaro.fr/rss/figaro_actualites.xml" },
                new Source { Id = 62, Name = "France 24", Url = "https://www.france24.com/en/rss" },
                
                // ========================
                // Italy
                // ========================
                new Source { Id = 63, Name = "Corriere della Sera", Url = "https://www.corriere.it/rss/homepage.xml" },
                new Source { Id = 64, Name = "La Repubblica", Url = "https://www.repubblica.it/rss/homepage/rss2.0.xml" },
                
                // ========================
                // Spain
                // ========================
                new Source { Id = 65, Name = "El País", Url = "https://feeds.elpais.com/mrss-s/pages/ep/site/elpais.com/portada" },
                new Source { Id = 66, Name = "El Mundo", Url = "https://www.elmundo.es/rss/portada.xml" },
                
                // ========================
                // Portugal
                // ========================
                new Source { Id = 67, Name = "Público", Url = "https://feeds.publico.pt/rss/home" },
                
                // ========================
                // Switzerland
                // ========================
                new Source { Id = 68, Name = "Swiss Info", Url = "https://www.swissinfo.ch/rss" },
                
                // ========================
                // USA / North America
                // ========================
                new Source { Id = 69, Name = "Scientific American", Url = "https://www.scientificamerican.com/feed/" },
                new Source { Id = 70, Name = "New Scientist", Url = "https://www.newscientist.com/feed/home/" },
                new Source { Id = 71, Name = "Nature News (US)", Url = "https://www.nature.com/news/rss" },
                new Source { Id = 72, Name = "TechRadar", Url = "https://www.techradar.com/rss" },
                new Source { Id = 73, Name = "Engadget", Url = "https://www.engadget.com/rss.xml" },
                new Source { Id = 74, Name = "PCMag", Url = "https://www.pcmag.com/feeds/all-news" },
                
                // ========================
                // UK
                // ========================
                new Source { Id = 75, Name = "The Independent", Url = "https://www.independent.co.uk/news/uk/rss" },
                new Source { Id = 76, Name = "Financial Times", Url = "https://www.ft.com/?format=rss" },
                new Source { Id = 77, Name = "Daily Mail", Url = "https://www.dailymail.co.uk/articles.rss" },
                
                // ========================
                // Canada
                // ========================
                new Source { Id = 78, Name = "CBC News", Url = "https://www.cbc.ca/cmlink/rss-canada" },
                
                // ========================
                // Australia / New Zealand
                // ========================
                new Source { Id = 79, Name = "ABC News AU", Url = "https://www.abc.net.au/news/feed/51120/rss.xml" },
                new Source { Id = 80, Name = "SBS News", Url = "https://www.sbs.com.au/news/feeds/rss/" },
                new Source { Id = 81, Name = "NZ Herald", Url = "https://www.nzherald.co.nz/rss/" },
                
                // ========================
                // Russia
                // ========================
                new Source { Id = 82, Name = "RT News", Url = "https://www.rt.com/rss/news/" },
                
                // ========================
                // Brazil
                // ========================
                new Source { Id = 83, Name = "Globo", Url = "https://g1.globo.com/rss/g1/" },
                
                // ========================
                // Argentina
                // ========================
                new Source { Id = 84, Name = "Clarin", Url = "https://www.clarin.com/rss/" },
                
                // ========================
                // Africa
                // ========================
                new Source { Id = 85, Name = "AllAfrica", Url = "https://allafrica.com/tools/headlines/rss/AllAfrica_headlines.xml" },
                new Source { Id = 86, Name = "News24 South Africa", Url = "https://www.news24.com/rss" },
                
                // ========================
                // Middle East
                // ========================
                new Source { Id = 87, Name = "Al Jazeera", Url = "https://www.aljazeera.com/xml/rss/all.xml" },
                new Source { Id = 88, Name = "Haaretz", Url = "https://www.haaretz.com/cmlink/1.6401109" },
                
                // ========================
                // Space / Astronomy (Global)
                // ========================
                new Source { Id = 89, Name = "NASA RSS", Url = "https://www.nasa.gov/rss/dyn/breaking_news.rss" },
                new Source { Id = 90, Name = "ESA News", Url = "https://www.esa.int/rssfeed" },
                
                // ========================
                // AI / Robotics / Tech (Global)
                // ========================
                new Source { Id = 91, Name = "MIT Technology Review", Url = "https://www.technologyreview.com/feed/" },
                new Source { Id = 92, Name = "Nature", Url = "https://www.nature.com/subjects/news/rss" },
                new Source { Id = 93, Name = "Science Daily", Url = "https://www.sciencedaily.com/rss/top/science.xml" },
                new Source { Id = 94, Name = "Ars Technica", Url = "http://feeds.arstechnica.com/arstechnica/index" },
                new Source { Id = 95, Name = "OpenAI Blog", Url = "https://openai.com/blog/rss/" },
                new Source { Id = 96, Name = "DeepMind Blog", Url = "https://deepmind.com/blog/feed/rss" },
                new Source { Id = 97, Name = "MIT CSAIL News", Url = "https://www.csail.mit.edu/news/rss.xml" },
                new Source { Id = 98, Name = "AI Trends", Url = "https://www.aitrends.com/feed/" },
                
                // ========================
                // Sports (Global)
                // ========================
                new Source { Id = 99, Name = "ESPN", Url = "http://www.espn.com/espn/rss/news" },
                new Source { Id = 100, Name = "Sky Sports", Url = "http://www.skysports.com/rss/12040" },
                new Source { Id = 101, Name = "BBC Sport", Url = "http://feeds.bbci.co.uk/sport/rss.xml?edition=uk" },
                new Source { Id = 102, Name = "Goal.com", Url = "https://www.goal.com/feeds/en/news" },
                new Source { Id = 103, Name = "Bleacher Report", Url = "https://bleacherreport.com/articles/feed" },
                new Source { Id = 104, Name = "FIFA News", Url = "https://www.fifa.com/rss/index.xml" },
                new Source { Id = 105, Name = "UEFA News", Url = "https://www.uefa.com/rssfeed" }
            );
        }
    }
}
