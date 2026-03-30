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
        var res = await service.CreateLike(dto);
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
}