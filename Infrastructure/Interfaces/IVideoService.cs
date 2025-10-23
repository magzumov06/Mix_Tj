using Domain.DTOs.VideosDto;
using Domain.Filters;
using Domain.Responces;

namespace Infrastructure.Interfaces;

public interface IVideoService
{ 
    Task<Responce<string>> CreateVideo(CreateVideoDto dto); 
    Task<Responce<string>> UpdateVideo(UpdateVideDto dto); 
    Task<Responce<string>> DeleteVideo(int id);
    Task<Responce<GetVideoDto>> GetVideo(int id);
    Task<PaginationResponce<List<GetVideoDto>>> GetVideos(VideoFilter filter);
    
}