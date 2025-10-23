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
        var res = await service.CreateVideo(video);
        return Ok(res);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateVide(UpdateVideDto video)
    {
        var res = await service.UpdateVideo(video);
        return Ok(res);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteVideo(int id)
    {
        var res = await service.DeleteVideo(id);
        return Ok(res);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetVideo(int id)
    {
        var res = await service.GetVideo(id);
        return Ok(res);
    }

    [HttpGet]
    public async Task<IActionResult> GetVideos([FromQuery] VideoFilter filter)
    {
        var res = await service.GetVideos(filter);
        return Ok(res);
    }
}