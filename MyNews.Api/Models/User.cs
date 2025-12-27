using System.ComponentModel.DataAnnotations;

namespace MyNews.Api.Models
{
    /// <summary>
    /// Represents an application user with authentication and profile information.
    /// </summary>
    public class User
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [MaxLength(20)]
        public string FirstName { get; set; } = string.Empty;

        [MaxLength(20)]
        public string LastName { get; set; } = string.Empty;

        public string Country { get; set; } = string.Empty;

        public string City { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public string? PasswordResetToken {  get; set; }

        public DateTime? PasswordResetTokenExpires { get; set; }

        public bool IsDeleted { get; set; } = false;

        /// <summary>
        /// Indicates whether the user has completed the onboarding process.
        /// </summary>
        public bool IsOnboardingCompleted { get; set; } = false;

        public ICollection<UserNewsRead> NewsReads { get; set; } = new List<UserNewsRead>();

        public ICollection<UserSectionPreference> UserSectionPreference { get; set; } = new List<UserSectionPreference>();

        public ICollection<UserSourcePreferences> UserSourcePreferences { get; set; } = new List<UserSourcePreferences>();
    }
}
