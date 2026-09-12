using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DentalClinic.Application.DTOs.Photo;
using DentalClinic.Application.Interfaces;
using DentalClinic.Domain.Entities;

namespace DentalClinic.Application.Services;

public class PhotoService : IPhotoService
{
    private readonly IUnitOfWork _unitOfWork;

    public PhotoService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<PhotoDto>> GetByDoctorIdAsync(Guid doctorId)
    {
        var photos = await _unitOfWork.BeforeAfterPhotos.FindAsync(p => p.DoctorId == doctorId);

        return photos
            .OrderBy(p => p.DisplayOrder)
            .Select(p => new PhotoDto
            {
                Id = p.Id,
                BeforeImageUrl = p.BeforeImageUrl,
                AfterImageUrl = p.AfterImageUrl,
                Description = p.Description,
                DisplayOrder = p.DisplayOrder
            }).ToList();
    }

    public async Task<PhotoDto> CreateAsync(Guid doctorId, CreatePhotoDto dto)
    {
        var doctor = await _unitOfWork.Doctors.GetByIdAsync(doctorId);
        if (doctor == null)
            throw new ArgumentException("Doctor not found");

        var photo = new BeforeAfterPhoto(doctorId, dto.BeforeImageUrl, dto.AfterImageUrl, dto.Description, dto.DisplayOrder);

        await _unitOfWork.BeforeAfterPhotos.AddAsync(photo);
        await _unitOfWork.SaveChangesAsync();

        return new PhotoDto
        {
            Id = photo.Id,
            BeforeImageUrl = photo.BeforeImageUrl,
            AfterImageUrl = photo.AfterImageUrl,
            Description = photo.Description,
            DisplayOrder = photo.DisplayOrder
        };
    }
    public async Task DeleteAsync(Guid id)
    {
        var photo = await _unitOfWork.BeforeAfterPhotos.GetByIdAsync(id)
            ?? throw new ArgumentException("Photo not found");
        _unitOfWork.BeforeAfterPhotos.Remove(photo);
        await _unitOfWork.SaveChangesAsync();
    }
}
