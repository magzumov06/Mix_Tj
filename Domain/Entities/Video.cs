using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Domain.Entities;

public class Video : BaseEntities
{
    [Required]
    [MinLength(5)]
    public string Title { get; set; }
    [MaxLength(200)]
    public string Description { get; set; }
    public string? Url { get; set; }
    public VideoType VideoType { get; set; }
    public int AuthorId { get; set; }
    public User? Author { get; set; }
    public List<Comment>? Comments { get; set; }
    public List<Like>? Likes { get; set; }
}