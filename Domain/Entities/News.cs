using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Domain.Entities;

public class News : BaseEntities
{
    [MinLength(10)]
    public string Title { get; set; }
    [MinLength(100)]
    public string Content { get; set; }
    public Category Category { get; set; }
    public string Tags { get; set; }
    public int AuthorId { get; set; }
    public User? Author { get; set; }
    public List<Comment>? Comments { get; set; }
    public List<Like>? Likes { get; set; }
}