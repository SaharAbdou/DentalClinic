using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinic.Application.DTOs.Doctor;

public class CreateDoctorDto
{
    public string Name { get; set; } = null!;
    public string Specialty { get; set; } = null!; // "Orthodontics" أو "RestorativeCosmetic"
    public string? Bio { get; set; }
    public string? ProfileImageUrl { get; set; }
}