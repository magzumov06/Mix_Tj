using Domain.Entities;
using Domain.Enums;

namespace Domain.DTOs.LikeDto;

public class CreateLikeDto
{
    public int UserId { get; set; }
    public int TargetId { get; set; }
    public LikeType Type { get; set; }
}