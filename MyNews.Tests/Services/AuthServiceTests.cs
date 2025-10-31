using Microsoft.EntityFrameworkCore;

using MyNews.Api.Data;
using MyNews.Api.DTOs;
using MyNews.Api.Interfaces;
using MyNews.Api.Models;
using MyNews.Api.Services;

using Moq;

namespace MyNews.Tests.Services
{
    /// <summary>
    /// Unit and integration tests for AuthService with InMemory base.
    /// </summary>
    public class AuthServiceTests
    {
        private readonly DbContextOptions<AppDbContext> _dbOptions;

        public AuthServiceTests()
        {
            // Each test gets a separate InMemory database (unique name)
            _dbOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
        }

        /// <summary>
        /// Successful registration of a new user.
        /// </summary>
        [Fact]
        public async Task RegisterAsync_ShouldCreateUser_WhenEmailDoesNotExist()
        {
            // Arrange
            using var context = new AppDbContext(_dbOptions);
            var jwtMock = new Mock<IJwtService>();
            var emailMock = new Mock<IEmailService>();

            jwtMock.Setup(j => j.GenerateToken(It.IsAny<Guid>(), It.IsAny<string>()))
                   .Returns("mock-token");

            var service = new AuthService(context, jwtMock.Object, emailMock.Object);

            var dto = new RegisterDto
            {
                Email = "new@user.com",
                Password = "Pass123!",
                FirstName = "John",
                LastName = "Doe",
                City = "Sofia",
                Country = "BG"
            };

            // Act
            var result = await service.RegisterAsync(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("mock-token", result.Token);
            Assert.Equal(dto.Email, result.Email);

            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            Assert.NotNull(user);
            Assert.NotEmpty(user.PasswordHash);
        }

        /// <summary>
        /// Attempting to register with an existing email throws an exception.
        /// </summary>
        [Fact]
        public async Task RegisterAsync_ShouldThrow_WhenEmailExists()
        {
            // Arrange
            using var context = new AppDbContext(_dbOptions);
            context.Users.Add(new User
            {
                Id = Guid.NewGuid(),
                Email = "existing@user.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("1234")
            });
            await context.SaveChangesAsync();

            var jwtMock = new Mock<IJwtService>();
            var emailMock = new Mock<IEmailService>();

            var service = new AuthService(context, jwtMock.Object, emailMock.Object);

            var dto = new RegisterDto
            {
                Email = "existing@user.com",
                Password = "newpass"
            };

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => service.RegisterAsync(dto));
        }

