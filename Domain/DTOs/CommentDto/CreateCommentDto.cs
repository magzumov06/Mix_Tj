using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.CommentDto;

public class CreateCommentDto
{
    [Required]
    [MinLength(20),MaxLength(5000)]
    public required string Text { get; set; }
    [MinLength(20),MaxLength(1000)]
    public int? ParentCommentId { get; set; }
    public int? NewsId { get; set; }
    public int? VideoId { get; set; }
}