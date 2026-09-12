using DentalClinic.Application.DTOs.Appointment;
using DentalClinic.Application.Interfaces;
using DentalClinic.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DentalClinic.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentsController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAppointmentDto dto)
    {
        try
        {
            var appointment = await _appointmentService.CreateAsync(dto);
            return CreatedAtAction(nameof(Create), new { id = appointment.Id }, appointment);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message }); // 409 - الميعاد محجوز
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message }); // 400 - بيانات غلط
        }
    }
    [HttpGet("by-phone/{phoneNumber}")]
    public async Task<IActionResult> GetByPhone(string phoneNumber)
    {
        var appointments = await _appointmentService.GetByPhoneNumberAsync(phoneNumber);
        return Ok(appointments);
    }

    [HttpPatch("{id}/cancel")]
    public async Task<IActionResult> Cancel(Guid id)
    {
        try
        {
            await _appointmentService.CancelAsync(id);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
    [Authorize]
    [HttpGet("all")]
    public async Task<IActionResult> GetAll([FromQuery] AppointmentStatus? status, [FromQuery] Guid? doctorId, [FromQuery] DateOnly? date)
    {
        var appointments = await _appointmentService.GetAllAsync(status, doctorId, date);
        return Ok(appointments);
    }

    [Authorize]
    [HttpPatch("{id}/confirm")]
    public async Task<IActionResult> Confirm(Guid id)
    {
        try { await _appointmentService.ConfirmAsync(id); return NoContent(); }
        catch (ArgumentException ex) { return NotFound(new { message = ex.Message }); }
    }

    [Authorize]
    [HttpPatch("{id}/complete")]
    public async Task<IActionResult> Complete(Guid id)
    {
        try { await _appointmentService.CompleteAsync(id); return NoContent(); }
        catch (ArgumentException ex) { return NotFound(new { message = ex.Message }); }
    }
}