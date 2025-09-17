namespace MyNews.Api.Interfaces
{
    public interface IEmailService
    {
        /// <summary>
        /// Sends a password reset email to a user.
        /// </summary>
        /// <param name="toEmail">User email.</param>
        /// <param name="firstName">User name (to personalize the message).</param>
        /// <param name="token">Password reset token.</param>
        Task SendPasswordResetEmailAsync(string toEmail, string firstName, string token);
    }
}
