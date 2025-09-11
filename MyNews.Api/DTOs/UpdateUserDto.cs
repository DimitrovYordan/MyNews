using System.ComponentModel.DataAnnotations;

namespace MyNews.Api.DTOs
{
    public class UpdateUserDto
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Country {  get; set; }

        public string? City {  get; set; }

        public string? Email { get; set; }

        public string? Password { get; set; }

        [Compare("Password", ErrorMessage = "Password do not match.")]
        public string? RepeatPassword { get; set; } = string.Empty;
    }
}
