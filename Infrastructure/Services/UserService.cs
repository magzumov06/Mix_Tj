using System.Net;
using Domain.DTOs.UserDto;
using Domain.Filters;
using Domain.Responces;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace Infrastructure.Services;

public class UserService(
    DataContext context) : IUserService
{
    public async Task<Responce<string>> UpdateUser(UpdateUserDto update)
    {
        try
        {
            Log.Information("Updating User");
            var updateUser = await context.Users.FirstOrDefaultAsync(x => x.Id == update.Id);
            if (updateUser == null) return new Responce<string>(HttpStatusCode.NotFound,"User not found");
            updateUser.UserName = update.UserName;
            updateUser.About = update.About;
            updateUser.Email = update.Email;
            updateUser.UpdatedAt = DateTime.UtcNow;
            var res = await context.SaveChangesAsync();
            return res > 0
                ? new Responce<string>(HttpStatusCode.OK,"User successfully updated")
                : new Responce<string>(HttpStatusCode.BadRequest,"User not updated");
        }
        catch (Exception e)
        {
            Log.Error("Error in UpdateUser");
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<string>> DeleteUser(int id)
    {
        try
        {
            Log.Information("Deleting User");
            var deleteUser = await context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (deleteUser == null) return new Responce<string>(HttpStatusCode.BadRequest,"User not found"); 
            deleteUser.IsDeleted = true;
            var res = await context.SaveChangesAsync();
            return res > 0
                ? new Responce<string>(HttpStatusCode.OK,"User successfully deleted")
                : new Responce<string>(HttpStatusCode.BadRequest,"User not deleted");
        }
        catch (Exception e)
        {
            Log.Error("Error in DeleteUser");
            return new Responce<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Responce<GetUserDto>> GetUser(int id)
    {
        try
        {
            Log.Information("Getting User");
            var getUser = await context.Users.FirstOrDefaultAsync(x => x.Id == id && x.IsDeleted == false);
            if (getUser == null) return new Responce<GetUserDto>(HttpStatusCode.NotFound,"User not found");
            var dto = new GetUserDto
            {
                Id = getUser.Id,
                UserName = getUser.UserName,
                About = getUser.About,
                Email = getUser.Email,
                CreatedAt = getUser.CreatedAt,
                UpdatedAt = getUser.UpdatedAt
            };
            return new Responce<GetUserDto>(dto);
        }
        catch (Exception e)
        {
            Log.Error("Error in GetUser");
            return new Responce<GetUserDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<PaginationResponce<List<GetUserDto>>> GetUsers(UserFilter filter)
    {
        try
        {
            Log.Information("Getting users");
            var query = context.Users.AsQueryable();
            if (filter.Id.HasValue)
            {
                query = query.Where(x => x.Id == filter.Id.Value);
            }
            if (!string.IsNullOrEmpty(filter.UserName))
            {
                query = query.Where(x => x.UserName.Contains(filter.UserName));
            }
            if (!string.IsNullOrEmpty(filter.About))
            {
                query = query.Where(x => x.PhoneNumber.Contains(filter.About));
            }
            query = query.Where(x=>x.IsDeleted==false);
            var totalCount = await query.CountAsync();
            var skip = (filter.PageNumber - 1) * filter.PageSize;
            var user = await query.Skip(skip).Take(filter.PageSize).ToListAsync();
            if(user.Count == 0 ) return new PaginationResponce<List<GetUserDto>>(HttpStatusCode.NotFound,"User not found");
            var dtos = user.Select(x => new GetUserDto()
            {
                Id = x.Id,
                
                UserName = x.UserName,
                Email = x.Email,
                CreatedAt = x.CreatedAt,
                UpdatedAt = x.UpdatedAt
            }).ToList();
            return new PaginationResponce<List<GetUserDto>>(dtos, totalCount, filter.PageNumber, filter.PageSize);
        }
        catch (Exception e)
        {
            Log.Error("Error in GetUsers");
            return new PaginationResponce<List<GetUserDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }
}