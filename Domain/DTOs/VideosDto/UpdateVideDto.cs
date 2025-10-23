using System.ComponentModel.DataAnnotations;
using Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace Domain.DTOs.VideosDto;

public class UpdateVideDto
{
    public int Id { get; set; }
    [MinLength(5)]
    public string Title { get; set; }
    [MaxLength(200)]
    public string Description { get; set; }
    public IFormFile? Url { get; set; }
    public VideoType VideoType { get; set; }
}