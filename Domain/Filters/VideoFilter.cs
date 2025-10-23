using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Domain.Filters;

public class VideoFilter : BaseFilter
{
    public int? Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public VideoType? VideoType { get; set; }
    public int? AuthorId { get; set; }
}