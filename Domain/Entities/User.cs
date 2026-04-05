using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class User : IdentityUser<int>
{
    [EmailAddress]
    [Required]
    public override string Email { get; set; }
    [MaxLength(30)]
    public override required string UserName { get; set; }
    [MaxLength(500)]
    public string? About { get; set; }
    public bool IsDeleted { get; set; } = false;
    public bool IsBlocked { get; set; } = false;
    public List<Video>? Videos { get; set; }
    public List<News>? News { get; set; }
    public List<Comment>? Comments { get; set; }
    public List<Like>? Likes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}