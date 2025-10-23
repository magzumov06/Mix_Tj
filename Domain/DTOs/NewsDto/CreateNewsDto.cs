using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Domain.DTOs.NewsDto;

public class CreateNewsDto
{
    [MinLength(10)]
    public string Title { get; set; }
    [MinLength(100)]
    public string Content { get; set; }
    public Category Category { get; set; }
    public string Tags { get; set; }
    public int AuthorId { get; set; }
}