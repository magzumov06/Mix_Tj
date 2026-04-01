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
   public async Task<Responce<string>> CreateComment(CreateCommentDto dto, int userId)
    {
        try
        {
            var comment = new Comment
            {
                Text = dto.Text,
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            if (dto.ParentCommentId != null)
            {
                var parent = await context.Comments.FirstOrDefaultAsync(c => c.Id == dto.ParentCommentId);
                if (parent == null)
                    return new Responce<string>(HttpStatusCode.BadRequest, "Parent comment not found");

                comment.ParentCommentId = dto.ParentCommentId;
                comment.NewsId = parent.NewsId; 
                comment.VideoId = parent.VideoId;
            }
            else
            {
                if (dto.NewsId != null)
                {
                    comment.NewsId = dto.NewsId;
                    comment.VideoId = null;
                }
                else if (dto.VideoId != null)
                {
                    comment.VideoId = dto.VideoId;
                    comment.NewsId = null;
                }
                else
                {
                    return new Responce<string>(HttpStatusCode.BadRequest, "Comment must belong to News, Video or a parent comment");
                }
            }

            await context.Comments.AddAsync(comment);
            var res = await context.SaveChangesAsync();
            return res > 0
                ? new Responce<string>(HttpStatusCode.Created, "Comment created")
                : new Responce<string>(HttpStatusCode.BadRequest, "Comment not created");
        }
        catch (Exception ex)
        {
            return new Responce<string>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    public async Task<Responce<string>> UpdateComment(UpdateCommentDto dto, int userId)
    {
        try
        {
            var comment = await context.Comments.FirstOrDefaultAsync(x=>x.Id == dto.Id && x.UserId == userId);
            if(comment == null) return new Responce<string>(HttpStatusCode.NotFound, "Comment not found");
            comment.Text = dto.Text;
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

    public async Task<Responce<string>> DeleteComment(int id,int userId)
    {
        try
        {
            var comment = await context.Comments.FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId);
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
                ParentCommentId = comment.ParentCommentId,
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

     private List<GetCommentDto> BuildReplies(Comment parent, List<Comment> allComments)
    {
        return allComments
            .Where(c => c.ParentCommentId == parent.Id)
            .Select(c => new GetCommentDto
            {
                Id = c.Id,
                Text = c.Text,
                ParentCommentId = c.ParentCommentId,
                UserId = c.UserId,
                VideoId = c.VideoId,
                NewsId = c.NewsId,
                CreatedAt = c.CreatedAt,
                UpdatedAt = c.UpdatedAt,
                Replies = BuildReplies(c, allComments)
            }).ToList();
    }

    public async Task<Responce<List<GetCommentDto>>> GetCommentsByNewsId(int newsId)
    {
        try
        {
            var comments = await context.Comments
                .Where(x => x.NewsId == newsId)
                .ToListAsync();

            if (!comments.Any())
                return new Responce<List<GetCommentDto>>(new List<GetCommentDto>());

            var dtos = comments
                .Where(c => c.ParentCommentId == null)
                .Select(c => new GetCommentDto
                {
                    Id = c.Id,
                    Text = c.Text,
                    ParentCommentId = c.ParentCommentId,
                    UserId = c.UserId,
                    VideoId = c.VideoId,
                    NewsId = c.NewsId,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt,
                    Replies = BuildReplies(c, comments)
                }).ToList();

            return new Responce<List<GetCommentDto>>(dtos);
        }
        catch (Exception e)
        {
            return new Responce<List<GetCommentDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<List<GetCommentDto>>> GetCommentsByVideoId(int videoId)
    {
        try
        {
            var comments = await context.Comments
                .Where(x => x.VideoId == videoId)
                .ToListAsync();

            if (!comments.Any())
                return new Responce<List<GetCommentDto>>(new List<GetCommentDto>());

            var dtos = comments
                .Where(c => c.ParentCommentId == null)
                .Select(c => new GetCommentDto
                {
                    Id = c.Id,
                    Text = c.Text,
                    ParentCommentId = c.ParentCommentId,
                    UserId = c.UserId,
                    VideoId = c.VideoId,
                    NewsId = c.NewsId,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt,
                    Replies = BuildReplies(c, comments)
                }).ToList();

            return new Responce<List<GetCommentDto>>(dtos);
        }
        catch (Exception e)
        {
            return new Responce<List<GetCommentDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }
}