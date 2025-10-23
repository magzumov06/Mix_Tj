namespace Domain.DTOs.UserDto;

public class UpdateUserDto
{
    public int Id { get; set; }
    public string Email { get; set; }
    public required string UserName { get; set; }
    public string? About { get; set; }
}