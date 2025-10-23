using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.CommentDto;

public class GetCommentDto
{
    public int Id { get; set; }
    public string Text { get; set; }
    public string? Reply { get; set; }
    public int UserId { get; set; }
    public int NewsId { get; set; }
    public int VideoId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}