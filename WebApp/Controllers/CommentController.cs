using System.Security.Claims;
using Domain.DTOs.CommentDto;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;


[ApiController]
[Route("api/[controller]")]
public class CommentController(ICommentService service) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post(CreateCommentDto dto)
    {
        
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null)
            return Unauthorized("User not found in token");

        var userId = int.Parse(userIdClaim);

        var res = await service.CreateComment(dto, userId);
        return StatusCode(res.StatusCode, res);
    }

    [HttpPut]
    public async Task<IActionResult> Put(UpdateCommentDto dto)
    {
        var res = await service.UpdateComment(dto);
        return StatusCode(res.StatusCode, res);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var res = await service.DeleteComment(id);
        return StatusCode(res.StatusCode, res);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetComments(int id)
    {
        var res = await service.GetComment(id);
        return StatusCode(res.StatusCode, res);
    }

    [HttpGet("news/{newsId}/comments")]
    public async Task<IActionResult> GetCommentsByNewsId(int newsId)
    {
        var res = await service.GetCommentsByNewsId(newsId);
        return StatusCode(res.StatusCode, res);
    }

    [HttpGet("videos/{videoId}/comments")]
    public async Task<IActionResult> GetCommentsByVideoId(int videoId)
    {
        var res = await service.GetCommentsByVideoId(videoId);
        return StatusCode(res.StatusCode, res);
    }
}