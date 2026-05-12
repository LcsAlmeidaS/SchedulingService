using Microsoft.AspNetCore.Mvc;
using Scheduling.API.DTOs.Staff;
using Scheduling.API.Services.Interfaces;

namespace Scheduling.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StaffController : ControllerBase
{
    private readonly IStaffService _staffService;

    public StaffController(IStaffService staffService)
    {
        _staffService = staffService;
    }

    [HttpGet("active")]
    public async Task<IActionResult> GetActive()
    {
        var result = await _staffService.GetActiveAsync();
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var result = await _staffService.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStaffDto dto)
    {
        var result = await _staffService.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPatch("{id:guid}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateStaffDto dto)
    {
        var result = await _staffService.UpdateAsync(id, dto);
        return Ok(result);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Deactivate([FromRoute] Guid id)
    {
        await _staffService.DeactivateAsync(id);
        return NoContent();
    }

    [HttpPost("{id:guid}/working-hours")]
    public async Task<IActionResult> AddWorkingHours([FromRoute] Guid id, [FromBody] AddWorkingHoursDto dto)
    {
        var result = await _staffService.AddWorkingHoursAsync(id, dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPost("{id:guid}/break-times")]
    public async Task<IActionResult> AddBreakTime([FromRoute] Guid id, [FromBody] AddBreakTimeDto dto)
    {
        var result = await _staffService.AddBreakTimeAsync(id, dto);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpGet("{id:guid}/appointment-summary")]
    public async Task<IActionResult> GetAppointmentSummary(
        [FromRoute] Guid id,
        [FromQuery] DateOnly? from = null,
        [FromQuery] DateOnly? to = null)
    {
        var result = await _staffService.GetAppointmentSummaryAsync(id, from, to);
        return Ok(result);
    }

    [HttpGet("{id:guid}/scheduled-hours")]
    public async Task<IActionResult> GetScheduledHours(
        [FromRoute] Guid id,
        [FromQuery] DateOnly? from = null,
        [FromQuery] DateOnly? to = null)
    {
        var result = await _staffService.GetScheduledHoursAsync(id, from, to);
        return Ok(result);
    }
}