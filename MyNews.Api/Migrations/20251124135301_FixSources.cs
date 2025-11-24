using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MyNews.Api.Migrations
{
    /// <inheritdoc />
    public partial class FixSources : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 40);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Capital", "https://www.capital.bg/rss/" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Mediapool", "https://www.mediapool.bg/rss" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Actualno", "https://www.actualno.com/rss" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Name", "Url" },
                values: new object[] { "BTA", "https://www.bta.bg/bg/rss/free" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Name", "Url" },
                values: new object[] { "SofiaGlobe", "http://feeds.feedburner.com/TheSofiaGlobe" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Pogled", "https://pogled.info/rss" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Gong", "https://gong.bg/rss" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Investor", "https://www.investor.bg/rss/news" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Vesti", "https://www.vesti.bg/rss" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Dnes", "https://www.dnes.bg/rss/news" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Blitz", "https://www.blitz.bg/rss" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Standart", "https://www.standartnews.com/rss" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Banker", "https://banker.bg/feed/" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "Name", "Url" },
                values: new object[] { "BBC News", "http://feeds.bbci.co.uk/news/rss.xml" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "Name", "Url" },
                values: new object[] { "BBC Technology", "http://feeds.bbci.co.uk/news/technology/rss.xml" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "Name", "Url" },
                values: new object[] { "NPR News", "https://feeds.npr.org/1001/rss.xml" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Fox News", "http://feeds.foxnews.com/foxnews/latest" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Wired", "https://www.wired.com/feed/rss" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Mashable", "http://feeds.mashable.com/Mashable" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Business Insider", "https://www.businessinsider.com/rss" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Yahoo News", "https://news.yahoo.com/rss/" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 30,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Politico", "https://www.politico.com/rss/politicopicks.xml" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 31,
                columns: new[] { "Name", "Url" },
                values: new object[] { "The Verge", "https://www.theverge.com/rss/index.xml" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 32,
                columns: new[] { "Name", "Url" },
                values: new object[] { "TechCrunch", "http://feeds.feedburner.com/TechCrunch/" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 33,
                columns: new[] { "Name", "Url" },
                values: new object[] { "The Japan Times", "https://www.japantimes.co.jp/feed/" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 34,
                columns: new[] { "Name", "Url" },
                values: new object[] { "NHK World", "https://www3.nhk.or.jp/rss/news/cat0.xml" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 35,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Asahi Shimbun", "http://www.asahi.com/rss/asahi/newsheadlines.rdf" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 36,
                columns: new[] { "Name", "Url" },
                values: new object[] { "China Daily", "http://www.chinadaily.com.cn/rss/china_rss.xml" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 37,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Xinhua News", "http://www.xinhuanet.com/english/rss/worldrss.xml" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 38,
                columns: new[] { "Name", "Url" },
                values: new object[] { "SCMP (South China Morning Post)", "https://www.scmp.com/rss/91/feed" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 39,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Korea Herald - All Storeis", "https://www.koreaherald.com/rss/newsAll" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 41,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Korea Herald - National", "https://www.koreaherald.com/rss/kh_National" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 42,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Korea Herald - Business", "https://www.koreaherald.com/rss/kh_Business" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 43,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Korea Herald - Life & Culture", "https://www.koreaherald.com/rss/kh_LifenCulture" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 44,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Korea Herald - Sports", "https://www.koreaherald.com/rss/kh_Sports" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 45,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Korea Herald - World", "https://www.koreaherald.com/rss/kh_World" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 46,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Korea Times – Sports", "https://www.koreatimes.co.kr/www/rss/sports.xml" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 47,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Hindustan Times - US Sports", "https://www.hindustantimes.com/feeds/rss/sports/us-sports/rssfeed.xml" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 48,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Hindustan Times - Music", "https://www.hindustantimes.com/feeds/rss/entertainment/music/rssfeed.xml" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 49,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Hindustan Times - Football", "https://www.hindustantimes.com/feeds/rss/sports/football/rssfeed.xml" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 50,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Hindustan Times - Other Sports", "https://www.hindustantimes.com/feeds/rss/sports/others/rssfeed.xml" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 51,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Hindustan Times - Cinema", "https://www.hindustantimes.com/feeds/rss/htcity/cinema/rssfeed.xml" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 52,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Hindustan Times - Trips & Tours", "https://www.hindustantimes.com/feeds/rss/htcity/trips-tours/rssfeed.xml" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 53,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Hindustan Times - Tennis", "https://www.hindustantimes.com/feeds/rss/sports/tennis/rssfeed.xml" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 54,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Hindustan Times - Sports", "https://www.hindustantimes.com/feeds/rss/sports/rssfeed.xml" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 55,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Hindustan Times - Cricket", "https://www.hindustantimes.com/feeds/rss/cricket/rssfeed.xml" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 56,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Hindustan Times - Business", "https://www.hindustantimes.com/feeds/rss/business/rssfeed.xml" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 57,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Hindustan Times - News", "https://www.hindustantimes.com/feeds/rss/india-news/rssfeed.xml" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 58,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Hindustan Times - Health", "https://www.hindustantimes.com/feeds/rss/lifestyle/health/rssfeed.xml" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 59,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Hindustan Times - Entertainment Others", "https://www.hindustantimes.com/feeds/rss/entertainment/others/rssfeed.xml" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 60,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Hindustan Times - Entertainment", "https://www.hindustantimes.com/feeds/rss/entertainment/rssfeed.xml" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 61,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Hindustan Times - Festivals", "https://www.hindustantimes.com/feeds/rss/lifestyle/festivals/rssfeed.xml" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 62,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Hindustan Times - Bollywood", "https://www.hindustantimes.com/feeds/rss/entertainment/bollywood/rssfeed.xml" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 63,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Hindustan Times - Hollywood", "https://www.hindustantimes.com/feeds/rss/entertainment/hollywood/rssfeed.xml" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 64,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Hindustan Times - Technology", "https://www.hindustantimes.com/feeds/rss/technology/rssfeed.xml" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 65,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Hindustan Times - Astrology", "https://www.hindustantimes.com/feeds/rss/astrology/rssfeed.xml" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 66,
                columns: new[] { "Name", "Url" },
                values: new object[] { "The Times of India", "https://timesofindia.indiatimes.com/rssfeeds/-2128936835.cms" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 67,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Der Spiegel", "https://www.spiegel.de/international/index.rss" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 68,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Frankfurter Allgemeine", "https://www.faz.net/rss/aktuell/" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 69,
                columns: new[] { "Name", "Url" },
                values: new object[] { "DW (Deutsche Welle)", "https://rss.dw.com/rdf/rss-en-all" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 70,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Sportschau", "https://www.sportschau.de/sportschauindex100~_type-rss.feed" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 71,
                columns: new[] { "Name", "Url" },
                values: new object[] { "FAZ Feuilleton", "https://www.faz.net/rss/aktuell/feuilleton/" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 72,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Musikexpress", "https://www.musikexpress.de/feed/" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 73,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Rolling Stone DE", "https://www.rollingstone.de/feed/" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 74,
                columns: new[] { "Name", "Url" },
                values: new object[] { "France 24", "https://www.france24.com/en/rss" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 75,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Le Monde", "https://www.lemonde.fr/rss/une.xml" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 76,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Le Figaro", "http://www.lefigaro.fr/rss/figaro_actualites.xml" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 77,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Le Monde Culture", "https://www.lemonde.fr/culture/rss_full.xml" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 78,
                columns: new[] { "Name", "Url" },
                values: new object[] { "France24 Culture", "https://www.france24.com/fr/culture/rss" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 79,
                columns: new[] { "Name", "Url" },
                values: new object[] { "RMC Sport - Football", "https://rmcsport.bfmtv.com/rss/football/" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 80,
                columns: new[] { "Name", "Url" },
                values: new object[] { "RMC Sport - Coupe du monde", "https://rmcsport.bfmtv.com/rss/football/coupe-du-monde/" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 81,
                columns: new[] { "Name", "Url" },
                values: new object[] { "RMC Sport - Football Euro", "https://rmcsport.bfmtv.com/rss/football/euro/" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 82,
                columns: new[] { "Name", "Url" },
                values: new object[] { "RMC Sport - Rugby", "https://rmcsport.bfmtv.com/rss/rugby/" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 83,
                columns: new[] { "Name", "Url" },
                values: new object[] { "RMC Sport - Rugby Coupe du monde", "https://rmcsport.bfmtv.com/rss/rugby/coupe-du-monde/" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 84,
                columns: new[] { "Name", "Url" },
                values: new object[] { "RMC Sport - Rugby Coupe d'Europe", "https://rmcsport.bfmtv.com/rss/rugby/coupe-d-europe/" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 85,
                columns: new[] { "Name", "Url" },
                values: new object[] { "RMC Sport - Tournoi des VI nations", "https://rmcsport.bfmtv.com/rss/rugby/tournoi-des-6-nations/" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 86,
                columns: new[] { "Name", "Url" },
                values: new object[] { "RMC Sport - Basket", "https://rmcsport.bfmtv.com/rss/basket/" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 87,
                columns: new[] { "Name", "Url" },
                values: new object[] { "RMC Sport - Basket NBA", "https://rmcsport.bfmtv.com/rss/basket/nba/" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 88,
                columns: new[] { "Name", "Url" },
                values: new object[] { "RMC Sport - Tennis", "https://rmcsport.bfmtv.com/rss/tennis/" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 89,
                columns: new[] { "Name", "Url" },
                values: new object[] { "RMC Sport - Tennis Roland Garros", "https://rmcsport.bfmtv.com/rss/tennis/roland-garros/" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 90,
                columns: new[] { "Name", "Url" },
                values: new object[] { "RMC Sport - Cyclisme", "https://rmcsport.bfmtv.com/rss/cyclisme/" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 91,
                columns: new[] { "Name", "Url" },
                values: new object[] { "RMC Sport - Cyclisme Tour de France", "https://rmcsport.bfmtv.com/rss/cyclisme/tour-de-france/" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 92,
                columns: new[] { "Name", "Url" },
                values: new object[] { "RMC Sport - Auto Moto Formule 1", "https://rmcsport.bfmtv.com/rss/auto-moto/f1/" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 93,
                columns: new[] { "Name", "Url" },
                values: new object[] { "RMC Sport - Jeux Olympiques", "https://rmcsport.bfmtv.com/rss/jeux-olympiques/" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 94,
                columns: new[] { "Name", "Url" },
                values: new object[] { "RMC Sport - Ligue des champions", "https://rmcsport.bfmtv.com/rss/football/ligue-des-champions/" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 95,
                columns: new[] { "Name", "Url" },
                values: new object[] { "RMC Sport - Ligue 1", "https://rmcsport.bfmtv.com/rss/football/ligue-1/" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 96,
                columns: new[] { "Name", "Url" },
                values: new object[] { "RMC Sport - Paris sportifs", "https://rmcsport.bfmtv.com/rss/pari-sportif/" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 97,
                columns: new[] { "Name", "Url" },
                values: new object[] { "RMC Sport - Golf", "https://rmcsport.bfmtv.com/rss/golf/" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 98,
                columns: new[] { "Name", "Url" },
                values: new object[] { "RMC Sport - Athlétisme", "https://rmcsport.bfmtv.com/rss/athletisme/" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 99,
                columns: new[] { "Name", "Url" },
                values: new object[] { "RMC Sport - Volleyball", "https://rmcsport.bfmtv.com/rss/volley/" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 100,
                columns: new[] { "Name", "Url" },
                values: new object[] { "RMC Sport - Auto moto", "https://rmcsport.bfmtv.com/rss/auto-moto/" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 101,
                columns: new[] { "Name", "Url" },
                values: new object[] { "RMC Sport - Sports de combat", "https://rmcsport.bfmtv.com/rss/sports-de-combat/" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 102,
                columns: new[] { "Name", "Url" },
                values: new object[] { "RMC Sport - Sports de combat Boxe", "https://rmcsport.bfmtv.com/rss/sports-de-combat/boxe/" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 103,
                columns: new[] { "Name", "Url" },
                values: new object[] { "RMC Sport - Voile", "https://rmcsport.bfmtv.com/rss/voile/" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 104,
                columns: new[] { "Name", "Url" },
                values: new object[] { "RMC Sport - Sport extrêmes", "https://rmcsport.bfmtv.com/rss/sports-extremes/" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 105,
                columns: new[] { "Name", "Url" },
                values: new object[] { "RMC Sport - Sport d'hiver", "https://rmcsport.bfmtv.com/rss/sports-d-hiver/" });

            migrationBuilder.InsertData(
                table: "Sources",
                columns: new[] { "Id", "LanguageCode", "Name", "Section", "Url" },
                values: new object[,]
                {
                    { 8, null, "BurgasNews", null, "https://www.burgasnews.com/feed" },
                    { 19, null, "Nature – Latest Research", null, "https://www.nature.com/nature.rss" },
                    { 20, null, "NY Times World", null, "https://rss.nytimes.com/services/xml/rss/nyt/World.xml" },
                    { 21, null, "NY Times Technology", null, "https://rss.nytimes.com/services/xml/rss/nyt/Technology.xml" },
                    { 22, null, "CNN", null, "http://rss.cnn.com/rss/cnn_topstories.rss" },
                    { 23, null, "The New York Times", null, "http://rss.nytimes.com/services/xml/rss/nyt/HomePage.xml" },
                    { 24, null, "NBC News", null, "http://feeds.nbcnews.com/nbcnews/public/news" },
                    { 106, null, "RMC Sport - Sport US", null, "https://rmcsport.bfmtv.com/rss/sport-us/" },
                    { 107, null, "Corriere della Sera", null, "https://www.corriere.it/rss/homepage.xml" },
                    { 108, null, "Corriere dello Sport", null, "https://www.corrieredellosport.it/rss/home" },
                    { 109, null, "La Repubblica", null, "https://www.repubblica.it/rss/homepage/rss2.0.xml" },
                    { 110, null, "ANSA", null, "https://www.ansa.it/sito/ansait_rss.xml" },
                    { 111, null, "ANSA Cultura", null, "https://www.ansa.it/sito/notizie/cultura/cultura_rss.xml" },
                    { 112, null, "Gazzetta dello Sport", null, "https://www.gazzetta.it/rss/home.xml" },
                    { 113, null, "Repubblica Cultura", null, "https://www.repubblica.it/rss/cultura/rss2.0.xml" },
                    { 114, null, "El País", null, "https://feeds.elpais.com/mrss-s/pages/ep/site/elpais.com/portada" },
                    { 115, null, "El Mundo", null, "https://www.elmundo.es/rss/portada.xml" },
                    { 116, null, "Marca", null, "https://e00-marca.uecdn.es/rss/portada.xml" },
                    { 117, null, "AS - LaLiga EA Sports", null, "https://feeds.as.com/mrss-s/pages/as/site/as.com/section/futbol/subsection/primera/" },
                    { 118, null, "AS - Champions League", null, "https://feeds.as.com/mrss-s/pages/as/site/as.com/section/futbol/subsection/champions/" },
                    { 119, null, "AS - Europa League", null, "https://feeds.as.com/mrss-s/pages/as/site/as.com/section/futbol/subsection/uefa/" },
                    { 120, null, "AS - Primera", null, "https://feeds.as.com/mrss-s/list/as/site/as.com/tag/primera_division_real_federacion_espanola_de_futbol_a/" },
                    { 121, null, "AS - Segunda", null, "https://feeds.as.com/mrss-s/list/as/site/as.com/tag/segunda_division_real_federacion_espanola_futbol_a/" },
                    { 122, null, "AS - Mundial", null, "https://feeds.as.com/mrss-s/pages/as/site/as.com/section/futbol/subsection/mundial/" },
                    { 123, null, "AS - Eurocopa", null, "https://feeds.as.com/mrss-s/pages/as/site/as.com/section/futbol/subsection/eurocopa/" },
                    { 124, null, "AS - Fórmula 1", null, "https://feeds.as.com/mrss-s/pages/as/site/as.com/section/motor/subsection/formula_1/" },
                    { 125, null, "AS - Motociclismo", null, "https://feeds.as.com/mrss-s/pages/as/site/as.com/section/motor/subsection/motociclismo/" },
                    { 126, null, "AS - Más Motor", null, "https://feeds.as.com/mrss-s/pages/as/site/as.com/section/motor/subsection/mas_motor/" },
                    { 127, null, "AS - Copa del Rey", null, "https://feeds.as.com/mrss-s/pages/as/site/as.com/section/baloncesto/subsection/copa_del_rey/" },
                    { 128, null, "AS - NBA", null, "https://feeds.as.com/mrss-s/pages/as/site/as.com/section/baloncesto/subsection/nba/" },
                    { 129, null, "AS - Euroliga", null, "https://feeds.as.com/mrss-s/pages/as/site/as.com/section/baloncesto/subsection/euroliga/" },
                    { 130, null, "AS - Eurocup", null, "https://feeds.as.com/mrss-s/pages/as/site/as.com/section/baloncesto/subsection/eurocup/" },
                    { 131, null, "AS - Mundial", null, "https://feeds.as.com/mrss-s/pages/as/site/as.com/section/baloncesto/subsection/mundial_baloncesto/" },
                    { 132, null, "AS - Open de Australia", null, "https://feeds.as.com/mrss-s/pages/as/site/as.com/section/tenis/subsection/open_australia/" },
                    { 133, null, "AS - Roland Garros", null, "https://feeds.as.com/mrss-s/pages/as/site/as.com/section/tenis/subsection/roland_garros/" },
                    { 134, null, "AS - Wimbledon", null, "https://feeds.as.com/mrss-s/pages/as/site/as.com/section/tenis/subsection/wimbledon/" },
                    { 135, null, "AS - US Open", null, "https://feeds.as.com/mrss-s/pages/as/site/as.com/section/tenis/subsection/us_open/" },
                    { 136, null, "AS - Más Tenis", null, "https://feeds.as.com/mrss-s/pages/as/site/as.com/section/tenis/subsection/mas_tenis/" },
                    { 137, null, "AS - Masters 1000", null, "https://feeds.as.com/mrss-s/pages/as/site/as.com/section/tenis/subsection/masters_1000/" },
                    { 138, null, "AS - Tour de Francia", null, "https://feeds.as.com/mrss-s/pages/as/site/as.com/section/ciclismo/subsection/tour_francia/" },
                    { 139, null, "AS - Giro de Italia", null, "https://feeds.as.com/mrss-s/pages/as/site/as.com/section/ciclismo/subsection/giro_italia/" },
                    { 140, null, "AS - Más Ciclismo", null, "https://feeds.as.com/mrss-s/pages/as/site/as.com/section/ciclismo/subsection/mas_ciclismo/" },
                    { 141, null, "AS - Atletismo", null, "https://feeds.as.com/mrss-s/pages/as/site/as.com/section/masdeporte/subsection/atletismo/" },
                    { 142, null, "AS - Polideportivo", null, "https://feeds.as.com/mrss-s/pages/as/site/as.com/section/masdeporte/subsection/polideportivo/" },
                    { 143, null, "AS - Golf", null, "https://feeds.as.com/mrss-s/pages/as/site/as.com/section/masdeporte/subsection/golf/" },
                    { 144, null, "AS - NFL", null, "https://feeds.as.com/mrss-s/pages/as/site/as.com/section/masdeporte/subsection/nfl/" },
                    { 145, null, "AS - Boxeo", null, "https://feeds.as.com/mrss-s/list/as/site/as.com/tag/boxeo_a/" },
                    { 146, null, "Sport ES - Ciclismo", null, "https://www.sport.es/es/rss/ciclismo/rss.xml" },
                    { 147, null, "Sport ES - Hockey", null, "https://www.sport.es/es/rss/hockey/rss.xml" },
                    { 148, null, "Sport ES - Deporte Extremo", null, "https://www.sport.es/es/rss/deporte-extremo/rss.xml" },
                    { 149, null, "Sport ES - Fútbol América", null, "https://www.sport.es/es/rss/futbol-america/rss.xml" },
                    { 150, null, "Sport ES - Europa League", null, "https://www.sport.es/es/rss/europa-league/rss.xml" },
                    { 151, null, "Sport ES - Moto", null, "https://www.sport.es/es/rss/motor/rss.xml" },
                    { 152, null, "Sport ES - Golf", null, "https://www.sport.es/es/rss/golf/rss.xml" },
                    { 153, null, "Sport ES - Fútbol Internacional", null, "https://www.sport.es/es/rss/futbol-internacional/rss.xml" },
                    { 154, null, "Sport ES - NBA", null, "https://www.sport.es/es/rss/nba/rss.xml" },
                    { 155, null, "Sport ES - Atletismo", null, "https://www.sport.es/es/rss/atletismo/rss.xml" },
                    { 156, null, "Sport ES - Liga de Campeones", null, "https://www.sport.es/es/rss/champions-league/rss.xml" },
                    { 157, null, "Sport ES - Euroliga", null, "https://www.sport.es/es/rss/euroliga/rss.xml" },
                    { 158, null, "Mundo Deportivo", null, "https://www.mundodeportivo.com/rss/home.xml" },
                    { 159, null, "ABC Cultura", null, "https://www.abc.es/rss/feeds/abc_Cultura.xml" },
                    { 160, null, "20 Minutos Música", null, "https://www.20minutos.es/rss/musica/" },
                    { 161, null, "Jenesaispop", null, "https://jenesaispop.com/feed/" },
                    { 162, null, "Observador", null, "https://observador.pt/feed/" },
                    { 163, null, "SwissInfo", null, "https://www.swissinfo.ch/eng/rss/rss" },
                    { 164, null, "New Scientist", null, "https://www.newscientist.com/feed/home/" },
                    { 165, null, "TechRadar", null, "https://www.techradar.com/rss" },
                    { 166, null, "Engadget", null, "https://www.engadget.com/rss.xml" },
                    { 167, null, "NPR Top Stories", null, "https://feeds.npr.org/1001/rss.xml" },
                    { 168, null, "Washington Post World", null, "https://feeds.washingtonpost.com/rss/world" },
                    { 169, null, "CNN Top Stories", null, "http://rss.cnn.com/rss/edition.rss" },
                    { 170, null, "ESPN Top Headlines", null, "https://www.espn.com/espn/rss/news" },
                    { 171, null, "CBS Sports", null, "https://www.cbssports.com/rss/headlines/" },
                    { 172, null, "NPR Arts", null, "https://www.npr.org/rss/rss.php?id=1008" },
                    { 173, null, "LA Times Entertainment", null, "https://www.latimes.com/entertainment-arts/rss2.0.xml" },
                    { 174, null, "NYT Arts", null, "https://www.nytimes.com/services/xml/rss/nyt/Arts.xml" },
                    { 175, null, "Rolling Stone Music News", null, "https://www.rollingstone.com/music/music-news/feed/" },
                    { 176, null, "Billboard", null, "https://www.billboard.com/feed/" },
                    { 177, null, "The Independent", null, "https://www.independent.co.uk/news/uk/rss" },
                    { 178, null, "Financial Times", null, "https://www.ft.com/?format=rss" },
                    { 179, null, "Daily Mail", null, "https://www.dailymail.co.uk/articles.rss" },
                    { 180, null, "BBC World", null, "http://feeds.bbci.co.uk/news/world/rss.xml" },
                    { 181, null, "BBC Sport", null, "http://feeds.bbci.co.uk/sport/rss.xml?edition=uk" },
                    { 182, null, "BBC Entertainment & Arts", null, "http://feeds.bbci.co.uk/news/entertainment_and_arts/rss.xml" },
                    { 183, null, "The Guardian World", null, "https://www.theguardian.com/world/rss" },
                    { 184, null, "The Guardian Technology", null, "https://www.theguardian.com/uk/technology/rss" },
                    { 185, null, "The Guardian Sport", null, "https://www.theguardian.com/uk/sport/rss" },
                    { 186, null, "Guardian Music", null, "https://www.theguardian.com/music/rss" },
                    { 187, null, "GU Culture", null, "https://www.theguardian.com/uk/culture/rss" },
                    { 188, null, "NME", null, "https://www.nme.com/rss" },
                    { 189, null, "Pitchfork News", null, "https://pitchfork.com/feed/feed-news/rss" },
                    { 190, null, "CBC News", null, "https://www.cbc.ca/cmlink/rss-canada" },
                    { 191, null, "Global News", null, "https://globalnews.ca/feed/" },
                    { 192, null, "Globo", null, "https://g1.globo.com/rss/g1/" },
                    { 193, null, "Clarin", null, "https://www.clarin.com/rss/" },
                    { 194, null, "NASA RSS", null, "https://www.nasa.gov/rss/dyn/breaking_news.rss" },
                    { 195, null, "MIT News - Space", null, "https://news.mit.edu/topic/mitspace-rss.xml" },
                    { 196, null, "ESA News", null, "https://www.esa.int/rssfeed/Our_Activities" },
                    { 197, null, "Space", null, "https://www.space.com/feeds/all" },
                    { 198, null, "MIT Technology Review", null, "https://www.technologyreview.com/feed/" },
                    { 199, null, "Science Daily", null, "https://www.sciencedaily.com/rss/top/science.xml" },
                    { 201, null, "Ars Technica", null, "http://feeds.arstechnica.com/arstechnica/index" },
                    { 202, null, "AI Trends", null, "https://www.aitrends.com/feed/" },
                    { 203, null, "MIT News - AI", null, "https://news.mit.edu/topic/mitartificial-intelligence2-rss.xml" },
                    { 204, null, "ESPN", null, "http://www.espn.com/espn/rss/news" },
                    { 205, null, "BBC Sport", null, "http://feeds.bbci.co.uk/sport/rss.xml?edition=uk" },
                    { 206, null, "The Economist", null, "https://www.economist.com/latest/rss.xml" },
                    { 207, null, "The Washington Post", null, "https://feeds.washingtonpost.com/rss/politics" },
                    { 208, null, "Los Angeles Times", null, "https://www.latimes.com/world-nation/rss2.0.xml" },
                    { 209, null, "Financial Post (Canada)", null, "https://financialpost.com/feed/" },
                    { 210, null, "The Hill", null, "https://thehill.com/feed/" },
                    { 211, null, "UN News", null, "https://news.un.org/feed/subscribe/en/news/all/rss.xml" },
                    { 212, null, "OECD Insights", null, "https://oecd-development-matters.org/feed/" },
                    { 213, null, "UN Environment Programme (UNEP)", null, "https://www.unep.org/rss.xml" },
                    { 214, null, "Smithsonian Magazine", null, "https://www.smithsonianmag.com/rss/latest_articles/" },
                    { 215, null, "LiveScience", null, "https://www.livescience.com/feeds/all" },
                    { 216, null, "Harvard Gazette", null, "https://news.harvard.edu/feed/" },
                    { 217, null, "Stanford News", null, "https://news.stanford.edu/feed/" },
                    { 218, null, "MIT News - Latest News", null, "https://news.mit.edu/rss/feed" },
                    { 219, null, "MIT News - Research", null, "https://news.mit.edu/rss/research" },
                    { 220, null, "AAAS (Science Magazine) – News", null, "https://www.science.org/rss/news_current.xml" },
                    { 221, null, "Science Magazine", null, "https://www.science.org/action/showFeed?type=etoc&feed=rss&jc=science" },
                    { 222, null, "Cell Press News", null, "https://www.cell.com/cell/current.rss" },
                    { 223, null, "ZDNet", null, "https://www.zdnet.com/news/rss.xml" },
                    { 224, null, "MakeUseOf (MUO)", null, "https://www.makeuseof.com/feed/" },
                    { 225, null, "Android Authority", null, "https://www.androidauthority.com/feed/" },
                    { 226, null, "IEEE Spectrum", null, "https://spectrum.ieee.org/rss/fulltext" },
                    { 227, null, "W3C News", null, "https://www.w3.org/blog/news/feed/" },
                    { 228, null, "Mozilla Security Blog", null, "https://blog.mozilla.org/security/feed/" },
                    { 229, null, "CNET", null, "https://www.cnet.com/rss/news/" },
                    { 230, null, "The Economist – Finance & Economics", null, "https://www.economist.com/finance-and-economics/rss.xml" },
                    { 231, null, "Financial Times Alphaville", null, "https://ftalphaville.ft.com/feed/" },
                    { 232, null, "Fortune", null, "https://fortune.com/feed/" },
                    { 233, null, "EU Observer", null, "https://euobserver.com/rss" },
                    { 234, null, "Politico EU", null, "https://www.politico.eu/feed/" },
                    { 235, null, "Euronews", null, "https://www.euronews.com/rss?level=theme&name=news" },
                    { 236, null, "Dagens Nyheter (Sweden)", null, "https://www.dn.se/rss/" },
                    { 237, null, "NRK Norway", null, "https://www.nrk.no/toppsaker.rss" },
                    { 238, null, "Aftenposten (Norway)", null, "https://www.aftenposten.no/rss" },
                    { 239, null, "The Local Sweden", null, "https://www.thelocal.se/feeds/rss.php" },
                    { 240, null, "The Jerusalem Post", null, "https://www.jpost.com/Rss/RssFeedsHeadlines.aspx" },
                    { 241, null, "Arab News", null, "https://www.arabnews.com/rss.xml" },
                    { 242, null, "Al Jazeera", null, "https://www.aljazeera.com/xml/rss/all.xml" },
                    { 243, null, "Mail & Guardian (South Africa)", null, "https://mg.co.za/feed/" },
                    { 244, null, "BBC Africa", null, "http://feeds.bbci.co.uk/news/world/africa/rss.xml" },
                    { 245, null, "The Age", null, "https://www.theage.com.au/rss/feed.xml" },
                    { 246, null, "The Sydney Morning Herald", null, "https://www.smh.com.au/rss/feed.xml" },
                    { 247, null, "ABC News AU", null, "https://www.abc.net.au/news/feed/51120/rss.xml" },
                    { 248, null, "Phys.org (Science & Tech)", null, "https://phys.org/rss-feed/" },
                    { 249, null, "MedicalXpress", null, "https://medicalxpress.com/rss-feed/" },
                    { 250, null, "TechSpot", null, "https://www.techspot.com/backend.xml" },
                    { 251, null, "Slashdot", null, "http://rss.slashdot.org/Slashdot/slashdotMain" },
                    { 252, null, "Krebs on Security", null, "https://krebsonsecurity.com/feed/" },
                    { 253, null, "BleepingComputer", null, "https://www.bleepingcomputer.com/feed/" },
                    { 254, null, "SecurityWeek", null, "https://www.securityweek.com/feed/" },
                    { 255, null, "SC Magazine", null, "https://www.scmagazine.com/rss" },
                    { 256, null, "MIT News - Biology and genetics", null, "https://news.mit.edu/rss/topic/biology-and-genetics" },
                    { 257, null, "MIT News - Cancer research", null, "https://news.mit.edu/rss/topic/cancer-research" },
                    { 258, null, "MIT News - Chemical engineering", null, "https://news.mit.edu/rss/topic/chemical-engineering" },
                    { 259, null, "MIT News - Robotics", null, "https://news.mit.edu/topic/mitrobotics-rss.xml" },
                    { 260, null, "Variety", null, "https://variety.com/feed/" },
                    { 261, null, "Rolling Stone", null, "https://www.rollingstone.com/feed/" },
                    { 262, null, "IGN", null, "https://feeds.ign.com/ign/all" },
                    { 263, null, "Variety", null, "https://variety.com/feed/" },
                    { 264, null, "UN Environment Programme", null, "https://www.unep.org/rss.xml" },
                    { 265, null, "IPCC (Intergovernmental Panel on Climate Change)", null, "https://www.ipcc.ch/feed/" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 106);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 107);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 108);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 109);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 110);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 111);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 112);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 113);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 114);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 115);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 116);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 117);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 118);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 119);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 120);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 121);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 122);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 123);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 124);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 125);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 126);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 127);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 128);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 129);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 130);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 131);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 132);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 133);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 134);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 135);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 136);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 137);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 138);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 139);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 140);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 141);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 142);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 143);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 144);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 145);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 146);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 147);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 148);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 149);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 150);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 151);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 152);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 153);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 154);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 155);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 156);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 157);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 158);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 159);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 160);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 161);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 162);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 163);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 164);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 165);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 166);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 167);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 168);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 169);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 170);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 171);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 172);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 173);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 174);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 175);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 176);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 177);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 178);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 179);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 180);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 181);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 182);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 183);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 184);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 185);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 186);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 187);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 188);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 189);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 190);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 191);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 192);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 193);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 194);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 195);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 196);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 197);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 198);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 199);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 201);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 202);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 203);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 204);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 205);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 206);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 207);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 208);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 209);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 210);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 211);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 212);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 213);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 214);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 215);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 216);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 217);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 218);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 219);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 220);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 221);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 222);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 223);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 224);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 225);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 226);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 227);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 228);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 229);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 230);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 231);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 232);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 233);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 234);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 235);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 236);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 237);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 238);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 239);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 240);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 241);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 242);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 243);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 244);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 245);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 246);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 247);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 248);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 249);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 250);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 251);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 252);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 253);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 254);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 255);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 256);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 257);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 258);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 259);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 260);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 261);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 262);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 263);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 264);

            migrationBuilder.DeleteData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 265);

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Novinite", "https://www.novinite.com/rss/news" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Capital", "https://www.capital.bg/rss/" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Mediapool", "https://www.mediapool.bg/rss" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Actualno", "https://www.actualno.com/rss" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Name", "Url" },
                values: new object[] { "24 Chasa", "https://www.24chasa.bg/rss_category/2/novini.html" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Name", "Url" },
                values: new object[] { "BTA", "https://www.bta.bg/bg/rss/free" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Name", "Url" },
                values: new object[] { "SofiaGlobe", "http://feeds.feedburner.com/TheSofiaGlobe" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Pogled", "https://pogled.info/rss" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Name", "Url" },
                values: new object[] { "BurgasNews", "https://www.burgasnews.com/feed" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Gong", "https://gong.bg/rss" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Investor", "https://www.investor.bg/rss/news" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Vesti", "https://www.vesti.bg/rss" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Dnes", "https://www.dnes.bg/rss/news" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Blitz", "https://www.blitz.bg/rss" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Standart", "https://www.standartnews.com/rss" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Banker", "https://banker.bg/feed/" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "Name", "Url" },
                values: new object[] { "BBC News", "http://feeds.bbci.co.uk/news/rss.xml" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "Name", "Url" },
                values: new object[] { "CNN", "http://rss.cnn.com/rss/cnn_topstories.rss" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 27,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Reuters", "http://feeds.reuters.com/reuters/topNews" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 28,
                columns: new[] { "Name", "Url" },
                values: new object[] { "The New York Times", "http://rss.nytimes.com/services/xml/rss/nyt/HomePage.xml" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 29,
                columns: new[] { "Name", "Url" },
                values: new object[] { "The Guardian", "https://www.theguardian.com/world/rss" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 30,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Associated Press", "https://apnews.com/rss" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 31,
                columns: new[] { "Name", "Url" },
                values: new object[] { "NBC News", "http://feeds.nbcnews.com/nbcnews/public/news" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 32,
                columns: new[] { "Name", "Url" },
                values: new object[] { "USA Today", "http://rssfeeds.usatoday.com/usatoday-NewsTopStories" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 33,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Fox News", "http://feeds.foxnews.com/foxnews/latest" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 34,
                columns: new[] { "Name", "Url" },
                values: new object[] { "HuffPost", "https://www.huffpost.com/section/front-page/feed" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 35,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Bloomberg", "https://www.bloomberg.com/feed/podcast/etf-report.xml" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 36,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Yahoo News", "https://news.yahoo.com/rss/" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 37,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Politico", "https://www.politico.com/rss/politicopicks.xml" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 38,
                columns: new[] { "Name", "Url" },
                values: new object[] { "The Verge", "https://www.theverge.com/rss/index.xml" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 39,
                columns: new[] { "Name", "Url" },
                values: new object[] { "TechCrunch", "http://feeds.feedburner.com/TechCrunch/" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 41,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Mashable", "http://feeds.mashable.com/Mashable" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 42,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Business Insider", "https://www.businessinsider.com/rss" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 43,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Bloomberg Technology", "https://www.bloomberg.com/feed/podcast/technology.xml" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 44,
                columns: new[] { "Name", "Url" },
                values: new object[] { "The Japan Times", "https://www.japantimes.co.jp/feed/" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 45,
                columns: new[] { "Name", "Url" },
                values: new object[] { "NHK World", "https://www3.nhk.or.jp/rss/news/cat0.xml" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 46,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Asahi Shimbun", "http://www.asahi.com/rss/asahi/newsheadlines.rdf" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 47,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Mainichi Shimbun", "https://mainichi.jp/rss/etc/flashnews.xml" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 48,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Kyodo News", "https://english.kyodonews.net/rss/news.xml" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 49,
                columns: new[] { "Name", "Url" },
                values: new object[] { "China Daily", "http://www.chinadaily.com.cn/rss/china_rss.xml" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 50,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Xinhua News", "http://www.xinhuanet.com/english/rss/worldrss.xml" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 51,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Global Times", "https://www.globaltimes.cn/rss/china.xml" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 52,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Caixin Global", "https://www.caixinglobal.com/feed/" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 53,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Korea Herald", "http://www.koreaherald.com/rss/018015000000/rssfeed.xml" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 54,
                columns: new[] { "Name", "Url" },
                values: new object[] { "The Times of India", "https://timesofindia.indiatimes.com/rssfeeds/-2128936835.cms" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 55,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Hindustan Times", "https://www.hindustantimes.com/rss/topnews/rssfeed.xml" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 56,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Der Spiegel", "https://www.spiegel.de/international/index.rss" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 57,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Die Zeit", "https://www.zeit.de/index.rss" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 58,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Frankfurter Allgemeine", "https://www.faz.net/rss/aktuell/" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 59,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Süddeutsche Zeitung", "https://www.sueddeutsche.de/rss" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 60,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Le Monde", "https://www.lemonde.fr/rss/une.xml" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 61,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Le Figaro", "http://www.lefigaro.fr/rss/figaro_actualites.xml" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 62,
                columns: new[] { "Name", "Url" },
                values: new object[] { "France 24", "https://www.france24.com/en/rss" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 63,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Corriere della Sera", "https://www.corriere.it/rss/homepage.xml" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 64,
                columns: new[] { "Name", "Url" },
                values: new object[] { "La Repubblica", "https://www.repubblica.it/rss/homepage/rss2.0.xml" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 65,
                columns: new[] { "Name", "Url" },
                values: new object[] { "El País", "https://feeds.elpais.com/mrss-s/pages/ep/site/elpais.com/portada" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 66,
                columns: new[] { "Name", "Url" },
                values: new object[] { "El Mundo", "https://www.elmundo.es/rss/portada.xml" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 67,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Público", "https://feeds.publico.pt/rss/home" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 68,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Swiss Info", "https://www.swissinfo.ch/rss" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 69,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Scientific American", "https://www.scientificamerican.com/feed/" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 70,
                columns: new[] { "Name", "Url" },
                values: new object[] { "New Scientist", "https://www.newscientist.com/feed/home/" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 71,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Nature News (US)", "https://www.nature.com/news/rss" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 72,
                columns: new[] { "Name", "Url" },
                values: new object[] { "TechRadar", "https://www.techradar.com/rss" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 73,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Engadget", "https://www.engadget.com/rss.xml" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 74,
                columns: new[] { "Name", "Url" },
                values: new object[] { "PCMag", "https://www.pcmag.com/feeds/all-news" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 75,
                columns: new[] { "Name", "Url" },
                values: new object[] { "The Independent", "https://www.independent.co.uk/news/uk/rss" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 76,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Financial Times", "https://www.ft.com/?format=rss" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 77,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Daily Mail", "https://www.dailymail.co.uk/articles.rss" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 78,
                columns: new[] { "Name", "Url" },
                values: new object[] { "CBC News", "https://www.cbc.ca/cmlink/rss-canada" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 79,
                columns: new[] { "Name", "Url" },
                values: new object[] { "ABC News AU", "https://www.abc.net.au/news/feed/51120/rss.xml" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 80,
                columns: new[] { "Name", "Url" },
                values: new object[] { "SBS News", "https://www.sbs.com.au/news/feeds/rss/" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 81,
                columns: new[] { "Name", "Url" },
                values: new object[] { "NZ Herald", "https://www.nzherald.co.nz/rss/" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 82,
                columns: new[] { "Name", "Url" },
                values: new object[] { "RT News", "https://www.rt.com/rss/news/" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 83,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Globo", "https://g1.globo.com/rss/g1/" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 84,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Clarin", "https://www.clarin.com/rss/" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 85,
                columns: new[] { "Name", "Url" },
                values: new object[] { "AllAfrica", "https://allafrica.com/tools/headlines/rss/AllAfrica_headlines.xml" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 86,
                columns: new[] { "Name", "Url" },
                values: new object[] { "News24 South Africa", "https://www.news24.com/rss" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 87,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Al Jazeera", "https://www.aljazeera.com/xml/rss/all.xml" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 88,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Haaretz", "https://www.haaretz.com/cmlink/1.6401109" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 89,
                columns: new[] { "Name", "Url" },
                values: new object[] { "NASA RSS", "https://www.nasa.gov/rss/dyn/breaking_news.rss" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 90,
                columns: new[] { "Name", "Url" },
                values: new object[] { "ESA News", "https://www.esa.int/rssfeed" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 91,
                columns: new[] { "Name", "Url" },
                values: new object[] { "MIT Technology Review", "https://www.technologyreview.com/feed/" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 92,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Nature", "https://www.nature.com/subjects/news/rss" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 93,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Science Daily", "https://www.sciencedaily.com/rss/top/science.xml" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 94,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Ars Technica", "http://feeds.arstechnica.com/arstechnica/index" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 95,
                columns: new[] { "Name", "Url" },
                values: new object[] { "OpenAI Blog", "https://openai.com/blog/rss/" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 96,
                columns: new[] { "Name", "Url" },
                values: new object[] { "DeepMind Blog", "https://deepmind.com/blog/feed/rss" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 97,
                columns: new[] { "Name", "Url" },
                values: new object[] { "MIT CSAIL News", "https://www.csail.mit.edu/news/rss.xml" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 98,
                columns: new[] { "Name", "Url" },
                values: new object[] { "AI Trends", "https://www.aitrends.com/feed/" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 99,
                columns: new[] { "Name", "Url" },
                values: new object[] { "ESPN", "http://www.espn.com/espn/rss/news" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 100,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Sky Sports", "http://www.skysports.com/rss/12040" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 101,
                columns: new[] { "Name", "Url" },
                values: new object[] { "BBC Sport", "http://feeds.bbci.co.uk/sport/rss.xml?edition=uk" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 102,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Goal.com", "https://www.goal.com/feeds/en/news" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 103,
                columns: new[] { "Name", "Url" },
                values: new object[] { "Bleacher Report", "https://bleacherreport.com/articles/feed" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 104,
                columns: new[] { "Name", "Url" },
                values: new object[] { "FIFA News", "https://www.fifa.com/rss/index.xml" });

            migrationBuilder.UpdateData(
                table: "Sources",
                keyColumn: "Id",
                keyValue: 105,
                columns: new[] { "Name", "Url" },
                values: new object[] { "UEFA News", "https://www.uefa.com/rssfeed" });

            migrationBuilder.InsertData(
                table: "Sources",
                columns: new[] { "Id", "LanguageCode", "Name", "Section", "Url" },
                values: new object[] { 40, null, "Wired", null, "https://www.wired.com/feed/rss" });
        }
    }
}
