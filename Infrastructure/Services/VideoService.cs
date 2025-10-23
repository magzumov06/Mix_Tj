using System.Net;
using Domain.DTOs.VideosDto;
using Domain.Entities;
using Domain.Filters;
using Domain.Responces;
using Infrastructure.Data;
using Infrastructure.FileStorage;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class VideoService(DataContext context,
    IFileStorage file) : IVideoService
{
    public async Task<Responce<string>> CreateVideo(CreateVideoDto dto)
    {
        try
        {
            var video = new Video()
            {
                Title = dto.Title,
                Description = dto.Description,
                VideoType = dto.VideoType,
                AuthorId = dto.AuthorId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            if (dto.Url != null)
            {
                video.Url = await file.SaveFile(dto.Url, "Video");
            }
            await context.Videos.AddAsync(video);
            var res = await context.SaveChangesAsync();
            return res > 0
                ? new Responce<string>(HttpStatusCode.Created,"Video created")
                : new Responce<string>(HttpStatusCode.BadRequest,"Video not created");
        }
        catch (Exception e)
        {
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<string>> UpdateVideo(UpdateVideDto dto)
    {
        try
        {
            var video = await context.Videos.FirstOrDefaultAsync(x=>x.Id == dto.Id);
            if (video == null) return new Responce<string>(HttpStatusCode.NotFound,"Video not found");
            video.Title = dto.Title;
            video.Description = dto.Description;
            video.VideoType = dto.VideoType;
            if (dto.Url != null)
            {
                if (!string.IsNullOrEmpty(video.Url))
                {
                    await file.DeleteFile(video.Url);
                }
                await file.SaveFile(dto.Url, "Video");
            }
            video.UpdatedAt = DateTime.UtcNow;
            var res = await context.SaveChangesAsync();
            return res > 0
                ? new Responce<string>(HttpStatusCode.OK,"Video updated")
                : new Responce<string>(HttpStatusCode.BadRequest,"Video not updated");
        }
        catch (Exception e)
        {
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<string>> DeleteVideo(int id)
    {
        try
        {
            var video = await context.Videos.FirstOrDefaultAsync(x => x.Id == id);
            if (video == null) return new Responce<string>(HttpStatusCode.NotFound,"Video not found");
            context.Videos.Remove(video);
            var res = await context.SaveChangesAsync();
            return res > 0
                ? new Responce<string>(HttpStatusCode.OK,"Video deleted")
                : new Responce<string>(HttpStatusCode.BadRequest,"Video not deleted");
        }
        catch (Exception e)
        {
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<GetVideoDto>> GetVideo(int id)
    {
        try
        {
            var video = await context.Videos.FirstOrDefaultAsync(x => x.Id == id);
            if (video == null) return new Responce<GetVideoDto>(HttpStatusCode.NotFound, "Video not found");
            var dto = new GetVideoDto()
            {
                Id = video.Id,
                Title = video.Title,
                Description = video.Description,
                VideoType = video.VideoType,
                Url = video.Url,
                AuthorId = video.AuthorId,
                CreatedAt = video.CreatedAt,
                UpdatedAt = video.UpdatedAt,
            };
            return new Responce<GetVideoDto>(dto);
        }
        catch (Exception e)
        {
            return new Responce<GetVideoDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<PaginationResponce<List<GetVideoDto>>> GetVideos(VideoFilter filter)
    {
        try
        {
            var query = context.Videos.AsQueryable();
            if (filter.Id.HasValue)
            {
                query = query.Where(x => x.Id == filter.Id);
            }

            if (!string.IsNullOrEmpty(filter.Title))
            {
                query = query.Where(x => x.Title.Contains(filter.Title));
            }

            if (!string.IsNullOrEmpty(filter.Description))
            {
                query = query.Where(x => x.Description.Contains(filter.Description));
            }

            if (filter.VideoType.HasValue)
            {
                query = query.Where(x => x.VideoType == filter.VideoType);
            }

            if (filter.AuthorId.HasValue)
            {
                query = query.Where(x => x.AuthorId == filter.AuthorId);
            }

            var count = await query.CountAsync();
            var skip = (filter.PageNumber - 1) * filter.PageSize;
            var videos = await query.Skip(skip).Take(filter.PageSize).ToListAsync();
            if (videos.Count == 0)
                return new PaginationResponce<List<GetVideoDto>>(HttpStatusCode.NotFound, "Videos not found");
            var dtos = videos.Select(x => new GetVideoDto()
            {
                Id = x.Id,
                Title = x.Title,
                Description = x.Description,
                VideoType = x.VideoType,
                AuthorId = x.AuthorId,
                Url = x.Url,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
            }).ToList();
            return new PaginationResponce<List<GetVideoDto>>(dtos, count, filter.PageNumber, filter.PageSize);
        }
        catch (Exception e)
        {
            return new PaginationResponce<List<GetVideoDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }
}