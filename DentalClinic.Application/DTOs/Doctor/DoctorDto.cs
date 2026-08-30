using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalClinic.Application.DTOs.Doctor;

public class DoctorDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Specialty { get; set; } = null!;
    public string? Bio { get; set; }
}
