namespace Domain.Filters;

public class UserFilter : BaseFilter
{
    public int? Id { get; set; } 
    public string? UserName { get; set; }
    public string? About { get; set; }
}