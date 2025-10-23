using Domain.Entities;
using Domain.Enums;

namespace Domain.DTOs.LikeDto;

public class GetLikeDto
{
    public int Id {get; set;}
    public int UserId { get; set; }
    public int TargetId { get; set; }
    public LikeType Type { get; set; } 
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}