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
        public DbSet<UserSectionPreference> UserSectionPreference { get; set; }
        public DbSet<UserSourcePreferences> UserSourcePreferences { get; set; }
        public DbSet<NewsTranslation> NewsTranslations { get; set; }
        public DbSet<UserActivity> UserActivities { get; set; }

        // Seed initial data
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().HasQueryFilter(u => !u.IsDeleted);
            modelBuilder.Entity<Source>().ToTable("Sources");
            modelBuilder.Entity<NewsItem>().ToTable("NewsItems");

            // Relationships
            modelBuilder.Entity<UserSectionPreference>()
                .HasKey(p => new { p.UserId, p.SectionType });

            modelBuilder.Entity<UserSectionPreference>()
                .HasOne(p => p.User)
                .WithMany(u => u.UserSectionPreference)
                .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<UserSectionPreference>()
                .HasQueryFilter(p => !p.User.IsDeleted);

            modelBuilder.Entity<UserSectionPreference>()
                .Property(p => p.SectionType)
                .HasConversion<int>();

            modelBuilder.Entity<UserSectionPreference>()
                .Property(p => p.OrderIndex)
                .HasDefaultValue(0);

            modelBuilder.Entity<UserSourcePreferences>()
                .HasKey(p => new { p.UserId, p.SourceId });

            modelBuilder.Entity<UserSourcePreferences>()
                .HasOne(p => p.User)
                .WithMany(u => u.UserSourcePreferences)
                .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<UserSourcePreferences>()
                .HasQueryFilter(p => !p.User.IsDeleted);

            modelBuilder.Entity<UserNewsRead>()
                .HasKey(unr => new { unr.UserId, unr.NewsItemId });

            modelBuilder.Entity<UserNewsRead>()
                .HasOne(unr => unr.User)
                .WithMany(u => u.NewsReads)
                .HasForeignKey(unr => unr.UserId);

            modelBuilder.Entity<UserNewsRead>()
                .HasQueryFilter(p => !p.User.IsDeleted);

            modelBuilder.Entity<UserNewsRead>()
                .HasOne(unr => unr.NewsItem)
                .WithMany(n => n.UserReads)
                .HasForeignKey(unr => unr.NewsItemId);

            modelBuilder.Entity<NewsTranslation>()
                .HasOne(nt => nt.NewsItem)
                .WithMany(n => n.Translations)
                .HasForeignKey(nt => nt.NewsItemId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<NewsTranslation>()
                .HasIndex(nt => new { nt.NewsItemId, nt.LanguageCode })
                .IsUnique();

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

            modelBuilder.Entity<NewsItem>()
                .HasIndex(n => new { n.Title, n.SourceId })
                .IsUnique();

            modelBuilder.Entity<UserActivity>()
                .HasKey(a => a.UserId);

            modelBuilder.Entity<UserActivity>()
                .HasOne(a => a.User)
                .WithOne()
                .HasForeignKey<UserActivity>(a => a.UserId);

            modelBuilder.Entity<UserActivity>()
                .HasQueryFilter(p => !p.User.IsDeleted);

            modelBuilder.Entity<Source>().HasData(
                // ========================
                // Bulgaria
                // ========================
                new Source { Id = 1, Name = "Dnevnik", Url = "https://www.dnevnik.bg/rss/" },
                new Source { Id = 2, Name = "Capital", Url = "https://www.capital.bg/rss/" },
                new Source { Id = 3, Name = "Mediapool", Url = "https://www.mediapool.bg/rss" },
                new Source { Id = 4, Name = "Actualno", Url = "https://www.actualno.com/rss" },
                new Source { Id = 5, Name = "BTA", Url = "https://www.bta.bg/bg/rss/free" },
                new Source { Id = 6, Name = "SofiaGlobe", Url = "http://feeds.feedburner.com/TheSofiaGlobe" },
                new Source { Id = 7, Name = "Pogled", Url = "https://pogled.info/rss" },
                new Source { Id = 8, Name = "BurgasNews", Url = "https://www.burgasnews.com/feed" },
                new Source { Id = 9, Name = "Gong", Url = "https://gong.bg/rss" },
                new Source { Id = 10, Name = "Investor", Url = "https://www.investor.bg/rss/news" },
                new Source { Id = 11, Name = "Vesti", Url = "https://www.vesti.bg/rss" },
                new Source { Id = 12, Name = "Dnes", Url = "https://www.dnes.bg/rss/news" },
                new Source { Id = 13, Name = "Blitz", Url = "https://www.blitz.bg/rss" },
                new Source { Id = 14, Name = "Standart", Url = "https://www.standartnews.com/rss" },
                new Source { Id = 15, Name = "Banker", Url = "https://banker.bg/feed/" },

                // ========================
                // Global / English
                // ========================
                new Source { Id = 16, Name = "BBC News", Url = "http://feeds.bbci.co.uk/news/rss.xml" },
                new Source { Id = 17, Name = "BBC Technology", Url = "http://feeds.bbci.co.uk/news/technology/rss.xml" },
                new Source { Id = 18, Name = "NPR News", Url = "https://feeds.npr.org/1001/rss.xml" },
                new Source { Id = 19, Name = "Nature – Latest Research", Url = "https://www.nature.com/nature.rss" },
                new Source { Id = 20, Name = "NY Times World", Url = "https://rss.nytimes.com/services/xml/rss/nyt/World.xml" },
                new Source { Id = 21, Name = "NY Times Technology", Url = "https://rss.nytimes.com/services/xml/rss/nyt/Technology.xml" },
                new Source { Id = 22, Name = "CNN", Url = "http://rss.cnn.com/rss/cnn_topstories.rss" },
                new Source { Id = 23, Name = "The New York Times", Url = "http://rss.nytimes.com/services/xml/rss/nyt/HomePage.xml" },
                new Source { Id = 24, Name = "NBC News", Url = "http://feeds.nbcnews.com/nbcnews/public/news" },
                new Source { Id = 25, Name = "Fox News", Url = "http://feeds.foxnews.com/foxnews/latest" },
                new Source { Id = 26, Name = "Wired", Url = "https://www.wired.com/feed/rss" },
                new Source { Id = 27, Name = "Mashable", Url = "http://feeds.mashable.com/Mashable" },
                new Source { Id = 28, Name = "Business Insider", Url = "https://www.businessinsider.com/rss" },
                new Source { Id = 29, Name = "Yahoo News", Url = "https://news.yahoo.com/rss/" },
                new Source { Id = 30, Name = "Politico", Url = "https://www.politico.com/rss/politicopicks.xml" },
                new Source { Id = 31, Name = "The Verge", Url = "https://www.theverge.com/rss/index.xml" },
                new Source { Id = 32, Name = "TechCrunch", Url = "http://feeds.feedburner.com/TechCrunch/" },

                // ========================
                // Japan
                // ========================
                new Source { Id = 33, Name = "The Japan Times", Url = "https://www.japantimes.co.jp/feed/" },
                new Source { Id = 34, Name = "NHK World", Url = "https://www3.nhk.or.jp/rss/news/cat0.xml" },
                new Source { Id = 35, Name = "Asahi Shimbun", Url = "http://www.asahi.com/rss/asahi/newsheadlines.rdf" },

                // ========================
                // China
                // ========================
                new Source { Id = 36, Name = "China Daily", Url = "http://www.chinadaily.com.cn/rss/china_rss.xml" },
                new Source { Id = 37, Name = "Xinhua News", Url = "http://www.xinhuanet.com/english/rss/worldrss.xml" },
                new Source { Id = 38, Name = "SCMP (South China Morning Post)", Url = "https://www.scmp.com/rss/91/feed" },

                // ========================
                // South Korea
                // ========================
                new Source { Id = 39, Name = "Korea Herald - All Storeis", Url = "https://www.koreaherald.com/rss/newsAll" },
                new Source { Id = 41, Name = "Korea Herald - National", Url = "https://www.koreaherald.com/rss/kh_National" },
                new Source { Id = 42, Name = "Korea Herald - Business", Url = "https://www.koreaherald.com/rss/kh_Business" },
                new Source { Id = 43, Name = "Korea Herald - Life & Culture", Url = "https://www.koreaherald.com/rss/kh_LifenCulture" },
                new Source { Id = 44, Name = "Korea Herald - Sports", Url = "https://www.koreaherald.com/rss/kh_Sports" },
                new Source { Id = 45, Name = "Korea Herald - World", Url = "https://www.koreaherald.com/rss/kh_World" },
                new Source { Id = 46, Name = "Korea Times – Sports", Url = "https://www.koreatimes.co.kr/www/rss/sports.xml" },

                // ========================
                // India
                // ========================                
                new Source { Id = 47, Name = "Hindustan Times - US Sports", Url = "https://www.hindustantimes.com/feeds/rss/sports/us-sports/rssfeed.xml" },
                new Source { Id = 48, Name = "Hindustan Times - Music", Url = "https://www.hindustantimes.com/feeds/rss/entertainment/music/rssfeed.xml" },
                new Source { Id = 49, Name = "Hindustan Times - Football", Url = "https://www.hindustantimes.com/feeds/rss/sports/football/rssfeed.xml" },
                new Source { Id = 50, Name = "Hindustan Times - Other Sports", Url = "https://www.hindustantimes.com/feeds/rss/sports/others/rssfeed.xml" },
                new Source { Id = 51, Name = "Hindustan Times - Cinema", Url = "https://www.hindustantimes.com/feeds/rss/htcity/cinema/rssfeed.xml" },
                new Source { Id = 52, Name = "Hindustan Times - Trips & Tours", Url = "https://www.hindustantimes.com/feeds/rss/htcity/trips-tours/rssfeed.xml" },
                new Source { Id = 53, Name = "Hindustan Times - Tennis", Url = "https://www.hindustantimes.com/feeds/rss/sports/tennis/rssfeed.xml" },
                new Source { Id = 54, Name = "Hindustan Times - Sports", Url = "https://www.hindustantimes.com/feeds/rss/sports/rssfeed.xml" },
                new Source { Id = 55, Name = "Hindustan Times - Cricket", Url = "https://www.hindustantimes.com/feeds/rss/cricket/rssfeed.xml" },
                new Source { Id = 56, Name = "Hindustan Times - Business", Url = "https://www.hindustantimes.com/feeds/rss/business/rssfeed.xml" },
                new Source { Id = 57, Name = "Hindustan Times - News", Url = "https://www.hindustantimes.com/feeds/rss/india-news/rssfeed.xml" },
                new Source { Id = 58, Name = "Hindustan Times - Health", Url = "https://www.hindustantimes.com/feeds/rss/lifestyle/health/rssfeed.xml" },
                new Source { Id = 59, Name = "Hindustan Times - Entertainment Others", Url = "https://www.hindustantimes.com/feeds/rss/entertainment/others/rssfeed.xml" },
                new Source { Id = 60, Name = "Hindustan Times - Entertainment", Url = "https://www.hindustantimes.com/feeds/rss/entertainment/rssfeed.xml" },
                new Source { Id = 61, Name = "Hindustan Times - Festivals", Url = "https://www.hindustantimes.com/feeds/rss/lifestyle/festivals/rssfeed.xml" },
                new Source { Id = 62, Name = "Hindustan Times - Bollywood", Url = "https://www.hindustantimes.com/feeds/rss/entertainment/bollywood/rssfeed.xml" },
                new Source { Id = 63, Name = "Hindustan Times - Hollywood", Url = "https://www.hindustantimes.com/feeds/rss/entertainment/hollywood/rssfeed.xml" },
                new Source { Id = 64, Name = "Hindustan Times - Technology", Url = "https://www.hindustantimes.com/feeds/rss/technology/rssfeed.xml" },
                new Source { Id = 65, Name = "Hindustan Times - Astrology", Url = "https://www.hindustantimes.com/feeds/rss/astrology/rssfeed.xml" },
                new Source { Id = 66, Name = "The Times of India", Url = "https://timesofindia.indiatimes.com/rssfeeds/-2128936835.cms" },

                // ========================
                // Germany
                // ========================
                new Source { Id = 67, Name = "Der Spiegel", Url = "https://www.spiegel.de/international/index.rss" },
                new Source { Id = 68, Name = "Frankfurter Allgemeine", Url = "https://www.faz.net/rss/aktuell/" },
                new Source { Id = 69, Name = "DW (Deutsche Welle)", Url = "https://rss.dw.com/rdf/rss-en-all" },
                new Source { Id = 70, Name = "Sportschau", Url = "https://www.sportschau.de/sportschauindex100~_type-rss.feed" },
                new Source { Id = 71, Name = "FAZ Feuilleton", Url = "https://www.faz.net/rss/aktuell/feuilleton/" },
                new Source { Id = 72, Name = "Musikexpress", Url = "https://www.musikexpress.de/feed/" },
                new Source { Id = 73, Name = "Rolling Stone DE", Url = "https://www.rollingstone.de/feed/" },

                // ========================
                // France
                // ========================
                new Source { Id = 74, Name = "France 24", Url = "https://www.france24.com/en/rss" },
                new Source { Id = 75, Name = "Le Monde", Url = "https://www.lemonde.fr/rss/une.xml" },
                new Source { Id = 76, Name = "Le Figaro", Url = "http://www.lefigaro.fr/rss/figaro_actualites.xml" },
                new Source { Id = 77, Name = "Le Monde Culture", Url = "https://www.lemonde.fr/culture/rss_full.xml" },
                new Source { Id = 78, Name = "France24 Culture", Url = "https://www.france24.com/fr/culture/rss" },
                new Source { Id = 79, Name = "RMC Sport - Football", Url = "https://rmcsport.bfmtv.com/rss/football/" },
                new Source { Id = 80, Name = "RMC Sport - Coupe du monde", Url = "https://rmcsport.bfmtv.com/rss/football/coupe-du-monde/" },
                new Source { Id = 81, Name = "RMC Sport - Football Euro", Url = "https://rmcsport.bfmtv.com/rss/football/euro/" },
                new Source { Id = 82, Name = "RMC Sport - Rugby", Url = "https://rmcsport.bfmtv.com/rss/rugby/" },
                new Source { Id = 83, Name = "RMC Sport - Rugby Coupe du monde", Url = "https://rmcsport.bfmtv.com/rss/rugby/coupe-du-monde/" },
                new Source { Id = 84, Name = "RMC Sport - Rugby Coupe d'Europe", Url = "https://rmcsport.bfmtv.com/rss/rugby/coupe-d-europe/" },
                new Source { Id = 85, Name = "RMC Sport - Tournoi des VI nations", Url = "https://rmcsport.bfmtv.com/rss/rugby/tournoi-des-6-nations/" },
                new Source { Id = 86, Name = "RMC Sport - Basket", Url = "https://rmcsport.bfmtv.com/rss/basket/" },
                new Source { Id = 87, Name = "RMC Sport - Basket NBA", Url = "https://rmcsport.bfmtv.com/rss/basket/nba/" },
                new Source { Id = 88, Name = "RMC Sport - Tennis", Url = "https://rmcsport.bfmtv.com/rss/tennis/" },
                new Source { Id = 89, Name = "RMC Sport - Tennis Roland Garros", Url = "https://rmcsport.bfmtv.com/rss/tennis/roland-garros/" },
                new Source { Id = 90, Name = "RMC Sport - Cyclisme", Url = "https://rmcsport.bfmtv.com/rss/cyclisme/" },
                new Source { Id = 91, Name = "RMC Sport - Cyclisme Tour de France", Url = "https://rmcsport.bfmtv.com/rss/cyclisme/tour-de-france/" },
                new Source { Id = 92, Name = "RMC Sport - Auto Moto Formule 1", Url = "https://rmcsport.bfmtv.com/rss/auto-moto/f1/" },
                new Source { Id = 93, Name = "RMC Sport - Jeux Olympiques", Url = "https://rmcsport.bfmtv.com/rss/jeux-olympiques/" },
                new Source { Id = 94, Name = "RMC Sport - Ligue des champions", Url = "https://rmcsport.bfmtv.com/rss/football/ligue-des-champions/" },
                new Source { Id = 95, Name = "RMC Sport - Ligue 1", Url = "https://rmcsport.bfmtv.com/rss/football/ligue-1/" },
                new Source { Id = 96, Name = "RMC Sport - Paris sportifs", Url = "https://rmcsport.bfmtv.com/rss/pari-sportif/" },
                new Source { Id = 97, Name = "RMC Sport - Golf", Url = "https://rmcsport.bfmtv.com/rss/golf/" },
                new Source { Id = 98, Name = "RMC Sport - Athlétisme", Url = "https://rmcsport.bfmtv.com/rss/athletisme/" },
                new Source { Id = 99, Name = "RMC Sport - Volleyball", Url = "https://rmcsport.bfmtv.com/rss/volley/" },
                new Source { Id = 100, Name = "RMC Sport - Auto moto", Url = "https://rmcsport.bfmtv.com/rss/auto-moto/" },
                new Source { Id = 101, Name = "RMC Sport - Sports de combat", Url = "https://rmcsport.bfmtv.com/rss/sports-de-combat/" },
                new Source { Id = 102, Name = "RMC Sport - Sports de combat Boxe", Url = "https://rmcsport.bfmtv.com/rss/sports-de-combat/boxe/" },
                new Source { Id = 103, Name = "RMC Sport - Voile", Url = "https://rmcsport.bfmtv.com/rss/voile/" },
                new Source { Id = 104, Name = "RMC Sport - Sport extrêmes", Url = "https://rmcsport.bfmtv.com/rss/sports-extremes/" },
                new Source { Id = 105, Name = "RMC Sport - Sport d'hiver", Url = "https://rmcsport.bfmtv.com/rss/sports-d-hiver/" },
                new Source { Id = 106, Name = "RMC Sport - Sport US", Url = "https://rmcsport.bfmtv.com/rss/sport-us/" },

                // ========================
                // Italy
                // ========================
                new Source { Id = 107, Name = "Corriere della Sera", Url = "https://www.corriere.it/rss/homepage.xml" },
                new Source { Id = 108, Name = "Corriere dello Sport", Url = "https://www.corrieredellosport.it/rss/home" },
                new Source { Id = 109, Name = "La Repubblica", Url = "https://www.repubblica.it/rss/homepage/rss2.0.xml" },
                new Source { Id = 110, Name = "ANSA", Url = "https://www.ansa.it/sito/ansait_rss.xml" },
                new Source { Id = 111, Name = "ANSA Cultura", Url = "https://www.ansa.it/sito/notizie/cultura/cultura_rss.xml" },
                new Source { Id = 112, Name = "Gazzetta dello Sport", Url = "https://www.gazzetta.it/rss/home.xml" },
                new Source { Id = 113, Name = "Repubblica Cultura", Url = "https://www.repubblica.it/rss/cultura/rss2.0.xml" },

                // ========================
                // Spain
                // ========================
                new Source { Id = 114, Name = "El País", Url = "https://feeds.elpais.com/mrss-s/pages/ep/site/elpais.com/portada" },
                new Source { Id = 115, Name = "El Mundo", Url = "https://www.elmundo.es/rss/portada.xml" },
                new Source { Id = 116, Name = "Marca", Url = "https://e00-marca.uecdn.es/rss/portada.xml" },
                new Source { Id = 117, Name = "AS - LaLiga EA Sports", Url = "https://feeds.as.com/mrss-s/pages/as/site/as.com/section/futbol/subsection/primera/" },
                new Source { Id = 118, Name = "AS - Champions League", Url = "https://feeds.as.com/mrss-s/pages/as/site/as.com/section/futbol/subsection/champions/" },
                new Source { Id = 119, Name = "AS - Europa League", Url = "https://feeds.as.com/mrss-s/pages/as/site/as.com/section/futbol/subsection/uefa/" },
                new Source { Id = 120, Name = "AS - Primera", Url = "https://feeds.as.com/mrss-s/list/as/site/as.com/tag/primera_division_real_federacion_espanola_de_futbol_a/" },
                new Source { Id = 121, Name = "AS - Segunda", Url = "https://feeds.as.com/mrss-s/list/as/site/as.com/tag/segunda_division_real_federacion_espanola_futbol_a/" },
                new Source { Id = 122, Name = "AS - Mundial", Url = "https://feeds.as.com/mrss-s/pages/as/site/as.com/section/futbol/subsection/mundial/" },
                new Source { Id = 123, Name = "AS - Eurocopa", Url = "https://feeds.as.com/mrss-s/pages/as/site/as.com/section/futbol/subsection/eurocopa/" },
                new Source { Id = 124, Name = "AS - Fórmula 1", Url = "https://feeds.as.com/mrss-s/pages/as/site/as.com/section/motor/subsection/formula_1/" },
                new Source { Id = 125, Name = "AS - Motociclismo", Url = "https://feeds.as.com/mrss-s/pages/as/site/as.com/section/motor/subsection/motociclismo/" },
                new Source { Id = 126, Name = "AS - Más Motor", Url = "https://feeds.as.com/mrss-s/pages/as/site/as.com/section/motor/subsection/mas_motor/" },
                new Source { Id = 127, Name = "AS - Copa del Rey", Url = "https://feeds.as.com/mrss-s/pages/as/site/as.com/section/baloncesto/subsection/copa_del_rey/" },
                new Source { Id = 128, Name = "AS - NBA", Url = "https://feeds.as.com/mrss-s/pages/as/site/as.com/section/baloncesto/subsection/nba/" },
                new Source { Id = 129, Name = "AS - Euroliga", Url = "https://feeds.as.com/mrss-s/pages/as/site/as.com/section/baloncesto/subsection/euroliga/" },
                new Source { Id = 130, Name = "AS - Eurocup", Url = "https://feeds.as.com/mrss-s/pages/as/site/as.com/section/baloncesto/subsection/eurocup/" },
                new Source { Id = 131, Name = "AS - Mundial", Url = "https://feeds.as.com/mrss-s/pages/as/site/as.com/section/baloncesto/subsection/mundial_baloncesto/" },
                new Source { Id = 132, Name = "AS - Open de Australia", Url = "https://feeds.as.com/mrss-s/pages/as/site/as.com/section/tenis/subsection/open_australia/" },
                new Source { Id = 133, Name = "AS - Roland Garros", Url = "https://feeds.as.com/mrss-s/pages/as/site/as.com/section/tenis/subsection/roland_garros/" },
                new Source { Id = 134, Name = "AS - Wimbledon", Url = "https://feeds.as.com/mrss-s/pages/as/site/as.com/section/tenis/subsection/wimbledon/" },
                new Source { Id = 135, Name = "AS - US Open", Url = "https://feeds.as.com/mrss-s/pages/as/site/as.com/section/tenis/subsection/us_open/" },
                new Source { Id = 136, Name = "AS - Más Tenis", Url = "https://feeds.as.com/mrss-s/pages/as/site/as.com/section/tenis/subsection/mas_tenis/" },
                new Source { Id = 137, Name = "AS - Masters 1000", Url = "https://feeds.as.com/mrss-s/pages/as/site/as.com/section/tenis/subsection/masters_1000/" },
                new Source { Id = 138, Name = "AS - Tour de Francia", Url = "https://feeds.as.com/mrss-s/pages/as/site/as.com/section/ciclismo/subsection/tour_francia/" },
                new Source { Id = 139, Name = "AS - Giro de Italia", Url = "https://feeds.as.com/mrss-s/pages/as/site/as.com/section/ciclismo/subsection/giro_italia/" },
                new Source { Id = 140, Name = "AS - Más Ciclismo", Url = "https://feeds.as.com/mrss-s/pages/as/site/as.com/section/ciclismo/subsection/mas_ciclismo/" },
                new Source { Id = 141, Name = "AS - Atletismo", Url = "https://feeds.as.com/mrss-s/pages/as/site/as.com/section/masdeporte/subsection/atletismo/" },
                new Source { Id = 142, Name = "AS - Polideportivo", Url = "https://feeds.as.com/mrss-s/pages/as/site/as.com/section/masdeporte/subsection/polideportivo/" },
                new Source { Id = 143, Name = "AS - Golf", Url = "https://feeds.as.com/mrss-s/pages/as/site/as.com/section/masdeporte/subsection/golf/" },
                new Source { Id = 144, Name = "AS - NFL", Url = "https://feeds.as.com/mrss-s/pages/as/site/as.com/section/masdeporte/subsection/nfl/" },
                new Source { Id = 145, Name = "AS - Boxeo", Url = "https://feeds.as.com/mrss-s/list/as/site/as.com/tag/boxeo_a/" },
                new Source { Id = 146, Name = "Sport ES - Ciclismo", Url = "https://www.sport.es/es/rss/ciclismo/rss.xml" },
                new Source { Id = 147, Name = "Sport ES - Hockey", Url = "https://www.sport.es/es/rss/hockey/rss.xml" },
                new Source { Id = 148, Name = "Sport ES - Deporte Extremo", Url = "https://www.sport.es/es/rss/deporte-extremo/rss.xml" },
                new Source { Id = 149, Name = "Sport ES - Fútbol América", Url = "https://www.sport.es/es/rss/futbol-america/rss.xml" },
                new Source { Id = 150, Name = "Sport ES - Europa League", Url = "https://www.sport.es/es/rss/europa-league/rss.xml" },
                new Source { Id = 151, Name = "Sport ES - Moto", Url = "https://www.sport.es/es/rss/motor/rss.xml" },
                new Source { Id = 152, Name = "Sport ES - Golf", Url = "https://www.sport.es/es/rss/golf/rss.xml" },
                new Source { Id = 153, Name = "Sport ES - Fútbol Internacional", Url = "https://www.sport.es/es/rss/futbol-internacional/rss.xml" },
                new Source { Id = 154, Name = "Sport ES - NBA", Url = "https://www.sport.es/es/rss/nba/rss.xml" },
                new Source { Id = 155, Name = "Sport ES - Atletismo", Url = "https://www.sport.es/es/rss/atletismo/rss.xml" },
                new Source { Id = 156, Name = "Sport ES - Liga de Campeones", Url = "https://www.sport.es/es/rss/champions-league/rss.xml" },
                new Source { Id = 157, Name = "Sport ES - Euroliga", Url = "https://www.sport.es/es/rss/euroliga/rss.xml" },
                new Source { Id = 158, Name = "Mundo Deportivo", Url = "https://www.mundodeportivo.com/rss/home.xml" },
                new Source { Id = 159, Name = "ABC Cultura", Url = "https://www.abc.es/rss/feeds/abc_Cultura.xml" },
                new Source { Id = 160, Name = "20 Minutos Música", Url = "https://www.20minutos.es/rss/musica/" },
                new Source { Id = 161, Name = "Jenesaispop", Url = "https://jenesaispop.com/feed/" },

                // ========================
                // Portugal
                // ========================
                new Source { Id = 162, Name = "Observador", Url = "https://observador.pt/feed/" },

                // ========================
                // Switzerland
                // ========================
                new Source { Id = 163, Name = "SwissInfo", Url = "https://www.swissinfo.ch/eng/rss/rss" },

                // ========================
                // USA
                // ========================
                new Source { Id = 164, Name = "New Scientist", Url = "https://www.newscientist.com/feed/home/" },
                new Source { Id = 165, Name = "TechRadar", Url = "https://www.techradar.com/rss" },
                new Source { Id = 166, Name = "Engadget", Url = "https://www.engadget.com/rss.xml" },
                new Source { Id = 167, Name = "NPR Top Stories", Url = "https://feeds.npr.org/1001/rss.xml" },
                new Source { Id = 168, Name = "Washington Post World", Url = "https://feeds.washingtonpost.com/rss/world" },
                new Source { Id = 169, Name = "CNN Top Stories", Url = "http://rss.cnn.com/rss/edition.rss" },
                new Source { Id = 170, Name = "ESPN Top Headlines", Url = "https://www.espn.com/espn/rss/news" },
                new Source { Id = 171, Name = "CBS Sports", Url = "https://www.cbssports.com/rss/headlines/" },
                new Source { Id = 172, Name = "NPR Arts", Url = "https://www.npr.org/rss/rss.php?id=1008" },
                new Source { Id = 173, Name = "LA Times Entertainment", Url = "https://www.latimes.com/entertainment-arts/rss2.0.xml" },
                new Source { Id = 174, Name = "NYT Arts", Url = "https://www.nytimes.com/services/xml/rss/nyt/Arts.xml" },
                new Source { Id = 175, Name = "Rolling Stone Music News", Url = "https://www.rollingstone.com/music/music-news/feed/" },
                new Source { Id = 176, Name = "Billboard", Url = "https://www.billboard.com/feed/" },

                // ========================
                // UK
                // ========================
                new Source { Id = 177, Name = "The Independent", Url = "https://www.independent.co.uk/news/uk/rss" },
                new Source { Id = 178, Name = "Financial Times", Url = "https://www.ft.com/?format=rss" },
                new Source { Id = 179, Name = "Daily Mail", Url = "https://www.dailymail.co.uk/articles.rss" },
                new Source { Id = 180, Name = "BBC World", Url = "http://feeds.bbci.co.uk/news/world/rss.xml" },
                new Source { Id = 181, Name = "BBC Sport", Url = "http://feeds.bbci.co.uk/sport/rss.xml?edition=uk" },
                new Source { Id = 182, Name = "BBC Entertainment & Arts", Url = "http://feeds.bbci.co.uk/news/entertainment_and_arts/rss.xml" },
                new Source { Id = 183, Name = "The Guardian World", Url = "https://www.theguardian.com/world/rss" },
                new Source { Id = 184, Name = "The Guardian Technology", Url = "https://www.theguardian.com/uk/technology/rss" },
                new Source { Id = 185, Name = "The Guardian Sport", Url = "https://www.theguardian.com/uk/sport/rss" },
                new Source { Id = 186, Name = "Guardian Music", Url = "https://www.theguardian.com/music/rss" },
                new Source { Id = 187, Name = "GU Culture", Url = "https://www.theguardian.com/uk/culture/rss" },
                new Source { Id = 188, Name = "NME", Url = "https://www.nme.com/rss" },
                new Source { Id = 189, Name = "Pitchfork News", Url = "https://pitchfork.com/feed/feed-news/rss" },

                // ========================
                // Canada
                // ========================
                new Source { Id = 190, Name = "CBC News", Url = "https://www.cbc.ca/cmlink/rss-canada" },
                new Source { Id = 191, Name = "Global News", Url = "https://globalnews.ca/feed/" },

                // ========================
                // Brazil
                // ========================
                new Source { Id = 192, Name = "Globo", Url = "https://g1.globo.com/rss/g1/" },

                // ========================
                // Argentina
                // ========================
                new Source { Id = 193, Name = "Clarin", Url = "https://www.clarin.com/rss/" },

                // ========================
                // Space / Astronomy (Global)
                // ========================
                new Source { Id = 194, Name = "NASA RSS", Url = "https://www.nasa.gov/rss/dyn/breaking_news.rss" },
                new Source { Id = 195, Name = "MIT News - Space", Url = "https://news.mit.edu/topic/mitspace-rss.xml" },
                new Source { Id = 196, Name = "ESA News", Url = "https://www.esa.int/rssfeed/Our_Activities" },
                new Source { Id = 197, Name = "Space", Url = "https://www.space.com/feeds/all" },

                // ========================
                // AI / Robotics / Tech (Global)
                // ========================
                new Source { Id = 198, Name = "MIT Technology Review", Url = "https://www.technologyreview.com/feed/" },
                new Source { Id = 199, Name = "Science Daily", Url = "https://www.sciencedaily.com/rss/top/science.xml" },
                new Source { Id = 201, Name = "Ars Technica", Url = "http://feeds.arstechnica.com/arstechnica/index" },
                new Source { Id = 202, Name = "AI Trends", Url = "https://www.aitrends.com/feed/" },
                new Source { Id = 203, Name = "MIT News - AI", Url = "https://news.mit.edu/topic/mitartificial-intelligence2-rss.xml" },

                // ========================
                // Sports (Global)
                // ========================
                new Source { Id = 204, Name = "ESPN", Url = "http://www.espn.com/espn/rss/news" },
                new Source { Id = 205, Name = "BBC Sport", Url = "http://feeds.bbci.co.uk/sport/rss.xml?edition=uk" },

                // ========================
                // Global / High-Trust News & Media
                // ========================
                new Source { Id = 206, Name = "The Economist", Url = "https://www.economist.com/latest/rss.xml" },
                new Source { Id = 207, Name = "The Washington Post", Url = "https://feeds.washingtonpost.com/rss/politics" },
                new Source { Id = 208, Name = "Los Angeles Times", Url = "https://www.latimes.com/world-nation/rss2.0.xml" },
                new Source { Id = 209, Name = "Financial Post (Canada)", Url = "https://financialpost.com/feed/" },
                new Source { Id = 210, Name = "The Hill", Url = "https://thehill.com/feed/" },
                new Source { Id = 211, Name = "UN News", Url = "https://news.un.org/feed/subscribe/en/news/all/rss.xml" },
                new Source { Id = 212, Name = "OECD Insights", Url = "https://oecd-development-matters.org/feed/" },
                new Source { Id = 213, Name = "UN Environment Programme (UNEP)", Url = "https://www.unep.org/rss.xml" },

                // ========================
                // Science / Research / Academia
                // ========================
                new Source { Id = 214, Name = "Smithsonian Magazine", Url = "https://www.smithsonianmag.com/rss/latest_articles/" },
                new Source { Id = 215, Name = "LiveScience", Url = "https://www.livescience.com/feeds/all" },
                new Source { Id = 216, Name = "Harvard Gazette", Url = "https://news.harvard.edu/feed/" },
                new Source { Id = 217, Name = "Stanford News", Url = "https://news.stanford.edu/feed/" },
                new Source { Id = 218, Name = "MIT News - Latest News", Url = "https://news.mit.edu/rss/feed" },
                new Source { Id = 219, Name = "MIT News - Research", Url = "https://news.mit.edu/rss/research" },
                new Source { Id = 220, Name = "AAAS (Science Magazine) – News", Url = "https://www.science.org/rss/news_current.xml" },
                new Source { Id = 221, Name = "Science Magazine", Url = "https://www.science.org/action/showFeed?type=etoc&feed=rss&jc=science" },
                new Source { Id = 222, Name = "Cell Press News", Url = "https://www.cell.com/cell/current.rss" },

                // ========================
                // Technology & Innovation
                // ========================
                new Source { Id = 223, Name = "ZDNet", Url = "https://www.zdnet.com/news/rss.xml" },
                new Source { Id = 224, Name = "MakeUseOf (MUO)", Url = "https://www.makeuseof.com/feed/" },
                new Source { Id = 225, Name = "Android Authority", Url = "https://www.androidauthority.com/feed/" },
                new Source { Id = 226, Name = "IEEE Spectrum", Url = "https://spectrum.ieee.org/rss/fulltext" },
                new Source { Id = 227, Name = "W3C News", Url = "https://www.w3.org/blog/news/feed/" },
                new Source { Id = 228, Name = "Mozilla Security Blog", Url = "https://blog.mozilla.org/security/feed/" },
                new Source { Id = 229, Name = "CNET", Url = "https://www.cnet.com/rss/news/" },

                // ========================
                // Business / Finance
                // ========================
                new Source { Id = 230, Name = "The Economist – Finance & Economics", Url = "https://www.economist.com/finance-and-economics/rss.xml" },
                new Source { Id = 231, Name = "Financial Times Alphaville", Url = "https://ftalphaville.ft.com/feed/" },
                new Source { Id = 232, Name = "Fortune", Url = "https://fortune.com/feed/" },

                // ========================
                // Europe (General)
                // ========================
                new Source { Id = 233, Name = "EU Observer", Url = "https://euobserver.com/rss" },
                new Source { Id = 234, Name = "Politico EU", Url = "https://www.politico.eu/feed/" },
                new Source { Id = 235, Name = "Euronews", Url = "https://www.euronews.com/rss?level=theme&name=news" },

                // ========================
                // Scandinavia
                // ========================
                new Source { Id = 236, Name = "Dagens Nyheter (Sweden)", Url = "https://www.dn.se/rss/" },
                new Source { Id = 237, Name = "NRK Norway", Url = "https://www.nrk.no/toppsaker.rss" },
                new Source { Id = 238, Name = "Aftenposten (Norway)", Url = "https://www.aftenposten.no/rss" },
                new Source { Id = 239, Name = "The Local Sweden", Url = "https://www.thelocal.se/feeds/rss.php" },

                // ========================
                // Middle East (High-Trust)
                // ========================
                new Source { Id = 240, Name = "The Jerusalem Post", Url = "https://www.jpost.com/Rss/RssFeedsHeadlines.aspx" },
                new Source { Id = 241, Name = "Arab News", Url = "https://www.arabnews.com/rss.xml" },
                new Source { Id = 242, Name = "Al Jazeera", Url = "https://www.aljazeera.com/xml/rss/all.xml" },

                // ========================
                // Africa (Major Newspapers)
                // ========================
                new Source { Id = 243, Name = "Mail & Guardian (South Africa)", Url = "https://mg.co.za/feed/" },
                new Source { Id = 244, Name = "BBC Africa", Url = "http://feeds.bbci.co.uk/news/world/africa/rss.xml" },

                // ========================
                // Australia & Oceania
                // ========================
                new Source { Id = 245, Name = "The Age", Url = "https://www.theage.com.au/rss/feed.xml" },
                new Source { Id = 246, Name = "The Sydney Morning Herald", Url = "https://www.smh.com.au/rss/feed.xml" },
                new Source { Id = 247, Name = "ABC News AU", Url = "https://www.abc.net.au/news/feed/51120/rss.xml" },

                // ========================
                // Other Valuable Global Sources
                // ========================
                new Source { Id = 248, Name = "Phys.org (Science & Tech)", Url = "https://phys.org/rss-feed/" },
                new Source { Id = 249, Name = "MedicalXpress", Url = "https://medicalxpress.com/rss-feed/" },
                new Source { Id = 250, Name = "TechSpot", Url = "https://www.techspot.com/backend.xml" },
                new Source { Id = 251, Name = "Slashdot", Url = "http://rss.slashdot.org/Slashdot/slashdotMain" },
                new Source { Id = 252, Name = "Krebs on Security", Url = "https://krebsonsecurity.com/feed/" },
                new Source { Id = 253, Name = "BleepingComputer", Url = "https://www.bleepingcomputer.com/feed/" },
                new Source { Id = 254, Name = "SecurityWeek", Url = "https://www.securityweek.com/feed/" },
                new Source { Id = 255, Name = "SC Magazine", Url = "https://www.scmagazine.com/rss" },
                new Source { Id = 256, Name = "MIT News - Biology and genetics", Url = "https://news.mit.edu/rss/topic/biology-and-genetics" },
                new Source { Id = 257, Name = "MIT News - Cancer research", Url = "https://news.mit.edu/rss/topic/cancer-research" },
                new Source { Id = 258, Name = "MIT News - Chemical engineering", Url = "https://news.mit.edu/rss/topic/chemical-engineering" },
                new Source { Id = 259, Name = "MIT News - Robotics", Url = "https://news.mit.edu/topic/mitrobotics-rss.xml" },

                // ========================
                // Entertainment / Culture
                // ========================
                new Source { Id = 260, Name = "Variety", Url = "https://variety.com/feed/" },
                new Source { Id = 261, Name = "Rolling Stone", Url = "https://www.rollingstone.com/feed/" },
                new Source { Id = 262, Name = "IGN", Url = "https://feeds.ign.com/ign/all" },
                new Source { Id = 263, Name = "Variety", Url = "https://variety.com/feed/" },

                // ========================
                // High-Trust Climate / Environment
                // ========================
                new Source { Id = 264, Name = "UN Environment Programme", Url = "https://www.unep.org/rss.xml" },
                new Source { Id = 265, Name = "IPCC (Intergovernmental Panel on Climate Change)", Url = "https://www.ipcc.ch/feed/" }
            );
        }
    }
}
