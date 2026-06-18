using System.ComponentModel.DataAnnotations;

namespace EWallet.Identity.Models;

public class RegisterRequest
{
    [Required]
    public string Username { get; set; }
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    [Required]
    [RegularExpression(
    @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z\d]).{6,}$",
    ErrorMessage = "Invalid password format.")]
    public string Password { get; set; }
}