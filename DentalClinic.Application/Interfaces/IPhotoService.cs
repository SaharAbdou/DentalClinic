using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DentalClinic.Application.DTOs.Photo;

namespace DentalClinic.Application.Interfaces;

public interface IPhotoService
{
    Task<IReadOnlyList<PhotoDto>> GetByDoctorIdAsync(Guid doctorId);
    Task<PhotoDto> CreateAsync(Guid doctorId, CreatePhotoDto dto);
    Task DeleteAsync(Guid id);
}
