using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MyNews.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Sources",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Section = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sources", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordResetToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PasswordResetTokenExpires = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NewsItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Section = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PublishedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FetchedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SourceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewsItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NewsItems_Sources_SourceId",
                        column: x => x.SourceId,
                        principalTable: "Sources",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSectionPreferences",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SectionType = table.Column<int>(type: "int", nullable: false),
                    IsSelected = table.Column<bool>(type: "bit", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSectionPreferences", x => new { x.UserId, x.SectionType });
                    table.ForeignKey(
                        name: "FK_UserSectionPreferences_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserNewsReads",
                columns: table => new
                {
                    NewsItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReadAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    HasClickedTitle = table.Column<bool>(type: "bit", nullable: false),
                    HasClickedLink = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserNewsReads", x => new { x.UserId, x.NewsItemId });
                    table.ForeignKey(
                        name: "FK_UserNewsReads_NewsItems_NewsItemId",
                        column: x => x.NewsItemId,
                        principalTable: "NewsItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserNewsReads_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Sources",
                columns: new[] { "Id", "Name", "Section", "Url" },
                values: new object[,]
                {
                    { 25, "BBC News", null, "http://feeds.bbci.co.uk/news/rss.xml" },
                    { 26, "CNN", null, "http://rss.cnn.com/rss/cnn_topstories.rss" },
                    { 27, "Reuters", null, "http://feeds.reuters.com/reuters/topNews" },
                    { 28, "The New York Times", null, "http://rss.nytimes.com/services/xml/rss/nyt/HomePage.xml" },
                    { 29, "The Guardian", null, "https://www.theguardian.com/world/rss" },
                    { 30, "Associated Press", null, "https://apnews.com/rss" },
                    { 31, "NBC News", null, "http://feeds.nbcnews.com/nbcnews/public/news" },
                    { 32, "USA Today", null, "http://rssfeeds.usatoday.com/usatoday-NewsTopStories" },
                    { 33, "Fox News", null, "http://feeds.foxnews.com/foxnews/latest" },
                    { 34, "HuffPost", null, "https://www.huffpost.com/section/front-page/feed" },
                    { 35, "Bloomberg", null, "https://www.bloomberg.com/feed/podcast/etf-report.xml" },
                    { 36, "Yahoo News", null, "https://news.yahoo.com/rss/" },
                    { 37, "Politico", null, "https://www.politico.com/rss/politicopicks.xml" },
                    { 38, "The Verge", null, "https://www.theverge.com/rss/index.xml" },
                    { 39, "TechCrunch", null, "http://feeds.feedburner.com/TechCrunch/" },
                    { 40, "Wired", null, "https://www.wired.com/feed/rss" },
                    { 41, "Mashable", null, "http://feeds.mashable.com/Mashable" },
                    { 42, "Business Insider", null, "https://www.businessinsider.com/rss" },
                    { 43, "Bloomberg Technology", null, "https://www.bloomberg.com/feed/podcast/technology.xml" },
                    { 44, "The Japan Times", null, "https://www.japantimes.co.jp/feed/" },
                    { 45, "NHK World", null, "https://www3.nhk.or.jp/rss/news/cat0.xml" },
                    { 46, "Asahi Shimbun", null, "http://www.asahi.com/rss/asahi/newsheadlines.rdf" },
                    { 47, "Mainichi Shimbun", null, "https://mainichi.jp/rss/etc/flashnews.xml" },
                    { 48, "Kyodo News", null, "https://english.kyodonews.net/rss/news.xml" },
                    { 49, "China Daily", null, "http://www.chinadaily.com.cn/rss/china_rss.xml" },
                    { 50, "Xinhua News", null, "http://www.xinhuanet.com/english/rss/worldrss.xml" },
                    { 51, "Global Times", null, "https://www.globaltimes.cn/rss/china.xml" },
                    { 52, "Caixin Global", null, "https://www.caixinglobal.com/feed/" },
                    { 53, "Korea Herald", null, "http://www.koreaherald.com/rss/018015000000/rssfeed.xml" },
                    { 54, "The Times of India", null, "https://timesofindia.indiatimes.com/rssfeeds/-2128936835.cms" },
                    { 55, "Hindustan Times", null, "https://www.hindustantimes.com/rss/topnews/rssfeed.xml" },
                    { 56, "Der Spiegel", null, "https://www.spiegel.de/international/index.rss" },
                    { 57, "Die Zeit", null, "https://www.zeit.de/index.rss" },
                    { 58, "Frankfurter Allgemeine", null, "https://www.faz.net/rss/aktuell/" },
                    { 59, "Süddeutsche Zeitung", null, "https://www.sueddeutsche.de/rss" },
                    { 60, "Le Monde", null, "https://www.lemonde.fr/rss/une.xml" },
                    { 61, "Le Figaro", null, "http://www.lefigaro.fr/rss/figaro_actualites.xml" },
                    { 62, "France 24", null, "https://www.france24.com/en/rss" },
                    { 63, "Corriere della Sera", null, "https://www.corriere.it/rss/homepage.xml" },
                    { 64, "La Repubblica", null, "https://www.repubblica.it/rss/homepage/rss2.0.xml" },
                    { 65, "El País", null, "https://feeds.elpais.com/mrss-s/pages/ep/site/elpais.com/portada" },
                    { 66, "El Mundo", null, "https://www.elmundo.es/rss/portada.xml" },
                    { 67, "Público", null, "https://feeds.publico.pt/rss/home" },
                    { 68, "Swiss Info", null, "https://www.swissinfo.ch/rss" },
                    { 69, "Scientific American", null, "https://www.scientificamerican.com/feed/" },
                    { 70, "New Scientist", null, "https://www.newscientist.com/feed/home/" },
                    { 71, "Nature News (US)", null, "https://www.nature.com/news/rss" },
                    { 72, "TechRadar", null, "https://www.techradar.com/rss" },
                    { 73, "Engadget", null, "https://www.engadget.com/rss.xml" },
                    { 74, "PCMag", null, "https://www.pcmag.com/feeds/all-news" },
                    { 75, "The Independent", null, "https://www.independent.co.uk/news/uk/rss" },
                    { 76, "Financial Times", null, "https://www.ft.com/?format=rss" },
                    { 77, "Daily Mail", null, "https://www.dailymail.co.uk/articles.rss" },
                    { 78, "CBC News", null, "https://www.cbc.ca/cmlink/rss-canada" },
                    { 79, "ABC News AU", null, "https://www.abc.net.au/news/feed/51120/rss.xml" },
                    { 80, "SBS News", null, "https://www.sbs.com.au/news/feeds/rss/" },
                    { 81, "NZ Herald", null, "https://www.nzherald.co.nz/rss/" },
                    { 82, "RT News", null, "https://www.rt.com/rss/news/" },
                    { 83, "Globo", null, "https://g1.globo.com/rss/g1/" },
                    { 84, "Clarin", null, "https://www.clarin.com/rss/" },
                    { 85, "AllAfrica", null, "https://allafrica.com/tools/headlines/rss/AllAfrica_headlines.xml" },
                    { 86, "News24 South Africa", null, "https://www.news24.com/rss" },
                    { 87, "Al Jazeera", null, "https://www.aljazeera.com/xml/rss/all.xml" },
                    { 88, "Haaretz", null, "https://www.haaretz.com/cmlink/1.6401109" },
                    { 89, "NASA RSS", null, "https://www.nasa.gov/rss/dyn/breaking_news.rss" },
                    { 90, "ESA News", null, "https://www.esa.int/rssfeed" },
                    { 91, "MIT Technology Review", null, "https://www.technologyreview.com/feed/" },
                    { 92, "Nature", null, "https://www.nature.com/subjects/news/rss" },
                    { 93, "Science Daily", null, "https://www.sciencedaily.com/rss/top/science.xml" },
                    { 94, "Ars Technica", null, "http://feeds.arstechnica.com/arstechnica/index" },
                    { 95, "OpenAI Blog", null, "https://openai.com/blog/rss/" },
                    { 96, "DeepMind Blog", null, "https://deepmind.com/blog/feed/rss" },
                    { 97, "MIT CSAIL News", null, "https://www.csail.mit.edu/news/rss.xml" },
                    { 98, "AI Trends", null, "https://www.aitrends.com/feed/" },
                    { 99, "ESPN", null, "http://www.espn.com/espn/rss/news" },
                    { 100, "Sky Sports", null, "http://www.skysports.com/rss/12040" },
                    { 101, "BBC Sport", null, "http://feeds.bbci.co.uk/sport/rss.xml?edition=uk" },
                    { 102, "Goal.com", null, "https://www.goal.com/feeds/en/news" },
                    { 103, "Bleacher Report", null, "https://bleacherreport.com/articles/feed" },
                    { 104, "FIFA News", null, "https://www.fifa.com/rss/index.xml" },
                    { 105, "UEFA News", null, "https://www.uefa.com/rssfeed" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_NewsItems_SourceId",
                table: "NewsItems",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_NewsItems_Title_SourceId",
                table: "NewsItems",
                columns: new[] { "Title", "SourceId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserNewsReads_NewsItemId",
                table: "UserNewsReads",
                column: "NewsItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserNewsReads");

            migrationBuilder.DropTable(
                name: "UserSectionPreferences");

            migrationBuilder.DropTable(
                name: "NewsItems");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Sources");
        }
    }
}
