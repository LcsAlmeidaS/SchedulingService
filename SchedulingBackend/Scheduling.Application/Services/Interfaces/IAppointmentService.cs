using Scheduling.Application.DTOs.Appointment;

namespace Scheduling.Application.Services.Interfaces;

public interface IAppointmentService
{
    Task<AppointmentResponseDto> CreateAsync(CreateAppointmentDto dto);
    Task<AppointmentResponseDto> GetByIdAsync(Guid id);
    Task<IEnumerable<AppointmentResponseDto>> GetByCustomerAsync(Guid customerId);
    Task<AppointmentResponseDto> ConfirmAsync(Guid id);
    Task<AppointmentResponseDto> CancelAsync(Guid id);
    Task<AppointmentResponseDto> CompleteAsync(Guid id);
    Task<AppointmentResponseDto> MarkAsNoShowAsync(Guid id);
}
