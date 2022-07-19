using System.ComponentModel.DataAnnotations;

namespace EmailVerificationAndForgotPassword.Models;

public class UserLoginRequest
{
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required, MinLength(6, ErrorMessage = "You should enter at leat 6 characters")]
    public string Password { get; set; } = string.Empty;
}