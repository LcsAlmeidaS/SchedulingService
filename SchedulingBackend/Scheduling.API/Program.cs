using Microsoft.EntityFrameworkCore;
using Scheduling.API.Infrastructure.Json;
using Scheduling.API.Infrastructure.Middleware;
using Scheduling.Application.Repositories;
using Scheduling.Application.Services;
using Scheduling.Application.Services.Interfaces;
using Scheduling.Infrastructure.Context;
using Scheduling.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

#region Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
#endregion

#region Repositories
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IStaffRepository, StaffRepository>();
builder.Services.AddScoped<IServiceOfferingRepository, ServiceOfferingRepository>();
#endregion

#region Services
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IStaffService, StaffService>();
builder.Services.AddScoped<IAvailabilityService, AvailabilityService>();
builder.Services.AddScoped<IServiceOfferingService, ServiceOfferingService>();
#endregion

#region Web
builder.Services.AddControllers()
    .AddJsonOptions(options =>
        options.JsonSerializerOptions.Converters.Add(new TimeOnlyJsonConverter()));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#endregion

var app = builder.Build();

#region Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseHttpsRedirection();
app.MapControllers();
#endregion

app.Run();
