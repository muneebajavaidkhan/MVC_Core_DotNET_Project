using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SecondProj.Models;

public partial class Register
{
    public int UserId { get; set; }

    [Required(ErrorMessage = "Name required")]
    [StringLength(100)]
    public string Name { get; set; } = null!;
    [Required(ErrorMessage = "Email required")]
    [EmailAddress]
    public string Email { get; set; } = null!;
    [Required(ErrorMessage = "Password required")]
    [StringLength(100, MinimumLength = 6)]
    public string Password { get; set; } = null!;
    [NotMapped]
    [Required]
    [Compare("Password", ErrorMessage = "Passwords don't match")]
    public string ConfirmPassword { get; set; } = null!;
    public string Role { get; set; } = null!;

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
