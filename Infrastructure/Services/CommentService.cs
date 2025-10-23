using System.Net;
using Domain.DTOs.CommentDto;
using Domain.Entities;
using Domain.Responces;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Infrastructure.Services;

public class CommentService(DataContext context) : ICommentService
{
    public async Task<Responce<string>> CreateComment(CreateCommentDto dto)
    {
        try
        { 
            var comment = new Comment()
            {
                Text = dto.Text,
                Reply = dto.Reply,
                UserId = dto.UserId,
                NewsId = dto.NewsId,
                VideoId = dto.VideoId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            }; 
            await context.Comments.AddAsync(comment);
            var res = await context.SaveChangesAsync();
            return res > 0
                ? new Responce<string>(HttpStatusCode.Created, "Comment created")
                : new Responce<string>(HttpStatusCode.BadRequest, "Comment not created");
        }
        catch (Exception e)
        {
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<string>> UpdateComment(UpdateCommentDto dto)
    {
        try
        {
            var comment = await context.Comments.FirstOrDefaultAsync(x=>x.Id == dto.Id);
            if(comment == null) return new Responce<string>(HttpStatusCode.NotFound, "Comment not found");
            comment.Text = dto.Text;
            comment.Reply = dto.Reply;
            comment.UpdatedAt = DateTime.UtcNow;
            var res =  await context.SaveChangesAsync();
            return res > 0
                ? new Responce<string>(HttpStatusCode.OK, "Comment updated")
                : new Responce<string>(HttpStatusCode.BadRequest, "Comment not updated");
        }
        catch (Exception e)
        {
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<string>> DeleteComment(int id)
    {
        try
        {
            var comment = await context.Comments.FirstOrDefaultAsync(x => x.Id == id);
            if (comment == null) return new Responce<string>(HttpStatusCode.NotFound, "Comment not found");
            context.Comments.Remove(comment);
            var res = await context.SaveChangesAsync();
            return res > 0
                ? new Responce<string>(HttpStatusCode.OK, "Comment deleted")
                : new Responce<string>(HttpStatusCode.BadRequest, "Comment not deleted");
        }
        catch (Exception e)
        {
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<GetCommentDto>> GetComment(int id)
    {
        try
        {
            var comment = await context.Comments.FirstOrDefaultAsync(x => x.Id == id);
            if (comment == null) return new Responce<GetCommentDto>(HttpStatusCode.NotFound, "Comment not found");
            var dto = new GetCommentDto()
            {
                Id = comment.Id,
                Text = comment.Text,
                Reply = comment.Reply,
                UserId = comment.UserId,
                NewsId = comment.NewsId,
                VideoId = comment.VideoId,
                CreatedAt = comment.CreatedAt,
                UpdatedAt = comment.UpdatedAt
            };
            return new Responce<GetCommentDto>(dto);
        }
        catch (Exception e)
        {
            return new Responce<GetCommentDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }
}