using Microsoft.EntityFrameworkCore;

using MyNews.Api.Data;
using MyNews.Api.DTOs;
using MyNews.Api.Interfaces;
using MyNews.Api.Models;

namespace MyNews.Api.Services
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IJwtService _jwtService;

        public AuthService(AppDbContext context, IJwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        public async Task<AuthResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                return null;
            }

            var token = _jwtService.GenerateToken(user.Id, user.Email);

            return new AuthResponseDto
            {
                Token = token,
                Email = loginDto.Email,
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            var esists = await _context.Users
                .AnyAsync(u => u.Email == registerDto.Email);

            if (esists)
            {
                throw new InvalidOperationException("User with this email already exists.");
            }

            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = registerDto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
                City = registerDto.City,
                Country = registerDto.Country,
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            var token = _jwtService.GenerateToken(user.Id, user.Email);

            return new AuthResponseDto
            {
                Token = token,
                Email = registerDto.Email,
                UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName
            };
        }
    }
}
