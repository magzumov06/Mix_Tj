using System.Security.Claims;
using Domain.DTOs.NewsDto;
using Domain.Filters;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NewsController(INewsService service) :  ControllerBase
{
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Post(CreateNewsDto dto)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null)
            return Unauthorized("User not authorized");
        var userId = int.Parse(userIdClaim);
        var res =  await service.CreateNews(dto, userId);
        return StatusCode(res.StatusCode, res);
    }

    [HttpPut]
    [Authorize]
    public async Task<IActionResult> Put(UpdateNewsDto dto)
    {
        var  userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null)
            return Unauthorized("User not authorized");
        var authorId = int.Parse(userIdClaim);
        var res = await service.UpdateNews(dto, authorId);
        return StatusCode(res.StatusCode, res);
    }

    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var  userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userIdClaim == null)
            return Unauthorized("User not authorized");
        var authorId = int.Parse(userIdClaim);
        var res = await service.DeleteNews(id, authorId);
        return StatusCode(res.StatusCode, res);
    }

    [HttpGet]
    [Authorize]
    public async Task<IActionResult> Get([FromQuery] NewsFilter filter)
    {
        var res = await service.GetNews(filter);
        return StatusCode(res.StatusCode, res);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> Get(int id)
    {
        var res = await service.GetNewsById(id);
        return StatusCode(res.StatusCode, res);
    }

    [HttpGet("myNews")]
    [Authorize]
    public async Task<IActionResult> GetMyNews()
    {
        var userClaimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (userClaimId == null)
            return Unauthorized("User not authorized");
        
        var authorId = int.Parse(userClaimId);
        
        var res = await service.GetMyNews(authorId);
        return StatusCode(res.StatusCode, res);
    }
}