using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DentalClinic.Application.DTOs.Photo;

public class CreatePhotoDto
{
    public string BeforeImageUrl { get; set; } = null!;
    public string AfterImageUrl { get; set; } = null!;
    public string? Description { get; set; }
    public int DisplayOrder { get; set; }
}
