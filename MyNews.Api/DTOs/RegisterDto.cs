﻿using System.ComponentModel.DataAnnotations;

namespace MyNews.Api.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string LastName {  get; set; } = string.Empty;

        public string Country {  get; set; } = string.Empty;

        public string City {  get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required, MinLength(6)]
        public string Password { get; set; } = string.Empty;

        [Required]
        [Compare("Password", ErrorMessage = "Password do not match.")]
        public string RepeatPassword { get; set; } = string.Empty;
    }
}
