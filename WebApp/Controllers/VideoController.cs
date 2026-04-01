using System.Security.Claims;
using Domain.DTOs.VideosDto;
using Domain.Filters;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VideoController(IVideoService service) :  ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "User,Admin")]
    public async Task<IActionResult> CreateVideo(CreateVideoDto video)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (userIdClaim == null)
            return Unauthorized("User not authenticated");

        var authorId = int.Parse(userIdClaim);
        
        var res = await service.CreateVideo(video, authorId);
        return StatusCode(res.StatusCode, res);
    }

    [HttpPut]
    [Authorize(Roles = "User,Admin")]
    public async Task<IActionResult> UpdateVide(UpdateVideDto video)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null)
            return Unauthorized("User not authenticated");
        var authorId = int.Parse(userIdClaim);
        var res = await service.UpdateVideo(video, authorId);
        return StatusCode(res.StatusCode, res);
    }

    [HttpDelete]
    [Authorize(Roles = "User,Admin")]
    public async Task<IActionResult> DeleteVideo(int id)
    {
        var userClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userClaim == null)
            return Unauthorized("User not authenticated");
        var authorId = int.Parse(userClaim);
        var res = await service.DeleteVideo(id,authorId);
        return StatusCode(res.StatusCode, res);
    }
    
    [HttpGet("{id}")]
    [Authorize(Roles = "User,Admin")]
    public async Task<IActionResult> GetVideo(int id)
    {
        var res = await service.GetVideoById(id);
        return StatusCode(res.StatusCode, res);
    }

    [HttpGet]
    [Authorize(Roles = "User,Admin")]
    public async Task<IActionResult> GetVideos([FromQuery] VideoFilter filter)
    {
        var res = await service.GetVideos(filter);
        return StatusCode(res.StatusCode, res);
    }

    [HttpGet("myVideos")]
    [Authorize(Roles = "User,Admin")]
    public async Task<IActionResult> GetMyVideos()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null)
            return Unauthorized("User not authenticated");
        var authorId = int.Parse(userIdClaim);
        var res = await service.GetMyVideos(authorId);
        return StatusCode(res.StatusCode, res);
    }
}