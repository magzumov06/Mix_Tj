using System.Security.Claims;
using Domain.DTOs.LikeDto;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LikeController(ILikeService service) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post(CreateLikeDto dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if(userIdClaim == null)
            return Unauthorized("User not authenticated");
        
        var userId = int.Parse(userIdClaim);
        
        var res = await service.CreateLike(dto, userId);
        return StatusCode(res.StatusCode, res);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var res = await service.DeleteLike(id);
        return StatusCode(res.StatusCode, res);
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var res = await service.GetLikes();
        return StatusCode(res.StatusCode, res);
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetMyLikes()
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            return Unauthorized("User not authenticated");
        var authorId = int.Parse(userId);
        var res = await service.GetMyLikes(authorId);
        return StatusCode(res.StatusCode, res);
    }
}