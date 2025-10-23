using Domain.Enums;

namespace Domain.Entities;

public class Like : BaseEntities
{
    public int UserId { get; set; }
    public int TargetId { get; set; }
    public LikeType Type { get; set; }
    public User? User { get; set; }
}