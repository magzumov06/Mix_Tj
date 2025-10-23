using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.Account;

public class Register
{
    [EmailAddress]
    [Required]
    public required string Email { get; set; }
    [MaxLength(30)]
    public required string Username { get; set; }
    [MaxLength(500)]
    public string? About { get; set; }
}