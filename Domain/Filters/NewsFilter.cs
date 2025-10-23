using Domain.Enums;

namespace Domain.Filters;

public class NewsFilter : BaseFilter
{
    public int? Id { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public Category? Category { get; set; }
    public string? Tags { get; set; }
    public int? AuthorId { get; set; }
}