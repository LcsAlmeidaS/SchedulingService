using Microsoft.AspNetCore.Mvc;
using Scheduling.API.DTOs.Appointment;
using Scheduling.API.Services.Interfaces;

namespace Scheduling.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentsController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var result = await _appointmentService.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetByCustomerId([FromQuery] Guid customerId)
    {
        var result = await _appointmentService.GetByCustomerAsync(customerId);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAppointmentDto dto)
    {
        var result = await _appointmentService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPatch("{id:guid}/confirm")]
    public async Task<IActionResult> Confirm([FromRoute] Guid id)
    {
        var result = await _appointmentService.ConfirmAsync(id);
        return Ok(result);
    }

    [HttpPatch("{id:guid}/cancel")]
    public async Task<IActionResult> Cancel([FromRoute] Guid id)
    {
        var result = await _appointmentService.CancelAsync(id);
        return Ok(result);
    }

    [HttpPatch("{id:guid}/complete")]
    public async Task<IActionResult> Complete([FromRoute] Guid id)
    {
        var result = await _appointmentService.CompleteAsync(id);
        return Ok(result);
    }

    [HttpPatch("{id:guid}/no-show")]
    public async Task<IActionResult> NoShow([FromRoute] Guid id)
    {
        var result = await _appointmentService.MarkAsNoShowAsync(id);
        return Ok(result);
    }
}
