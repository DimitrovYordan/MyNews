using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using MyNews.Api.Services;

using Moq;

namespace MyNews.Tests.Services
{
    public class EmailServiceTests
    {
        private readonly EmailService _emailService;
        private readonly Mock<ILogger<EmailService>> _loggerMock;

        public EmailServiceTests()
        {
            var inMemorySettings = new Dictionary<string, string> {
            {"Email:FromAddress", "noreply@mynews.com"},
            {"Email:SmtpServer", "smtp.mynews.com"},
            {"Email:Port", "587"},
            {"Email:Username", "user"},
            {"Email:Password", "pass"},
            {"Email:EnableSsl", "true"},
            {"FrontendUrl", "https://mynews.com/"}
        };

            IConfiguration config = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _loggerMock = new Mock<ILogger<EmailService>>();
            _emailService = new EmailService(config, _loggerMock.Object);
        }

        [Fact]
        public async Task SendContactMessageAsync_ShouldWriteLog_WhenExceptionOccurs()
        {
            var tempBase = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            Directory.CreateDirectory(tempBase);

            var inMemorySettings = new Dictionary<string, string> {
            {"Email:FromAddress", "noreply@mynews.com"},
            {"Email:SmtpServer", "invalid.smtp.server"},
            {"Email:Port", "1"},
            {"Email:Username", "user"},
            {"Email:Password", "pass"},
            {"Email:EnableSsl", "true"},
            {"FrontendUrl", "https://mynews.com/"}
        };

            var config = new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings).Build();
            var loggerMock = new Mock<ILogger<EmailService>>();

            // temporarily change AppContext.BaseDirectory using reflection is not possible;
            // instead we assert the log file existence by searching under AppContext.BaseDirectory for "Logs/errors.txt"
            var service = new EmailService(config, loggerMock.Object);

            // Act & Assert
            // We expect an exception from the underlying SmtpClient call; the service should create a Logs/errors.txt entry before rethrowing.
            await Assert.ThrowsAnyAsync<Exception>(() => service.SendContactMessageAsync("Test", "Body", "from@test.com"));

            // Find the logs folder under current AppContext.BaseDirectory
            var logPath = Path.Combine(AppContext.BaseDirectory, "Logs", "errors.txt");
            Assert.True(File.Exists(logPath), $"Expected log file at {logPath} but it does not exist.");
        }
    }
}
