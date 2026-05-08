using Scheduling.API.DTOs.Appointment;
using Scheduling.API.DTOs.Mappings;
using Scheduling.API.Entities;
using Scheduling.API.Repositories.Interfaces;
using Scheduling.API.Services.Interfaces;

namespace Scheduling.API.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IServiceOfferingRepository _serviceOfferingRepository;

    public AppointmentService(
        IAppointmentRepository appointmentRepository,
        IServiceOfferingRepository serviceOfferingRepository)
    {
        _appointmentRepository = appointmentRepository;
        _serviceOfferingRepository = serviceOfferingRepository;
    }

    public async Task<AppointmentResponseDto> CreateAsync(CreateAppointmentDto dto)
    {
        var service = await _serviceOfferingRepository.GetByIdAsync(dto.ServiceOfferingId)
            ?? throw new KeyNotFoundException("Service offering not found.");

        if (!service.IsActive)
            throw new InvalidOperationException("Service offering is not active.");

        var endTime = dto.StartTime.Add(service.Duration);

        var hasConflict = await _appointmentRepository.HasConflictAsync(dto.StaffId, dto.StartTime, endTime);
        if (hasConflict)
            throw new InvalidOperationException("The staff member already has an appointment during this time.");

        var appointment = new Appointment(dto.CustomerId, dto.StaffId, dto.ServiceOfferingId, dto.StartTime, endTime);

        await _appointmentRepository.AddAsync(appointment);
        await _appointmentRepository.SaveChangesAsync();

        return await GetByIdOrThrowWithDetailsAsync(appointment.Id);
    }

    public async Task<AppointmentResponseDto> GetByIdAsync(Guid id)
        => await GetByIdOrThrowWithDetailsAsync(id);

    public async Task<IEnumerable<AppointmentResponseDto>> GetByCustomerAsync(Guid customerId)
    {
        var appointments = await _appointmentRepository.GetByCustomerAsync(customerId);
        return appointments.Select(a => a.ToDto());
    }

    public async Task<AppointmentResponseDto> ConfirmAsync(Guid id)
    {
        var appointment = await GetByIdOrThrowAsync(id);
        appointment.Confirm();
        await _appointmentRepository.SaveChangesAsync();
        return await GetByIdOrThrowWithDetailsAsync(id);
    }

    public async Task<AppointmentResponseDto> CancelAsync(Guid id)
    {
        var appointment = await GetByIdOrThrowAsync(id);
        appointment.Cancel();
        await _appointmentRepository.SaveChangesAsync();
        return await GetByIdOrThrowWithDetailsAsync(id);
    }

    public async Task<AppointmentResponseDto> CompleteAsync(Guid id)
    {
        var appointment = await GetByIdOrThrowAsync(id);
        appointment.Complete();
        await _appointmentRepository.SaveChangesAsync();
        return await GetByIdOrThrowWithDetailsAsync(id);
    }

    public async Task<AppointmentResponseDto> MarkAsNoShowAsync(Guid id)
    {
        var appointment = await GetByIdOrThrowAsync(id);
        appointment.MarkAsNoShow();
        await _appointmentRepository.SaveChangesAsync();
        return await GetByIdOrThrowWithDetailsAsync(id);
    }

    private async Task<Appointment> GetByIdOrThrowAsync(Guid id)
        => await _appointmentRepository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Appointment {id} not found.");

    private async Task<AppointmentResponseDto> GetByIdOrThrowWithDetailsAsync(Guid id)
    {
        var appointment = await _appointmentRepository.GetByIdWithDetailsAsync(id)
            ?? throw new KeyNotFoundException($"Appointment {id} not found.");
        return appointment.ToDto();
    }
}
