using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Comment : BaseEntities
{
    [Required]
    [MinLength(20),MaxLength(5000)]
    public string Text { get; set; }
    [MinLength(20),MaxLength(1000)]
    public string? Reply { get; set; }
    public int UserId { get; set; }
    public int NewsId { get; set; }
    public int VideoId { get; set; }
    public User? User { get; set; }
    public News? News { get; set; }
    public Video? Video { get; set; }
    public List<Like>? Likes { get; set; }
}