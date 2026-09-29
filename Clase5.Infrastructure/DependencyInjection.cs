using Clase2.Application.Abstractions.Entities;
using Clase5.Infrastructure.Data;
using Clase5.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Clase5.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("Default")));

        services.AddScoped<ICursoRepository, CursoRepository>();
        services.AddScoped<IProfesorRepository, ProfesorRepository>();

        return services;
    }
}