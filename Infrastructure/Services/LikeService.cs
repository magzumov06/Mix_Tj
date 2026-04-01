using System.Net;
using Domain.DTOs.LikeDto;
using Domain.Entities;
using Domain.Enums;
using Domain.Responces;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Infrastructure.Services;

public class LikeService(DataContext context) : ILikeService
{
    public async Task<Responce<string>> CreateLike(CreateLikeDto dto,int  userId)
    {
        try
        {
            bool targetExists = dto.TargetType switch
            {
                TargetType.Video => await context.Videos.AnyAsync(v => v.Id == dto.TargetId),
                TargetType.News => await context.Newses.AnyAsync(n => n.Id == dto.TargetId),
                TargetType.Comment => await context.Comments.AnyAsync(c => c.Id == dto.TargetId),
                _ => false
            };

            if (!targetExists)
                return new Responce<string>(HttpStatusCode.BadRequest, "Target not found");

            var existingLike = await context.Likes
                .AnyAsync(l => l.UserId == userId &&
                               l.TargetId == dto.TargetId &&
                               l.TargetType == dto.TargetType);

            if (existingLike)
                return new Responce<string>(HttpStatusCode.BadRequest, "Already liked/disliked");

            
            var like = new Like()
            {
                UserId = dto.UserId,
                TargetId = dto.TargetId,
                Type = dto.Type,
                TargetType = dto.TargetType,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };
            await context.Likes.AddAsync(like);
            var res = await context.SaveChangesAsync();
            return res > 0
                ? new Responce<string>(HttpStatusCode.Created,"Like created")
                : new Responce<string>(HttpStatusCode.BadRequest,"Like not created");
        }
        catch (Exception e)
        {
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<string>> DeleteLike(int id, int  userId)
    {
        try
        {
            var like = await context.Likes.FirstOrDefaultAsync(x=>x.Id == id && x.UserId == userId);
            if (like ==  null) return new Responce<string>(HttpStatusCode.NotFound,"Like not found");
            context.Likes.Remove(like);
            var res = await context.SaveChangesAsync();
            return res > 0
                ? new Responce<string>(HttpStatusCode.OK,"Like deleted")
                : new Responce<string>(HttpStatusCode.BadRequest,"Like not deleted");
        }
        catch (Exception e)
        {
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<List<GetLikeDto>>> GetLikes()
    {
        try
        {
            var likes = await context.Likes.ToListAsync();
            if (likes.Count == 0) return new Responce<List<GetLikeDto>>(HttpStatusCode.NotFound,"Like not found");
            var dto = likes.Select(x=>new GetLikeDto()
            {
                Id = x.Id,
                UserId = x.UserId,
                TargetId = x.TargetId,
                Type = x.Type,
                TargetType = x.TargetType,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt,
            }).ToList();
            return new Responce<List<GetLikeDto>>(dto);
        }
        catch (Exception e)
        {
            return new Responce<List<GetLikeDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<List<GetLikeDto>>> GetMyLikes(int userId)
    {
        try
        {
            var dtos = await context.Likes
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedAt)
                .Select(x => new GetLikeDto
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    TargetId = x.TargetId,
                    Type = x.Type,
                    TargetType = x.TargetType,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                })
                .ToListAsync();
            return new Responce<List<GetLikeDto>>(dtos);
        }
        catch (Exception e)
        {
            return new Responce<List<GetLikeDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }
}