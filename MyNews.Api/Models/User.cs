using System.ComponentModel.DataAnnotations;

namespace MyNews.Api.Models
{
    public class User
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, MaxLength(20)]
        public string FirstName { get; set; } = string.Empty;

        [Required, MaxLength(20)]
        public string LastName { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string Country { get; set; } = string.Empty;

        [Required, MaxLength(50)]
        public string City { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public string? PasswordResetToken {  get; set; }

        public DateTime? PasswordResetTokenExpires { get; set; }

        public bool IsDeleted { get; set; } = false;

        public ICollection<UserNewsRead> NewsReads { get; set; } = new List<UserNewsRead>();

        public ICollection<UserSectionPreference> SectionPreferences { get; set; } = new List<UserSectionPreference>();
    }
}
