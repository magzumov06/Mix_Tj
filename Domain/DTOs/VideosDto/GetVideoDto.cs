using Domain.Enums;

namespace Domain.DTOs.VideosDto;

public class GetVideoDto 
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string? Url { get; set; }
    public VideoType VideoType { get; set; }
    public int AuthorId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}