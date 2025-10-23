namespace Domain.DTOs.NewsDto;

public class GetNewsDto : UpdateNewsDto
{
    public int AuthorId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}