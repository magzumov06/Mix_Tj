using System.ComponentModel.DataAnnotations;
using Domain.Enums;

namespace Domain.Entities;

public class Files : BaseEntities
{
    [Required]
    public required string FileName { get; set; }
    public required string Url { get; set; }
    public int Size { get; set; }
    public FileType Type { get; set; }
}