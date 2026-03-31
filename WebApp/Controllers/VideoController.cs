using System.Security.Claims;
using Domain.DTOs.VideosDto;
using Domain.Filters;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VideoController(IVideoService service) :  ControllerBase
{
    [HttpPost]
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
    public async Task<IActionResult> UpdateVide(UpdateVideDto video)
    {
        var res = await service.UpdateVideo(video);
        return StatusCode(res.StatusCode, res);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteVideo(int id)
    {
        var res = await service.DeleteVideo(id);
        return StatusCode(res.StatusCode, res);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetVideo(int id)
    {
        var res = await service.GetVideo(id);
        return StatusCode(res.StatusCode, res);
    }

    [HttpGet]
    public async Task<IActionResult> GetVideos([FromQuery] VideoFilter filter)
    {
        var res = await service.GetVideos(filter);
        return StatusCode(res.StatusCode, res);
    }

    [HttpGet("myVideos")]
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