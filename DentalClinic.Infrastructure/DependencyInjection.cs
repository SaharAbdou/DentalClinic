using DentalClinic.Application.Interfaces;
using DentalClinic.Application.Services;
using DentalClinic.Infrastructure.Persistence;
using DentalClinic.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DentalClinic.Application.Services;
namespace DentalClinic.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IDoctorService, DoctorService>();
        services.AddScoped<IScheduleService, ScheduleService>();
        services.AddScoped<IDoctorAvailabilityService, DoctorAvailabilityService>();
        services.AddScoped<IAppointmentService, AppointmentService>();
        services.AddScoped<IPhotoService, PhotoService>();
        return services;
    }
}