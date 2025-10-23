namespace Domain.DTOs.UserDto;

public class GetUserDto
{
    public int Id { get; set; }
    public required string Email { get; set; }
    public required string UserName { get; set; }
    public string? About { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}