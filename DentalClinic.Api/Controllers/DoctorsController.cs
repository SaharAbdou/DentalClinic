using DentalClinic.Application.DTOs.Doctor;
using DentalClinic.Application.DTOs.Photo;
using DentalClinic.Application.DTOs.Schedule;
using DentalClinic.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] CreateDoctorDto dto)
    {
        try { await _doctorService.UpdateAsync(id, dto); return NoContent(); }
        catch (ArgumentException ex) { return NotFound(new { message = ex.Message }); }
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try { await _doctorService.DeleteAsync(id); return NoContent(); }
        catch (ArgumentException ex) { return NotFound(new { message = ex.Message }); }
    }
    [Authorize]
    [HttpPut("weekly-availability/{id}")]
    public async Task<IActionResult> UpdateAvailability(Guid id, [FromBody] CreateWeeklyAvailabilityDto dto)
    {
        try { await _availabilityService.UpdateWeeklyAvailabilityAsync(id, dto); return NoContent(); }
        catch (ArgumentException ex) { return NotFound(new { message = ex.Message }); }
    }

    [Authorize]
    [HttpDelete("weekly-availability/{id}")]
    public async Task<IActionResult> DeleteAvailability(Guid id)
    {
        try { await _availabilityService.DeleteWeeklyAvailabilityAsync(id); return NoContent(); }
        catch (ArgumentException ex) { return NotFound(new { message = ex.Message }); }
    }
    [Authorize]
    [HttpGet("{doctorId}/exceptions")]
    public async Task<IActionResult> GetExceptions(Guid doctorId)
    {
        return Ok(await _availabilityService.GetExceptionsAsync(doctorId));
    }

    [Authorize]
    [HttpPost("{doctorId}/exceptions")]
    public async Task<IActionResult> AddException(Guid doctorId, [FromBody] CreateExceptionDto dto)
    {
        var result = await _availabilityService.AddExceptionAsync(doctorId, dto);
        return CreatedAtAction(nameof(GetExceptions), new { doctorId }, result);
    }

    [Authorize]
    [HttpDelete("exceptions/{id}")]
    public async Task<IActionResult> DeleteException(Guid id)
    {
        try { await _availabilityService.DeleteExceptionAsync(id); return NoContent(); }
        catch (ArgumentException ex) { return NotFound(new { message = ex.Message }); }
    }
    [Authorize]
    [HttpDelete("photos/{id}")]
    public async Task<IActionResult> DeletePhoto(Guid id)
    {
        try { await _photoService.DeleteAsync(id); return NoContent(); }
        catch (ArgumentException ex) { return NotFound(new { message = ex.Message }); }
    }
}