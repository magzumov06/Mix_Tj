using System.Security.Claims;
using Domain.DTOs.UserDto;
using Domain.Filters;
using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controller;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService service) : ControllerBase
{
    [HttpPut]
    [Authorize]
    public async Task<IActionResult> Put([FromForm] UpdateUserDto updateUserDto)
    {
        var res = await service.UpdateUser(updateUserDto);
        return StatusCode(res.StatusCode, res);
    }
    
    [HttpDelete]
    [Authorize]
    public async Task<IActionResult> DeleteUser()
    {
        var userClaimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (userClaimId == null)
            return Unauthorized("User not authorized");
        
        var id = int.Parse(userClaimId);
        
        var res = await service.DeleteUser(id);
        return StatusCode(res.StatusCode, res);
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetUsers([FromQuery] UserFilter filter)
    {
        var res = await service.GetUsers(filter);
        return StatusCode(res.StatusCode, res);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetUser()
    {
        var userClaimId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (userClaimId == null)
            return Unauthorized("User not authorized");
        
        var id = int.Parse(userClaimId);
        
        var res = await service.GetUser(id);
        return StatusCode(res.StatusCode, res);
    }
}