// LoginViewModel.cs
using System.ComponentModel.DataAnnotations;

public class Login
{
    [Required(ErrorMessage = "Email required")]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Password required")]
    public string Password { get; set; } = null!;
}