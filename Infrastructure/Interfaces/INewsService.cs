using Domain.DTOs.NewsDto;
using Domain.Filters;
using Domain.Responces;

namespace Infrastructure.Interfaces;

public interface INewsService
{
    Task<Responce<string>> CreateNews(CreateNewsDto create);
    Task<Responce<string>> UpdateNews(UpdateNewsDto update);
    Task<Responce<string>> DeleteNews(int id);
    Task<PaginationResponce<List<GetNewsDto>>> GetNews(NewsFilter filter);
    Task<Responce<GetNewsDto>> GetNewsById (int id);
}