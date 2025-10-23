using System.Net;
using Domain.DTOs.NewsDto;
using Domain.Entities;
using Domain.Filters;
using Domain.Responces;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Infrastructure.Services;

public class NewsServices(DataContext context) : INewsService 
{
    public async Task<Responce<string>> CreateNews(CreateNewsDto create)
    {
        try
        {
            Log.Information("Creating new news");
            var news = new News()
            {
                Title = create.Title,
                Content = create.Content,
                Category = create.Category,
                Tags = create.Tags,
                AuthorId = create.AuthorId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            await context.AddAsync(news);
            var res = await context.SaveChangesAsync();
            return res > 0 
                ? new Responce<string>(HttpStatusCode.Created,"News created successfully!")
                : new Responce<string>(HttpStatusCode.BadRequest,"News not created");
        }
        catch (Exception e)
        {
            Log.Error("Error creating new news");
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<string>> UpdateNews(UpdateNewsDto update)
    {
        try
        {
            Log.Information("Updating new news");
            var res = await context.Newses.FirstOrDefaultAsync(x=>x.Id == update.Id);
            if(res == null) return new Responce<string>(HttpStatusCode.NotFound,"News not found");
            res.Title = update.Title;
            res.Content = update.Content;
            res.Category = update.Category;
            res.Tags = update.Tags;
            res.UpdatedAt = DateTime.UtcNow;
            var result = await context.SaveChangesAsync();
            return result > 0
                ? new Responce<string>(HttpStatusCode.OK, "News updated successfully!")
                : new Responce<string>(HttpStatusCode.BadRequest, "News not updated");
        }
        catch (Exception e)
        {
            Log.Error("Error updating new news");
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<string>> DeleteNews(int id)
    {
        try
        {
            Log.Information("Deleting news");
            var res = await context.Newses.FirstOrDefaultAsync(x => x.Id == id);
            if (res == null) return new Responce<string>(HttpStatusCode.NotFound, "News not found");
            res.IsDeleted = true;
            var result = await context.SaveChangesAsync();
            return result > 0
                ? new Responce<string>(HttpStatusCode.OK, "News deleted successfully!")
                : new Responce<string>(HttpStatusCode.BadRequest, "News not deleted");
        }
        catch (Exception e)
        {
            Log.Error("Error deleting new news");
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<PaginationResponce<List<GetNewsDto>>> GetNews(NewsFilter filter)
    {
        try
        {
            Log.Information("Getting news");
            var query = context.Newses.AsQueryable();
            if (filter.Id.HasValue)
            {
                query = query.Where(x => x.Id == filter.Id);
            }

            if (!string.IsNullOrWhiteSpace(filter.Title))
            {
                query = query.Where(x => x.Title.Contains(filter.Title));
            }

            if (!string.IsNullOrWhiteSpace(filter.Content))
            {
                query = query.Where(x => x.Content.Contains(filter.Content));
            }

            if (filter.Category.HasValue)
            {
                query = query.Where(x => x.Category == filter.Category);
            }

            if (!string.IsNullOrWhiteSpace(filter.Tags))
            {
                query = query.Where(x => x.Tags.Contains(filter.Tags));
            }

            if (filter.AuthorId.HasValue)
            {
                query = query.Where(x => x.AuthorId == filter.AuthorId);
            }

            query = query.Where(x => x.IsDeleted == false);
            var count = await query.CountAsync();
            var skip = (filter.PageNumber - 1) * filter.PageSize;
            var news = await query.Skip(skip).Take(filter.PageSize).ToListAsync();
            if(news.Count == 0) return new PaginationResponce<List<GetNewsDto>>(HttpStatusCode.NotFound,"News not found");
            var dtos = news.Select(x => new GetNewsDto()
            {
                Id = x.Id,
                Title = x.Title,
                Content = x.Content,
                Category = x.Category,
                CreatedAt = x.CreatedAt,
                AuthorId = x.AuthorId,
                UpdatedAt = x.UpdatedAt,
            }).ToList();
            return new PaginationResponce<List<GetNewsDto>>(dtos, count, filter.PageNumber, filter.PageSize );
        }
        catch (Exception e)
        {
            Log.Error("Error getting new news");
            return new PaginationResponce<List<GetNewsDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<GetNewsDto>> GetNewsById(int id)
    {
        try
        {
            Log.Information("Getting news");
            var res = await context.Newses.FirstOrDefaultAsync(x => x.Id == id);
            if(res == null) return new Responce<GetNewsDto>(HttpStatusCode.NotFound, "News not found");
            var dto = new GetNewsDto()
            {
                Id = res.Id,
                Title = res.Title,
                Content = res.Content,
                Category = res.Category,
                AuthorId = res.AuthorId,
                CreatedAt = res.CreatedAt,
                UpdatedAt = res.UpdatedAt,
            };
            return new Responce<GetNewsDto>(dto);
        }
        catch (Exception e)
        {
            Log.Error("Error getting new news");
            return new Responce<GetNewsDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }
}