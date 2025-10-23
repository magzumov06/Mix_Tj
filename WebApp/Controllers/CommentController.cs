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
        var res = await service.CreateComment(dto);
        return Ok(res);
    }

    [HttpPut]
    public async Task<IActionResult> Put(UpdateCommentDto dto)
    {
        var res = await service.UpdateComment(dto);
        return Ok(res);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var res = await service.DeleteComment(id);
        return Ok(res);
    }

    [HttpGet]
    public async Task<IActionResult> GetComments(int id)
    {
        var res = await service.GetComment(id);
        return Ok(res);
    }
}