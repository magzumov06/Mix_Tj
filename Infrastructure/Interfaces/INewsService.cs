using Domain.DTOs.NewsDto;
using Domain.Filters;
using Domain.Responces;

namespace Infrastructure.Interfaces;

public interface INewsService
{
    Task<Responce<string>> CreateNews(CreateNewsDto create, int userId);
    Task<Responce<string>> UpdateNews(UpdateNewsDto update, int authorId);
    Task<Responce<string>> DeleteNews(int id,int authorId);
    Task<PaginationResponce<List<GetNewsDto>>> GetNews(NewsFilter filter);
    Task<Responce<GetNewsDto>> GetNewsById (int id);
    Task<Responce<List<GetNewsDto>>> GetMyNews(int authorId);
}