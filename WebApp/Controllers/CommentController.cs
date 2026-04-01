using System.Security.Claims;
using Domain.DTOs.CommentDto;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;


[ApiController]
[Route("api/[controller]")]
public class CommentController(ICommentService service) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Admin,User")]
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
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> Put(UpdateCommentDto dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null)
            return Unauthorized("User not found in token");
        var userId = int.Parse(userIdClaim);
        var res = await service.UpdateComment(dto, userId);
        return StatusCode(res.StatusCode, res);
    }

    [HttpDelete]
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> Delete(int id)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null)
            return Unauthorized("User not found in token");
        var userId = int.Parse(userIdClaim);
        var res = await service.DeleteComment(id,userId);
        return StatusCode(res.StatusCode, res);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> GetComments(int id)
    {
        var res = await service.GetComment(id);
        return StatusCode(res.StatusCode, res);
    }

    [HttpGet("news/{newsId}/comments")]
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> GetCommentsByNewsId(int newsId)
    {
        var res = await service.GetCommentsByNewsId(newsId);
        return StatusCode(res.StatusCode, res);
    }

    [HttpGet("videos/{videoId}/comments")]
    [Authorize(Roles = "Admin,User")]
    public async Task<IActionResult> GetCommentsByVideoId(int videoId)
    {
        var res = await service.GetCommentsByVideoId(videoId);
        return StatusCode(res.StatusCode, res);
    }
}