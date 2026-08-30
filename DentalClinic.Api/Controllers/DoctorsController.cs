using DentalClinic.Application.DTOs.Doctor;
using DentalClinic.Application.DTOs.Photo;
using DentalClinic.Application.DTOs.Schedule;
using DentalClinic.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DentalClinic.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DoctorsController : ControllerBase
{
    private readonly IDoctorService _doctorService;

    private readonly IDoctorAvailabilityService _availabilityService;

    private readonly IPhotoService _photoService;

    public DoctorsController(IDoctorService doctorService, IDoctorAvailabilityService availabilityService, IPhotoService photoService)
    {
        _doctorService = doctorService;
        _availabilityService = availabilityService;
        _photoService = photoService;
    }

    [HttpPost("{doctorId}/weekly-availability")]
    public async Task<IActionResult> AddWeeklyAvailability(Guid doctorId, [FromBody] CreateWeeklyAvailabilityDto dto)
    {
        await _availabilityService.AddWeeklyAvailabilityAsync(doctorId, dto);
        return NoContent();
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var doctors = await _doctorService.GetAllAsync();
        return Ok(doctors);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var doctor = await _doctorService.GetByIdAsync(id);
        if (doctor == null) return NotFound();

        return Ok(doctor);
    }
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateDoctorDto dto)
    {
        var doctor = await _doctorService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = doctor.Id }, doctor);
    }
    [HttpGet("{doctorId}/photos")]
    public async Task<IActionResult> GetPhotos(Guid doctorId)
    {
        var photos = await _photoService.GetByDoctorIdAsync(doctorId);
        return Ok(photos);
    }

    [HttpPost("{doctorId}/photos")]
    public async Task<IActionResult> AddPhoto(Guid doctorId, [FromBody] CreatePhotoDto dto)
    {
        var photo = await _photoService.CreateAsync(doctorId, dto);
        return CreatedAtAction(nameof(GetPhotos), new { doctorId }, photo);
    }
}