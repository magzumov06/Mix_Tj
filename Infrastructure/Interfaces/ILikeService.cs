using Domain.DTOs.LikeDto;
using Domain.Responces;

namespace Infrastructure.Interfaces;

public interface ILikeService
{
    Task<Responce<string>> CreateLike (CreateLikeDto dto,int  userId);
    Task<Responce<string>> DeleteLike  (int id);
    Task<Responce<List<GetLikeDto>>> GetLikes ();
    Task<Responce<List<GetLikeDto>>> GetMyLikes (int userId);
}