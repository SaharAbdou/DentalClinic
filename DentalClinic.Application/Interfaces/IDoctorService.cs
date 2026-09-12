using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DentalClinic.Application.DTOs.Doctor;

namespace DentalClinic.Application.Interfaces;

public interface IDoctorService
{
    Task<IReadOnlyList<DoctorDto>> GetAllAsync();
    Task<DoctorDto?> GetByIdAsync(Guid id);
    Task<DoctorDto> CreateAsync(CreateDoctorDto dto);
    Task UpdateAsync(Guid id, CreateDoctorDto dto);
    Task DeleteAsync(Guid id);
}