        /// <summary>
        /// Successful login returns AuthResponseDto with a token.
        /// </summary>
        [Fact]
        public async Task LoginAsync_ShouldReturnAuthResponse_WhenCredentialsValid()
        {
            // Arrange
            using var context = new AppDbContext(_dbOptions);
            var hashed = BCrypt.Net.BCrypt.HashPassword("secret123");

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = "login@user.com",
                PasswordHash = hashed,
                FirstName = "Maria",
                LastName = "Petrova"
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var jwtMock = new Mock<IJwtService>();
            jwtMock.Setup(j => j.GenerateToken(user.Id, user.Email)).Returns("valid-token");

            var emailMock = new Mock<IEmailService>();
            var service = new AuthService(context, jwtMock.Object, emailMock.Object);

            var dto = new LoginDto { Email = "login@user.com", Password = "secret123" };

            // Act
            var result = await service.LoginAsync(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(user.Email, result.Email);
            Assert.Equal("valid-token", result.Token);
        }

        /// <summary>
        /// Login returns null if password is incorrect.
        /// </summary>
        [Fact]
        public async Task LoginAsync_ShouldReturnNull_WhenPasswordInvalid()
        {
            // Arrange
            using var context = new AppDbContext(_dbOptions);
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = "invalid@user.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("correct-pass")
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var jwtMock = new Mock<IJwtService>();
            var emailMock = new Mock<IEmailService>();
            var service = new AuthService(context, jwtMock.Object, emailMock.Object);

            var dto = new LoginDto { Email = "invalid@user.com", Password = "wrong-pass" };

            // Act
            var result = await service.LoginAsync(dto);

            // Assert
            Assert.Null(result);
        }

        /// <summary>
        /// ForgotPassword returns true and generates a token for an existing email.
        /// </summary>
        [Fact]
        public async Task ForgotPasswordAsync_ShouldReturnTrue_WhenEmailExists()
        {
            // Arrange
            using var context = new AppDbContext(_dbOptions);
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = "forgot@user.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("test")
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var jwtMock = new Mock<IJwtService>();
            var emailMock = new Mock<IEmailService>();
            emailMock.Setup(e => e.SendPasswordResetEmailAsync(user.Email, user.FirstName, It.IsAny<string>()))
                     .Returns(Task.CompletedTask);

            var service = new AuthService(context, jwtMock.Object, emailMock.Object);

            // Act
            var result = await service.ForgotPasswordAsync(user.Email);

            // Assert
            Assert.True(result);

            var updatedUser = await context.Users.FirstAsync();
            Assert.NotNull(updatedUser.PasswordResetToken);
            Assert.NotNull(updatedUser.PasswordResetTokenExpires);
        }

        /// <summary>
        /// ForgotPassword returns false if email does not exist.
        /// </summary>
        [Fact]
        public async Task ForgotPasswordAsync_ShouldReturnFalse_WhenEmailNotFound()
        {
            // Arrange
            using var context = new AppDbContext(_dbOptions);
            var jwtMock = new Mock<IJwtService>();
            var emailMock = new Mock<IEmailService>();
            var service = new AuthService(context, jwtMock.Object, emailMock.Object);

            // Act
            var result = await service.ForgotPasswordAsync("unknown@user.com");

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// ResetPassword returns true and changes the password if the token is valid.
        /// </summary>
        [Fact]
        public async Task ResetPasswordAsync_ShouldReturnTrue_WhenTokenValid()
        {
            // Arrange
            using var context = new AppDbContext(_dbOptions);
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = "reset@user.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("oldpass"),
                PasswordResetToken = "valid-token",
                PasswordResetTokenExpires = DateTime.UtcNow.AddDays(1)
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var jwtMock = new Mock<IJwtService>();
            var emailMock = new Mock<IEmailService>();
            var service = new AuthService(context, jwtMock.Object, emailMock.Object);

            // Act
            var result = await service.ResetPasswordAsync("valid-token", "newpassword123");

            // Assert
            Assert.True(result);

            var updated = await context.Users.FirstAsync();
            Assert.True(BCrypt.Net.BCrypt.Verify("newpassword123", updated.PasswordHash));
            Assert.Null(updated.PasswordResetToken);
            Assert.Null(updated.PasswordResetTokenExpires);
        }

        /// <summary>
        /// ResetPassword returns false on expired token.
        /// </summary>
        [Fact]
        public async Task ResetPasswordAsync_ShouldReturnFalse_WhenTokenExpired()
        {
            // Arrange
            using var context = new AppDbContext(_dbOptions);
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = "expired@user.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("oldpass"),
                PasswordResetToken = "expired-token",
                PasswordResetTokenExpires = DateTime.UtcNow.AddDays(-1)
            };
            context.Users.Add(user);
            await context.SaveChangesAsync();

            var jwtMock = new Mock<IJwtService>();
            var emailMock = new Mock<IEmailService>();
            var service = new AuthService(context, jwtMock.Object, emailMock.Object);

            // Act
            var result = await service.ResetPasswordAsync("expired-token", "newpassword123");

            // Assert
            Assert.False(result);
        }

        /// <summary>
        /// ResetPassword returns false if the token does not exist.
        /// </summary>
        [Fact]
        public async Task ResetPasswordAsync_ShouldReturnFalse_WhenTokenInvalid()
        {
            // Arrange
            using var context = new AppDbContext(_dbOptions);
            var jwtMock = new Mock<IJwtService>();
            var emailMock = new Mock<IEmailService>();
            var service = new AuthService(context, jwtMock.Object, emailMock.Object);

            // Act
            var result = await service.ResetPasswordAsync("not-found", "newpass");

            // Assert
            Assert.False(result);
        }
    }
}
