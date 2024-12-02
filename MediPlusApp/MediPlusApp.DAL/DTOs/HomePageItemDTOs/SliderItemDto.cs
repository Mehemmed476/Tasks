using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace MediPlusApp.DAL.DTOs.HomePageItemDTOs;

public class SliderItemDto
{
    public string Title { get; set; }
    public string Text { get; set; }
    public string? ImageUrl { get; set; }
    [NotMapped]
    public IFormFile ImageFile { get; set; }
}