using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Comment : BaseEntities
{
    [Required(ErrorMessage = "Text is required")]
    [MinLength(20, ErrorMessage = "Text must be at least 20 characters")]
    [MaxLength(5000, ErrorMessage = "Text cannot exceed 5000 characters")]
    public required string Text { get; set; }
    public int? ParentCommentId { get; set; } 
    public int UserId { get; set; }
    public int? NewsId { get; set; }
    public int? VideoId { get; set; }
    public Comment? ParentComment { get; set; } 
    public List<Comment>? Replies { get; set; }
    public User? User { get; set; }
    public News? News { get; set; }
    public Video? Video { get; set; }
    public List<Like>? Likes { get; set; }
}