using Domain.DTOs.UserDto;
using Domain.Filters;
using Domain.Responces;

namespace Infrastructure.Interfaces;

public interface IUserService
{
    Task<Responce<string>> UpdateUser(UpdateUserDto update);
    Task<Responce<string>> DeleteUser(int id);
    Task<Responce<string>> BlockUser(int id);
    Task<Responce<string>> UnblockUser(int id);
    Task<Responce<GetUserDto>> GetUser(int id);
    Task<PaginationResponce<List<GetUserDto>>> GetUsers(UserFilter filter);
}