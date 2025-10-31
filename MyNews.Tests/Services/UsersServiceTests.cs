using Microsoft.EntityFrameworkCore;

using MyNews.Api.Data;
using MyNews.Api.DTOs;
using MyNews.Api.Models;
using MyNews.Api.Services;

namespace MyNews.Tests.Services
{
    public class UsersServiceTests
    {
        private readonly AppDbContext _context;
        private readonly UsersService _service;

        public UsersServiceTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);
            _service = new UsersService(_context);
        }

        [Fact]
        public async Task GetProfileAsync_ShouldReturnUserProfile_WhenUserExists()
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                Country = "USA",
                City = "NY",
                Email = "john@example.com",
                PasswordHash = "123"
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var result = await _service.GetProfileAsync(user.Id);
            Assert.NotNull(result);
            Assert.Contains("John", result.ToString());
        }

        [Fact]
        public async Task GetProfileAsync_ShouldReturnNull_WhenUserDoesNotExist()
        {
            var result = await _service.GetProfileAsync(Guid.NewGuid());
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateProfileAsync_ShouldUpdateFields_WhenValidData()
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                Country = "USA",
                City = "NY",
                Email = "john@example.com",
                PasswordHash = "123"
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var dto = new UpdateUserDto
            {
                FirstName = "Jane",
                LastName = "Smith",
                Country = "Canada",
                City = "Toronto",
                Email = "jane@example.com",
                Password = "pass123",
                RepeatPassword = "pass123"
            };

            var result = await _service.UpdateProfileAsync(user.Id, dto);
            Assert.Equal("Profile updated successfully.", result);

            var updatedUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
            Assert.Equal("Jane", updatedUser.FirstName);
            Assert.Equal("Smith", updatedUser.LastName);
            Assert.Equal("jane@example.com", updatedUser.Email);
        }

        [Fact]
        public async Task UpdateProfileAsync_ShouldReturnError_WhenPasswordsDoNotMatch()
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "John",
                LastName = "Doe",
                Country = "USA",
                City = "NY",
                Email = "john@example.com",
                PasswordHash = "123"
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var dto = new UpdateUserDto
            {
                Password = "pass1",
                RepeatPassword = "pass2"
            };

            var result = await _service.UpdateProfileAsync(user.Id, dto);
            Assert.Equal("Passwords do not match.", result);
        }

        [Fact]
        public async Task UpdateProfileAsync_ShouldReturnError_WhenEmailAlreadyExists()
        {
            var user1 = new User
            {
                Id = Guid.NewGuid(),
                Email = "test1@example.com",
                FirstName = "User1",
                LastName = "A",
                Country = "A",
                City = "A",
                PasswordHash = "1"
            };
            var user2 = new User
            {
                Id = Guid.NewGuid(),
                Email = "test2@example.com",
                FirstName = "User2",
                LastName = "B",
                Country = "B",
                City = "B",
                PasswordHash = "2"
            };
            _context.Users.AddRange(user1, user2);
            await _context.SaveChangesAsync();

            var dto = new UpdateUserDto
            {
                Email = "test1@example.com"
            };

            var result = await _service.UpdateProfileAsync(user2.Id, dto);
            Assert.Equal("Email is already in use.", result);
        }

        [Fact]
        public async Task DeleteUserAsync_ShouldMarkUserAsDeleted()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new User
            {
                Id = userId,
                FirstName = "John",
                LastName = "Doe",
                Email = "john@example.com",
                Country = "BG",
                City = "Sofia",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("123456"),
                IsDeleted = false
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var service = new UsersService(_context);

            // Act
            var result = await service.DeleteUserAsync(userId);

            // Assert
            var updatedUser = await _context.Users.FindAsync(userId);
            Assert.Equal("User deleted successfully.", result);
            Assert.True(updatedUser.IsDeleted);
        }

        [Fact]
        public async Task MarkOnboardingCompletedAsync_ShouldSetFlagToTrue()
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = "onboard@example.com",
                FirstName = "User",
                LastName = "One",
                Country = "X",
                City = "Y",
                PasswordHash = "pass"
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var result = await _service.MarkOnboardingCompletedAsync(user.Id);
            Assert.True(result);

            var updatedUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.Id);
            Assert.True(updatedUser.IsOnboardingCompleted);
        }

        [Fact]
        public async Task GetOnboardingStatusAsync_ShouldReturnTrue_WhenCompleted()
        {
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = "onboarding@example.com",
                FirstName = "On",
                LastName = "Board",
                Country = "A",
                City = "B",
                PasswordHash = "123",
                IsOnboardingCompleted = true
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var status = await _service.GetOnboardingStatusAsync(user.Id);
            Assert.NotNull(status);
            Assert.True(status.Value);
        }

        [Fact]
        public async Task GetOnboardingStatusAsync_ShouldReturnNull_WhenUserNotFound()
        {
            var result = await _service.GetOnboardingStatusAsync(Guid.NewGuid());
            Assert.Null(result);
        }
    }
}
