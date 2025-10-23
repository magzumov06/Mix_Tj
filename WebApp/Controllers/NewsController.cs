using Domain.DTOs.NewsDto;
using Domain.Filters;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NewsController(INewsService service) :  ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Post(CreateNewsDto dto)
    {
        var res =  await service.CreateNews(dto);
        return Ok(res);
    }

    [HttpPut]
    public async Task<IActionResult> Put(UpdateNewsDto dto)
    {
        var res = await service.UpdateNews(dto);
        return Ok(res);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var res = await service.DeleteNews(id);
        return Ok(res);
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] NewsFilter filter)
    {
        var res = await service.GetNews(filter);
        return Ok(res);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var res = await service.GetNewsById(id);
        return Ok(res);
    }
}