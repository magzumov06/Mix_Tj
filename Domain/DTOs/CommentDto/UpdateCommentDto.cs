namespace Domain.DTOs.CommentDto;

public class UpdateCommentDto
{
    public int Id { get; set; }
    public string Text { get; set; }
    public string? Reply { get; set; }
}