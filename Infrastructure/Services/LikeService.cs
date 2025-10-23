using System.Net;
using Domain.DTOs.LikeDto;
using Domain.Entities;
using Domain.Responces;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Infrastructure.Services;

public class LikeService(DataContext context) : ILikeService
{
    public async Task<Responce<string>> CreateLike(CreateLikeDto dto)
    {
        try
        {
            var like = new Like()
            {
                UserId = dto.UserId,
                TargetId = dto.TargetId,
                Type = dto.Type,
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

    public async Task<Responce<string>> DeleteLike(int id)
    {
        try
        {
            var like = await context.Likes.FirstOrDefaultAsync(x=>x.Id == id);
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
}